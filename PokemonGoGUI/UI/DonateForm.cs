using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokemonGoGUI.UI
{
    public partial class DonateForm : Form
    {
        private const string _paypalDonation = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=4YL26WANHNFE6";
        private const string _btcDonation = "1Ln4w8UJpxpDFfjuGshfK765SDjsk7ZmRP";

        public DonateForm()
        {
            InitializeComponent();
        }

        private void DonateForm_Load(object sender, EventArgs e)
        {
            linkLabelPaypal.Links.Add(new LinkLabel.Link
                {
                    LinkData = _paypalDonation
                });

            textBoxBtc.Text = _btcDonation;
        }

        private void linkLabelPaypal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
