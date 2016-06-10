using System;
using System.Collections.Generic;
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

namespace VeDraw.Controls
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using Models;
    using Utils;
    public class VDCanvas : Panel
    {
        #region Static

        static VDCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VDCanvas),
                new FrameworkPropertyMetadata(typeof(VDCanvas)));
        }

        public static readonly DependencyProperty CreateCommandProperty =
            DependencyProperty.Register(nameof(CreateCommand), typeof(VDCanvasCommand), typeof(VDCanvas), new PropertyMetadata(VDCanvasCommand.None, OnVDCanvasCommandChanged));
        public static readonly DependencyProperty FiguresProperty =
            DependencyProperty.Register(nameof(Figures), typeof(ObservableCollection<VDFigure>), typeof(VDCanvas), new PropertyMetadata(null, OnFiguresChanged));
        public static readonly DependencyProperty CurrentFigureProperty =
            DependencyProperty.Register(nameof(CurrentFigure), typeof(VDFigure), typeof(VDCanvas), new PropertyMetadata(null, OnCurrentFigureChanged));
        public static readonly DependencyProperty CurrentTokenProperty =
            DependencyProperty.Register(nameof(CurrentToken), typeof(Token), typeof(VDCanvas), new PropertyMetadata(null, OnCurrentTokenChanged));

        public static readonly DependencyProperty StartWithMoveProperty =
            DependencyProperty.Register(nameof(StartWithMove), typeof(bool), typeof(VDCanvas), new PropertyMetadata(false));

        private static void OnVDCanvasCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vDCanvas = d as VDCanvas;
            if (vDCanvas != null)
            {
                if ((VDCanvasCommand)e.NewValue != VDCanvasCommand.None)
                {
                    vDCanvas.GoToCreateMode();
                }
            }
        }

        private static void OnFiguresChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue) return;

            var vDCanvas = d as VDCanvas;
            if (vDCanvas != null)
            {
                // Clear canvas from all elements
                vDCanvas.Children.Clear();

                var figures = (ObservableCollection<VDFigure>)e.NewValue;
                if (figures != null)
                {
                    // Wrap each existing figure in Path
                    foreach (var figure in figures)
                    {
                        vDCanvas.Children.Add(vDCanvas.CreateFigureVisualization(figure));
                    }

                    // Get notified about collection changes
                    figures.CollectionChanged += vDCanvas.OnFiguresChanged;
                }
            }
        }

        private static void OnCurrentFigureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vDCanvas = d as VDCanvas;
            if (vDCanvas != null)
            {
                if (e.NewValue == e.OldValue) return;
                var figure = (VDFigure)e.NewValue;

                foreach (var child in vDCanvas.Children.OfType<Path>())
                {
                    if (child.DataContext != figure)
                    {
                        child.Opacity = 0.5;
                    }
                    else
                    {
                        child.Opacity = 1.0;
                    }
                }
            }
        }

        private static void OnCurrentTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue) return;

            var vDCanvas = d as VDCanvas;
            if (vDCanvas != null)
            {
                if (e.NewValue as Token == null)
                {
                    vDCanvas.GoToNoneMode();
                }
                else
                {
                    vDCanvas.GoToEditMode();
                }
            }
        }

        #endregion
        
        private VDCanvasMode Mode;

        #region Public

        public VDCanvas()
        {
            Cache = new VDCanvasCache();
            Mode = VDCanvasMode.None;
        }


        public VDCanvasCommand CreateCommand
        {
            get { return (VDCanvasCommand)GetValue(CreateCommandProperty); }
            set { SetValue(CreateCommandProperty, value); }
        }

        public ObservableCollection<VDFigure> Figures
        {
            get { return (ObservableCollection<VDFigure>)GetValue(FiguresProperty); }
            set { SetValue(FiguresProperty, value); }
        }
        
        public VDFigure CurrentFigure
        {
            get { return (VDFigure)GetValue(CurrentFigureProperty); }
            set { SetValue(CurrentFigureProperty, value); }
        }

        public Token CurrentToken
        {
            get { return (Token)GetValue(CurrentTokenProperty); }
            set { SetValue(CurrentTokenProperty, value); }
        }

        public bool StartWithMove
        {
            get { return (bool)GetValue(StartWithMoveProperty); }
            set { SetValue(StartWithMoveProperty, value); }
        }
        
        #endregion

        #region Private

        private VDCanvasCache Cache { get; }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (CurrentFigure == null) return;

            Cache.MouseDownPoint = e.GetPosition(this);

            if (Mode == VDCanvasMode.Create)
            {
                Create();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            VDPointThumb.Bounds = sizeInfo.NewSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement element in InternalChildren)
            {
                element.Measure(availableSize);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement element in InternalChildren)
            {
                if (element is Path)
                {
                    element.Arrange(new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height));
                }
                else if (element is VDPointThumb)
                {
                    var vDPointThumb = (VDPointThumb)element;
                    element.Arrange(new Rect(
                        vDPointThumb.X - vDPointThumb.DesiredSize.Width / 2.0,
                        vDPointThumb.Y - vDPointThumb.DesiredSize.Height / 2.0,
                        vDPointThumb.DesiredSize.Width,
                        vDPointThumb.DesiredSize.Height));
                }
                else
                {
                    // TODO: Do nothing yet
                }
            }

            return finalSize;
        }

        
        private void GoToNoneMode()
        {
            Mode = VDCanvasMode.None;
            CreateCommand = VDCanvasCommand.None;
            RemoveThumbs();
        }

        private void GoToCreateMode()
        {
            Mode = VDCanvasMode.Create;
            RemoveThumbs();
        }

        private void GoToEditMode()
        {
            Mode = VDCanvasMode.Edit;
            CreateCommand = VDCanvasCommand.None;
            RemoveThumbs();

            Edit(CurrentToken);
        }

        private void Create()
        {
            RemoveThumbs();

            // Create Move (M) command if this is the first token
            if (!CurrentFigure.Tokens.Any())
            {
                CurrentFigure.Tokens.Add(
                    new Move(
                        Cache.MouseDownPoint.X - 50.0, 
                        Cache.MouseDownPoint.Y + 50.0));
            }

            if (StartWithMove && !(CurrentFigure.Tokens.Last() is Move))
            {
                CurrentFigure.Tokens.Add(
                    new Move(
                        Cache.MouseDownPoint.X - 50.0,
                        Cache.MouseDownPoint.Y + 50.0));
            }

            // Token which will be created
            Token token = null;

            // Depending on command create corresponding token
            if (CreateCommand == VDCanvasCommand.CreateLine)
            {
                token = new Line(Cache.MouseDownPoint.X + 50.0, Cache.MouseDownPoint.Y - 50.0);
                
            }
            else if (CreateCommand == VDCanvasCommand.CreateCubicCurve)
            {
                token = new CubicBezierCurve(
                    Cache.MouseDownPoint.X - 50.0,
                    Cache.MouseDownPoint.Y,
                    Cache.MouseDownPoint.X + 50.0,
                    Cache.MouseDownPoint.Y,
                    Cache.MouseDownPoint.X + 50.0,
                    Cache.MouseDownPoint.Y - 50.0);
            }
            else if (CreateCommand == VDCanvasCommand.CreateQuadraticCurve)
            {
                token = new QuadraticBezierCurve(
                    Cache.MouseDownPoint.X,
                    Cache.MouseDownPoint.Y,
                    Cache.MouseDownPoint.X + 50.0,
                    Cache.MouseDownPoint.Y - 50.0);
            }
            else if (CreateCommand == VDCanvasCommand.CreateSmoothCubicCurve)
            {
                token = new SmoothCubicBezierCurve(
                    Cache.MouseDownPoint.X,
                    Cache.MouseDownPoint.Y,
                    Cache.MouseDownPoint.X + 50.0,
                    Cache.MouseDownPoint.Y - 50.0);
            }
            else if (CreateCommand == VDCanvasCommand.CreateSmoothQuadraticCurve)
            {
                token = new SmoothQuadraticBezierCurve(
                    Cache.MouseDownPoint.X,
                    Cache.MouseDownPoint.Y,
                    Cache.MouseDownPoint.X + 50.0,
                    Cache.MouseDownPoint.Y - 50.0);
            }

            // Add just created token to the figure
            CurrentFigure.Tokens.Add(token);

            // Depending on Shift key status continue to create elements
            if (Keyboard.GetKeyStates(Key.LeftShift) != KeyStates.Down)
            {
                Edit(token);
            }
            else
            {
                GoToCreateMode();
            }

            InvalidateMeasure();
        }

        private void Edit(Token token)
        {
            Mode = VDCanvasMode.Edit;
            CreateCommand = VDCanvasCommand.None;

            AddThumbForPreviousToken(token);

            if (token is Line)
            {
                AddThumb(token, nameof(Line.X), nameof(Line.Y));
            }
            else if (token is CubicBezierCurve)
            {
                AddThumb(token, nameof(CubicBezierCurve.X1), nameof(CubicBezierCurve.Y1), notBound: true);
                AddThumb(token, nameof(CubicBezierCurve.X2), nameof(CubicBezierCurve.Y2), notBound: true);
                AddThumb(token, nameof(CubicBezierCurve.X3), nameof(CubicBezierCurve.Y3));
            }
            else if (token is QuadraticBezierCurve)
            {
                AddThumb(token, nameof(QuadraticBezierCurve.X1), nameof(QuadraticBezierCurve.Y1), notBound: true);
                AddThumb(token, nameof(QuadraticBezierCurve.X2), nameof(QuadraticBezierCurve.Y2));
            }
            else if (token is SmoothCubicBezierCurve)
            {
                AddThumb(token, nameof(SmoothCubicBezierCurve.X1), nameof(SmoothCubicBezierCurve.Y1), notBound: true);
                AddThumb(token, nameof(SmoothCubicBezierCurve.X2), nameof(SmoothCubicBezierCurve.Y2));
            }
            else if (token is SmoothQuadraticBezierCurve)
            {
                AddThumb(token, nameof(SmoothQuadraticBezierCurve.X1), nameof(SmoothQuadraticBezierCurve.Y1), notBound: true);
                AddThumb(token, nameof(SmoothQuadraticBezierCurve.X2), nameof(SmoothQuadraticBezierCurve.Y2));
            }
        }


        private void AddThumbForPreviousToken(Token token)
        {
            var currentTokenIndex = CurrentFigure.Tokens.ToList().IndexOf(token);
            var previousTokenIndex = currentTokenIndex - 1;
            var previousToken = CurrentFigure.Tokens.ElementAt(previousTokenIndex);
            string x, y;

            if (previousToken is Move)
            {
                x = nameof(Move.X);
                y = nameof(Move.Y);
            }
            else if (previousToken is Line)
            {
                x = nameof(Line.X);
                y = nameof(Line.Y);
            }
            else if (previousToken is CubicBezierCurve)
            {
                x = nameof(CubicBezierCurve.X3);
                y = nameof(CubicBezierCurve.Y3);
            }
            else if (previousToken is QuadraticBezierCurve)
            {
                x = nameof(QuadraticBezierCurve.X2);
                y = nameof(QuadraticBezierCurve.Y2);
            }
            else if (previousToken is SmoothCubicBezierCurve)
            {
                x = nameof(SmoothCubicBezierCurve.X2);
                y = nameof(SmoothCubicBezierCurve.Y2);
            }
            else if (previousToken is SmoothQuadraticBezierCurve)
            {
                x = nameof(SmoothQuadraticBezierCurve.X2);
                y = nameof(SmoothQuadraticBezierCurve.Y2);
            }
            else if (previousToken is EllipticalArc)
            {
                x = nameof(EllipticalArc.X2);
                y = nameof(EllipticalArc.Y2);
            }
            else
            {
                throw new NotImplementedException();
            }
            
            AddThumb(previousToken, x, y);
        }

        private void AddThumb(Token token, string x, string y, bool notBound = false)
        {
            Children.Add(new VDPointThumb(token, x, y, InvalidateMeasure, notBound));
        }

        private void RemoveThumbs()
        {
            // Remove all editings
            foreach (VDPointThumb thumb in Children.OfType<VDPointThumb>().ToList())
            {
                Children.Remove(thumb);
            }
        }

        private Path CreateFigureVisualization(VDFigure figure)
        {
            Path visualilzation = new Path { DataContext = figure };
            visualilzation.SetBinding(Path.DataProperty,
                new Binding(nameof(figure.Markup)) { Mode = BindingMode.OneWay, Converter = new StringToGeometryConverter() });
            visualilzation.SetBinding(Path.FillProperty,
                new Binding(nameof(figure.FillColor)) { Mode = BindingMode.OneWay, Converter = new StringToBrushConverter() });
            visualilzation.SetBinding(Path.StrokeProperty,
                new Binding(nameof(figure.StrokeColor)) { Mode = BindingMode.OneWay, Converter = new StringToBrushConverter() });
            visualilzation.SetBinding(Path.StrokeThicknessProperty,
                new Binding(nameof(figure.StrokeThickness)) { Mode = BindingMode.OneWay });

            return visualilzation;
        }


        private void OnFiguresChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Remove all thumbs
            RemoveThumbs();

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    Children.Add(CreateFigureVisualization((VDFigure)item));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    Children.Remove(Children.OfType<Path>().First(x => x.DataContext == item));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (var item in e.OldItems)
                {
                    Children.Remove(Children.OfType<Path>().First(x => x.DataContext == item));
                }
                foreach (var item in e.NewItems)
                {
                    Children.Add(CreateFigureVisualization((VDFigure)item));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {

            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
            }
        }


        private class VDCanvasCache
        {
            private Token currentToken;
            private Point mouseDownPoint;
            private bool mouseDownPointUpdated;
                        
            public Token CurrentToken
            {
                get { return currentToken; }
                set
                {
                    if (currentToken != value)
                    {
                        currentToken = value;
                    }
                }
            }

            public Point MouseDownPoint
            {
                get { return mouseDownPoint; }
                set
                {
                    if (mouseDownPoint != value)
                    {
                        mouseDownPoint = value;
                        mouseDownPointUpdated = true;
                    }
                }
            }

            public bool MouseDownPointUpdated
            {
                get { return mouseDownPointUpdated; }
                set
                {
                    if (mouseDownPointUpdated != value)
                    {
                        mouseDownPointUpdated = value;
                    }
                }
            }
        }

        private enum VDCanvasMode
        {
            None,
            Edit,
            Create
        }

        #endregion
    }

    public enum VDCanvasCommand
    {
        None,
        CreateLine,
        CreateCubicCurve,
        CreateQuadraticCurve,
        CreateSmoothCubicCurve,
        CreateSmoothQuadraticCurve
    }
}
