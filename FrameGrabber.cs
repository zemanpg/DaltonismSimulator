using SharpDX.Direct3D11;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaltonismSimulator
{
    public class FrameGrabber 
    {
        public RefCntSkBitmap lastFrame = new RefCntSkBitmap(null, "lastFrame bitmap");
        int frameCounter = 0;
        private Thread captureThread;
        private volatile bool isRunning = true;
        Filter filter = new Filter();
        private Rectangle? captureRegion;
        public Rectangle CaptureRegion => captureRegion ?? Rectangle.Empty;
        private Action doOnNewFrame;


        // -----------------------------------------------------------------
        public FrameGrabber()
        {
            captureThread = new Thread(CaptureLoop);
        }


        // -----------------------------------------------------------------
        public void Start()
        {
            captureThread.Start();
        }


        // -----------------------------------------------------------------
        public void Stop()
        {
            isRunning = false;
            captureThread.Join();
        }


        // -----------------------------------------------------------------
        public void WithFilter(Filter filter)
        {
            this.filter = filter;
        }


        // -----------------------------------------------------------------
        public void WithCropRegion(Rectangle newCropRegion)
        {
            this.captureRegion = newCropRegion;
        }


        // -----------------------------------------------------------------
        public void WithDoOnNewFrame(Action doOnNewFrame)
        {
            this.doOnNewFrame = doOnNewFrame;
        }


        // -----------------------------------------------------------------
        private SKBitmap CropToRegion(SKBitmap input, Rectangle region)
        {
            SKBitmap cropped = new SKBitmap(region.Width, region.Height);

            using (SKCanvas canvas = new SKCanvas(cropped))
            {
                canvas.DrawBitmap(input,
                    new SKRect(region.X, region.Y, region.X + region.Width, region.Y + region.Height),
                    new SKRect(0, 0, region.Width, region.Height));
            }

            return cropped;
        }


        // -----------------------------------------------------------------
        private void CaptureLoop()
        {
            var duplicator = new DesktopDuplicator(0); // main screen (ID 0)

            while (isRunning)
            {

                // Get frame from Desktop Duplication API
                using SKBitmap frame = duplicator.GetLatestFrame();
                if (frame == null || frame.IsEmpty)
                    continue;

                Rectangle rect = this.captureRegion ?? new Rectangle(0, 0, frame.Width, frame.Height);
                SKBitmap croppedBitmap = CropToRegion(frame, rect);
                filter.Apply(croppedBitmap);
                Interlocked.Increment(ref frameCounter);

                // set new frame bitmap as reference counted IDisposable (because 2 threads can use it)
                lastFrame.Change(croppedBitmap, $"frame[{frameCounter}]");

                if (doOnNewFrame != null)
                    doOnNewFrame();
            }

            duplicator.Dispose();
        }


    }
}
