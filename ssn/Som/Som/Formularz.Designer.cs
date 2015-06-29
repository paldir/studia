namespace Som
{
    partial class Formularz
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
            this.suwak = new System.Windows.Forms.TrackBar();
            this.etykietaSzybkości = new System.Windows.Forms.Label();
            this.przyciskStartu = new System.Windows.Forms.Button();
            this.etykietaIlości = new System.Windows.Forms.Label();
            this.ilość = new System.Windows.Forms.NumericUpDown();
            this.przyciskStopu = new System.Windows.Forms.Button();
            this.mapa = new Som.Mapa();
            ((System.ComponentModel.ISupportInitialize)(this.suwak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilość)).BeginInit();
            this.SuspendLayout();
            // 
            // suwak
            // 
            this.suwak.Location = new System.Drawing.Point(622, 29);
            this.suwak.Maximum = 100;
            this.suwak.Name = "suwak";
            this.suwak.Size = new System.Drawing.Size(153, 45);
            this.suwak.TabIndex = 1;
            this.suwak.TickFrequency = 4;
            this.suwak.Value = 90;
            this.suwak.Scroll += new System.EventHandler(this.suwak_Scroll);
            // 
            // etykietaSzybkości
            // 
            this.etykietaSzybkości.AutoSize = true;
            this.etykietaSzybkości.Location = new System.Drawing.Point(619, 13);
            this.etykietaSzybkości.Name = "etykietaSzybkości";
            this.etykietaSzybkości.Size = new System.Drawing.Size(100, 13);
            this.etykietaSzybkości.TabIndex = 2;
            this.etykietaSzybkości.Text = "Szybkość animacji: ";
            // 
            // przyciskStartu
            // 
            this.przyciskStartu.Location = new System.Drawing.Point(622, 119);
            this.przyciskStartu.Name = "przyciskStartu";
            this.przyciskStartu.Size = new System.Drawing.Size(75, 23);
            this.przyciskStartu.TabIndex = 4;
            this.przyciskStartu.Text = "Start";
            this.przyciskStartu.UseVisualStyleBackColor = true;
            this.przyciskStartu.Click += new System.EventHandler(this.przyciskStartu_Click);
            // 
            // etykietaIlości
            // 
            this.etykietaIlości.AutoSize = true;
            this.etykietaIlości.Location = new System.Drawing.Point(619, 77);
            this.etykietaIlości.Name = "etykietaIlości";
            this.etykietaIlości.Size = new System.Drawing.Size(82, 13);
            this.etykietaIlości.TabIndex = 5;
            this.etykietaIlości.Text = "Ilość neuronów:";
            // 
            // ilość
            // 
            this.ilość.Location = new System.Drawing.Point(622, 93);
            this.ilość.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.ilość.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.ilość.Name = "ilość";
            this.ilość.Size = new System.Drawing.Size(120, 20);
            this.ilość.TabIndex = 6;
            this.ilość.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // przyciskStopu
            // 
            this.przyciskStopu.Enabled = false;
            this.przyciskStopu.Location = new System.Drawing.Point(697, 119);
            this.przyciskStopu.Name = "przyciskStopu";
            this.przyciskStopu.Size = new System.Drawing.Size(75, 23);
            this.przyciskStopu.TabIndex = 7;
            this.przyciskStopu.Text = "Stop";
            this.przyciskStopu.UseVisualStyleBackColor = true;
            this.przyciskStopu.Click += new System.EventHandler(this.przyciskStopu_Click);
            // 
            // mapa
            // 
            this.mapa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapa.LiczbaNeuronów = 0;
            this.mapa.Location = new System.Drawing.Point(13, 13);
            this.mapa.Name = "mapa";
            this.mapa.Size = new System.Drawing.Size(600, 600);
            this.mapa.Stop = false;
            this.mapa.SzybkośćAnimacji = 0;
            this.mapa.TabIndex = 0;
            // 
            // Formularz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 661);
            this.Controls.Add(this.przyciskStopu);
            this.Controls.Add(this.ilość);
            this.Controls.Add(this.etykietaIlości);
            this.Controls.Add(this.przyciskStartu);
            this.Controls.Add(this.etykietaSzybkości);
            this.Controls.Add(this.suwak);
            this.Controls.Add(this.mapa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Formularz";
            this.Text = "Som";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.suwak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilość)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Mapa mapa;
        private System.Windows.Forms.TrackBar suwak;
        private System.Windows.Forms.Label etykietaSzybkości;
        private System.Windows.Forms.Button przyciskStartu;
        private System.Windows.Forms.Label etykietaIlości;
        private System.Windows.Forms.NumericUpDown ilość;
        private System.Windows.Forms.Button przyciskStopu;
    }
}

