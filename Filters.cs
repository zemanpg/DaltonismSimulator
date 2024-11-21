using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------
public class Filter
{

    protected void ApplyWithMatrix(SKBitmap inputBitmap, float[] matrix)
    {
        var colorFilter = SKColorFilter.CreateColorMatrix(matrix);

        using (var canvas = new SKCanvas(inputBitmap))
        using (var paint = new SKPaint { ColorFilter = colorFilter })
        {
            canvas.DrawBitmap(inputBitmap, new SKRect(0, 0, inputBitmap.Width, inputBitmap.Height), paint);
        }
    }

    public virtual string FilterName => "NO FILTER";

    public virtual void Apply(SKBitmap inputBitmap)
    {
        // do nothing
    }
}





// -----------------------------------------------------------------
public class DeuteranopiaFilter : Filter
{
    public override void Apply(SKBitmap inputBitmap)
    {
        var colorMatrix = new float[]
        {
            0.8f, 0.2f, 0.0f, 0.0f, 0.0f, // Red
            0.0f, 0.7f, 0.3f, 0.0f, 0.0f, // Green
            0.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Blue
            0.0f, 0.0f, 0.0f, 1.0f, 0.0f, // Alpha
        };
        ApplyWithMatrix(inputBitmap, colorMatrix);
    }
    public override string FilterName => "Deuteranopia";
}




// -----------------------------------------------------------------
public class ProtanopiaFilter : Filter
{
    public override void Apply(SKBitmap inputBitmap)
    {
        var colorMatrix = new float[]
        {
            0.567f, 0.433f, 0.0f, 0.0f, 0.0f, // Red
            0.558f, 0.442f, 0.0f, 0.0f, 0.0f, // Green
            0.0f,   0.242f, 0.758f, 0.0f, 0.0f, // Blue
            0.0f,   0.0f,   0.0f,   1.0f, 0.0f, // Alpha
        };
        ApplyWithMatrix(inputBitmap, colorMatrix);
    }
    public override string FilterName => "Protanopia";
}




// -----------------------------------------------------------------
public class TritanopiaFilter : Filter
{
    public override void Apply(SKBitmap inputBitmap)
    {
        var colorMatrix = new float[]
        {
            0.95f, 0.05f, 0.0f, 0.0f, 0.0f, // Red
            0.0f,  0.433f, 0.567f, 0.0f, 0.0f, // Green
            0.0f,  0.475f, 0.525f, 0.0f, 0.0f, // Blue
            0.0f,  0.0f,   0.0f,   1.0f, 0.0f, // Alpha
        };
        ApplyWithMatrix(inputBitmap, colorMatrix);
    }
    public override string FilterName => "Tritanopia";
}
