using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VeDraw.Utils
{
    using System.Text.RegularExpressions;
    using Models;

    public class XamlReader
    {
        public static IEnumerable<VDFigure> ReadFigures(string fileName)
        {
            // User should specify file name anyway
            if (String.IsNullOrEmpty(fileName)) return null;
            try
            {
                // Exit if file not exists
                if (!File.Exists(fileName)) return null;

                // Read all markup from file by lines
                string[] lines = File.ReadAllLines(fileName);
                List<VDFigure> figures = new List<VDFigure>();

                foreach (var line in lines)
                {
                    if (String.IsNullOrWhiteSpace(line)) continue;
                    try
                    {
                        var matchName = Regex.Match(line, @"x:Name=""([^""]*)""");
                        var matchFillColor = Regex.Match(line, @"Fill=""(\S{6})""");
                        var matchStrokeColor = Regex.Match(line, @"Stroke=""(\S{6})""");
                        var matchStrokeThickness = Regex.Match(line, @"StrokeThickness=""(\d+\.\d+|\d+)""");
                        var matchData = Regex.Match(line, @"Data=""([^""]*)""");

                        var figure = new VDFigure
                        {
                            Name = !String.IsNullOrEmpty(matchName.Groups[1].Value) ? matchName.Groups[1].Value : $"Figure {lines.ToList().IndexOf(line)}",
                            FillColor = !String.IsNullOrEmpty(matchFillColor.Groups[1].Value) ? matchName.Groups[1].Value : "#00FFFFFF",
                            StrokeColor = !String.IsNullOrEmpty(matchStrokeColor.Groups[1].Value) ? matchName.Groups[1].Value : "#FF000000",
                            StrokeThickness = !String.IsNullOrEmpty(matchStrokeThickness.Groups[1].Value) ? Double.Parse(matchStrokeThickness.Groups[1].Value) : 1.0,
                            Markup = !String.IsNullOrEmpty(matchData.Groups[1].Value) ? matchData.Groups[1].Value : "",
                        };
                        figures.Add(figure);
                    }
                    catch
                    {
                        continue;
                    }
                }

                return figures;
            }
            catch (Exception e)
            {
                // TODO: Log exception
                throw;
            }
        }
    }
}
