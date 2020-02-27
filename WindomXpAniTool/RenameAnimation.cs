using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindomXpAniTool
{
    public partial class RenameAnimation : Form
    {
        public RenameAnimation()
        {
            InitializeComponent();
        }

        private void RenameAnimation_Load(object sender, EventArgs e)
        {

        }

        public void setTxtName(string text)
        {
            txtName.Text = text;
        }

        public string getTxtName()
        {
            return txtName.Text;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
