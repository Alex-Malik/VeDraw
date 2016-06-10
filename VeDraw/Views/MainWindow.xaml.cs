using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VeDraw
{
    using Models;
    using Controls;
    using ViewModels;
    using System.Windows.Controls.Primitives;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel Model;
        private readonly ToggleButton[] DrawingCommandButtons;

        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = Model = new MainWindowViewModel();
            DrawingCommandButtons = new []{ dcb1, dcb2, dcb3 };
        }

        private void OnClickAddLineButton(object sender, RoutedEventArgs e)
        {
            ToggleOffDrawingCommandButtons();
            ToggleOnDrawingCommandButton((ToggleButton)sender);
            vdc.CreateCommand = VDCanvasCommand.CreateLine;
        }

        private void OnClickAddCubicCurveButton(object sender, RoutedEventArgs e)
        {
            ToggleOffDrawingCommandButtons();
            ToggleOnDrawingCommandButton((ToggleButton)sender);
            vdc.CreateCommand = VDCanvasCommand.CreateCubicCurve;
        }

        private void OnClickAddQuadraticCurveButton(object sender, RoutedEventArgs e)
        {
            ToggleOffDrawingCommandButtons();
            ToggleOnDrawingCommandButton((ToggleButton)sender);
            vdc.CreateCommand = VDCanvasCommand.CreateQuadraticCurve;
        }
        
        private void OnSelectedItemChangedTreeView(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tv = sender as TreeView;
            if (tv != null)
            {
                // Ignore selection of the Move token
                if (tv.SelectedItem is Move) return;

                // Update selected item in the model
                // This property will update SelectedFigure and SelectedToken
                // so we don't need to update it manualy here
                Model.SelectedItem = tv.SelectedItem;
            }
        }

        private void OnVDCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            ToggleOffDrawingCommandButtons();
        }

        private void ToggleOffDrawingCommandButtons()
        {
            foreach (var button in DrawingCommandButtons)
            {
                button.IsChecked = false;
            }
        }

        private void ToggleOnDrawingCommandButton(ToggleButton button)
        {
            button.IsChecked = true;
        }
    }
}
