namespace emulador
{
    partial class FormConfigConex
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
            this.label1 = new System.Windows.Forms.Label();
            this.cb_COM = new System.Windows.Forms.ComboBox();
            this.cb_baudrate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_databits = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_stopbit = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_paridade = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Porta";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // cb_COM
            // 
            this.cb_COM.ForeColor = System.Drawing.SystemColors.InfoText;
            this.cb_COM.FormattingEnabled = true;
            this.cb_COM.Location = new System.Drawing.Point(73, 39);
            this.cb_COM.Name = "cb_COM";
            this.cb_COM.Size = new System.Drawing.Size(121, 21);
            this.cb_COM.TabIndex = 2;
            // 
            // cb_baudrate
            // 
            this.cb_baudrate.ForeColor = System.Drawing.SystemColors.InfoText;
            this.cb_baudrate.FormattingEnabled = true;
            this.cb_baudrate.Location = new System.Drawing.Point(73, 66);
            this.cb_baudrate.Name = "cb_baudrate";
            this.cb_baudrate.Size = new System.Drawing.Size(121, 21);
            this.cb_baudrate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "BaudRate";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cb_databits
            // 
            this.cb_databits.ForeColor = System.Drawing.SystemColors.InfoText;
            this.cb_databits.FormattingEnabled = true;
            this.cb_databits.Location = new System.Drawing.Point(73, 93);
            this.cb_databits.Name = "cb_databits";
            this.cb_databits.Size = new System.Drawing.Size(121, 21);
            this.cb_databits.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data bits";
            // 
            // cb_stopbit
            // 
            this.cb_stopbit.ForeColor = System.Drawing.SystemColors.InfoText;
            this.cb_stopbit.FormattingEnabled = true;
            this.cb_stopbit.Location = new System.Drawing.Point(73, 120);
            this.cb_stopbit.Name = "cb_stopbit";
            this.cb_stopbit.Size = new System.Drawing.Size(121, 21);
            this.cb_stopbit.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Stop bit";
            // 
            // cb_paridade
            // 
            this.cb_paridade.ForeColor = System.Drawing.SystemColors.InfoText;
            this.cb_paridade.FormattingEnabled = true;
            this.cb_paridade.Location = new System.Drawing.Point(73, 147);
            this.cb_paridade.Name = "cb_paridade";
            this.cb_paridade.Size = new System.Drawing.Size(121, 21);
            this.cb_paridade.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Paridade";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Parâmetros da conexão serial:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(73, 186);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "CONFIGURAR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormConfigConex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 221);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cb_paridade);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cb_stopbit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cb_databits);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cb_baudrate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_COM);
            this.Controls.Add(this.label1);
            this.Name = "FormConfigConex";
            this.Text = "Conexão";
            this.Load += new System.EventHandler(this.FormConfigConex_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_COM;
        private System.Windows.Forms.ComboBox cb_baudrate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_databits;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_stopbit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_paridade;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.IO.Ports.SerialPort serialPort1;
    }
}