using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLibrary
{
    public class ReminderModel
    {
        public string reminder { get; set; }

        public ReminderModel(string reminder)
        {
            this.reminder = reminder;
        }

    }
}
