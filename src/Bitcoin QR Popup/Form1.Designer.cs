namespace Bitcoin_QR_Popup
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox_qr = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_amount = new System.Windows.Forms.TextBox();
            this.label_amount = new System.Windows.Forms.Label();
            this.label_exchangeA = new System.Windows.Forms.Label();
            this.label_exchangeB = new System.Windows.Forms.Label();
            this.textBox_address = new System.Windows.Forms.TextBox();
            this.notifyIcon_bitcoin_qr_popup = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_currency = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.label_currency = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.timer_poll_amount = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_qr)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_qr
            // 
            this.pictureBox_qr.Location = new System.Drawing.Point(36, 41);
            this.pictureBox_qr.Name = "pictureBox_qr";
            this.pictureBox_qr.Size = new System.Drawing.Size(300, 300);
            this.pictureBox_qr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_qr.TabIndex = 1;
            this.pictureBox_qr.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(42, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Amount:";
            // 
            // textBox_amount
            // 
            this.textBox_amount.Location = new System.Drawing.Point(105, 11);
            this.textBox_amount.Name = "textBox_amount";
            this.textBox_amount.Size = new System.Drawing.Size(74, 20);
            this.textBox_amount.TabIndex = 4;
            this.textBox_amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_amount.TextChanged += new System.EventHandler(this.textBox_amount_TextChanged);
            // 
            // label_amount
            // 
            this.label_amount.AutoSize = true;
            this.label_amount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_amount.Location = new System.Drawing.Point(52, 386);
            this.label_amount.Name = "label_amount";
            this.label_amount.Size = new System.Drawing.Size(24, 25);
            this.label_amount.TabIndex = 5;
            this.label_amount.Text = "0";
            // 
            // label_exchangeA
            // 
            this.label_exchangeA.AutoSize = true;
            this.label_exchangeA.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_exchangeA.Location = new System.Drawing.Point(197, 374);
            this.label_exchangeA.Name = "label_exchangeA";
            this.label_exchangeA.Size = new System.Drawing.Size(112, 18);
            this.label_exchangeA.TabIndex = 6;
            this.label_exchangeA.Text = "                          ";
            // 
            // label_exchangeB
            // 
            this.label_exchangeB.AutoSize = true;
            this.label_exchangeB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_exchangeB.Location = new System.Drawing.Point(197, 393);
            this.label_exchangeB.Name = "label_exchangeB";
            this.label_exchangeB.Size = new System.Drawing.Size(112, 18);
            this.label_exchangeB.TabIndex = 8;
            this.label_exchangeB.Text = "                          ";
            // 
            // textBox_address
            // 
            this.textBox_address.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_address.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_address.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_address.Location = new System.Drawing.Point(36, 352);
            this.textBox_address.Name = "textBox_address";
            this.textBox_address.ReadOnly = true;
            this.textBox_address.Size = new System.Drawing.Size(308, 17);
            this.textBox_address.TabIndex = 9;
            this.textBox_address.Text = "1FiVXpdBdcnv6GAVZDKRxeSJqZQrSx2s2Q";
            // 
            // notifyIcon_bitcoin_qr_popup
            // 
            this.notifyIcon_bitcoin_qr_popup.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon_bitcoin_qr_popup.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon_bitcoin_qr_popup.Icon")));
            this.notifyIcon_bitcoin_qr_popup.Text = "Bitcoin QR Popup";
            this.notifyIcon_bitcoin_qr_popup.Visible = true;
            this.notifyIcon_bitcoin_qr_popup.Click += new System.EventHandler(this.notifyIcon_bitcoin_qr_popup_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_currency,
            this.toolStripMenuItem_exit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 48);
            // 
            // toolStripMenuItem_currency
            // 
            this.toolStripMenuItem_currency.Name = "toolStripMenuItem_currency";
            this.toolStripMenuItem_currency.Size = new System.Drawing.Size(122, 22);
            this.toolStripMenuItem_currency.Text = "Currency";
            // 
            // toolStripMenuItem_exit
            // 
            this.toolStripMenuItem_exit.Name = "toolStripMenuItem_exit";
            this.toolStripMenuItem_exit.Size = new System.Drawing.Size(122, 22);
            this.toolStripMenuItem_exit.Text = "Exit";
            this.toolStripMenuItem_exit.Click += new System.EventHandler(this.toolStripMenuItem_exit_Click);
            // 
            // label_currency
            // 
            this.label_currency.AutoSize = true;
            this.label_currency.Location = new System.Drawing.Point(181, 15);
            this.label_currency.Name = "label_currency";
            this.label_currency.Size = new System.Drawing.Size(30, 13);
            this.label_currency.TabIndex = 11;
            this.label_currency.Text = "USD";
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(269, 3);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(102, 35);
            this.button_cancel.TabIndex = 12;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // timer_poll_amount
            // 
            this.timer_poll_amount.Interval = 1000;
            this.timer_poll_amount.Tick += new System.EventHandler(this.timer_poll_amount_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(31, 386);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 25);
            this.label2.TabIndex = 13;
            this.label2.Text = "฿";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 420);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.textBox_address);
            this.Controls.Add(this.label_currency);
            this.Controls.Add(this.label_exchangeB);
            this.Controls.Add(this.label_exchangeA);
            this.Controls.Add(this.label_amount);
            this.Controls.Add(this.pictureBox_qr);
            this.Controls.Add(this.textBox_amount);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Bitcoin QR Popup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_qr)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_qr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_amount;
        private System.Windows.Forms.Label label_amount;
        private System.Windows.Forms.Label label_exchangeA;
        private System.Windows.Forms.Label label_exchangeB;
        private System.Windows.Forms.TextBox textBox_address;
        private System.Windows.Forms.NotifyIcon notifyIcon_bitcoin_qr_popup;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_currency;
        private System.Windows.Forms.Label label_currency;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_exit;
        private System.Windows.Forms.Timer timer_poll_amount;
        private System.Windows.Forms.Label label2;
    }
}

