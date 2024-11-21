using SkiaSharp;
using SkiaSharp.Views.Desktop;
using RefCounted;

namespace DaltonismSimulator
{
    public partial class MainForm : Form
    {
        private Filter filter = new Filter();
        private FrameGrabber grabber = new FrameGrabber();


        // -----------------------------------------------------------------
        public MainForm()
        {
            InitializeComponent();
            CalculateCaptureRegion();
            grabber.WithFilter(filter);
            grabber.WithDoOnNewFrame(() => skc.Invalidate());
            grabber.Start();
        }


        // -----------------------------------------------------------------
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            grabber.Stop();
        }


        // -----------------------------------------------------------------
        private void Skc_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (IsAppOnPrimaryScreen())
            {
                e.Surface.Canvas.Clear(SKColors.Black);
                PaintText(e.Surface.Canvas, "MOVE APP TO ANOTHER SCREEN!!", 10, 30);
                return;
            }

            using (Reference<SKBitmap> refBmp = grabber.lastFrame.GetReference4Using())
            {
                SKBitmap bmp = refBmp.Instance;
                if (bmp == null || bmp.Handle == IntPtr.Zero)
                {
                    return;
                }
                SKCanvas canvas = e.Surface.Canvas;
                {
                    canvas.Clear(SKColors.Red);
                    canvas.DrawBitmap(bmp, new SKRect(0, 0, skc.Width, skc.Height));
                }
                PaintCursor(canvas);
                PaintText(canvas, filter.FilterName, 10, 30);
            }
        }


        // -----------------------------------------------------------------
        private bool IsAppOnPrimaryScreen()
        {
            Screen mainScreen = Screen.PrimaryScreen;
            return
                this.Left >= mainScreen.Bounds.Left &&
                this.Left <= mainScreen.Bounds.Right &&
                this.Top >= mainScreen.Bounds.Top &&
                this.Left <= mainScreen.Bounds.Bottom;
        }


        // -----------------------------------------------------------------
        private void CalculateCaptureRegion()
        {
            Point controlLocationOnForm = skc.PointToScreen(Point.Empty);

            var screenContainingForm = Screen.FromControl(this); 

            int controlXOnMonitor = controlLocationOnForm.X - screenContainingForm.Bounds.X;
            int controlYOnMonitor = controlLocationOnForm.Y - screenContainingForm.Bounds.Y;

            Rectangle captureRegion = new Rectangle(
                controlXOnMonitor, // X on main screen
                controlYOnMonitor, // Y on main screen
                skc.Width,         // skia control width
                skc.Height         // skia control height
            );

            grabber.WithCropRegion( captureRegion );
        }


        // -----------------------------------------------------------------
        private void PaintText(SKCanvas canvas, string txt, int x, int y)
        {
            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.Lime; 
                paint.TextSize = 30;         
                paint.Typeface = SKTypeface.FromFamilyName("Arial"); 
                paint.IsAntialias = true;    

                paint.Color = SKColors.Black; 
                canvas.DrawText(txt, x-2, y-2, paint);
                paint.Color = SKColors.Lime; 
                canvas.DrawText(txt, x, y, paint);
            }
        }


        // -----------------------------------------------------------------
        private void PaintCursor(SKCanvas canvas)
        {
            var cursorData = CursorHelper.GetCursorBitmap();
            if (cursorData.HasValue)
            {
                var (cursorBitmap, cursorPosition) = cursorData.Value;

                try
                {
                    if (grabber.CaptureRegion.Contains(cursorPosition))
                    {
                        var localCursorX = cursorPosition.X - grabber.CaptureRegion.X;
                        var localCursorY = cursorPosition.Y - grabber.CaptureRegion.Y;

                        var scaleX = (float)skc.Width / grabber.CaptureRegion.Width;
                        var scaleY = (float)skc.Height / grabber.CaptureRegion.Height;

                        var transformedCursorX = localCursorX * scaleX;
                        var transformedCursorY = localCursorY * scaleY;

                        canvas.DrawBitmap(cursorBitmap, new SKPoint(transformedCursorX, transformedCursorY));
                    }
                }
                finally
                {
                    cursorBitmap.Dispose();
                }
            }
        }


        // -----------------------------------------------------------------
        private void tsiNormal_Click(object sender, EventArgs e)
        {
            filter = new Filter();
            grabber.WithFilter(filter);
            skc.Invalidate();
        }


        // -----------------------------------------------------------------
        private void tsiProtanomaly_Click(object sender, EventArgs e)
        {
            filter = new ProtanopiaFilter();
            grabber.WithFilter(filter);
            skc.Invalidate();
        }


        // -----------------------------------------------------------------
        private void tsiDeuteranomaly_Click(object sender, EventArgs e)
        {
            filter = new DeuteranopiaFilter();
            grabber.WithFilter(filter);
            skc.Invalidate();
        }


        // -----------------------------------------------------------------
        private void tsiTritanomaly_Click(object sender, EventArgs e)
        {
            filter = new TritanopiaFilter();
            grabber.WithFilter(filter);
            skc.Invalidate();
        }


        // -----------------------------------------------------------------
        private void Form_Resize(object sender, EventArgs e)
        {
            CalculateCaptureRegion();
        }


        // -----------------------------------------------------------------
        private void Form_Move(object sender, EventArgs e)
        {
            CalculateCaptureRegion();

        }
    }
}
