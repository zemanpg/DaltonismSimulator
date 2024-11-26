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
           // I removed matrix because I'm not sure if they are patent protected      
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
           // I removed matrix because I'm not sure if they are patent protected      
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
           // I removed matrix because I'm not sure if they are patent protected      
        };
        ApplyWithMatrix(inputBitmap, colorMatrix);
    }
    public override string FilterName => "Tritanopia";
}
