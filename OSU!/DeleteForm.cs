using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OSU_
{
    public partial class DeleteForm : Form
    {
        DatabaseConnection dc = new DatabaseConnection();
        int idNum;

        public DeleteForm(int idnum)
        {
            InitializeComponent();
            idNum = idnum;

        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            string db = "osu";

            string delquery = "DELETE FROM `songs` WHERE ID = " + idNum;
            dc.UpdateDB(db, delquery);

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
