using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASOOSDuser
{
    public partial class RoleForm : Form
    {
        Dictionary<String,String> roles_names;

        public RoleForm()
        {
            InitializeComponent();
        }
         public RoleForm(Dictionary<String,String> rn)
        {
            roles_names = rn;
            InitializeComponent();
        }
        
        private void init_dgv_roles(Dictionary<String, String> names)
        {
            int i = 0;
            dgv_roles.RowCount = names.Count;
            foreach(String key in names.Keys)
            {
                String[] row = {key, names[key].ToString()};
                dgv_roles.Rows[i].SetValues(row);
                i++;
            }
        }

        private void RoleForm_Load(object sender, EventArgs e)
        {
            init_dgv_roles(roles_names);
        }
    }
}
