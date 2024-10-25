using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using HarmonyLib;

using SlimDX.Direct3D9;

#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
namespace Zbx1425.DXDynamicTexture
{
    public static class TouchManager
    {
        internal static List<TouchTextureHandle> Handles = new List<TouchTextureHandle>();
        private static int TouchPhase;
        private static MouseButtons Button;
        private static bool IsUp;
        private static int mouseX, mouseY;
        private static byte Nonse;
        private static int potentialID = -1;
        private static readonly Random random = new Random();

        private static readonly HashSet<int> EventsToHandle = new HashSet<int>();

        public enum EventType
        {
            Down = 0,
            Up = 1,
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);

        internal static void Initialize()
        {
            try
            {
                Form mainForm = Application.OpenForms[0];
                TextureManager.Harmony.Patch(
                    mainForm.GetType().GetMethod("OnPaint", BindingFlags.NonPublic | BindingFlags.Instance),
                    new HarmonyMethod(typeof(TouchManager), nameof(OnPaintPrefix)),
                    new HarmonyMethod(typeof(TouchManager), nameof(OnPaintPostfix))
                );
                mainForm.MouseDown += TouchManager_MouseDown;
                mainForm.MouseUp += TouchManager_MouseUp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static void TouchManager_MouseDown(object sender, MouseEventArgs e)
        {
            if (EventsToHandle.Contains((int)e.Button))
            {
                TouchPhase = 1;
                IsUp = false;
                mouseX = e.X; mouseY = e.Y;
                Button = e.Button;
            }
        }

        private static void TouchManager_MouseUp(object sender, MouseEventArgs e)
        {
            if (EventsToHandle.Contains((int)e.Button + 1))
            {
                TouchPhase = 1;
                IsUp = true;
                mouseX = e.X; mouseY = e.Y;
                Button = e.Button;
            }
        }

        public static void EnableEvent(MouseButtons button, EventType type)
        {
            int eventID = (int)button + (int)type;
            EventsToHandle.Add(eventID);
        }

        public static void DisableEvent(MouseButtons button, EventType type)
        {
            int eventID = (int)button + (int)type;
            EventsToHandle.Remove(eventID);
        }

        private static void OnPaintPrefix()
        {
            if (TouchPhase == 0) return;

            switch (TouchPhase)
            {
                case 1:
                    Nonse = (byte)random.Next(0, 256);
                    for (int i = 0; i < Handles.Count; i++)
                    {
                        Handles[i].BackupTexture();
                        Handles[i].FillColorPhase1(Nonse, (byte)i);
                    }
                    break;
                case 2:
                    Handles[potentialID].FillColorPhase2((byte)potentialID);
                    break;
            }
        }

        private static void OnPaintPostfix()
        {
            if (TouchPhase == 0 || TextureManager.DXDevice == null) return;

            var target = TextureManager.DXDevice.GetRenderTarget(0);
            var destinationSurface = Surface.CreateOffscreenPlain(
                    TextureManager.DXDevice,
                    target.Description.Width, target.Description.Height,
                    Format.X8R8G8B8, Pool.SystemMemory
                );
            TextureManager.DXDevice.GetRenderTargetData(target, destinationSurface);
            var rect = destinationSurface.LockRectangle(LockFlags.ReadOnly);
            rect.Data.Seek((mouseY * target.Description.Width + mouseX) * 4, System.IO.SeekOrigin.Begin);
            int b = rect.Data.ReadByte(), g = rect.Data.ReadByte(), r = rect.Data.ReadByte();
            Color color = Color.FromArgb(r, g, b);
            destinationSurface.UnlockRectangle();
            destinationSurface.Dispose();
            target.Dispose();

            switch (TouchPhase)
            {
                case 1:
                    if (color.R + color.B == 255 && color.G < Handles.Count)
                    {
                        potentialID = color.G;
                        TouchPhase = 2;
                    }
                    else
                    {
                        potentialID = -1;
                        TouchPhase = 0;
                    }
                    for (int i = 0; i < Handles.Count; i++)
                    {
                        if (i == potentialID) continue;
                        Handles[i].RestoreTexture();
                    }
                    break;
                case 2:
                    Handles[potentialID].RestoreTexture();
                    TouchPhase = 0;
                    if (color.G == potentialID)
                    {
                        Handles[potentialID].RaiseEvent(color.R, color.B, Button, IsUp, color);
                    }
                    break;
            }
        }

        public static TouchTextureHandle Register(string fileNameEnding, int width, int height)
        {
            if (fileNameEnding.Trim().Length == 0) throw new ArgumentException("Must not be empty.", nameof(fileNameEnding));
            if (!TextureManager.IsPowerOfTwo(width)) throw new ArgumentException("Must be a integral power of 2.", nameof(width));
            if (!TextureManager.IsPowerOfTwo(height)) throw new ArgumentException("Must be a integral power of 2.", nameof(height));

            fileNameEnding = Path.GetFullPath(Path.Combine("a:", fileNameEnding.ToLowerInvariant())).Substring(3);
            if (TextureManager.Handles.ContainsKey(fileNameEnding))
                return (TouchTextureHandle)TextureManager.Handles[fileNameEnding];
            var result = new TouchTextureHandle(width, height);
            Handles.Add(result);
            TextureManager.Handles.Add(fileNameEnding, result);
            return result;
        }

        public static TouchTextureHandle Register(Texture texture)
        {
            foreach (var item in TextureManager.Handles)
            {
                if (item.Value?.DXTexture == texture) return (TouchTextureHandle)item.Value;
            }

            var result = new TouchTextureHandle(texture);
            Handles.Add(result);
            string guidString;
            do
            {
                guidString = Guid.NewGuid().ToString();
            } while (TextureManager.Handles.ContainsKey(guidString));
            TextureManager.Handles.Add(guidString, result);
            return result;
        }

        private static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
    }
}
