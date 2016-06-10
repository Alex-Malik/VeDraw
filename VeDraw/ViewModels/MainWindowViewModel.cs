using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VeDraw.ViewModels
{
    using Models;
    using Utils;
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Public

        public MainWindowViewModel()
        {
            // Initialize observable (for binding) collection of figures
            Figures = new ObservableCollection<VDFigure>();

            // Initialize commands
            OpenCommand = new Command(Open, CanOpen);
            SaveCommand = new Command(Save, CanSave);
            CreateFigureCommand = new Command(CreateFigure, CanCreateFigure);
            DeleteCommand = new Command(Delete, CanDelete);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<VDFigure> Figures { get; }

        /// <summary>
        /// Gets or sets the selected item in the project tree view. The
        /// item can be of the <see cref="Token"/> type or <see cref="VDFigure"/> type.
        /// </summary>
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));

                    // Update corresponding property
                    if (selectedItem is VDFigure)
                        SelectedFigure = selectedItem as VDFigure;
                    else if (selectedItem is Token)
                        SelectedToken = selectedItem as Token;

                    // Update DeleteCommand to enable/disable it
                    DeleteCommand.Update();
                }
            }
        }

        public VDFigure SelectedFigure
        {
            get { return selectedFigure; }
            set
            {
                if (selectedFigure != value)
                {
                    selectedFigure = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFigure)));

                    // Update DeleteCommand to enable/disable it
                    DeleteCommand.Update();
                }
            }
        }

        public Token SelectedToken
        {
            get { return selectedToken; }
            set
            {
                if (selectedToken != value)
                {
                    selectedToken = value;

                    // Select Figure which is parent for this token
                    SelectedFigure = Figures.First(f => f.Tokens.Contains(selectedToken));

                    // Update binded properties (note: this action should be done after the figure selection) 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedToken)));

                    // Update DeleteCommand to enable/disable it
                    DeleteCommand.Update();
                }
            }
        }

        public Command CreateFigureCommand { get; }
        
        public Command DeleteCommand { get; }

        public Command OpenCommand { get; }

        public Command SaveCommand { get; }

        #endregion

        #region Private

        private object selectedItem = null;
        private VDFigure selectedFigure = null;
        private Token selectedToken = null;

        private void CreateFigure()
        {
            Figures.Add(new VDFigure());
        }

        private bool CanCreateFigure()
        {
            return Figures != null;
        }

        private void Delete()
        {
            if (SelectedItem is VDFigure)
            {
                Figures.Remove(SelectedFigure);
            }
            else if (SelectedItem is Token)
            {
                Figures.First(f => f.Tokens.Contains(SelectedToken)).Tokens.Remove(SelectedToken);
            }

            // If there is no more figures in collection
            // then we save command should be disabled
            SaveCommand.Update();
        }

        private bool CanDelete()
        {
            return SelectedItem != null;
        }

        private void Open()
        {
            var figures = XamlReader.ReadFigures(OpenFile());
            if (figures != null)
            {
                if (Figures.Any()) Figures.Clear();

                foreach (var figure in figures)
                {
                    Figures.Add(figure);
                }
            }
        }

        private bool CanOpen()
        {
            return true;
        }

        private void Save()
        {
            XamlWriter.WriteFigures(OpenFile(true), Figures);
        }

        private bool CanSave()
        {
            return Figures?.Any() ?? false;
        }


        private string OpenFile(bool allowCreateFile = false)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = 
                "XAML files (*.xaml)|*.xaml|" +
                "XML files (*.xml)|*.xml|" +
                "All files (*.*)|*.*";
            openFileDialog.CheckFileExists = !allowCreateFile;
            openFileDialog.CheckPathExists = !allowCreateFile;
            openFileDialog.ValidateNames = !allowCreateFile;

            if (openFileDialog.ShowDialog() ?? false)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
