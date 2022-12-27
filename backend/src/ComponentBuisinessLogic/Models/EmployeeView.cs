#nullable disable

using System;

namespace ComponentBuisinessLogic
{
    public class EmployeeView
    {
        public EmployeeView(int _employeeid = 0,
            string _login = "",
            string _name_ = "",
            string _surname = null,
            string _department = null,
            int _permission_ = 0)
        {
            Employeeid = _employeeid;
            Login = _login;
            Name_ = _name_;
            Surname = _surname;
            Department = _department;
            Permission_ = _permission_ switch
            {
                0 => "Employee",
                1 => "Responsible",
                2 => "Manager",
                3 => "HR",
                4 => "Founder",
                _ => throw new Exception()
            };
        }

        public int Employeeid { get; set; }
        public string Login { get; set; }
        public string Name_ { get; set; }
        public string Surname { get; set; }
        public string Department { get; set; }
        public string Permission_ { get; }
    }
}
