using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace flatButton
{
    public class FlatButton : Button
    {
        public FlatButton()
        {
            theme_color = Color.DodgerBlue;
            BackColor = Color.FromArgb(220, theme_color);
            ForeColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.Clear(Color.White);
            pevent.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(pevent.Graphics, Text, Font, new Point(Width + 3, Height / 2), ForeColor, flags);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            BackColor = Color.FromArgb(150, theme_color);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            BackColor = Color.FromArgb(220, theme_color);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            BackColor = Color.FromArgb(250, theme_color);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            BackColor = Color.FromArgb(220, theme_color);
        }

        private Color theme_color;
        private Color mouse_down_color;
        public Color mouseDownColor {
            set {
                mouse_down_color = value;
            }
            get {
                return mouse_down_color;
            }
        }
    }
}
