
namespace MediaTekDocuments.view
{
    partial class ExpirationForm
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
            this.btnFermerFenetre = new System.Windows.Forms.Button();
            this.dgvExpiration = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpiration)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFermerFenetre
            // 
            this.btnFermerFenetre.Location = new System.Drawing.Point(633, 390);
            this.btnFermerFenetre.Name = "btnFermerFenetre";
            this.btnFermerFenetre.Size = new System.Drawing.Size(155, 39);
            this.btnFermerFenetre.TabIndex = 0;
            this.btnFermerFenetre.Text = "OK";
            this.btnFermerFenetre.UseVisualStyleBackColor = true;
            this.btnFermerFenetre.Click += new System.EventHandler(this.btnFermerFenetre_Click);
            // 
            // dgvExpiration
            // 
            this.dgvExpiration.AllowUserToAddRows = false;
            this.dgvExpiration.AllowUserToDeleteRows = false;
            this.dgvExpiration.AllowUserToOrderColumns = true;
            this.dgvExpiration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExpiration.Location = new System.Drawing.Point(0, 0);
            this.dgvExpiration.Name = "dgvExpiration";
            this.dgvExpiration.ReadOnly = true;
            this.dgvExpiration.RowHeadersWidth = 62;
            this.dgvExpiration.RowTemplate.Height = 28;
            this.dgvExpiration.Size = new System.Drawing.Size(788, 366);
            this.dgvExpiration.TabIndex = 1;
            // 
            // ExpirationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvExpiration);
            this.Controls.Add(this.btnFermerFenetre);
            this.Name = "ExpirationForm";
            this.Text = "Abonnements bientôt expirés";
            this.Load += new System.EventHandler(this.ExpirationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpiration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFermerFenetre;
        private System.Windows.Forms.DataGridView dgvExpiration;
    }
}