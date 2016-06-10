using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeDraw.Models
{
    /// <summary>
    /// Specifies a straight line between the current point and the specified end point.
    /// </summary>
    public class Line : Token, INotifyPropertyChanged
    {
        private double x, y;

        /// <summary>
        /// Initializes a new instance of <see cref="Line"/> class.
        /// </summary>
        public Line()
        {
            x = 0.0;
            y = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Line"/> class 
        /// with specified coordinates of the end point.
        /// </summary>
        /// <param name="x">The X coordinate of the line end point.</param>
        /// <param name="y">The Y coordinate of the line end point.</param>
        public Line(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets or sets the X coordinate of the end point of the line.
        /// </summary>
        public double X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X)));
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the end point of the line.
        /// </summary>
        public double Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Line Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup, @"(L|l)\s*(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)");
            var x = Convert.ToDouble(match.Groups[2].Value);
            var y = Convert.ToDouble(match.Groups[4].Value);

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new Line(x, y);
        }

        public override string ToString() => $"L {X} {Y}";
    }
}
