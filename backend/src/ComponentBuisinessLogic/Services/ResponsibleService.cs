using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public class ResponsibleService : EmployeeService, IResponsibleService
    {
        public ResponsibleService(IUserRepository UserRep,
                              ICompanyRepository CompanyRep,
                              IDepartmentRepository DepartmentRep,
                              IEmployeeRepository EmployeeRep,
                              IObjectiveRepository ObjectiveRep,
                              IResponsibilityRepository ResponsibilityRep) :
            base(UserRep, CompanyRep, DepartmentRep, EmployeeRep, ObjectiveRep, ResponsibilityRep)
        {
        }
        private async Task<bool> CheckResponsibility(Employee empl, int tid)
        {
            bool flag = false;
            var o = await ObjectiveRepository.GetObjectiveByID(tid);

            if (o == null)
                return flag;

            List<Employee> responsibles = await EmployeeRepository.GetResponsibleEmployees(o.Objectiveid);
            foreach (var resp in responsibles)
                if (resp.User_ == empl.User_)
                    flag = true;

            while (o.Parentobjective != null)
            {
                o = await ObjectiveRepository.GetObjectiveByID(o.Parentobjective);

                responsibles = await EmployeeRepository.GetResponsibleEmployees(o.Objectiveid);
                foreach (var resp in responsibles)
                    if (resp.User_ == empl.User_)
                        flag = true;
            }

            return flag;
        }
        public async Task<int> AddSubObjective(Employee empl, int pid, string title, DateTime termBegin, DateTime termEnd, TimeSpan estimatedTime)
        {
            var tmp = await ObjectiveRepository.GetObjectiveByID(pid);

            if (!(CheckWorkplace(empl, tmp) && CheckResponsibility(empl, pid).Result))
                return -1;

            Objective Objective = new Objective(_objectiveid: 0,
                                 _parentobjective: pid,
                                 _title: title,
                                 _company: empl.Company,
                                 _department: empl.Department,
                                 _termbegin: termBegin,
                                 _termend: termEnd,
                                 _estimatedtime: estimatedTime);
            int res = await ObjectiveRepository.Add(Objective);
            return res;
        }
        public async Task<bool> UpdateObjective(Employee empl, int tid, string title, DateTime termBegin, DateTime termEnd, TimeSpan estimatedTime)
        {
            var tmp = await ObjectiveRepository.GetObjectiveByID(tid);

            if (!(CheckWorkplace(empl, tmp) && CheckResponsibility(empl, tid).Result))
                return false;

            Objective Objective = new Objective(_objectiveid: tid,
                                 _parentobjective: tmp.Parentobjective,
                                 _title: title,
                                 _company: empl.Company,
                                 _department: empl.Department,
                                 _termbegin: termBegin,
                                 _termend: termEnd,
                                 _estimatedtime: estimatedTime);
            await ObjectiveRepository.Update(Objective);
            return true;
        }
        public async Task<bool> DeleteSubObjective(Employee empl, int id)
        {
            Objective o = await ObjectiveRepository.GetObjectiveByID(id);

            if (!(CheckWorkplace(empl, o) && CheckResponsibility(empl, id).Result))
                return false;

            if (o.Parentobjective == null)
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
        public async Task<bool> AddResponsibility(Employee empl, int eid, int tid, TimeSpan timeAmount)
        {
            var tmpObjective = await ObjectiveRepository.GetObjectiveByID(tid);
            var tmpEmployee = await EmployeeRepository.GetEmployeeByID(eid);

            if (!(CheckСonformance(empl, tmpEmployee, tmpObjective) && CheckResponsibility(empl, tid).Result))
                return false;

            Responsibility oldResponsibility = await ResponsibilityRepository.GetResponsibilityByObjectiveAndEmployee(tid, eid);
            Responsibility newResponsibility;

            if (oldResponsibility == null)
            {
                newResponsibility = new Responsibility(_responsibilityid: 0,
                                             _employee: eid,
                                             _objective: tid,
                                             _timespent: timeAmount);
                await ResponsibilityRepository.Add(newResponsibility);
            }
            else
            {
                newResponsibility = new Responsibility(_responsibilityid: oldResponsibility.Responsibilityid,
                                             _employee: eid,
                                             _objective: tid,
                                             _timespent: timeAmount);
                await ResponsibilityRepository.Update(newResponsibility);
            }

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
    }
}
