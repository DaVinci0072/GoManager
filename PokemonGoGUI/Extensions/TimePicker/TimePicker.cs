using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*
 * Credit: http://www.codeproject.com/Articles/170684/Time-Picker
 */
namespace PokemonGoGUI.Extensions.TimePicker
{
    [DefaultEvent("OnValueChanged")]
    public partial class TimePicker : UserControl
    {
        [Browsable(true)]
        public event EventHandler OnValueChanged;

        public int Hours
        {
            get
            {
                string Part = TheTimeBox.Text.Split(":".ToCharArray())[0];
                int Value = 0;
                int.TryParse(Part, out Value);
                return Value;
            }
            set
            {
                string[] Parts = TheTimeBox.Text.Split(":".ToCharArray());

                Parts[0] = value.ToString("D2");

                SetText(Parts[0], Parts[1]);
            }
        }
        public int Minutes
        {
            get
            {
                string Part = TheTimeBox.Text.Split(":".ToCharArray())[1];
                int Value = 0;
                int.TryParse(Part, out Value);
                return Value;
            }
            set
            {
                string[] Parts = TheTimeBox.Text.Split(":".ToCharArray());

                Parts[1] = value.ToString("D2");

                SetText(Parts[0], Parts[1]);
            }
        }

        public TimeSpan Value
        {
            get
            {
                return new TimeSpan(0, Hours, Minutes);
            }
            set
            {
                Hours = value.Hours;
                Minutes = value.Minutes;
            }
        }

        public TimePicker()
        {
            InitializeComponent();

            TheTimeBox.LostFocus += new EventHandler(TheTimeBox_LostFocus);
            this.LostFocus += new EventHandler(TheTimeBox_LostFocus);
        }

        private void TheTimeBox_LostFocus(object sender, EventArgs e)
        {
            FormatText();
        }

        private void FormatText()
        {
            string[] Parts = TheTimeBox.Text.Trim().Split(':');

            int Hours = 0;

            if (!int.TryParse(Parts[0], out Hours))
            {
                Hours = 0;
            }

            if (Hours >= 23)
            {
                Hours = 23;
            }

            int Minutes = 0;

            if (Parts.Length < 2 || !int.TryParse(Parts[1], out Minutes))
            {
                Minutes = 0;
            }
            if (Minutes >= 59)
            {
                Minutes = 59;
            }

            SetText(Hours.ToString("D2"), Minutes.ToString("D2"));
            if (OnValueChanged != null)
            {
                OnValueChanged.Invoke(null, new EventArgs());
            }
        }

        bool DoingFormatting = false;

        private void SetText(string Hour, string Minute)
        {
            int SelectedIndex = TheTimeBox.SelectionStart;

            TheTimeBox.Text = Hour + ":" + Minute;

            SelectedIndex = SelectedIndex > TheTimeBox.Text.Length ? TheTimeBox.Text.Length : SelectedIndex;
            SelectCorrectText(SelectedIndex);

            if (!DoingFormatting)
            {
                DoingFormatting = true;
                FormatText();
                DoingFormatting = false;
            }
        }

        private void TheTimeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                string[] Parts = TheTimeBox.Text.Split(":".ToCharArray());

                if (TheTimeBox.SelectionStart <= 2)
                {
                    int PartNum = 0;
                    if (int.TryParse(Parts[0], out PartNum))
                    {
                        PartNum++;
                        if (PartNum >= 24)
                        {
                            PartNum = 0;
                        }
                        Parts[0] = PartNum.ToString("D2");
                    }
                }
                else if (TheTimeBox.SelectionStart <= 5)
                {
                    int PartNum = 0;
                    if (int.TryParse(Parts[1], out PartNum))
                    {
                        PartNum++;
                        if (PartNum >= 60)
                        {
                            PartNum = 0;
                        }
                        Parts[1] = PartNum.ToString("D2");
                    }
                }

                SetText(Parts[0], Parts[1]);

                FormatText();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                string[] Parts = TheTimeBox.Text.Split(":".ToCharArray());

                if (TheTimeBox.SelectionStart <= 2)
                {
                    int PartNum = 0;
                    if (int.TryParse(Parts[0], out PartNum))
                    {
                        PartNum--;
                        if (PartNum < 0)
                        {
                            PartNum = 23;
                        }
                        Parts[0] = PartNum.ToString("D2");
                    }
                }
                else if (TheTimeBox.SelectionStart <= 5)
                {
                    int PartNum = 0;
                    if (int.TryParse(Parts[1], out PartNum))
                    {
                        PartNum--;
                        if (PartNum < 0)
                        {
                            PartNum = 59;
                        }
                        Parts[1] = PartNum.ToString("D2");
                    }
                }

                SetText(Parts[0], Parts[1]);
                FormatText();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                FormatText();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                FormatText();
                if (TheTimeBox.SelectionStart <= 2)
                {
                    TheTimeBox.Select(9, 3);
                }
                else if (TheTimeBox.SelectionStart <= 5)
                {
                    TheTimeBox.Select(0, 2);
                }
                else if (TheTimeBox.SelectionStart <= 8)
                {
                    TheTimeBox.Select(3, 2);
                }
                else
                {
                    TheTimeBox.Select(6, 2);
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                FormatText();
                if (TheTimeBox.SelectionStart <= 2)
                {
                    TheTimeBox.Select(3, 2);
                }
                else if (TheTimeBox.SelectionStart <= 5)
                {
                    TheTimeBox.Select(6, 2);
                }
                else if (TheTimeBox.SelectionStart <= 8)
                {
                    TheTimeBox.Select(9, 3);
                }
                else
                {
                    TheTimeBox.Select(0, 2);
                }
                e.Handled = true;
            }
            else if (!((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)))
            {
                NonNumKeyPressed = true;
                e.Handled = true;
            }
        }

        bool NonNumKeyPressed = false;

        private void SelectCorrectText(int SelectedIndex)
        {
            if (SelectedIndex <= 2)
            {
                TheTimeBox.Select(0, 2);
            }
            else if (SelectedIndex <= 5)
            {
                TheTimeBox.Select(3, 2);
            }
            else if (SelectedIndex <= 8)
            {
                TheTimeBox.Select(6, 2);
            }
            else
            {
                TheTimeBox.Select(9, 3);
            }
        }

        private void TheTimeBox_Click(object sender, EventArgs e)
        {
            FormatText();
        }

        private void TheTimeBox_TextChanged(object sender, EventArgs e)
        {
            if (NonNumKeyPressed)
            {
                NonNumKeyPressed = false;
                FormatText();
            }
        }

    }
}
