namespace Gui
{
    partial class WykresStopniaPosortowaniaKolekcji
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tytuł = new System.Windows.Forms.Label();
            this._czas = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tytuł
            // 
            this._tytuł.AutoSize = true;
            this._tytuł.Location = new System.Drawing.Point(3, 0);
            this._tytuł.Name = "tytuł";
            this._tytuł.Size = new System.Drawing.Size(74, 13);
            this._tytuł.TabIndex = 0;
            this._tytuł.Text = "Tytuł wykresu";
            // 
            // czas
            // 
            this._czas.AutoSize = true;
            this._czas.Location = new System.Drawing.Point(3, 13);
            this._czas.Name = "czas";
            this._czas.Size = new System.Drawing.Size(0, 13);
            this._czas.TabIndex = 1;
            // 
            // WykresStopniaPosortowaniaKolekcji
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._czas);
            this.Controls.Add(this._tytuł);
            this.DoubleBuffered = true;
            this.Name = "WykresStopniaPosortowaniaKolekcji";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _tytuł;
        private System.Windows.Forms.Label _czas;



    }
}
