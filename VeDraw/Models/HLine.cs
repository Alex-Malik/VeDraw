using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeDraw.Models
{
    /// <summary>
    /// Specifies a horizontal line between the current point and the specified x-coordinate.
    /// </summary>
    public class HLine : Token
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HLine"/> class.
        /// </summary>
        public HLine()
        {
            X = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HLine"/> class 
        /// with specified coordinates of the end point.
        /// </summary>
        /// <param name="x">The X coordinate of the line end point.</param>
        public HLine(double x)
        {
            X = x;
        }

        /// <summary>
        /// Gets or sets the X coordinate of the end point of the line.
        /// </summary>
        public double X { get; set; }


        public static HLine Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup, @"(H|h)\s*(\d+\.\d+|\d+)");
            var x = Convert.ToDouble(match.Groups[2].Value);

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new HLine(x);
        }

        public override string ToString() => $"H {X}";
    }
}
