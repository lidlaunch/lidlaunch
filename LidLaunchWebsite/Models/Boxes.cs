using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public static class Boxes
    {
        public static class BOX8x8x6
        {
            public static string Name = "8x8x6";
            public static int MinimumHats = 1;
            public static int MaxHats = 4;
            public static decimal Cost = 0.49M;
        }

        public static class BOX10x8x6
        {
            public static string Name = "10x8x6";
            public static int MinimumHats = 5;
            public static int MaxHats = 6;
            public static decimal Cost = 0.46M;
        }

        public static class BOX12x8x6
        {
            public static string Name = "12x8x6";
            public static int MinimumHats = 7;
            public static int MaxHats = 10;
            public static decimal Cost = 0.48M;
        }

        public static class BOX16x8x6
        {
            public static string Name = "16x8x6";
            public static int MinimumHats = 11;
            public static int MaxHats = 16;
            public static decimal Cost = 0.60M;
        }

        //we reuse these from the manufacturer
        public static class BOX24x8x6
        {
            public static string Name = "24x8x6";
            public static int MinimumHats = 17;
            public static int MaxHats = 30;
            public static decimal Cost = 0.00M;
        }

        //we reuse these from the manufacturer
        public static class BOX24x8x12
        {
            public static string Name = "24x8x12";
            public static int MinimumHats = 40;
            public static int MaxHats = 60;
            public static decimal Cost = 0.00M;
        }

        //we reuse these from the manufacturer
        public static class BOXExtraLarge
        {
            public static string Name = "EXTRA LARGE";
            public static int MinimumHats = 100;
            public static int MaxHats = 144;
            public static decimal Cost = 0.00M;
        }

    }
}