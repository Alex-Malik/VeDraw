using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeDraw.Models
{
    public class EllipticalArc : Token
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EllipticalArc"/> class.
        /// </summary>
        public EllipticalArc()
        {
            X1 = 0.0;
            Y1 = 0.0;
            Angle = 0.0;
            IsLargeArcFlag = false;
            SweepDirectionFlag = false;
            X2 = 0.0;
            Y2 = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EllipticalArc"/> class 
        /// with specified coordinates of the start point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rotationAngle"></param>
        /// <param name="isLargeArcFlag"></param>
        /// <param name="sweepDirectionFlag"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public EllipticalArc(double x, double y, double rotationAngle, bool isLargeArcFlag, bool sweepDirectionFlag, double x2, double y2)
        {
            X1 = x;
            Y1 = y;
            Angle = rotationAngle;
            IsLargeArcFlag = isLargeArcFlag;
            SweepDirectionFlag = sweepDirectionFlag;
            X2 = x2;
            Y2 = y2;
        }

        /// <summary>
        /// Gets or sets the X-radius of the arc.
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the Y-radius of the arc.
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the ellipse, in degrees.
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the flag, which determines whether 
        /// the angle of the arc should be 180 degrees or greater.
        /// </summary>
        public bool IsLargeArcFlag { get; set; }

        /// <summary>
        /// Gets or sets the flag, which determines whether 
        /// the arc is drawn in a positive-angle direction.
        /// </summary>
        public bool SweepDirectionFlag { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of the point to which the arc is drawn.
        /// </summary>
        public double X2 { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the point to which the arc is drawn.
        /// </summary>
        public double Y2 { get; set; }

        public static EllipticalArc Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup, 
                @"(A|a)\s*(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)" +    // Lexeme + space + first control point.
                @"\s*" + @"(\d+\.\d+|\d+)" +                        // Space + rotation angle.
                @"\s*" + @"(0|1)" + @"\s*" + @"(0|1)" +             // Space + flag + space + flag.
                @"\s*" + @"(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)");   // Space + end point.
            var x1 = Convert.ToDouble(match.Groups[2].Value);
            var y1 = Convert.ToDouble(match.Groups[4].Value);
            var a = Convert.ToDouble(match.Groups[5].Value);
            var f1 = Convert.ToBoolean(match.Groups[6].Value);
            var f2 = Convert.ToBoolean(match.Groups[7].Value);
            var x2 = Convert.ToDouble(match.Groups[8].Value);
            var y2 = Convert.ToDouble(match.Groups[10].Value);

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new EllipticalArc(x1, y1, a, f1, f2, x2, y2);
        }

        public override string ToString() => $"A {X1} {Y1} {Angle} {(IsLargeArcFlag ? 1 : 0)} {(SweepDirectionFlag ? 1 : 0)} {X2} {Y2}";
    }
}
