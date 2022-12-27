#nullable disable

using System;

namespace ComponentBuisinessLogic
{
    public class WorkplaceView
    {
        public WorkplaceView(int _EmployeeID = 0, Company _company = null, Department _department = null, int _permission_ = 0)
        {
            EmployeeID = _EmployeeID;
            Company = _company;
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

        public int EmployeeID { get; }
        public Company Company { get; }
        public Department Department { get; }
        public string Permission_ { get; }
    }
}
