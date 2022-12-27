#nullable disable

using System;

namespace ComponentBuisinessLogic
{
    public class ResponsibilityView
    {
        public ResponsibilityView(string _employee, string _objective, TimeSpan _timespent)
        {
            Employee = _employee;
            Objective = _objective;
            Timespent = _timespent;
        }

        public string Employee { get; set; }
        public string Objective { get; set; }
        public TimeSpan Timespent { get; set; }
    }
}
