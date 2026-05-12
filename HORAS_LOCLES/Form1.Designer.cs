namespace HORAS_LOCLES
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.txt_cedula = new System.Windows.Forms.TextBox();
            this.txt_token = new System.Windows.Forms.TextBox();
            this.txt_observacion = new System.Windows.Forms.TextBox();

            this.btnEntrada = new System.Windows.Forms.Button();
            this.btnSalida = new System.Windows.Forms.Button();
            this.btnSalidaPartido = new System.Windows.Forms.Button();
            this.btnEntradaPartido = new System.Windows.Forms.Button();
            this.btnAlmuerzoSalida = new System.Windows.Forms.Button();
            this.btnAlmuerzoEntrada = new System.Windows.Forms.Button();

            this.lblCedula = new System.Windows.Forms.Label();
            this.lblToken = new System.Windows.Forms.Label();
            this.lblObs = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // txt_cedula
            this.txt_cedula.Location = new System.Drawing.Point(24, 52);
            this.txt_cedula.Name = "txt_cedula";
            this.txt_cedula.Size = new System.Drawing.Size(240, 23);
            this.txt_cedula.TabIndex = 0;

            // txt_token
            this.txt_token.Location = new System.Drawing.Point(24, 112);
            this.txt_token.Name = "txt_token";
            this.txt_token.Size = new System.Drawing.Size(240, 23);
            this.txt_token.TabIndex = 1;
            this.txt_token.MaxLength = 6;

            // txt_observacion
            this.txt_observacion.Location = new System.Drawing.Point(24, 172);
            this.txt_observacion.Multiline = true;
            this.txt_observacion.Name = "txt_observacion";
            this.txt_observacion.Size = new System.Drawing.Size(380, 70);
            this.txt_observacion.TabIndex = 2;

            // btnEntrada
            this.btnEntrada.Location = new System.Drawing.Point(24, 260);
            this.btnEntrada.Name = "btnEntrada";
            this.btnEntrada.Size = new System.Drawing.Size(120, 32);
            this.btnEntrada.TabIndex = 3;
            this.btnEntrada.Text = "Entrada";
            this.btnEntrada.UseVisualStyleBackColor = true;
            this.btnEntrada.Click += new System.EventHandler(this.btnEntrada_Click);

            // btnSalida
            this.btnSalida.Location = new System.Drawing.Point(164, 260);
            this.btnSalida.Name = "btnSalida";
            this.btnSalida.Size = new System.Drawing.Size(120, 32);
            this.btnSalida.TabIndex = 4;
            this.btnSalida.Text = "Salida";
            this.btnSalida.UseVisualStyleBackColor = true;
            this.btnSalida.Click += new System.EventHandler(this.btnSalida_Click);

            // btnSalidaPartido
            this.btnSalidaPartido.Location = new System.Drawing.Point(24, 308);
            this.btnSalidaPartido.Name = "btnSalidaPartido";
            this.btnSalidaPartido.Size = new System.Drawing.Size(180, 32);
            this.btnSalidaPartido.TabIndex = 5;
            this.btnSalidaPartido.Text = "Salida Turno partido";
            this.btnSalidaPartido.UseVisualStyleBackColor = true;
            this.btnSalidaPartido.Click += new System.EventHandler(this.btnSalidaPartido_Click);

            // btnEntradaPartido
            this.btnEntradaPartido.Location = new System.Drawing.Point(224, 308);
            this.btnEntradaPartido.Name = "btnEntradaPartido";
            this.btnEntradaPartido.Size = new System.Drawing.Size(180, 32);
            this.btnEntradaPartido.TabIndex = 6;
            this.btnEntradaPartido.Text = "Entrada Turno partido";
            this.btnEntradaPartido.UseVisualStyleBackColor = true;
            this.btnEntradaPartido.Click += new System.EventHandler(this.btnEntradaPartido_Click);

            // btnAlmuerzoSalida
            this.btnAlmuerzoSalida.Location = new System.Drawing.Point(24, 356);
            this.btnAlmuerzoSalida.Name = "btnAlmuerzoSalida";
            this.btnAlmuerzoSalida.Size = new System.Drawing.Size(180, 32);
            this.btnAlmuerzoSalida.TabIndex = 7;
            this.btnAlmuerzoSalida.Text = "Almuerzo Salida";
            this.btnAlmuerzoSalida.UseVisualStyleBackColor = true;
            this.btnAlmuerzoSalida.Click += new System.EventHandler(this.btnAlmuerzoSalida_Click);

            // btnAlmuerzoEntrada
            this.btnAlmuerzoEntrada.Location = new System.Drawing.Point(224, 356);
            this.btnAlmuerzoEntrada.Name = "btnAlmuerzoEntrada";
            this.btnAlmuerzoEntrada.Size = new System.Drawing.Size(180, 32);
            this.btnAlmuerzoEntrada.TabIndex = 8;
            this.btnAlmuerzoEntrada.Text = "Almuerzo Entrada";
            this.btnAlmuerzoEntrada.UseVisualStyleBackColor = true;
            this.btnAlmuerzoEntrada.Click += new System.EventHandler(this.btnAlmuerzoEntrada_Click);

            // lblCedula
            this.lblCedula.AutoSize = true;
            this.lblCedula.Location = new System.Drawing.Point(24, 32);
            this.lblCedula.Name = "lblCedula";
            this.lblCedula.Size = new System.Drawing.Size(110, 15);
            this.lblCedula.TabIndex = 9;
            this.lblCedula.Text = "Número de cédula:";

            // lblToken
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(24, 92);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(132, 15);
            this.lblToken.TabIndex = 10;
            this.lblToken.Text = "Código Authenticator:";

            // lblObs
            this.lblObs.AutoSize = true;
            this.lblObs.Location = new System.Drawing.Point(24, 152);
            this.lblObs.Name = "lblObs";
            this.lblObs.Size = new System.Drawing.Size(76, 15);
            this.lblObs.TabIndex = 11;
            this.lblObs.Text = "Observación:";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 410);

            this.Controls.Add(this.lblObs);
            this.Controls.Add(this.lblToken);
            this.Controls.Add(this.lblCedula);

            this.Controls.Add(this.btnAlmuerzoEntrada);
            this.Controls.Add(this.btnAlmuerzoSalida);
            this.Controls.Add(this.btnEntradaPartido);
            this.Controls.Add(this.btnSalidaPartido);
            this.Controls.Add(this.btnSalida);
            this.Controls.Add(this.btnEntrada);

            this.Controls.Add(this.txt_observacion);
            this.Controls.Add(this.txt_token);
            this.Controls.Add(this.txt_cedula);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Marcaciones";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txt_cedula;
        private System.Windows.Forms.TextBox txt_token;
        private System.Windows.Forms.TextBox txt_observacion;

        private System.Windows.Forms.Button btnEntrada;
        private System.Windows.Forms.Button btnSalida;
        private System.Windows.Forms.Button btnSalidaPartido;
        private System.Windows.Forms.Button btnEntradaPartido;
        private System.Windows.Forms.Button btnAlmuerzoSalida;
        private System.Windows.Forms.Button btnAlmuerzoEntrada;

        private System.Windows.Forms.Label lblCedula;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.Label lblObs;
    }
}
