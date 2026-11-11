using System.Drawing;
using System.Windows.Forms;

namespace KS2
{
    internal static class AppStyle
    {
        public static readonly Color Background = Color.FromArgb(245, 247, 251);
        public static readonly Color Surface = Color.White;
        public static readonly Color Primary = Color.FromArgb(37, 99, 235);
        public static readonly Color PrimaryHover = Color.FromArgb(29, 78, 216);
        public static readonly Color PrimaryLight = Color.FromArgb(219, 234, 254);
        public static readonly Color Text = Color.FromArgb(23, 32, 51);
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
        public static readonly Color Border = Color.FromArgb(226, 232, 240);
        public static readonly Color Success = Color.FromArgb(22, 163, 74);
        public static readonly Color Warning = Color.FromArgb(245, 158, 11);
        public static readonly Color Error = Color.FromArgb(220, 38, 38);

        public static readonly Font BodyFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font LabelFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font HeadingFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        public static readonly Font TitleFont = new Font("Segoe UI", 18F, FontStyle.Bold);

        public static void Apply(Control control)
        {
            control.Font = BodyFont;
            control.ForeColor = Text;

            switch (control)
            {
                case Form form:
                    form.BackColor = Background;
                    form.Font = BodyFont;
                    break;
                case MenuStrip strip:
                    strip.BackColor = Surface;
                    strip.ForeColor = Text;
                    strip.Font = BodyFont;
                    strip.RenderMode = ToolStripRenderMode.System;
                    break;
                case GroupBox:
                    control.BackColor = Surface;
                    control.Font = HeadingFont;
                    break;
                case Panel panel:
                    panel.BackColor = panel.Name == "panel1" ? Surface : Background;
                    break;
                case Button button:
                    button.BackColor = Surface;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Border;
                    button.FlatAppearance.MouseOverBackColor = PrimaryLight;
                    button.ForeColor = Text;
                    button.Font = BodyFont;
                    break;
                case ComboBox comboBox:
                    comboBox.BackColor = Surface;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.ForeColor = Text;
                    break;
                case TextBox textBox:
                    textBox.BackColor = Surface;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case CheckBox checkBox:
                    checkBox.BackColor = Color.Transparent;
                    checkBox.ForeColor = Text;
                    break;
                case Label label:
                    label.BackColor = Color.Transparent;
                    label.ForeColor = Text;
                    label.Font = LabelFont;
                    break;
            }

            if (control.Name == "rodzaj_label")
            {
                control.Font = TitleFont;
                control.ForeColor = Primary;
            }

            if (control.Name == "dalej_button")
            {
                var button = (Button)control;
                button.BackColor = Primary;
                button.FlatAppearance.BorderColor = Primary;
                button.FlatAppearance.MouseOverBackColor = PrimaryHover;
                button.ForeColor = Color.White;
                button.Font = HeadingFont;
            }

            if (control.Name == "Wstecz_button")
            {
                var button = (Button)control;
                button.BackColor = Surface;
                button.FlatAppearance.BorderColor = Border;
                button.FlatAppearance.MouseOverBackColor = PrimaryLight;
                button.ForeColor = Text;
            }

            if (control is MenuStrip menuStrip)
            {
                foreach (ToolStripItem item in menuStrip.Items)
                    ApplyMenuItemStyle(item);
            }

            foreach (Control child in control.Controls)
                Apply(child);
        }

        private static void ApplyMenuItemStyle(ToolStripItem item)
        {
            item.ForeColor = Text;
            item.BackColor = Surface;
            item.Font = BodyFont;

            if (item is ToolStripDropDownItem dropDownItem)
            {
                foreach (ToolStripItem child in dropDownItem.DropDownItems)
                    ApplyMenuItemStyle(child);
            }
        }
    }
}
