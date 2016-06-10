using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeDraw.Models
{
    public class Close : Token
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Close"/> class.
        /// </summary>
        public Close()
        {
        }

        public static Close Parse(string markup, out int s, out int e)
        {
            // Let Regex get the X and the Y values.
            var match = Regex.Match(markup, @"(Z|z)");

            // Get token start and end point.
            s = match.Index;
            e = s + match.Length - 1;

            return new Close();
        }

        public override string ToString() => $"Z";
    }
}
