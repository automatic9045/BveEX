using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using SlimDX.Direct3D9;

#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
namespace Zbx1425.DXDynamicTexture
{
    public class TextureHandle : IDisposable
    {
        public bool IsCreated
        {
            get
            {
                return DXTexture != null;
            }
        }
        public SurfaceDescription Description { get; private set; }
        public readonly int Width, Height;

        public event EventHandler Created;

        private long lastUpdateTime = 0;

        internal TextureHandle(int width, int height)
        {
            this.Width = width; this.Height = height;
        }

        internal TextureHandle(Texture texture)
        {
            DXTexture = texture;
            Description = texture.GetLevelDescription(0);
            if (Description.Format != Format.A8R8G8B8) throw new NotSupportedException("The format of the texture must be Format.A8R8G8B8.");
            Width = Description.Width;
            Height = Description.Height;
        }

        private Texture _DXTexture;
        public Texture DXTexture
        {
            get
            {
                return _DXTexture;
            }
            private set
            {
                _DXTexture = value;
                Description = _DXTexture.GetLevelDescription(0);
            }
        }

        public void Update(Bitmap bmp)
        {
            if (DXTexture == null || bmp == null) return;
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

            var rect = DXTexture.LockRectangle(0, LockFlags.Discard);
            if (rect.Data.CanWrite) rect.Data.WriteRange(bmpData.Scan0, rect.Pitch * Description.Height);
            bmp.UnlockBits(bmpData);
            DXTexture.UnlockRectangle(0);
            lastUpdateTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public void Update(GDIHelper helper)
        {
            if (DXTexture == null || helper == null) return;

            if (helper.Bitmap == null)
                throw new ArgumentException("You can only use GDIHelper created with a width and height.", "helper");
            if (helper.HasAcquiredHDC())
                throw new InvalidOperationException("You must call EndGDI() on the GDIHelper before updating.");
            Update(helper.Bitmap);
            lastUpdateTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public void Update(byte[] data)
        {
            if (DXTexture == null || data == null) return;

            var rect = DXTexture.LockRectangle(0, LockFlags.Discard);
            var pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
            if (rect.Data.CanWrite) rect.Data.WriteRange(pinnedArray.AddrOfPinnedObject(), data.Length);
            pinnedArray.Free();
            DXTexture.UnlockRectangle(0);
            lastUpdateTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public bool HasEnoughTimePassed(double intendedFPS)
        {
            int IntendedInterval = intendedFPS == 0 ? 0 : Convert.ToInt32(1000.0 / intendedFPS);
            long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            bool result = IntendedInterval == 0 || lastUpdateTime > currentTime || currentTime - lastUpdateTime > IntendedInterval;
            return result;
        }

        internal Texture GetOrCreate(Device device)
        {
            if (IsCreated) return DXTexture;
            DXTexture = new Texture(device, Width, Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            Created?.Invoke(this, new EventArgs());
            return DXTexture;
        }

        public void Dispose()
        {
            if (_DXTexture != null)
            {
                try
                {
                    _DXTexture.Dispose();
                }
                catch
                {
                }
                _DXTexture = null;
            }
        }
    }
}
