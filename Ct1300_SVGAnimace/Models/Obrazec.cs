using System.Drawing;

namespace Ct1300_SVGAnimace.Models
{
    public abstract class Obrazec
    {
        public Obrazec(TypObrazce typObrazce, Color barva, int pozX, int pozY) 
        { 
            TypObrazce = typObrazce;
            Barva = barva;
            PozX = pozX;
            PozY = pozY;
        }
        public TypObrazce TypObrazce { get; set; }
        public int PozX { get; set; }
        public int PozY { get; set;}
        public Color Barva { get; set; }

        public void PosunObjekt(int krok, int svgWidth, int svgHeight)
        {
            PozX += krok;
            PozY += krok;
            if (PozX > svgWidth) { PozX = 0; }
            if (PozY > svgHeight) { PozY = 0; }
        }
    }
}
