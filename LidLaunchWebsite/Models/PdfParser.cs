using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class PdfParser
    {

        public const string ES65Format = "Wilcom ES-65";
        public const string WilcomDesigning = "Wilcom EmbroideryStudio – Designing";

        public int StitchCount { get; set; }
        public int TotalColors { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Thread> ColorWays { get; set; }
    }
}