using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeDraw.Models
{
    public class QuadraticBezierCurve : Token, INotifyPropertyChanged
    {
        private double x1, y1, x2, y2;

        /// <summary>
        /// Initializes a new instance of <see cref="QuadraticBezierCurve"/> class.
        /// </summary>
        public QuadraticBezierCurve()
        {
            x1 = 0.0;
            y1 = 0.0;
            x2 = 0.0;
            y2 = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="QuadraticBezierCurve"/> class 
        /// with specified coordinates of the end points.
        /// </summary>
        /// <param name="x1">The X coordinate of the control point.</param>
        /// <param name="y1">The Y coordinate of the control point.</param>
        /// <param name="x2">The X coordinate of the point to which the curve is drawn.</param>
        /// <param name="y2">The Y coordinate of the point to which the curve is drawn.</param>
        public QuadraticBezierCurve(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the X coordinate of the control point of the curve, 
        /// which determines the starting and ending tangents of the curve.
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
        /// Gets or sets the Y coordinate of the control point of the curve, 
        /// which determines the starting and ending tangents of the curve.
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
        /// Gets or sets the X coordinate of the point to which the curve is drawn.
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
        /// Gets or sets the Y coordinate of the point to which the curve is drawn.
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

        public static QuadraticBezierCurve Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup,
                @"(Q|q)\s*" + @"(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)" +  // Lexeme + space + first control point.
                @"\s*" + @"(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)");       // Space + end point.
            var x1 = Convert.ToDouble(match.Groups[2].Value);
            var y1 = Convert.ToDouble(match.Groups[4].Value);
            var x2 = Convert.ToDouble(match.Groups[5].Value);
            var y2 = Convert.ToDouble(match.Groups[7].Value);

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new QuadraticBezierCurve(x1, y1, x2, y2);
        }

        public override string ToString() => $"Q {X1} {Y1} {X2} {Y2}";
    }
}
