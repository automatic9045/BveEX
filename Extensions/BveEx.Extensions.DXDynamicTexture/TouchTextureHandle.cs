using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using SlimDX.Direct3D9;

#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
namespace Zbx1425.DXDynamicTexture
{
    public class TouchTextureHandle : TextureHandle
    {
        private readonly byte[] tempBufferA, tempBufferB;
        private Rectangle clickableArea;

        public event EventHandler<TouchEventArgs> MouseDown;
        public event EventHandler<TouchEventArgs> MouseUp;

        internal TouchTextureHandle(int width, int height) : base(width, height)
        {
            tempBufferA = new byte[width * height * PIXEL_LEN];
            tempBufferB = new byte[width * height * PIXEL_LEN];
            clickableArea = new Rectangle(0, 0, width, height);
        }

        internal TouchTextureHandle(Texture texture) : base(texture)
        {
            tempBufferA = new byte[Width * Height * PIXEL_LEN];
            tempBufferB = new byte[Width * Height * PIXEL_LEN];
            clickableArea = new Rectangle(0, 0, Width, Height);
        }

        public void SetClickableArea(int x0, int y0, int width, int height)
        {
            clickableArea = new Rectangle(x0, y0, width, height);
            Array.Clear(tempBufferB, 0, tempBufferB.Length);
        }

        internal void BackupTexture()
        {
            if (DXTexture == null) return;
            var rect = DXTexture.LockRectangle(0, LockFlags.ReadOnly);
            rect.Data.Read(tempBufferA, 0, tempBufferA.Length);
            DXTexture.UnlockRectangle(0);
        }

        internal void RestoreTexture()
        {
            Update(tempBufferA);
        }

        private const int OFFSET_R = 2, OFFSET_G = 1, OFFSET_B = 0, OFFSET_A = 3, PIXEL_LEN = 4;

        internal void FillColorPhase1(byte nonse, byte id)
        {
            unsafe
            {
                for (int y = 0; y < clickableArea.Height; y++)
                {
                    for (int x = 0; x < clickableArea.Width; x++)
                    {
                        int offset = ((y + clickableArea.Y) * Description.Width + (x + clickableArea.X)) * PIXEL_LEN;
                        tempBufferB[offset + OFFSET_R] = nonse;
                        tempBufferB[offset + OFFSET_G] = id;
                        tempBufferB[offset + OFFSET_B] = (byte)(0xFF - nonse);
                        tempBufferB[offset + OFFSET_A] = 0xFF;
                    }
                }
            }
            Update(tempBufferB);
        }

        internal void FillColorPhase2(byte id)
        {
            unsafe
            {
                for (int y = 0; y < clickableArea.Height; y++)
                {
                    for (int x = 0; x < clickableArea.Width; x++)
                    {
                        int offset = ((y + clickableArea.Y) * Description.Width + (x + clickableArea.X)) * PIXEL_LEN;
                        tempBufferB[offset + OFFSET_R] = (byte)((double)x / clickableArea.Width * 255);
                        // tempBufferB[offset + OFFSET_G] = id;
                        tempBufferB[offset + OFFSET_B] = (byte)((double)y / clickableArea.Height * 255);
                        // tempBufferB[offset + OFFSET_A] = 0xFF;
                    }
                }
            }
            Update(tempBufferB);
        }

        internal bool RaiseEvent(int rx, int ry, MouseButtons button, bool isUp, Color color)
        {
            int x = (int)((double)rx / 255 * clickableArea.Width);
            int y = (int)((double)ry / 255 * clickableArea.Height);
            var eventArgs = new TouchEventArgs(x, y, button);
            eventArgs.Color = color;
            if (isUp)
            {
                MouseUp?.Invoke(this, eventArgs);
            }
            else
            {
                MouseDown?.Invoke(this, eventArgs);
            }
            return eventArgs.PreventDefault;
        }
    }
}
