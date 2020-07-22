using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class AviunUtils
    {
        public static object Get { get; private set; }

        public struct BlockReferencePlacement
        {
            public Point2d xy;
        }
        public static (string diameter,string spacing) CountChar(string full)
        {
            int count = 0;
            int countC = 0;
            int countA = 0;
            foreach(char c in full)
            {
                count++;
                if (c == 'c') { countC = count; }
                if(c=='a') { countA = count; }
            }
            string diameter = full.Substring(countC, countA - countC-1);
            string spacing = full.Substring(countA);
            return (diameter, spacing);
        }

        public static string GetDiameter(string full)
        {
            string diameter = CountChar(full).diameter;
            return diameter;
        }
        public static string GetSpacing(string full)
        {
            string spacing = CountChar(full).spacing;
            return spacing;
        }

    }
}
