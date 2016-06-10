using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeDraw.Models
{
    /// <summary>
    /// Specifies a vertical line between the current point and the specified y-coordinate.
    /// </summary>
    public class VLine : Token
    {
        /// <summary>
        /// Initializes a new instance of <see cref="VLine"/> class.
        /// </summary>
        public VLine()
        {
            Y = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="VLine"/> class 
        /// with specified coordinates of the end point.
        /// </summary>
        /// <param name="y">The X coordinate of the line end point.</param>
        public VLine(double y)
        {
            Y = y;
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the end point of the line.
        /// </summary>
        public double Y { get; set; }


        public static VLine Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup, @"(V|v)\s*(\d+\.\d+|\d+)");
            var y = Convert.ToDouble(match.Groups[2].Value);

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new VLine(y);
        }

        public override string ToString() => $"V {Y}";
    }
}
