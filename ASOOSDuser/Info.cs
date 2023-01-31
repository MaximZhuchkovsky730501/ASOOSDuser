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
    public partial class InfoForm : Form
    {
        public User selected_user;

        public InfoForm()
        {
            InitializeComponent();
        }

        public InfoForm(User user)
        {
            selected_user = user;
            InitializeComponent();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            init_dgv_info(selected_user);
        }

        private void init_dgv_info(User user)
        {
            dgv_info.RowCount = 16;
            for (int i = 0; i < 16; i++)
            {
                String[] row = { User.columns[i], user.data[i] };
                dgv_info.Rows[i].SetValues(row);
            }
        }
    }
}
