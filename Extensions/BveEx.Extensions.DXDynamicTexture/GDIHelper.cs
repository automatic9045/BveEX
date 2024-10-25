using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
namespace Zbx1425.DXDynamicTexture
{
    public class GDIHelper : IDisposable
    {
        [DllImport("gdi32")]
        internal static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);
        [DllImport("gdi32")]
        internal static extern int SelectObject([In] IntPtr hdc, [In] IntPtr hObject);
        [DllImport("gdi32")]
        internal static extern int DeleteDC([In] IntPtr hdc);
        [DllImport("gdi32")]
        internal static extern int DeleteObject([In] IntPtr hObject);
        [DllImport("gdi32")]
        internal static extern int BitBlt([In] IntPtr hDestDC, int x, int y, int nWidth, int nHeight,
            [In] IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        internal const int SRCCOPY = 0xCC0020;

        [DllImport("gdi32")]
        internal static extern int Rectangle([In] IntPtr hdc, int x1, int y1, int x2, int y2);
        [DllImport("gdi32")]
        internal static extern IntPtr CreateSolidBrush(int crColor);

        private readonly Dictionary<Bitmap, ReferenceCountable<IntPtr>> Images = new Dictionary<Bitmap, ReferenceCountable<IntPtr>>();
        private readonly Dictionary<Color, ReferenceCountable<IntPtr>> Brushes = new Dictionary<Color, ReferenceCountable<IntPtr>>();

        private readonly Queue<Bitmap> ImageKeys = new Queue<Bitmap>();
        private readonly Queue<Color> BrushKeys = new Queue<Color>();

        public Graphics Graphics;
        public Bitmap Bitmap;

        public int ImageCacheCapacity { get; set; } = 16;
        public int BrushCacheCapacity { get; set; } = 16;

        public GDIHelper(Graphics g)
        {
            Graphics = g;
        }

        public GDIHelper(int width, int height)
        {
            Bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.Clear(Color.Transparent);
        }

        private IntPtr hDC;
        private IntPtr hMemDC;

        public void BeginGDI()
        {
            if (hDC != IntPtr.Zero) return;
            hDC = Graphics.GetHdc();
            hMemDC = CreateCompatibleDC(hDC);
        }

        public void EndGDI()
        {
            if (hDC == IntPtr.Zero) return;
            DeleteDC(hMemDC);
            Graphics.ReleaseHdc();
            hDC = IntPtr.Zero;
        }

        public bool HasAcquiredHDC()
        {
            return hDC != IntPtr.Zero;
        }

        public void DrawImage(Bitmap img, int x, int y)
        {
            if (hDC == IntPtr.Zero) throw new InvalidOperationException("You must call BeginGDI() first!");
            SelectObject(hMemDC, GetHBitmap(img));
            BitBlt(hDC, x, y, img.Width, img.Height, hMemDC, 0, 0, SRCCOPY);
        }

        public void DrawImage(Bitmap img, int x, int y, int sy, int h)
        {
            if (hDC == IntPtr.Zero) throw new InvalidOperationException("You must call BeginGDI() first!");
            SelectObject(hMemDC, GetHBitmap(img));
            BitBlt(hDC, x, y, img.Width, h, hMemDC, 0, sy, SRCCOPY);
        }

        public void FillRect12(Color color, int x1, int y1, int x2, int y2)
        {
            if (hDC == IntPtr.Zero) throw new InvalidOperationException("You must call BeginGDI() first!");
            SelectObject(hDC, GetSolidBrush(color));
            Rectangle(hDC, x1, y1, x2, y2);
        }

        public void FillRectWH(Color color, int x1, int y1, int w, int h)
        {
            if (hDC == IntPtr.Zero) throw new InvalidOperationException("You must call BeginGDI() first!");
            SelectObject(hDC, GetSolidBrush(color));
            Rectangle(hDC, x1, y1, x1 + w, y1 + h);
        }

        private IntPtr GetHBitmap(Bitmap image)
        {
            if (Images.TryGetValue(image, out ReferenceCountable<IntPtr> value))
            {
                value.Reference();
                return value.Value;
            }
            else
            {
                ReferenceCountable<IntPtr> newValue = new ReferenceCountable<IntPtr>(image.GetHbitmap());
                Images.Add(image, newValue);
                ImageKeys.Enqueue(image);
                CollectGarbage(Images, ImageKeys, ImageCacheCapacity);
                return newValue.Value;
            }
        }

        private IntPtr GetSolidBrush(Color color)
        {
            if (Brushes.TryGetValue(color, out ReferenceCountable<IntPtr> value))
            {
                value.Reference();
                return value.Value;
            }
            else
            {
                ReferenceCountable<IntPtr> newValue = new ReferenceCountable<IntPtr>(CreateSolidBrush(color.R | color.G << 8 | color.B << 16));
                Brushes.Add(color, newValue);
                BrushKeys.Enqueue(color);
                CollectGarbage(Brushes, BrushKeys, BrushCacheCapacity);
                return newValue.Value;
            }
        }

        private void CollectGarbage<T>(Dictionary<T, ReferenceCountable<IntPtr>> dictionary, Queue<T> keys, int capacity)
        {
            while (capacity < dictionary.Count)
            {
                T keyToDelete = keys.Dequeue();
                ReferenceCountable<IntPtr> valueToDelete = dictionary[keyToDelete];
                if (valueToDelete.ReferencedCount == 0)
                {
                    DeleteObject(valueToDelete.Value);
                    dictionary.Remove(keyToDelete);
                }
                else
                {
                    valueToDelete.Reset();
                    keys.Enqueue(keyToDelete);
                }
            }
        }

        public void Dispose()
        {
            EndGDI();
            if (Bitmap != null) Bitmap.Dispose();
            foreach (var pair in Images) DeleteObject(pair.Value.Value);
            Images.Clear();
            foreach (var pair in Brushes) DeleteObject(pair.Value.Value);
            Brushes.Clear();
        }


        private class ReferenceCountable<T>
        {
            public T Value { get; }
            public int ReferencedCount { get; private set; } = 0;

            public ReferenceCountable(T value)
            {
                Value = value;
            }

            public void Reference()
            {
                ReferencedCount++;
            }

            public void Reset()
            {
                ReferencedCount = 0;
            }
        }
    }
}
