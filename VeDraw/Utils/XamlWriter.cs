using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VeDraw.Utils
{
    using Models;

    public class XamlWriter
    {
        public static void WriteFigures(string fileName, IEnumerable<VDFigure> figures)
        {
            // User should specify file name anyway
            if (String.IsNullOrEmpty(fileName)) return;
            try
            {
                // Create file if not exists
                if (!File.Exists(fileName))
                {
                    var stream = File.Create(fileName);
                    stream.Close();
                    stream.Dispose();
                }

                // Here will be the generated markup
                string content = String.Empty;

                foreach (var figure in figures)
                {
                    // Generate Path markup from given figure
                    content += $"<Path x:Name=\"{figure.Name}\" "
                        + $"Fill=\"{figure.FillColor}\""
                        + $"Stroke=\"{figure.StrokeColor}\""
                        + $"StrokeThickness=\"{figure.StrokeThickness}\""
                        + $"Data=\"{figure.Markup}\""
                        + " />\n\r";
                }

                // Remove new line symbols from the end
                content.TrimEnd('\n', '\r');

                File.WriteAllText(fileName, content);
            }
            catch (Exception e)
            {
                // TODO: Log exception
                throw;
            }
        }
    }
}
