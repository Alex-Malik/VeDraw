using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VeDraw.Controls
{
    using VeDraw.Models;
    
    public class VDPointThumb : Thumb
    {
        static VDPointThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VDPointThumb), 
                new FrameworkPropertyMetadata(typeof(VDPointThumb)));
        }

        public static Size Bounds { get; internal set; }

        private readonly Token Token;
        private readonly bool NotBound;
        private readonly PropertyInfo PropertyX, PropertyY;
        
        public VDPointThumb(Token token, string x, string y, Action update, bool notBound = false)
        {
            Token = token;
            PropertyX = token.GetType().GetProperty(x);
            PropertyY = token.GetType().GetProperty(y);
            NotBound = notBound;

            X = (double)PropertyX.GetValue(token);
            Y = (double)PropertyY.GetValue(token);
            
            DragDelta += OnDrag;

            if (update != null) Updated += update;
        }

        ~VDPointThumb()
        {
            DragDelta -= OnDrag;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public event Action Updated;

        private void OnDrag(object sender, DragDeltaEventArgs e)
        {
            Update(e.HorizontalChange, e.VerticalChange);
        }

        private void Update(double h, double v)
        {
            double dX = (double)PropertyX.GetValue(Token) + h;
            double dY = (double)PropertyY.GetValue(Token) + v;

            if (0.0 <= dX && dX <= Bounds.Width || NotBound) X = dX;
            if (0.0 <= dY && dY <= Bounds.Height || NotBound) Y = dY;
            
            PropertyX.SetValue(Token, X);
            PropertyY.SetValue(Token, Y);

            Updated?.Invoke();
        }
    }
}
