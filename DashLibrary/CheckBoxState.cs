using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLibrary
{
    public class CheckBoxState
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public CheckBoxState(string name, bool isChecked)
        {
            Name = name;
            IsChecked = isChecked;
        }
    }
}
