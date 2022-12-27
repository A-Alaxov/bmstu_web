using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public class HRService : EmployeeService, IHRService
    {
        public HRService(IUserRepository UserRep,
                              ICompanyRepository CompanyRep,
                              IDepartmentRepository DepartmentRep,
                              IEmployeeRepository EmployeeRep,
                              IObjectiveRepository ObjectiveRep,
                              IResponsibilityRepository ResponsibilityRep) :
            base(UserRep, CompanyRep, DepartmentRep, EmployeeRep, ObjectiveRep, ResponsibilityRep)
        {
        }
        public async Task<List<EmployeeView>> GetResponsibleEmployees(Employee empl, int id)
        {
            List<Employee> tmpemployees = await EmployeeRepository.GetResponsibleEmployees(id);
            List<Employee> employees = GetWorkplaceEmployees(empl, tmpemployees);
            List<EmployeeView> final = new List<EmployeeView>();
            foreach (var m in employees)
            {
                var user = await UserRepository.GetUserByLogin(m.User_);
                var department = await DepartmentRepository.GetDepartmentByID(m.Department);
                final.Add(new EmployeeView
                (
                    _employeeid: m.Employeeid,
                    _login: m.User_,
                    _name_: user.Name_,
                    _surname: user.Surname,
                    _department: department?.Title,
                    _permission_: m.Permission_
                ));
            }
            return final;
        }
        public async Task<EmployeeView> AddEmployee(User usr, Employee empl, string user_, int permission_, int? department = -1)
        {
            if (user_ == usr.Login)
                return null;

            if (permission_ == (int)Permissions.Founder)
                return null;

            if (department != -1)
            {
                if (empl.Department != null)
                    return null;
            }
            else
                department = empl.Department;

            Employee employee = new Employee(_employeeid: 0,
                                 _user_: user_,
                                 _company: empl.Company,
                                 _department: department,
                                 _permission_: permission_);
            var id = await EmployeeRepository.Add(employee);

            var user = await UserRepository.GetUserByLogin(user_);
            var departmentRes = await DepartmentRepository.GetDepartmentByID(department);
            var final = new EmployeeView
            (
                _employeeid: id,
                _login: user_,
                _name_: user.Name_,
                _surname: user.Surname,
                _department: departmentRes?.Title,
                _permission_: permission_
            );
            return final;
        }
        public async Task<bool> UpdateEmployee(Employee empl, int id, string user_, int permission_, int? department = -1)
        {
            if (id == empl.Employeeid)
                return false;

            if (permission_ == (int)Permissions.Founder)
                return false;

            if (department != -1)
            {
                if (empl.Department != null)
                    return false;
            }
            else
                department = empl.Department;

            var tmpEmployee = await EmployeeRepository.GetEmployeeByID(id);

            if (!CheckWorkplace(empl, tmpEmployee))
                return false;

            Employee employee = new Employee(_employeeid: id,
                                 _user_: user_,
                                 _company: empl.Company,
                                 _department: department,
                                 _permission_: permission_);
            await EmployeeRepository.Update(employee);
            return true;
        }
        public async Task<bool> DeleteEmployee(Employee empl, int id)
        {
            Employee employee = await EmployeeRepository.GetEmployeeByID(id);

            if (!CheckWorkplace(empl, employee))
                return false;

            if (employee == null)
                return false;

            await EmployeeRepository.Delete(employee);
            return true;
        }
    }
}
