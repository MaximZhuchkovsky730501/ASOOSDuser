namespace ASOOSDuser
{
    partial class RoleForm
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
            this.dgv_roles = new System.Windows.Forms.DataGridView();
            this.ROLE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROLE_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_roles)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_roles
            // 
            this.dgv_roles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_roles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_roles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ROLE_ID,
            this.ROLE_NAME});
            this.dgv_roles.Location = new System.Drawing.Point(12, 11);
            this.dgv_roles.Name = "dgv_roles";
            this.dgv_roles.Size = new System.Drawing.Size(379, 327);
            this.dgv_roles.TabIndex = 0;
            // 
            // ROLE_ID
            // 
            this.ROLE_ID.FillWeight = 50.76143F;
            this.ROLE_ID.HeaderText = "ROLE_ID";
            this.ROLE_ID.Name = "ROLE_ID";
            this.ROLE_ID.ReadOnly = true;
            // 
            // ROLE_NAME
            // 
            this.ROLE_NAME.FillWeight = 149.2386F;
            this.ROLE_NAME.HeaderText = "ROLE_NAME";
            this.ROLE_NAME.Name = "ROLE_NAME";
            this.ROLE_NAME.ReadOnly = true;
            // 
            // RoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 350);
            this.Controls.Add(this.dgv_roles);
            this.Name = "RoleForm";
            this.Text = "Роли полльзователя";
            this.Load += new System.EventHandler(this.RoleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_roles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_roles;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROLE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROLE_NAME;
    }
}