using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VeDraw.Models;

namespace VeDraw.Utils
{
    public class VDTreeViewDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (item is VDFigure)
            {
                return Application.Current.Resources["DataTemplate.TreeView.Figure"] as DataTemplate;
            }
            else if (item is Move)
            {
                return Application.Current.Resources["DataTemplate.TreeView.Move"] as DataTemplate;
            }
            else if (item is Line)
            {
                return Application.Current.Resources["DataTemplate.TreeView.Line"] as DataTemplate;
            }
            else if (item is CubicBezierCurve)
            {
                return Application.Current.Resources["DataTemplate.TreeView.CubicBezierCurve"] as DataTemplate;
            }
            else if (item is QuadraticBezierCurve)
            {
                return Application.Current.Resources["DataTemplate.TreeView.QuadraticBezierCurve"] as DataTemplate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
