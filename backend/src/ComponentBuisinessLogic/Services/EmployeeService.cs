using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public enum Permissions : int
    {
        Employee,
        Responsible,
        Manager,
        HR,
        Founder
    }
    public class EmployeeService : UserService, IEmployeeService
    {
        protected readonly IObjectiveRepository ObjectiveRepository;
        protected readonly IResponsibilityRepository ResponsibilityRepository;

        public EmployeeService(IUserRepository UserRep,
                              ICompanyRepository CompanyRep,
                              IDepartmentRepository DepartmentRep,
                              IEmployeeRepository EmployeeRep,
                              IObjectiveRepository ObjectiveRep,
                              IResponsibilityRepository ResponsibilityRep) :
            base(UserRep, CompanyRep, DepartmentRep, EmployeeRep)
        {
            ObjectiveRepository = ObjectiveRep;
            ResponsibilityRepository = ResponsibilityRep;
        }
        protected bool CheckWorkplace(Employee empl, Employee el)
        {
            if (el == null)
                return false;

            return el.Company == empl.Company &&
                   (empl.Department == null || el.Department == empl.Department);
        }
        protected bool CheckWorkplace(Employee empl, Objective el)
        {
            if (el == null)
                return false;

            return el.Company == empl.Company &&
                   (empl.Department == null || el.Department == empl.Department);
        }
        protected List<Employee> GetWorkplaceEmployees(Employee empl, List<Employee> employes)
        {
            return employes.Where(el => CheckWorkplace(empl, el)).ToList();
        }
        protected List<Objective> GetWorkplaceObjectives(Employee empl, List<Objective> objectives)
        {
            return objectives.Where(el => CheckWorkplace(empl, el)).ToList();
        }
        public async Task<List<Department>> GetAllDepartments(Employee empl)
        {
            if (empl.Department != null)
                return null;

            return await DepartmentRepository.GetDepartmentsByCompany(empl.Company);
        }
        public async Task<List<EmployeeView>> GetAllEmployees(Employee empl)
        {
            List<Employee> tmpemployees = await EmployeeRepository.GetAll();
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
        public async Task<List<Objective>> GetAllObjectives(Employee empl)
        {
            List<Objective> AllObjectives = await ObjectiveRepository.GetAll();
            return GetWorkplaceObjectives(empl, AllObjectives);
        }
        public async Task<List<Objective>> GetAllObjectivesTest(Employee empl)
        {
            List<Objective> AllObjectives = await ObjectiveRepository.GetAll();
            return AllObjectives;
        }
        public async Task<List<Objective>> GetObjectiveByID(Employee empl, int tid)
        {
            Objective o = await ObjectiveRepository.GetObjectiveByID(tid);
            if (o == null)
                return null;

            List<Objective> final = new List<Objective>();
            final.Add(o);
            final.AddRange(await ObjectiveRepository.GetSubObjectives(tid));
            return GetWorkplaceObjectives(empl, final);
        }
        public async Task<List<Objective>> GetObjectivesByTitle(Employee empl, string title)
        {
            List<Objective> AllObjectives = await ObjectiveRepository.GetObjectivesByTitle(title);
            return GetWorkplaceObjectives(empl, AllObjectives);
        }
        public async Task<WorkplaceView> GetWorkplace(Employee empl)
        {
            var company = await CompanyRepository.GetCompanyByID(empl.Company);
            var department = await DepartmentRepository.GetDepartmentByID(empl.Department);
            WorkplaceView final = new WorkplaceView
            (
                _EmployeeID: empl.Employeeid,
                _company: company,
                _department: department,
                _permission_: empl.Permission_
            );
            return final;
        }
    }
}
