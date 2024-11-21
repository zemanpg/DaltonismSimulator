using RefCounted;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaltonismSimulator
{
    public class RefCntSkBitmap : Reference<SKBitmap>
    {
        public RefCntSkBitmap(SKBitmap instance, string name) : base(instance, name, "") { }
    }

}
