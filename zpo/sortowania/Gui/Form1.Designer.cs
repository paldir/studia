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
            this.wykres1 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres2 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres3 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres4 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres5 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres6 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.SuspendLayout();
            // 
            // wykres1
            // 
            this.wykres1.Location = new System.Drawing.Point(12, 12);
            this.wykres1.Name = "wykres1";
            this.wykres1.Size = new System.Drawing.Size(300, 300);
            this.wykres1.TabIndex = 0;
            // 
            // wykres2
            // 
            this.wykres2.Location = new System.Drawing.Point(318, 12);
            this.wykres2.Name = "wykres2";
            this.wykres2.Size = new System.Drawing.Size(300, 300);
            this.wykres2.TabIndex = 1;
            // 
            // wykres3
            // 
            this.wykres3.Location = new System.Drawing.Point(624, 12);
            this.wykres3.Name = "wykres3";
            this.wykres3.Size = new System.Drawing.Size(300, 300);
            this.wykres3.TabIndex = 2;
            // 
            // wykres4
            // 
            this.wykres4.Location = new System.Drawing.Point(12, 318);
            this.wykres4.Name = "wykres4";
            this.wykres4.Size = new System.Drawing.Size(300, 300);
            this.wykres4.TabIndex = 3;
            // 
            // wykres5
            // 
            this.wykres5.Location = new System.Drawing.Point(318, 318);
            this.wykres5.Name = "wykres5";
            this.wykres5.Size = new System.Drawing.Size(300, 300);
            this.wykres5.TabIndex = 4;
            // 
            // wykres6
            // 
            this.wykres6.Location = new System.Drawing.Point(624, 318);
            this.wykres6.Name = "wykres6";
            this.wykres6.Size = new System.Drawing.Size(300, 300);
            this.wykres6.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 641);
            this.Controls.Add(this.wykres6);
            this.Controls.Add(this.wykres5);
            this.Controls.Add(this.wykres4);
            this.Controls.Add(this.wykres3);
            this.Controls.Add(this.wykres2);
            this.Controls.Add(this.wykres1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private WykresStopniaPosortowaniaKolekcji wykres1;
        private WykresStopniaPosortowaniaKolekcji wykres2;
        private WykresStopniaPosortowaniaKolekcji wykres3;
        private WykresStopniaPosortowaniaKolekcji wykres4;
        private WykresStopniaPosortowaniaKolekcji wykres5;
        private WykresStopniaPosortowaniaKolekcji wykres6;
    }
}

