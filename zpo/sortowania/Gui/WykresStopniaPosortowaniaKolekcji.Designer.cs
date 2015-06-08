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
            this.tytuł = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tytuł
            // 
            this.tytuł.AutoSize = true;
            this.tytuł.Location = new System.Drawing.Point(3, 0);
            this.tytuł.Name = "tytuł";
            this.tytuł.Size = new System.Drawing.Size(74, 13);
            this.tytuł.TabIndex = 0;
            this.tytuł.Text = "Tytuł wykresu";
            // 
            // WykresStopniaPosortowaniaKolekcji
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tytuł);
            this.DoubleBuffered = true;
            this.Name = "WykresStopniaPosortowaniaKolekcji";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tytuł;



    }
}
