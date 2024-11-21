using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SkiaSharp;

public class CursorHelper
{
    [DllImport("user32.dll")]
    private static extern bool GetCursorInfo(ref CURSORINFO pci);

    [DllImport("user32.dll")]
    private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("user32.dll")]
    private static extern bool DrawIconEx(
        IntPtr hdc,
        int x,
        int y,
        IntPtr hIcon,
        int cxWidth,
        int cyWidth,
        int istepIfAniCur,
        IntPtr hbrFlickerFreeDraw,
        int diFlags);

    private const int CURSOR_SHOWING = 0x00000001;
    private const int DI_NORMAL = 0x0003;

    [StructLayout(LayoutKind.Sequential)]
    private struct CURSORINFO
    {
        public int cbSize;
        public int flags;
        public IntPtr hCursor;
        public Point ptScreenPos;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ICONINFO
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    public static (SKBitmap, Point)? GetCursorBitmap()
    {
        var cursorInfo = new CURSORINFO { cbSize = Marshal.SizeOf(typeof(CURSORINFO)) };

        if (!GetCursorInfo(ref cursorInfo) || cursorInfo.flags != CURSOR_SHOWING)
        {
            return null; // Nie ma kursora do narysowania
        }

        // Pobierz pozycję kursora
        var position = cursorInfo.ptScreenPos;

        // Tworzenie grafiki GDI+ dla kursora
        using (var bmp = new Bitmap(32, 32)) // Zakładamy maksymalny rozmiar kursora 32x32
        {
            using (var g = Graphics.FromImage(bmp))
            {
                var hdc = g.GetHdc();
                DrawIconEx(hdc, 0, 0, cursorInfo.hCursor, 32, 32, 0, IntPtr.Zero, DI_NORMAL);
                g.ReleaseHdc(hdc);
            }

            // Konwersja Bitmap na SKBitmap
            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Seek(0, System.IO.SeekOrigin.Begin);

                var skBitmap = SKBitmap.Decode(ms);
                return (skBitmap, position);
            }
        }
    }
}
