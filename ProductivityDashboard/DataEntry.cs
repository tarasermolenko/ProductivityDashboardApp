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
            this.KeyPreview = true;  // will handle key events even if a specific control, such as a ListBox, is currently focused.
            LoadData();
            WireUpList();
            WireUpEvents();
            this.FormClosing += new FormClosingEventHandler(DataEntry_FormClosing);

        }

        private void WireUpEvents()
        {
            TaskList.KeyDown += new KeyEventHandler(TaskList_KeyDown);
            ReminderList.KeyDown += new KeyEventHandler(ReminderList_KeyDown);

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

        private void TaskList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (TaskList.SelectedItem != null)
                {
                    TaskModel selectedTask = (TaskModel)TaskList.SelectedItem;
                    tasks.Remove(selectedTask);
                    SaveData(); 
                }
            }
        }

        private void ReminderList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (ReminderList.SelectedItem != null)
                {
                    ReminderModel selectedReminder = (ReminderModel)ReminderList.SelectedItem;
                    reminders.Remove(selectedReminder);
                    SaveData(); 
                }
            }
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

        private void DataEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }

        private void ClearCheckListButton_Click(object sender, EventArgs e)
        {
            foreach (var control in this.Controls)
            {
                if (control is CheckBox checkbox && checkbox.Name.StartsWith("Daily"))
                {
                    checkbox.Checked = false;
                }
            }

            SaveData(); 
        }

        private void ClearWeeklyButton_Click(object sender, EventArgs e)
        {
            foreach (var control in this.Controls)
            {
                if (control is CheckBox checkbox && !checkbox.Name.StartsWith("Daily"))
                {
                    checkbox.Checked = false;
                }
            }
            SaveData();
        }
    }
}
