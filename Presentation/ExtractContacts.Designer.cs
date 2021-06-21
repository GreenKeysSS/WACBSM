namespace Presentation
{
    partial class ExtractContacts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractContacts));
            this.panel1 = new System.Windows.Forms.Panel();
            this.extracteddgv = new System.Windows.Forms.DataGridView();
            this.extractbtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tosearchtxt = new System.Windows.Forms.TextBox();
            this.exportcsvbtn = new System.Windows.Forms.Button();
            this.tocontactsbtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extracteddgv)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.tocontactsbtn);
            this.panel1.Controls.Add(this.exportcsvbtn);
            this.panel1.Controls.Add(this.extracteddgv);
            this.panel1.Controls.Add(this.extractbtn);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tosearchtxt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 505);
            this.panel1.TabIndex = 0;
            // 
            // extracteddgv
            // 
            this.extracteddgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.extracteddgv.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.extracteddgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.extracteddgv.Location = new System.Drawing.Point(16, 130);
            this.extracteddgv.Name = "extracteddgv";
            this.extracteddgv.Size = new System.Drawing.Size(421, 320);
            this.extracteddgv.TabIndex = 35;
            this.extracteddgv.Visible = false;
            // 
            // extractbtn
            // 
            this.extractbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(94)))), ((int)(((byte)(171)))));
            this.extractbtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.extractbtn.FlatAppearance.BorderSize = 0;
            this.extractbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extractbtn.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extractbtn.ForeColor = System.Drawing.Color.White;
            this.extractbtn.Location = new System.Drawing.Point(250, 72);
            this.extractbtn.Margin = new System.Windows.Forms.Padding(5);
            this.extractbtn.Name = "extractbtn";
            this.extractbtn.Size = new System.Drawing.Size(187, 26);
            this.extractbtn.TabIndex = 34;
            this.extractbtn.Text = "Extraer";
            this.extractbtn.UseVisualStyleBackColor = false;
            this.extractbtn.Click += new System.EventHandler(this.extractbtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 18);
            this.label2.TabIndex = 32;
            this.label2.Text = "Grupo";
            // 
            // tosearchtxt
            // 
            this.tosearchtxt.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tosearchtxt.Location = new System.Drawing.Point(16, 36);
            this.tosearchtxt.Margin = new System.Windows.Forms.Padding(5);
            this.tosearchtxt.Name = "tosearchtxt";
            this.tosearchtxt.Size = new System.Drawing.Size(421, 26);
            this.tosearchtxt.TabIndex = 31;
            // 
            // exportcsvbtn
            // 
            this.exportcsvbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(140)))), ((int)(((byte)(126)))));
            this.exportcsvbtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.exportcsvbtn.FlatAppearance.BorderSize = 0;
            this.exportcsvbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportcsvbtn.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportcsvbtn.ForeColor = System.Drawing.Color.White;
            this.exportcsvbtn.Location = new System.Drawing.Point(313, 467);
            this.exportcsvbtn.Margin = new System.Windows.Forms.Padding(5);
            this.exportcsvbtn.Name = "exportcsvbtn";
            this.exportcsvbtn.Size = new System.Drawing.Size(125, 26);
            this.exportcsvbtn.TabIndex = 36;
            this.exportcsvbtn.Text = "Exportar CSV";
            this.exportcsvbtn.UseVisualStyleBackColor = false;
            this.exportcsvbtn.Click += new System.EventHandler(this.exportcsvbtn_Click);
            // 
            // tocontactsbtn
            // 
            this.tocontactsbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.tocontactsbtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.tocontactsbtn.FlatAppearance.BorderSize = 0;
            this.tocontactsbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tocontactsbtn.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tocontactsbtn.ForeColor = System.Drawing.Color.Black;
            this.tocontactsbtn.Location = new System.Drawing.Point(136, 467);
            this.tocontactsbtn.Margin = new System.Windows.Forms.Padding(5);
            this.tocontactsbtn.Name = "tocontactsbtn";
            this.tocontactsbtn.Size = new System.Drawing.Size(167, 26);
            this.tocontactsbtn.TabIndex = 36;
            this.tocontactsbtn.Text = "Agregar a Contactos";
            this.tocontactsbtn.UseVisualStyleBackColor = false;
            this.tocontactsbtn.Click += new System.EventHandler(this.tocontactsbtn_Click);
            // 
            // ExtractContacts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 505);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "ExtractContacts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extraer Contactos";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extracteddgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button extractbtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tosearchtxt;
        public System.Windows.Forms.DataGridView extracteddgv;
        private System.Windows.Forms.Button tocontactsbtn;
        private System.Windows.Forms.Button exportcsvbtn;
    }
}