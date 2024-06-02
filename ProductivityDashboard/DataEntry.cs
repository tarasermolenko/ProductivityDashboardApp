using DashLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Windows.Forms.VisualStyles;


namespace ProductivityDashboard
{
    public partial class DataEntry : Form
    {
        BindingList<TaskModel> tasks = new BindingList<TaskModel>();
        BindingList<ReminderModel> reminders = new BindingList<ReminderModel>();
        List<DashLibrary.CheckBoxState> checkBoxStates = new List<DashLibrary.CheckBoxState>();
        
        private const string CheckboxesFileName = "checkboxes.json";
        private const string TasksFileName = "tasks.json";
        private const string RemindersFileName = "reminders.json";

        public DataEntry()
        {
            InitializeComponent();
            LoadData();
            WireUpList();
        }
        private void LoadData()
        {
            if (File.Exists(TasksFileName))
            {
                var tasksJson = File.ReadAllText(TasksFileName);
                tasks = JsonSerializer.Deserialize<BindingList<TaskModel>>(tasksJson);
            }

            if (File.Exists(RemindersFileName))
            {
                var remindersJson = File.ReadAllText(RemindersFileName);
                reminders = JsonSerializer.Deserialize<BindingList<ReminderModel>>(remindersJson);
            }

            // Load checkbox states
            checkBoxStates = LoadCheckboxStates();
            InitializeCheckboxStates();

        }

        private List<DashLibrary.CheckBoxState> LoadCheckboxStates()
        {
            if (File.Exists(CheckboxesFileName))
            {
                string checkboxesJson = File.ReadAllText(CheckboxesFileName);
                return JsonSerializer.Deserialize<List<DashLibrary.CheckBoxState>>(checkboxesJson);
            }
            return new List<DashLibrary.CheckBoxState>();
        }

        private void InitializeCheckboxStates()
        {
            foreach (var control in this.Controls)
            {
                if (control is CheckBox checkbox)
                {
                    var state = checkBoxStates.Find(c => c.Name == checkbox.Name);
                    if (state != null)
                    {
                        checkbox.Checked = state.IsChecked;
                    }
                }
            }
        }

        private void WireUpList() 
        {

            TaskList.DataSource = tasks;
            ReminderList.DataSource = reminders;

            TaskList.DisplayMember = nameof(TaskModel.task);
            ReminderList.DisplayMember = nameof(ReminderModel.reminder);

        }


        private void TaskAddButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskText.Text))
            {
                MessageBox.Show("Enter A Task To Add", "Blank Message Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                tasks.Add(new TaskModel(TaskText.Text));
                TaskText.Text = "";
            }

            TaskText.Focus(); // put cursor back into bar
        }

        private void ReminderAddButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReminderText.Text))
            {
                MessageBox.Show("Enter A Task To Add", "Blank Message Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                reminders.Add(new ReminderModel(ReminderText.Text));
                ReminderText.Text = "";
            }

            ReminderText.Focus();
        }

        private void SaveData()
        {
            var tasksJson = JsonSerializer.Serialize(tasks);
            File.WriteAllText(TasksFileName, tasksJson);

            var remindersJson = JsonSerializer.Serialize(reminders);
            File.WriteAllText(RemindersFileName, remindersJson);

            // Save checkbox states
            checkBoxStates.Clear();
            foreach (var control in this.Controls)
            {
                if (control is CheckBox checkbox)
                {
                    checkBoxStates.Add(new DashLibrary.CheckBoxState(checkbox.Name, checkbox.Checked));
                }
            }
            string checkboxesJson = JsonSerializer.Serialize(checkBoxStates);
            File.WriteAllText(CheckboxesFileName, checkboxesJson);

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveData();
        }
    }
}
