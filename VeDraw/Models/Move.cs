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
    /// Specifies the start point of a new figure. 
    /// NOTE: If a figure was previously started with a move command, 
    /// but not closed with a close command, the open figure will be
    /// ended (not closed) when a new move command is issued.
    /// </summary>
    public class Move : Token, INotifyPropertyChanged
    {
        private double x, y;

        /// <summary>
        /// Initializes a new instance of <see cref="Move"/> class.
        /// </summary>
        public Move()
        {
            x = 0.0;
            y = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Move"/> class 
        /// with specified coordinates of the start point.
        /// </summary>
        /// <param name="x">The X coordinate of the figure start point.</param>
        /// <param name="y">The Y coordinate of the figure start point.</param>
        public Move(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the X coordinate of the start point of a figure.
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
        /// Gets or sets the Y coordinate of the start point of a figure.
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

        public static Move Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup, @"(M|m)\s*(\d+\.\d+|\d+)(\s|\,)(\d+\.\d+|\d+)");
            var x = Convert.ToDouble(match.Groups[2].Value);
            var y = Convert.ToDouble(match.Groups[4].Value);

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new Move(x, y);
        }

        public override string ToString() => $"M {X} {Y}";
    }
}
