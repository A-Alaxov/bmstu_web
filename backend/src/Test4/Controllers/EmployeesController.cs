using ComponentAccessToDB;
using ComponentBuisinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyJira.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : BaseController
    {
        IEmployeeService _employee;
        IResponsibleService _responsible;
        IManagerService _manager;
        IHRService _HR;
        IFounderService _founder;

        public EmployeesController(
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

        [HttpGet]
        [Authorize(Roles = "Employee, Manager, Responsible, HR, Founder")]
        public async Task<IActionResult> Get()
        {
            List<EmployeeView> res;
            if (User.IsInRole("Employee"))
                res = await _employee.GetAllEmployees(await GetCurEmployee());
            else if (User.IsInRole("Responsible"))
                res = await _responsible.GetAllEmployees(await GetCurEmployee());
            else if (User.IsInRole("Manager"))
                res = await _manager.GetAllEmployees(await GetCurEmployee());
            else if (User.IsInRole("HR"))
                res = await _HR.GetAllEmployees(await GetCurEmployee());
            else
                res = await _founder.GetAllEmployees(await GetCurEmployee());

            return Ok(res);
        }

        [HttpGet("/Employees/{employeeID:int}")]
        [Authorize(Roles = "Manager, Founder")]
        public async Task<IActionResult> GetResponsibilityByEmployee(int employeeID)
        {
            List<ResponsibilityView> res;
            if (User.IsInRole("Manager"))
                res = await _manager.GetResponsibilityByEmployee(await GetCurEmployee(), employeeID);
            else
                res = await _founder.GetResponsibilityByEmployee(await GetCurEmployee(), employeeID);

            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = "HR, Founder")]
        public async Task<IActionResult> Post(EmployeeUI value)
        {
            EmployeeView res;
            if (User.IsInRole("HR"))
                res = await _HR.AddEmployee(await GetCurUser(), await GetCurEmployee(), value.User_, value.Permission_, value.Department);
            else
                res = await _founder.AddEmployee(await GetCurEmployee(), value.User_, value.Permission_, value.Department);

            if (res == null)
                return BadRequest();

            return Ok(res);
        }

        [HttpDelete("/Employees/{employeeID:int}")]
        [Authorize(Roles = "HR, Founder")]
        public async Task<IActionResult> Delete(int employeeID)
        {
            bool res;
            if (User.IsInRole("HR"))
                res = await _HR.DeleteEmployee(await GetCurEmployee(), employeeID);
            else
                res = await _founder.DeleteEmployee(await GetCurEmployee(), employeeID);

            if (!res)
                return BadRequest();

            return Ok();
        }
    }
}
