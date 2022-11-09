
namespace Rent
{
    partial class subForm
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
            this.rates = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.rates)).BeginInit();
            this.SuspendLayout();
            // 
            // rates
            // 
            this.rates.AllowUserToAddRows = false;
            this.rates.AllowUserToResizeColumns = false;
            this.rates.AllowUserToResizeRows = false;
            this.rates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rates.ColumnHeadersVisible = false;
            this.rates.Location = new System.Drawing.Point(2, 2);
            this.rates.Name = "rates";
            this.rates.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.rates.Size = new System.Drawing.Size(238, 80);
            this.rates.TabIndex = 0;
            this.rates.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.rates_CellValueChanged);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(59, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // subForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(385, 285);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rates);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "subForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Тарифы";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.subForm_FormClosing);
            this.Load += new System.EventHandler(this.subForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rates)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.DataGridView rates;
    }
}