using DashLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProductivityDashboard
{
    public partial class DataEntry : Form
    {
        BindingList<TaskModel> tasks = new BindingList<TaskModel>();

        public DataEntry()
        {
            InitializeComponent();

            TaskList.DataSource = tasks;
        }

        private void TaskList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
