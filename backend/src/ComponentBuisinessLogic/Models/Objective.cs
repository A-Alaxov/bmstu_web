using NodaTime;
using System;
using System.Collections.Generic;

#nullable disable

namespace ComponentBuisinessLogic
{
    public class Objective
    {
        public Objective(int _objectiveid,
            int? _parentobjective, 
            string _title, 
            int _company,
            int? _department,
            DateTime _termbegin,
            DateTime _termend,
            TimeSpan _estimatedtime)
        {
            Objectiveid = _objectiveid;
            Parentobjective = _parentobjective;
            Title = _title;
            Company = _company;
            Department = _department;
            Termbegin = _termbegin;
            Termend = _termend;
            Estimatedtime = _estimatedtime;
        }

        public int Objectiveid { get; set; }
        public int? Parentobjective { get; set; }
        public string Title { get; set; }
        public int Company { get; set; }
        public int? Department { get; set; }
        public DateTime Termbegin { get; set; }
        public DateTime Termend { get; set; }
        public TimeSpan Estimatedtime { get; set; }
    }
}
