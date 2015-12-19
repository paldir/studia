namespace Gui
{
    partial class Start
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
            this._liczbaElementów = new System.Windows.Forms.NumericUpDown();
            this._dalej = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._liczbaGrup = new System.Windows.Forms.NumericUpDown();
            this._losowa = new System.Windows.Forms.RadioButton();
            this._odwrotna = new System.Windows.Forms.RadioButton();
            this._kolekcja = new System.Windows.Forms.GroupBox();
            this._wstępnie = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this._liczbaElementów)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._liczbaGrup)).BeginInit();
            this._kolekcja.SuspendLayout();
            this.SuspendLayout();
            // 
            // _liczbaElementów
            // 
            this._liczbaElementów.Location = new System.Drawing.Point(12, 29);
            this._liczbaElementów.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this._liczbaElementów.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._liczbaElementów.Name = "_liczbaElementów";
            this._liczbaElementów.Size = new System.Drawing.Size(120, 20);
            this._liczbaElementów.TabIndex = 0;
            this._liczbaElementów.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this._liczbaElementów.ValueChanged += new System.EventHandler(this._liczbaElementów_ValueChanged);
            // 
            // _dalej
            // 
            this._dalej.Location = new System.Drawing.Point(12, 161);
            this._dalej.Name = "_dalej";
            this._dalej.Size = new System.Drawing.Size(75, 23);
            this._dalej.TabIndex = 1;
            this._dalej.Text = "OK";
            this._dalej.UseVisualStyleBackColor = true;
            this._dalej.Click += new System.EventHandler(this._dalej_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Liczba elementów:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Liczba grup:";
            // 
            // _liczbaGrup
            // 
            this._liczbaGrup.Location = new System.Drawing.Point(138, 29);
            this._liczbaGrup.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._liczbaGrup.Name = "_liczbaGrup";
            this._liczbaGrup.Size = new System.Drawing.Size(120, 20);
            this._liczbaGrup.TabIndex = 4;
            this._liczbaGrup.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // _losowa
            // 
            this._losowa.AutoSize = true;
            this._losowa.Checked = true;
            this._losowa.Location = new System.Drawing.Point(6, 19);
            this._losowa.Name = "_losowa";
            this._losowa.Size = new System.Drawing.Size(62, 17);
            this._losowa.TabIndex = 5;
            this._losowa.TabStop = true;
            this._losowa.Tag = "";
            this._losowa.Text = "Losowa";
            this._losowa.UseVisualStyleBackColor = true;
            // 
            // _odwrotna
            // 
            this._odwrotna.AutoSize = true;
            this._odwrotna.Location = new System.Drawing.Point(6, 42);
            this._odwrotna.Name = "_odwrotna";
            this._odwrotna.Size = new System.Drawing.Size(137, 17);
            this._odwrotna.TabIndex = 6;
            this._odwrotna.Text = "Odwrotnie posortowana";
            this._odwrotna.UseVisualStyleBackColor = true;
            // 
            // _kolekcja
            // 
            this._kolekcja.Controls.Add(this._wstępnie);
            this._kolekcja.Controls.Add(this._losowa);
            this._kolekcja.Controls.Add(this._odwrotna);
            this._kolekcja.Location = new System.Drawing.Point(12, 55);
            this._kolekcja.Name = "_kolekcja";
            this._kolekcja.Size = new System.Drawing.Size(200, 100);
            this._kolekcja.TabIndex = 7;
            this._kolekcja.TabStop = false;
            this._kolekcja.Text = "Kolekcja";
            // 
            // _wstępnie
            // 
            this._wstępnie.AutoSize = true;
            this._wstępnie.Location = new System.Drawing.Point(6, 65);
            this._wstępnie.Name = "_wstępnie";
            this._wstępnie.Size = new System.Drawing.Size(134, 17);
            this._wstępnie.TabIndex = 7;
            this._wstępnie.TabStop = true;
            this._wstępnie.Text = "Wstępnie posortowana";
            this._wstępnie.UseVisualStyleBackColor = true;
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 189);
            this.Controls.Add(this._kolekcja);
            this.Controls.Add(this._liczbaGrup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._dalej);
            this.Controls.Add(this._liczbaElementów);
            this.Name = "Start";
            this.Text = "Start";
            ((System.ComponentModel.ISupportInitialize)(this._liczbaElementów)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._liczbaGrup)).EndInit();
            this._kolekcja.ResumeLayout(false);
            this._kolekcja.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown _liczbaElementów;
        private System.Windows.Forms.Button _dalej;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _liczbaGrup;
        private System.Windows.Forms.RadioButton _losowa;
        private System.Windows.Forms.RadioButton _odwrotna;
        private System.Windows.Forms.GroupBox _kolekcja;
        private System.Windows.Forms.RadioButton _wstępnie;
    }
}