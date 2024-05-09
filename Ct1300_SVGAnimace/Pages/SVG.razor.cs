using Ct1300_SVGAnimace.Models;
using Newtonsoft.Json;
using System.Drawing;

namespace Ct1300_SVGAnimace.Pages
{
    public partial class SVG
    {
        public SVG() 
        { 
            //nastaveni velikosti kontejneru
            SvgWidth = 1600;
            SvgHeight = 800;
        }
        protected override async Task OnInitializedAsync()
        {
            await NacistJson();
            await base.OnInitializedAsync();
        }
        public int SvgWidth { get; set; }
        public int SvgHeight { get; set;}
        public int MinRozmer { get; set; } = 20;
        public int MaxRozmer { get; set; } = 80;
        public int KrokAnimace { get; set; } = 1;
        public List<Models.Obrazec> SvgObrazceList { get; set; } = new List<Models.Obrazec>();

        private Random rnd = new Random();
        private int TypObrazceCount => Enum.GetNames(typeof(TypObrazce)).Length;
        private void PridejObrazec(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            TypObrazce typObrazce = (TypObrazce)rnd.Next(TypObrazceCount);
            Color color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            
            int stranaA = rnd.Next(MinRozmer,MaxRozmer + 1);
            int pozX = rnd.Next(SvgWidth - stranaA + 1);
            Obrazec? obr = null;
            switch (typObrazce)
            {
                case TypObrazce.ctverec:
                    obr = new Models.Ctverec(typObrazce, color, pozX, rnd.Next(SvgHeight - stranaA + 1), stranaA);
                    break;
                case TypObrazce.obdelnik:
                    int stranaB = rnd.Next(MinRozmer, MaxRozmer + 1);
                    obr = new Models.Obdelnik(typObrazce, color, pozX, rnd.Next(SvgHeight - stranaB + 1), stranaA,stranaB);
                    break;
                case TypObrazce.kruh:
                    obr = new Models.Kruh(typObrazce, color, pozX, rnd.Next(SvgHeight - stranaA + 1), stranaA);
                    break;
            }
            if (obr != null)
                SvgObrazceList.Add(obr);
        }

        private void OdebratObrazec()
        {
            if (SvgObrazceList.Any())
                SvgObrazceList.RemoveAt(SvgObrazceList.Count - 1);
        }
        private async Task Smazat(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            await localStorage.RemoveItemAsync("data");
        }
        private async Task Ulozit(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            string json = JsonConvert.SerializeObject(SvgObrazceList, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto});

            await localStorage.SetItemAsync("data", json);
        }
        private async Task NacistJson()
        {
            var json = await localStorage.GetItemAsync<string>("data");
            if (json != null)
            {
                List<Models.Obrazec>? products = JsonConvert.DeserializeObject<List<Models.Obrazec>>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                if (products != null)
                    SvgObrazceList.AddRange(products);
            }
        }
        private async Task PosunSpustit()
        {
            do 
            {
                foreach (var item in SvgObrazceList)
                {
                    item.PosunObjekt(KrokAnimace, SvgWidth, SvgHeight);
                }
                StateHasChanged();
                await Task.Delay(10);
            } while (true);
        }
    }
}
