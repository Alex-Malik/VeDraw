using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeDraw.Models
{
    public class CubicBezierCurve : Token, INotifyPropertyChanged
    {
        private double x1, y1, x2, y2, x3, y3;

        /// <summary>
        /// Initializes a new instance of <see cref="CubicBezierCurve"/> class.
        /// </summary>
        public CubicBezierCurve()
        {
            x1 = 0.0;
            y1 = 0.0;
            x2 = 0.0;
            y2 = 0.0;
            x3 = 0.0;
            y3 = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CubicBezierCurve"/> class 
        /// with specified coordinates of the end points.
        /// </summary>
        /// <param name="x1">The X coordinate of the first control point.</param>
        /// <param name="y1">The Y coordinate of the first control point.</param>
        /// <param name="x2">The X coordinate of the second control point.</param>
        /// <param name="y2">The Y coordinate of the second control point.</param>
        /// <param name="x3">The X coordinate of the point to which the curve is drawn.</param>
        /// <param name="y3">The Y coordinate of the point to which the curve is drawn.</param>
        public CubicBezierCurve(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.x3 = x3;
            this.y3 = y3;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the X coordinate of the first control point 
        /// of the curve, which determines the starting tangent of the curve.
        /// </summary>
        public double X1
        {
            get { return x1; }
            set
            {
                if (x1 != value)
                {
                    x1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X1)));
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the first control point 
        /// of the curve, which determines the starting tangent of the curve.
        /// </summary>
        public double Y1
        {
            get { return y1; }
            set
            {
                if (y1 != value)
                {
                    y1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y1)));
                }
            }
        }

        /// <summary>
        /// Gets or sets the X coordinate of the second control point 
        /// of the curve, which determines the ending tangent of the curve.
        /// </summary>
        public double X2
        {
            get { return x2; }
            set
            {
                if (x2 != value)
                {
                    x2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X2)));
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the second control point 
        /// of the curve, which determines the ending tangent of the curve.
        /// </summary>
        public double Y2
        {
            get { return y2; }
            set
            {
                if (y2 != value)
                {
                    y2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y2)));
                }
            }
        }

        /// <summary>
        /// Gets or sets the X coordinate of the point to which the curve is drawn.
        /// </summary>
        public double X3
        {
            get { return x3; }
            set
            {
                if (x3 != value)
                {
                    x3 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X3)));
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the point to which the curve is drawn.
        /// </summary>
        public double Y3
        {
            get { return y3; }
            set
            {
                if (y3 != value)
                {
                    y3 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y3)));
                }
            }
        }

        public static CubicBezierCurve Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup, 
                @"(C|c)\s*" + @"(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)" +  // Lexeme + space + first control point.
                @"\s*" + @"(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)" +       // Space + second control point.
                @"\s*" + @"(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)");       // Space + end point.
            var x1 = Convert.ToDouble(match.Groups[2].Value);
            var y1 = Convert.ToDouble(match.Groups[4].Value);
            var x2 = Convert.ToDouble(match.Groups[5].Value);
            var y2 = Convert.ToDouble(match.Groups[7].Value);
            var x3 = Convert.ToDouble(match.Groups[8].Value);
            var y3 = Convert.ToDouble(match.Groups[10].Value);

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new CubicBezierCurve(x1, y1, x2, y2, x3, y3);
        }

        public override string ToString() => $"C {X1} {Y1} {X2} {Y2} {X3} {Y3}";
    }
}
