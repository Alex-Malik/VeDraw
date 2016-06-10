using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace VeDraw.Models
{
    public class VDFigure : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Token> tokens;
        private string name = "New Figure";
        private string fillColor = "#00FFFFFF";
        private string strokeColor = "#FF000000";
        private double strokeThickness = 1.0;

        public VDFigure()
        {
            tokens = new ObservableCollection<Token>();
            tokens.CollectionChanged += OnCollectionChanged;
        }

        public VDFigure(string markup) : this()
        {
            Parse(markup);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Token> Tokens
        {
            get { return tokens; }
        }

        public string Markup
        {
            get
            {
                if (Tokens.Any())
                    return Tokens.Select(x => x.ToString()).Aggregate((x, y) => x + " " + y);
                else
                    return String.Empty;
            }
            set
            {
                if (Markup != value)
                {
                    Parse(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Markup)));
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public string FillColor
        {
            get { return fillColor; }
            set
            {
                if (fillColor != value)
                {
                    fillColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FillColor)));
                }
            }
        }

        public string StrokeColor
        {
            get { return strokeColor; }
            set
            {
                if (strokeColor != value)
                {
                    strokeColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeColor)));
                }
            }
        }

        public double StrokeThickness
        {
            get { return strokeThickness; }
            set
            {
                if (strokeThickness != value)
                {
                    strokeThickness = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeThickness)));
                }
            }
        }


        public void Parse(string markup)
        {
            // Prepare markup for parsing.
            // Replace multiply spaces and commas with a single space.
            markup = Regex.Replace(markup, "[ ]{2,}", " ", RegexOptions.None);
            markup = Regex.Replace(markup, "[,]", " ", RegexOptions.None);
            markup = markup.Trim();

            // We will change current tokens collection only 
            // after the parsing process finishes successful.
            var newTokens = new List<Token>();

            // Markup is ready for parsing. Now we can 
            // analyse the syntax and get tokens.
            try
            {
                for (int i = 0; i < markup.Length; i++)
                {
                    // Parse markup and create token. Also we can get
                    // start (s) and end (e) points of the token in given markup.
                    char symbol = markup[i];
                    int s = 0, e = 0;

                    if (symbol == 'M' || symbol == 'm')
                    {
                        // Create token and save it to the tokens collection.
                        newTokens.Add(Move.Parse(markup.Substring(i), out s, out e));
                    }
                    else if (symbol == 'L' || symbol == 'l')
                    {
                        newTokens.Add(Line.Parse(markup.Substring(i), out s, out e));
                        i += e;
                    }
                    else if (symbol == 'H' || symbol == 'h')
                    {
                        newTokens.Add(HLine.Parse(markup.Substring(i), out s, out e));
                        i += e;
                    }
                    else if (symbol == 'V' || symbol == 'v')
                    {
                        newTokens.Add(VLine.Parse(markup.Substring(i), out s, out e));
                    }
                    else if (symbol == 'C' || symbol == 'c')
                    {
                        newTokens.Add(CubicBezierCurve.Parse(markup.Substring(i), out s, out e));
                    }
                    else if (symbol == 'Q' || symbol == 'q')
                    {
                        newTokens.Add(QuadraticBezierCurve.Parse(markup.Substring(i), out s, out e));
                    }
                    else if (symbol == 'S' || symbol == 's')
                    {
                        newTokens.Add(SmoothCubicBezierCurve.Parse(markup.Substring(i), out s, out e));
                    }
                    else if (symbol == 'T' || symbol == 't')
                    {
                        newTokens.Add(SmoothQuadraticBezierCurve.Parse(markup.Substring(i), out s, out e));
                    }
                    else if (symbol == 'A' || symbol == 'a')
                    {
                        newTokens.Add(EllipticalArc.Parse(markup.Substring(i), out s, out e));
                    }
                    else if (symbol == 'Z' || symbol == 'z')
                    {
                        newTokens.Add(Close.Parse(markup.Substring(i), out s, out e));
                    }
                    else if (symbol == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        // TODO: Throw an exception or log error and finish parsing.
                    }

                    // Continue loop from the end point of the token.
                    i += e;
                }

                // Parsing process finshed successful so we can update tokens collection.
                if (Tokens.Any()) Tokens.Clear();

                foreach (var token in newTokens)
                {
                    Tokens.Add(token);
                }
            }
            catch (Exception e)
            {
                // TODO: Log exception.
                // Left collection not changed.
            }
        }


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        (item as INotifyPropertyChanged).PropertyChanged += OnTokenPropertyChanged;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        (item as INotifyPropertyChanged).PropertyChanged -= OnTokenPropertyChanged;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        (item as INotifyPropertyChanged).PropertyChanged -= OnTokenPropertyChanged;
                    }
                }
                foreach (var item in e.NewItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        (item as INotifyPropertyChanged).PropertyChanged += OnTokenPropertyChanged;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {

            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        (item as INotifyPropertyChanged).PropertyChanged -= OnTokenPropertyChanged;
                    }
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Markup)));
        }

        private void OnTokenPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Markup)));
        }
    }
}
