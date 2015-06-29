namespace Hopfield
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
            this._dodawanieWektoraUczącego = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._liczbaWzorców = new System.Windows.Forms.Label();
            this._usuwanieWzorców = new System.Windows.Forms.Button();
            this._koniecUczenia = new System.Windows.Forms.Button();
            this._symulacja = new System.Windows.Forms.Button();
            this._nowyWektorTestowy = new System.Windows.Forms.Button();
            this._anulujWszystko = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _dodawanieWektoraUczącego
            // 
            this._dodawanieWektoraUczącego.AutoSize = true;
            this._dodawanieWektoraUczącego.Location = new System.Drawing.Point(185, 12);
            this._dodawanieWektoraUczącego.Name = "_dodawanieWektoraUczącego";
            this._dodawanieWektoraUczącego.Size = new System.Drawing.Size(87, 23);
            this._dodawanieWektoraUczącego.TabIndex = 0;
            this._dodawanieWektoraUczącego.Text = "Dodaj wzorzec";
            this._dodawanieWektoraUczącego.UseVisualStyleBackColor = true;
            this._dodawanieWektoraUczącego.Click += new System.EventHandler(this._dodawanieWektoraUczącego_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(278, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Liczba zapamiętanych wzorców: ";
            // 
            // _liczbaWzorców
            // 
            this._liczbaWzorców.AutoSize = true;
            this._liczbaWzorców.Location = new System.Drawing.Point(278, 30);
            this._liczbaWzorców.Name = "_liczbaWzorców";
            this._liczbaWzorców.Size = new System.Drawing.Size(13, 13);
            this._liczbaWzorców.TabIndex = 2;
            this._liczbaWzorców.Text = "0";
            // 
            // _usuwanieWzorców
            // 
            this._usuwanieWzorców.AutoSize = true;
            this._usuwanieWzorców.Location = new System.Drawing.Point(185, 41);
            this._usuwanieWzorców.Name = "_usuwanieWzorców";
            this._usuwanieWzorców.Size = new System.Drawing.Size(79, 23);
            this._usuwanieWzorców.TabIndex = 3;
            this._usuwanieWzorców.Text = "Usuń wzorce";
            this._usuwanieWzorców.UseVisualStyleBackColor = true;
            this._usuwanieWzorców.Click += new System.EventHandler(this._usuwanieWzorców_Click);
            // 
            // _koniecUczenia
            // 
            this._koniecUczenia.AutoSize = true;
            this._koniecUczenia.Location = new System.Drawing.Point(185, 70);
            this._koniecUczenia.Name = "_koniecUczenia";
            this._koniecUczenia.Size = new System.Drawing.Size(92, 23);
            this._koniecUczenia.TabIndex = 4;
            this._koniecUczenia.Text = "Zakończ naukę";
            this._koniecUczenia.UseVisualStyleBackColor = true;
            this._koniecUczenia.Click += new System.EventHandler(this._koniecUczenia_Click);
            // 
            // _symulacja
            // 
            this._symulacja.Enabled = false;
            this._symulacja.Location = new System.Drawing.Point(185, 99);
            this._symulacja.Name = "_symulacja";
            this._symulacja.Size = new System.Drawing.Size(75, 23);
            this._symulacja.TabIndex = 5;
            this._symulacja.Text = "Start";
            this._symulacja.UseVisualStyleBackColor = true;
            this._symulacja.Click += new System.EventHandler(this._symulacja_Click);
            // 
            // _nowyWektorTestowy
            // 
            this._nowyWektorTestowy.AutoSize = true;
            this._nowyWektorTestowy.Enabled = false;
            this._nowyWektorTestowy.Location = new System.Drawing.Point(185, 129);
            this._nowyWektorTestowy.Name = "_nowyWektorTestowy";
            this._nowyWektorTestowy.Size = new System.Drawing.Size(118, 23);
            this._nowyWektorTestowy.TabIndex = 6;
            this._nowyWektorTestowy.Text = "Nowy wektor testowy";
            this._nowyWektorTestowy.UseVisualStyleBackColor = true;
            this._nowyWektorTestowy.Click += new System.EventHandler(this._nowyWektorTestowy_Click);
            // 
            // _anulujWszystko
            // 
            this._anulujWszystko.AutoSize = true;
            this._anulujWszystko.Location = new System.Drawing.Point(185, 158);
            this._anulujWszystko.Name = "_anulujWszystko";
            this._anulujWszystko.Size = new System.Drawing.Size(107, 23);
            this._anulujWszystko.TabIndex = 7;
            this._anulujWszystko.Text = "Wszystko od nowa";
            this._anulujWszystko.UseVisualStyleBackColor = true;
            this._anulujWszystko.Click += new System.EventHandler(this._anulujWszystko_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 211);
            this.Controls.Add(this._anulujWszystko);
            this.Controls.Add(this._nowyWektorTestowy);
            this.Controls.Add(this._symulacja);
            this.Controls.Add(this._koniecUczenia);
            this.Controls.Add(this._usuwanieWzorców);
            this.Controls.Add(this._liczbaWzorców);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._dodawanieWektoraUczącego);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Hopfield";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _dodawanieWektoraUczącego;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _liczbaWzorców;
        private System.Windows.Forms.Button _usuwanieWzorców;
        private System.Windows.Forms.Button _koniecUczenia;
        private System.Windows.Forms.Button _symulacja;
        private System.Windows.Forms.Button _nowyWektorTestowy;
        private System.Windows.Forms.Button _anulujWszystko;
    }
}

