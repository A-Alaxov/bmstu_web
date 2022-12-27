using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public class UserService : IUserService
    {
        protected readonly IUserRepository UserRepository;
        protected readonly ICompanyRepository CompanyRepository;
        protected readonly IDepartmentRepository DepartmentRepository;
        protected readonly IEmployeeRepository EmployeeRepository;

        public UserService(IUserRepository UserRep,
                              ICompanyRepository CompanyRep,
                              IDepartmentRepository DepartmentRep,
                              IEmployeeRepository EmployeeRep)
        {
            EmployeeRepository = EmployeeRep;
            UserRepository = UserRep;
            CompanyRepository = CompanyRep;
            DepartmentRepository = DepartmentRep;
        }

        public UserView GetUser(User usr)
        {
            UserView userview = new UserView(_login: usr.Login,
                                        _name_: usr.Name_,
                                        _surname: usr.Surname);
            return userview;
        }
        public async Task<WorkplaceView> AddCompany(User usr, string title, int foundationyear)
        {
            Company company = new Company(_companyid: 0,
                                 _title: title,
                                 _foundationyear: foundationyear);
            await CompanyRepository.Add(company);

            var tmp = await CompanyRepository.GetAll();
            Employee employee = new Employee(_employeeid: 0,
                                 _user_: usr.Login,
                                 _company: tmp.Last().Companyid,
                                 _department: null,
                                 _permission_: (int)Permissions.Founder);
            var id = await EmployeeRepository.Add(employee);

            var final = new WorkplaceView
                (
                    _EmployeeID: id,
                    _company: company,
                    _department: null,
                    _permission_: (int)Permissions.Founder
                );
            return final;
        }
        public async Task<List<WorkplaceView>> GetWorkplaces(User usr)
        {
            var tempRmployees = await EmployeeRepository.GetAll();
            var employees = tempRmployees.Where(el => el.User_ == usr.Login).ToList();
            List<WorkplaceView> final = new List<WorkplaceView>();
            foreach (var m in employees)
            {
                var company = await CompanyRepository.GetCompanyByID(m.Company);
                var department = await DepartmentRepository.GetDepartmentByID(m.Department);
                final.Add(new WorkplaceView
                (
                    _EmployeeID: m.Employeeid,
                    _company: company,
                    _department: department,
                    _permission_: m.Permission_
                ));
            }
            return final;
        }
        public async Task<Employee> GetEmployeeByWorkplace(User usr, int id)
        {
            return await EmployeeRepository.GetEmployeeByID(id);
        }
        public async Task<UserView> UpdateUser(User usr, string name_, string surname)
        {
            User user = new User(_login: usr.Login,
                                 _password_: null,
                                 _name_: name_,
                                 _surname: surname);
            await UserRepository.Update(user);
            return new UserView(_login: usr.Login,
                                 _name_: name_,
                                 _surname: surname);
        }
        public async Task<bool> DeleteUser(User usr)
        {
            User user = await UserRepository.GetUserByLogin(usr.Login);

            if (user == null)
                return false;

            await UserRepository.Delete(user);
            return true;
        }
    }
}
