using System;

#nullable disable

namespace ComponentBuisinessLogic
{
    public class Responsibility 
    {
        public Responsibility(int _responsibilityid, int _employee, int _objective, TimeSpan _timespent)
        {
            Responsibilityid = _responsibilityid;
            Employee = _employee;
            Objective = _objective;
            Timespent = _timespent;
        }

        public int Responsibilityid { get; set; }
        public int Employee { get; set; }
        public int Objective { get; set; }
        public TimeSpan Timespent { get; set; }
    }
}
