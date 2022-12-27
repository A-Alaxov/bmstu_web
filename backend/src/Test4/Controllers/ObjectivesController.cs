using ComponentBuisinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJira.Models;

namespace MyJira.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ObjectivesController : BaseController
    {
        IEmployeeService _employee;
        IResponsibleService _responsible;
        IManagerService _manager;
        IHRService _HR;
        IFounderService _founder;

        public ObjectivesController(
            IEmployeeService employee,
            IResponsibleService responsible,
            IManagerService manager,
            IHRService HR,
            IFounderService founder,
            IUserRepository _userRep,
            IEmployeeRepository _employeeRep) :
            base(_userRep, _employeeRep)
        {
            _employee = employee;
            _responsible = responsible;
            _manager = manager;
            _HR = HR;
            _founder = founder;
        }

        private async Task<IActionResult> GetProjects()
        {
            List<Objective> res;
            if (User.IsInRole("Employee"))
                res = await _employee.GetAllObjectives(await GetCurEmployee());
            else if (User.IsInRole("Responsible"))
                res = await _responsible.GetAllObjectives(await GetCurEmployee());
            else if (User.IsInRole("Manager"))
                res = await _manager.GetAllObjectives(await GetCurEmployee());
            else if (User.IsInRole("HR"))
                res = await _HR.GetAllObjectives(await GetCurEmployee());
            else
                res = await _founder.GetAllObjectives(await GetCurEmployee());

            return Ok(res);
        }

        [HttpGet("/Objectives")]
        [Authorize(Roles = "Employee, Manager, Responsible, HR, Founder")]
        public async Task<IActionResult> GetObjectiveByTitle([FromQuery(Name = "title")] string? title)
        {
            if (title == null)
                return await GetProjects();

            List<Objective> res;
            if (User.IsInRole("Employee"))
                res = await _employee.GetObjectivesByTitle(await GetCurEmployee(), title);
            else if (User.IsInRole("Responsible"))
                res = await _responsible.GetObjectivesByTitle(await GetCurEmployee(), title);
            else if (User.IsInRole("Manager"))
                res = await _manager.GetObjectivesByTitle(await GetCurEmployee(), title);
            else if (User.IsInRole("HR"))
                res = await _HR.GetObjectivesByTitle(await GetCurEmployee(), title);
            else
                res = await _founder.GetObjectivesByTitle(await GetCurEmployee(), title);

            return Ok(res);
        }

        [HttpGet("/Objectives/{objectiveID:int}")]
        [Authorize(Roles = "Employee, Manager, Responsible, HR, Founder")]
        public async Task<IActionResult> GetObjectiveByID(int objectiveID)
        {
            List<Objective> res;
            if (User.IsInRole("Employee"))
                res = await _employee.GetObjectiveByID(await GetCurEmployee(), objectiveID);
            else if (User.IsInRole("Responsible"))
                res = await _responsible.GetObjectiveByID(await GetCurEmployee(), objectiveID);
            else if (User.IsInRole("Manager"))
                res = await _manager.GetObjectiveByID(await GetCurEmployee(), objectiveID);
            else if (User.IsInRole("HR"))
                res = await _HR.GetObjectiveByID(await GetCurEmployee(), objectiveID);
            else
                res = await _founder.GetObjectiveByID(await GetCurEmployee(), objectiveID);

            return Ok(res);
        }

        [HttpPost("/Objectives/{objectiveID:int}")]
        [Authorize(Roles = "Responsible, Manager, Founder")]
        public async Task<IActionResult> PostSubtask(int objectiveID, ObjectiveUI value)
        {
            int res;
            if (User.IsInRole("Manager"))
                res = await _manager.AddObjective(await GetCurEmployee(), objectiveID, value.Title, value.Termbegin, value.Termend, TimeSpan.FromTicks(value.Estimatedtime), value.Department);
            else if (User.IsInRole("Founder"))
                res = await _founder.AddObjective(await GetCurEmployee(), objectiveID, value.Title, value.Termbegin, value.Termend, TimeSpan.FromTicks(value.Estimatedtime), value.Department);
            else
                res = await _responsible.AddSubObjective(await GetCurEmployee(), objectiveID, value.Title, value.Termbegin, value.Termend, TimeSpan.FromTicks(value.Estimatedtime));

            if (res == -1)
                return BadRequest();
            
            return Ok(res);
        }

        [HttpPut("/Objectives/{objectiveID:int}")]
        [Authorize(Roles = "Responsible, Manager, Founder")]
        public async Task<IActionResult> Put(int objectiveID, ObjectiveUI value)
        {
            bool res;
            if (User.IsInRole("Manager"))
                res = await _manager.UpdateObjective(await GetCurEmployee(), objectiveID, value.Title, value.Termbegin, value.Termend, TimeSpan.FromTicks(value.Estimatedtime), value.Department);
            else if (User.IsInRole("Founder"))
                res = await _founder.UpdateObjective(await GetCurEmployee(), objectiveID, value.Title, value.Termbegin, value.Termend, TimeSpan.FromTicks(value.Estimatedtime), value.Department);
            else
                res = await _responsible.UpdateObjective(await GetCurEmployee(), objectiveID, value.Title, value.Termbegin, value.Termend, TimeSpan.FromTicks(value.Estimatedtime));

            if (!res)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("/Objectives/{objectiveID:int}")]
        [Authorize(Roles = "Responsible, Manager, Founder")]
        public async Task<IActionResult> DeleteSubtask(int objectiveID)
        {
            bool res;
            if (User.IsInRole("Manager"))
                res = await _manager.DeleteObjective(await GetCurEmployee(), objectiveID);
            else if (User.IsInRole("Founder"))
                res = await _founder.DeleteObjective(await GetCurEmployee(), objectiveID);
            else
                res = await _responsible.DeleteSubObjective(await GetCurEmployee(), objectiveID);

            if (!res)
                return BadRequest();

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Founder")]
        public async Task<IActionResult> PostProject(ObjectiveUI value)
        {
            Console.Write(await GetCurEmployee());
            int res;
            if (User.IsInRole("Manager"))
                res = await _manager.AddObjective(await GetCurEmployee(), null, value.Title, value.Termbegin, value.Termend, TimeSpan.FromTicks(value.Estimatedtime), value.Department);
            else
                res = await _founder.AddObjective(await GetCurEmployee(), null, value.Title, value.Termbegin, value.Termend, TimeSpan.FromTicks(value.Estimatedtime), value.Department);

            if (res == -1)
                return BadRequest();

            return Ok(res);
        }

        [HttpGet("/Objectives/{objectiveID:int}/Responsibles")]
        [Authorize(Roles = "Manager, Responsible, HR, Founder")]
        public async Task<IActionResult> GetResponsibleEmployees(int objectiveID)
        {
            List<EmployeeView> res;
            if (User.IsInRole("Responsible"))
                res = await _responsible.GetResponsibleEmployees(await GetCurEmployee(), objectiveID);
            else if (User.IsInRole("Manager"))
                res = await _manager.GetResponsibleEmployees(await GetCurEmployee(), objectiveID);
            else if (User.IsInRole("HR"))
                res = await _HR.GetResponsibleEmployees(await GetCurEmployee(), objectiveID);
            else
                res = await _founder.GetResponsibleEmployees(await GetCurEmployee(), objectiveID);

            return Ok(res);
        }

        // [HttpGet("/Timetest/{objectiveID:int}")]
        // [Authorize(Roles = "Responsible")]
        // public IActionResult TimeTest(int objectiveID)
        // {
        //     ControllersInit();
        // 
        //     List<EmployeeView> res = _responsible.TimeTest(objectiveID);
        // 
        //     return Ok(res);
        // }
    }
}
