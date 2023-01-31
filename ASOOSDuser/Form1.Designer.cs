namespace ASOOSDuser
{
    partial class ASOOSDuser
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_search_options = new System.Windows.Forms.Label();
            this.tb_login = new System.Windows.Forms.TextBox();
            this.tb_last_name = new System.Windows.Forms.TextBox();
            this.cb_department = new System.Windows.Forms.ComboBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.dgv_result = new System.Windows.Forms.DataGridView();
            this.USER_LASTNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USER_FIRSTNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USER_SURNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USER_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEPARTMENT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_replic = new System.Windows.Forms.Button();
            this.btn_info = new System.Windows.Forms.Button();
            this.btn_role = new System.Windows.Forms.Button();
            this.chb_login = new System.Windows.Forms.CheckBox();
            this.chb_last_name = new System.Windows.Forms.CheckBox();
            this.chb_department = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_result)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_search_options
            // 
            this.lb_search_options.AutoSize = true;
            this.lb_search_options.Location = new System.Drawing.Point(42, 34);
            this.lb_search_options.Name = "lb_search_options";
            this.lb_search_options.Size = new System.Drawing.Size(135, 13);
            this.lb_search_options.TabIndex = 7;
            this.lb_search_options.Text = "Выберете способ поиска";
            // 
            // tb_login
            // 
            this.tb_login.AcceptsReturn = true;
            this.tb_login.Enabled = false;
            this.tb_login.Location = new System.Drawing.Point(79, 93);
            this.tb_login.Name = "tb_login";
            this.tb_login.Size = new System.Drawing.Size(171, 20);
            this.tb_login.TabIndex = 10;
            // 
            // tb_last_name
            // 
            this.tb_last_name.Enabled = false;
            this.tb_last_name.Location = new System.Drawing.Point(79, 161);
            this.tb_last_name.Name = "tb_last_name";
            this.tb_last_name.Size = new System.Drawing.Size(171, 20);
            this.tb_last_name.TabIndex = 4;
            // 
            // cb_department
            // 
            this.cb_department.Enabled = false;
            this.cb_department.FormattingEnabled = true;
            this.cb_department.IntegralHeight = false;
            this.cb_department.Location = new System.Drawing.Point(79, 229);
            this.cb_department.MaxDropDownItems = 10;
            this.cb_department.Name = "cb_department";
            this.cb_department.Size = new System.Drawing.Size(171, 21);
            this.cb_department.TabIndex = 6;
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(45, 275);
            this.btn_search.Name = "btn_search";
            this.btn_search.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_search.Size = new System.Drawing.Size(205, 37);
            this.btn_search.TabIndex = 1;
            this.btn_search.Text = "Искать";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // dgv_result
            // 
            this.dgv_result.AllowUserToAddRows = false;
            this.dgv_result.AllowUserToDeleteRows = false;
            this.dgv_result.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_result.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.USER_LASTNAME,
            this.USER_FIRSTNAME,
            this.USER_SURNAME,
            this.USER_NAME,
            this.DEPARTMENT_ID});
            this.dgv_result.Location = new System.Drawing.Point(295, 34);
            this.dgv_result.Name = "dgv_result";
            this.dgv_result.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_result.Size = new System.Drawing.Size(793, 216);
            this.dgv_result.TabIndex = 9;
            // 
            // USER_LASTNAME
            // 
            this.USER_LASTNAME.HeaderText = "Фамилия";
            this.USER_LASTNAME.Name = "USER_LASTNAME";
            this.USER_LASTNAME.ReadOnly = true;
            // 
            // USER_FIRSTNAME
            // 
            this.USER_FIRSTNAME.HeaderText = "Имя";
            this.USER_FIRSTNAME.Name = "USER_FIRSTNAME";
            this.USER_FIRSTNAME.ReadOnly = true;
            // 
            // USER_SURNAME
            // 
            this.USER_SURNAME.HeaderText = "Отчество";
            this.USER_SURNAME.Name = "USER_SURNAME";
            this.USER_SURNAME.ReadOnly = true;
            // 
            // USER_NAME
            // 
            this.USER_NAME.HeaderText = "Логин";
            this.USER_NAME.Name = "USER_NAME";
            this.USER_NAME.ReadOnly = true;
            // 
            // DEPARTMENT_ID
            // 
            this.DEPARTMENT_ID.HeaderText = "Подразделение";
            this.DEPARTMENT_ID.Name = "DEPARTMENT_ID";
            this.DEPARTMENT_ID.ReadOnly = true;
            // 
            // btn_replic
            // 
            this.btn_replic.Enabled = false;
            this.btn_replic.Location = new System.Drawing.Point(883, 275);
            this.btn_replic.Name = "btn_replic";
            this.btn_replic.Size = new System.Drawing.Size(205, 37);
            this.btn_replic.TabIndex = 2;
            this.btn_replic.Text = "Перенести пользователя";
            this.btn_replic.UseVisualStyleBackColor = true;
            this.btn_replic.Click += new System.EventHandler(this.btn_replic_Click);
            // 
            // btn_info
            // 
            this.btn_info.Enabled = false;
            this.btn_info.Location = new System.Drawing.Point(295, 275);
            this.btn_info.Name = "btn_info";
            this.btn_info.Size = new System.Drawing.Size(127, 37);
            this.btn_info.TabIndex = 11;
            this.btn_info.Text = "Полная информация";
            this.btn_info.UseVisualStyleBackColor = true;
            this.btn_info.Click += new System.EventHandler(this.btn_info_Click);
            // 
            // btn_role
            // 
            this.btn_role.Enabled = false;
            this.btn_role.Location = new System.Drawing.Point(428, 275);
            this.btn_role.Name = "btn_role";
            this.btn_role.Size = new System.Drawing.Size(127, 37);
            this.btn_role.TabIndex = 12;
            this.btn_role.Text = "Роли пользователя";
            this.btn_role.UseVisualStyleBackColor = true;
            this.btn_role.Click += new System.EventHandler(this.btn_role_Click);
            // 
            // chb_login
            // 
            this.chb_login.AutoSize = true;
            this.chb_login.Location = new System.Drawing.Point(45, 70);
            this.chb_login.Name = "chb_login";
            this.chb_login.Size = new System.Drawing.Size(113, 17);
            this.chb_login.TabIndex = 13;
            this.chb_login.Text = "искать по логину";
            this.chb_login.UseVisualStyleBackColor = true;
            this.chb_login.CheckedChanged += new System.EventHandler(this.chb_login_CheckedChanged);
            // 
            // chb_last_name
            // 
            this.chb_last_name.AutoSize = true;
            this.chb_last_name.Location = new System.Drawing.Point(45, 138);
            this.chb_last_name.Name = "chb_last_name";
            this.chb_last_name.Size = new System.Drawing.Size(125, 17);
            this.chb_last_name.TabIndex = 14;
            this.chb_last_name.Text = "искать по фамилии";
            this.chb_last_name.UseVisualStyleBackColor = true;
            this.chb_last_name.CheckedChanged += new System.EventHandler(this.chb_last_name_CheckedChanged);
            // 
            // chb_department
            // 
            this.chb_department.AutoSize = true;
            this.chb_department.Location = new System.Drawing.Point(45, 206);
            this.chb_department.Name = "chb_department";
            this.chb_department.Size = new System.Drawing.Size(159, 17);
            this.chb_department.TabIndex = 15;
            this.chb_department.Text = "искать по подразделению";
            this.chb_department.UseVisualStyleBackColor = true;
            this.chb_department.CheckedChanged += new System.EventHandler(this.chb_department_CheckedChanged);
            // 
            // ASOOSDuser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 348);
            this.Controls.Add(this.chb_department);
            this.Controls.Add(this.chb_last_name);
            this.Controls.Add(this.chb_login);
            this.Controls.Add(this.btn_role);
            this.Controls.Add(this.btn_info);
            this.Controls.Add(this.btn_replic);
            this.Controls.Add(this.dgv_result);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.cb_department);
            this.Controls.Add(this.tb_last_name);
            this.Controls.Add(this.tb_login);
            this.Controls.Add(this.lb_search_options);
            this.Name = "ASOOSDuser";
            this.Text = "Перенос пользователей";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_result)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_search_options;
        private System.Windows.Forms.TextBox tb_login;
        private System.Windows.Forms.TextBox tb_last_name;
        private System.Windows.Forms.ComboBox cb_department;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.DataGridView dgv_result;
        private System.Windows.Forms.Button btn_replic;
        private System.Windows.Forms.Button btn_info;
        private System.Windows.Forms.Button btn_role;
        private System.Windows.Forms.CheckBox chb_login;
        private System.Windows.Forms.CheckBox chb_last_name;
        private System.Windows.Forms.CheckBox chb_department;
        private System.Windows.Forms.DataGridViewTextBoxColumn USER_LASTNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn USER_FIRSTNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn USER_SURNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn USER_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn DEPARTMENT_ID;
    }
}

