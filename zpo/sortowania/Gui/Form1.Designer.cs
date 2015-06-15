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
            this.szybkość = new System.Windows.Forms.TrackBar();
            this.wykres6 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres5 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres4 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres3 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres2 = new Gui.WykresStopniaPosortowaniaKolekcji();
            this.wykres1 = new Gui.WykresStopniaPosortowaniaKolekcji();
            ((System.ComponentModel.ISupportInitialize)(this.szybkość)).BeginInit();
            this.SuspendLayout();
            // 
            // szybkość
            // 
            this.szybkość.Location = new System.Drawing.Point(12, 12);
            this.szybkość.Maximum = 1000;
            this.szybkość.Minimum = 10;
            this.szybkość.Name = "szybkość";
            this.szybkość.Size = new System.Drawing.Size(349, 45);
            this.szybkość.TabIndex = 6;
            this.szybkość.TickFrequency = 10;
            this.szybkość.Value = 10;
            this.szybkość.Scroll += new System.EventHandler(this.szybkość_Scroll);
            // 
            // wykres6
            // 
            this.wykres6.Dane = null;
            this.wykres6.Location = new System.Drawing.Point(624, 369);
            this.wykres6.MaksymalnyElement = 0;
            this.wykres6.Name = "wykres6";
            this.wykres6.Size = new System.Drawing.Size(300, 300);
            this.wykres6.TabIndex = 5;
            this.wykres6.TytułWykresu = "Tytuł wykresu";
            // 
            // wykres5
            // 
            this.wykres5.Dane = null;
            this.wykres5.Location = new System.Drawing.Point(318, 369);
            this.wykres5.MaksymalnyElement = 0;
            this.wykres5.Name = "wykres5";
            this.wykres5.Size = new System.Drawing.Size(300, 300);
            this.wykres5.TabIndex = 4;
            this.wykres5.TytułWykresu = "Tytuł wykresu";
            // 
            // wykres4
            // 
            this.wykres4.Dane = null;
            this.wykres4.Location = new System.Drawing.Point(12, 369);
            this.wykres4.MaksymalnyElement = 0;
            this.wykres4.Name = "wykres4";
            this.wykres4.Size = new System.Drawing.Size(300, 300);
            this.wykres4.TabIndex = 3;
            this.wykres4.TytułWykresu = "Tytuł wykresu";
            // 
            // wykres3
            // 
            this.wykres3.Dane = null;
            this.wykres3.Location = new System.Drawing.Point(624, 63);
            this.wykres3.MaksymalnyElement = 0;
            this.wykres3.Name = "wykres3";
            this.wykres3.Size = new System.Drawing.Size(300, 300);
            this.wykres3.TabIndex = 2;
            this.wykres3.TytułWykresu = "Tytuł wykresu";
            // 
            // wykres2
            // 
            this.wykres2.Dane = null;
            this.wykres2.Location = new System.Drawing.Point(318, 63);
            this.wykres2.MaksymalnyElement = 0;
            this.wykres2.Name = "wykres2";
            this.wykres2.Size = new System.Drawing.Size(300, 300);
            this.wykres2.TabIndex = 1;
            this.wykres2.TytułWykresu = "Tytuł wykresu";
            // 
            // wykres1
            // 
            this.wykres1.Dane = null;
            this.wykres1.Location = new System.Drawing.Point(12, 63);
            this.wykres1.MaksymalnyElement = 0;
            this.wykres1.Name = "wykres1";
            this.wykres1.Size = new System.Drawing.Size(300, 300);
            this.wykres1.TabIndex = 0;
            this.wykres1.TytułWykresu = "Tytuł wykresu";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 671);
            this.Controls.Add(this.szybkość);
            this.Controls.Add(this.wykres6);
            this.Controls.Add(this.wykres5);
            this.Controls.Add(this.wykres4);
            this.Controls.Add(this.wykres3);
            this.Controls.Add(this.wykres2);
            this.Controls.Add(this.wykres1);
            this.Name = "Form1";
            this.Text = "Porównanie sortowań";
            ((System.ComponentModel.ISupportInitialize)(this.szybkość)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WykresStopniaPosortowaniaKolekcji wykres1;
        private WykresStopniaPosortowaniaKolekcji wykres2;
        private WykresStopniaPosortowaniaKolekcji wykres3;
        private WykresStopniaPosortowaniaKolekcji wykres4;
        private WykresStopniaPosortowaniaKolekcji wykres5;
        private WykresStopniaPosortowaniaKolekcji wykres6;
        private System.Windows.Forms.TrackBar szybkość;
    }
}

