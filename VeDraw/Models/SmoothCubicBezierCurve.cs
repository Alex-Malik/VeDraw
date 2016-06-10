using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeDraw.Models
{
    public class SmoothCubicBezierCurve : Token
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SmoothCubicBezierCurve"/> class.
        /// </summary>
        public SmoothCubicBezierCurve()
        {
            X1 = 0.0;
            Y1 = 0.0;
            X2 = 0.0;
            Y2 = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SmoothCubicBezierCurve"/> class 
        /// with specified coordinates of the end points.
        /// </summary>
        /// <param name="x1">The X coordinate of the control point.</param>
        /// <param name="y1">The Y coordinate of the control point.</param>
        /// <param name="x2">The X coordinate of the point to which the curve is drawn.</param>
        /// <param name="y2">The Y coordinate of the point to which the curve is drawn.</param>
        public SmoothCubicBezierCurve(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        /// <summary>
        /// Gets or sets the X coordinate of the control point of the curve, 
        /// which determines the ending tangent of the curve.
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the control point of the curve, 
        /// which determines the ending tangent of the curve.
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the point to which the curve is drawn.
        /// </summary>
        public double X2 { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the point to which the curve is drawn.
        /// </summary>
        public double Y2 { get; set; }

        public static SmoothCubicBezierCurve Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup,
                @"(S|s)\s*" + @"(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)" +  // Lexeme + space + first control point.
                @"\s*" + @"(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)");       // Space + end point.
            var x1 = Convert.ToDouble(match.Groups[2].Value);
            var y1 = Convert.ToDouble(match.Groups[4].Value);
            var x2 = Convert.ToDouble(match.Groups[5].Value);
            var y2 = Convert.ToDouble(match.Groups[7].Value);

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new SmoothCubicBezierCurve(x1, y1, x2, y2);
        }

        public override string ToString() => $"S {X1} {Y1} {X2} {Y2}";
    }
}
