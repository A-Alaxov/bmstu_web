using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ComponentBuisinessLogic
{
    public class FounderService : ManagerService, IFounderService
    {
        public FounderService(IUserRepository UserRep,
                              ICompanyRepository CompanyRep,
                              IDepartmentRepository DepartmentRep,
                              IEmployeeRepository EmployeeRep,
                              IObjectiveRepository ObjectiveRep,
                              IResponsibilityRepository ResponsibilityRep) :
            base(UserRep, CompanyRep, DepartmentRep, EmployeeRep, ObjectiveRep, ResponsibilityRep)
        {
        }
        public async Task<EmployeeView> AddEmployee(Employee empl, string user_, int permission_, int? department = -1)
        {
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
        public async Task<WorkplaceView> UpdateCompany(Employee empl, string title, int foundationyear)
        {
            Company company = new Company(_companyid: empl.Company,
                                 _title: title,
                                 _foundationyear: foundationyear);
            await CompanyRepository.Update(company);

            var final = new WorkplaceView
            (
                    _EmployeeID: empl.Employeeid,
                    _company: company,
                    _department: null,
                    _permission_: (int)Permissions.Founder
                );
            return final;
        }
        public async Task<bool> DeleteCompany(Employee empl)
        {
            Company company = await CompanyRepository.GetCompanyByID(empl.Company);

            if (company == null)
                return false;

            await CompanyRepository.Delete(company);
            return true;
        }
    }
}
