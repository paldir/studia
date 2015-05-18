namespace Gui
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
            this.wykresStopniaPosortowaniaKolekcji = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.SuspendLayout();
            // 
            // wykresStopniaPosortowaniaKolekcji
            // 
            this.wykresStopniaPosortowaniaKolekcji.Kolekcja = null;
            this.wykresStopniaPosortowaniaKolekcji.Location = new System.Drawing.Point(12, 12);
            this.wykresStopniaPosortowaniaKolekcji.Name = "wykresStopniaPosortowaniaKolekcji";
            this.wykresStopniaPosortowaniaKolekcji.Size = new System.Drawing.Size(300, 300);
            this.wykresStopniaPosortowaniaKolekcji.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 561);
            this.Controls.Add(this.wykresStopniaPosortowaniaKolekcji);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private WykresStopniaPosortowaniaKolekcji wykresStopniaPosortowaniaKolekcji;
    }
}

