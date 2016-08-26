namespace PokemonGoGUI.UI
{
    partial class DonateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDone = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBtc = new System.Windows.Forms.TextBox();
            this.linkLabelPaypal = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 64);
            this.label1.TabIndex = 0;
            this.label1.Text = "This program is currently being built by a single developer.\r\n\r\nIf you wish to he" +
    "lp support the development of this project, \r\ndonation information is below.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.Location = new System.Drawing.Point(337, 150);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 1;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Paypal:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "BTC:";
            // 
            // textBoxBtc
            // 
            this.textBoxBtc.Location = new System.Drawing.Point(83, 124);
            this.textBoxBtc.Name = "textBoxBtc";
            this.textBoxBtc.Size = new System.Drawing.Size(283, 22);
            this.textBoxBtc.TabIndex = 3;
            // 
            // linkLabelPaypal
            // 
            this.linkLabelPaypal.AutoSize = true;
            this.linkLabelPaypal.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelPaypal.LinkColor = System.Drawing.Color.Gray;
            this.linkLabelPaypal.Location = new System.Drawing.Point(80, 93);
            this.linkLabelPaypal.Name = "linkLabelPaypal";
            this.linkLabelPaypal.Size = new System.Drawing.Size(89, 16);
            this.linkLabelPaypal.TabIndex = 4;
            this.linkLabelPaypal.TabStop = true;
            this.linkLabelPaypal.Text = "Donation Link";
            this.linkLabelPaypal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPaypal_LinkClicked);
            // 
            // DonateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 185);
            this.Controls.Add(this.linkLabelPaypal);
            this.Controls.Add(this.textBoxBtc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.label1);
            this.Name = "DonateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Donations";
            this.Load += new System.EventHandler(this.DonateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxBtc;
        private System.Windows.Forms.LinkLabel linkLabelPaypal;
    }
}