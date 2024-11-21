using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SkiaSharp;
using System.Diagnostics;
using System.Drawing;




// -----------------------------------------------------------------
public class DesktopDuplicator : IDisposable
{
    private readonly OutputDuplication duplicator;
    private readonly SharpDX.Direct3D11.Device device;



    // -----------------------------------------------------------------
    public DesktopDuplicator(int adapterId)
    {
        // DirectX initialization
        Factory1 factory = new Factory1();
        Adapter adapter = factory.Adapters[adapterId];
        device = new SharpDX.Direct3D11.Device(adapter);

        Output output = adapter.Outputs[0];
        Output1 output1 = output.QueryInterface<Output1>();
        duplicator = output1.DuplicateOutput(device);
    }


    // -----------------------------------------------------------------
    public SKBitmap GetLatestFrame()
    {
        OutputDuplicateFrameInformation frameInfo;
        SharpDX.DXGI.Resource desktopResource;

        Result result = duplicator.TryAcquireNextFrame(500, out frameInfo, out desktopResource);
        if (result.Failure)
        {
            desktopResource?.Dispose();
            Console.WriteLine("Failed to acquire next frame: " + result.Code);
            return null;
        }

        using (var desktopTexture = desktopResource.QueryInterface<Texture2D>())
        {
            var bitmap = TextureToBitmap(desktopTexture);

            desktopResource.Dispose();
            duplicator.ReleaseFrame();

            return bitmap;
        }
    }


    // -----------------------------------------------------------------
    private SKBitmap TextureToBitmap(Texture2D texture)
    {
        Texture2DDescription desc = texture.Description;

        Texture2DDescription stagingDesc = new Texture2DDescription
        {
            Width = desc.Width,
            Height = desc.Height,
            MipLevels = 1,
            ArraySize = 1,
            Format = desc.Format,
            SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
            Usage = ResourceUsage.Staging, // Konieczne do mapowania
            BindFlags = BindFlags.None,
            CpuAccessFlags = CpuAccessFlags.Read,
            OptionFlags = ResourceOptionFlags.None
        };

        using (Texture2D stagingTexture = new Texture2D(device, stagingDesc))
        {
            device.ImmediateContext.CopyResource(texture, stagingTexture);
            DataBox dataBox = device.ImmediateContext.MapSubresource(stagingTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
            try
            {
                SKBitmap bitmap = new SKBitmap(desc.Width, desc.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
                SKImageInfo imageInfo = new SKImageInfo(desc.Width, desc.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
                unsafe
                {
                    byte* dest = (byte*)bitmap.GetPixels(); 
                    byte* src = (byte*)dataBox.DataPointer; 

                    for (int y = 0; y < desc.Height; y++)
                    {
                        void* destPtr = dest + imageInfo.RowBytes * y;
                        void* srcPtr = src + dataBox.RowPitch * y;
                        System.Buffer.MemoryCopy(srcPtr, destPtr, imageInfo.RowBytes, imageInfo.RowBytes);
                    }
                }
                return bitmap;
            }
            finally
            {
                device.ImmediateContext.UnmapSubresource(stagingTexture, 0);
            }
        }
    }


    // -----------------------------------------------------------------
    public void Dispose()
    {
        duplicator.Dispose();
        device.Dispose();
    }
}
