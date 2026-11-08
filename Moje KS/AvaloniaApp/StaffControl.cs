using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace MojeKS.AvaloniaApp
{
    public class StaffControl : Control
    {
        public static readonly StyledProperty<double> InterlineProperty =
            AvaloniaProperty.Register<StaffControl, double>(nameof(Interline), 14.0);

        public double Interline
        {
            get => GetValue(InterlineProperty);
            set => SetValue(InterlineProperty, value);
        }

        public static readonly StyledProperty<double> LeftMarginProperty =
            AvaloniaProperty.Register<StaffControl, double>(nameof(LeftMargin), 20.0);

        public double LeftMargin
        {
            get => GetValue(LeftMarginProperty);
            set => SetValue(LeftMarginProperty, value);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            var pen = new Pen(Brushes.Black, 1);
            double top = 20;
            int lines = 5;

            for (int i = 0; i < lines; i++)
            {
                double y = top + i * Interline;
                context.DrawLine(pen, new Point(LeftMargin, y), new Point(Bounds.Width - 10, y));
            }

            // sample filled note (ellipse)
            double noteX = LeftMargin + 100;
            double noteY = top + 2 * Interline; // middle line
            var noteRect = new Rect(noteX, noteY - Interline * 0.35, Interline * 0.8, Interline * 0.6);
            context.DrawGeometry(Brushes.Black, null, new EllipseGeometry(noteRect));
        }
    }
}
