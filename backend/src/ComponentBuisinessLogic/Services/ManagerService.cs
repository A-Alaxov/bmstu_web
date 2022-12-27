using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public class ManagerService : EmployeeService, IManagerService
    {
        public ManagerService(IUserRepository UserRep,
                              ICompanyRepository CompanyRep,
                              IDepartmentRepository DepartmentRep,
                              IEmployeeRepository EmployeeRep,
                              IObjectiveRepository ObjectiveRep,
                              IResponsibilityRepository ResponsibilityRep) :
            base(UserRep, CompanyRep, DepartmentRep, EmployeeRep, ObjectiveRep, ResponsibilityRep)
        {
        }
        public async Task<int> AddObjective(Employee empl, int? pid, string title, DateTime termBegin, DateTime termEnd, TimeSpan estimatedTime, int? department = -1)
        {
            if (department != -1)
            {
                if (empl.Department != null)
                    return -1;
            }
            else
                department = empl.Department;

            if (pid != null)
            {
                var tmp = await ObjectiveRepository.GetObjectiveByID(pid);

                if (!CheckWorkplace(empl, tmp))
                    return -1;
            }

            Objective Objective = new Objective(_objectiveid: 0,
                                 _parentobjective: pid,
                                 _title: title,
                                 _company: empl.Company,
                                 _department: department,
                                 _termbegin: termBegin,
                                 _termend: termEnd,
                                 _estimatedtime: estimatedTime);
            int res = await ObjectiveRepository.Add(Objective);
            return res;
        }
        public async Task<bool> UpdateObjective(Employee empl, int tid, string title, DateTime termBegin, DateTime termEnd, TimeSpan estimatedTime, int? department = -1)
        {
            if (department != -1 )
            {
                if (empl.Department != null)
                    return false;
            }
            else
                department = empl.Department;

            var tmp = await ObjectiveRepository.GetObjectiveByID(tid);

            if (!CheckWorkplace(empl, tmp))
                return false;

            Objective task = new Objective(_objectiveid: tid,
                                 _parentobjective: tmp.Parentobjective,
                                 _title: title,
                                 _company: empl.Company,
                                 _department: department,
                                 _termbegin: termBegin,
                                 _termend: termEnd,
                                 _estimatedtime: estimatedTime);
            await ObjectiveRepository.Update(task);
            return true;
        }
        public async Task<bool> DeleteObjective(Employee empl, int id)
        {
            Objective o = await ObjectiveRepository.GetObjectiveByID(id);

            if (!CheckWorkplace(empl, o))
                return false;

            if (o == null)
                return false;

            await ObjectiveRepository.Delete(o);
            return true;
        }
        private bool CheckСonformance(Employee empl, Employee emp, Objective obj)
        {
            return CheckWorkplace(empl, emp) && CheckWorkplace(empl, obj) &&
                   (emp.Department == null || emp.Department == obj.Department);
        }
        public async Task<bool> AddResponsible(Employee empl, int eid, int tid)
        {
            var tmpObjective = await ObjectiveRepository.GetObjectiveByID(tid);
            var tmpEmployee = await EmployeeRepository.GetEmployeeByID(eid);

            if (!CheckСonformance(empl, tmpEmployee, tmpObjective))
                return false;

            Responsibility oldResponsibility = await ResponsibilityRepository.GetResponsibilityByObjectiveAndEmployee(tid, eid);
            Responsibility newResponsibility;

            if (oldResponsibility == null)
            {
                newResponsibility = new Responsibility(_responsibilityid: 0,
                                             _employee: eid,
                                             _objective: tid,
                                             _timespent: TimeSpan.Zero);
                await ResponsibilityRepository.Add(newResponsibility);
            }
            else
                return false;

            return true;
        }
        public async Task<bool> DeleteResponsibility(Employee empl, int rid)
        {
            var tmpResponsibility = await ResponsibilityRepository.GetResponsibilityByID(rid);

            var tmpObjective = await ObjectiveRepository.GetObjectiveByID(tmpResponsibility.Objective);
            var tmpEmployee = await EmployeeRepository.GetEmployeeByID(tmpResponsibility.Employee);

            if (!CheckСonformance(empl, tmpEmployee, tmpObjective))
                return false;

            Responsibility o = await ResponsibilityRepository.GetResponsibilityByObjectiveAndEmployee(tmpResponsibility.Objective, tmpResponsibility.Employee);

            if (o == null)
                return false;

            await ResponsibilityRepository.Delete(o);
            return true;
        }
        public async Task<List<ResponsibilityView>> GetResponsibilityByEmployee(Employee empl, int id)
        {
            var tmpEmployee = await EmployeeRepository.GetEmployeeByID(id);

            if (!CheckWorkplace(empl, tmpEmployee))
                return null;

            var tmpResps = await ResponsibilityRepository.GetResponsibilityByEmployee(id);
            List<ResponsibilityView> final = new List<ResponsibilityView>();
            foreach (var m in tmpResps)
            {
                var objective = await ObjectiveRepository.GetObjectiveByID(m.Objective);
                final.Add(new ResponsibilityView
                (
                    _employee: empl.User_,
                    _objective: objective.Title,
                    _timespent: m.Timespent
                ));
            }
            return final;
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
        public async Task<int> AddDepartment(Employee empl, string title, int foundationyear, string activityfield)
        {
            if (empl.Department != null)
                return -1;

            Department department = new Department(_departmentid: 0,
                                 _title: title,
                                 _company: empl.Company,
                                 _foundationyear: foundationyear,
                                 _activityfield: activityfield);
            return await DepartmentRepository.Add(department);
        }
        public bool UpdateDepartment(Employee empl, int id, string title, int foundationyear, string activityfield)
        {
            if (empl.Department != null && empl.Department != id)
                return false;

            Department department = new Department(_departmentid: id,
                                 _title: title,
                                 _company: empl.Company,
                                 _foundationyear: foundationyear,
                                 _activityfield: activityfield);
            DepartmentRepository.Update(department);
            return true;
        }
        public async Task<bool> DeleteDepartment(Employee empl, int id)
        {
            if (empl.Department != null)
                return false;

            Department department = await DepartmentRepository.GetDepartmentByID(id);

            if (department == null)
                return false;

            await DepartmentRepository.Delete(department);
            return true;
        }

        public bool CheckEmployeeDepartment(Employee empl, int id)
        {
            return empl.Department == id;
        }
    }
}
