namespace HORAS_LOCLES
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            button1 = new Button();
            groupBox1 = new GroupBox();
            txt_observacion = new TextBox();
            label2 = new Label();
            txt_cedula = new TextBox();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Blue;
            label1.Location = new Point(3, 30);
            label1.Name = "label1";
            label1.Size = new Size(127, 17);
            label1.TabIndex = 0;
            label1.Text = "Número de Cédula:";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(287, 24);
            button1.Name = "button1";
            button1.Size = new Size(80, 29);
            button1.TabIndex = 1;
            button1.Text = "&Marcar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.ButtonHighlight;
            groupBox1.Controls.Add(txt_observacion);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txt_cedula);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button1);
            groupBox1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(1, 80);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(372, 136);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Registro";
            // 
            // txt_observacion
            // 
            txt_observacion.Location = new Point(106, 57);
            txt_observacion.Multiline = true;
            txt_observacion.Name = "txt_observacion";
            txt_observacion.Size = new Size(261, 73);
            txt_observacion.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Blue;
            label2.Location = new Point(14, 59);
            label2.Name = "label2";
            label2.Size = new Size(89, 17);
            label2.TabIndex = 3;
            label2.Text = "Observación:";
            // 
            // txt_cedula
            // 
            txt_cedula.Location = new Point(133, 26);
            txt_cedula.Name = "txt_cedula";
            txt_cedula.Size = new Size(148, 25);
            txt_cedula.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.dunkin_logo;
            pictureBox1.Location = new Point(4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(134, 74);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 0, 192);
            label3.Location = new Point(139, 25);
            label3.Name = "label3";
            label3.Size = new Size(229, 25);
            label3.TabIndex = 4;
            label3.Text = "Registro de Marcaciones";
            label3.Click += label3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(375, 217);
            Controls.Add(label3);
            Controls.Add(pictureBox1);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Marcaciones";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private GroupBox groupBox1;
        private TextBox txt_cedula;
        private Label label2;
        private TextBox txt_observacion;
        private PictureBox pictureBox1;
        private Label label3;
    }
}
