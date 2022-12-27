using ComponentAccessToDB;
using ComponentBuisinessLogic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyJira.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController
    {
        IEmployeeService _employee;
        IResponsibleService _responsible;
        IManagerService _manager;
        IHRService _HR;
        IFounderService _founder;

        public DepartmentsController(
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
            List<Department> res;
            if (User.IsInRole("Employee"))
                res = await _employee.GetAllDepartments(await GetCurEmployee());
            else if (User.IsInRole("Responsible"))
                res = await _responsible.GetAllDepartments(await GetCurEmployee());
            else if (User.IsInRole("Manager"))
                res = await _manager.GetAllDepartments(await GetCurEmployee());
            else if (User.IsInRole("HR"))
                res = await _HR.GetAllDepartments(await GetCurEmployee());
            else
                res = await _founder.GetAllDepartments(await GetCurEmployee());

            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Founder")]
        public async Task<IActionResult> Post(DepartmentUI value)
        {
            int res;
            if (User.IsInRole("Manager"))
                res = await _manager.AddDepartment(await GetCurEmployee(), value.Title, value.Foundationyear, value.Activityfield);
            else
                res = await _founder.AddDepartment(await GetCurEmployee(), value.Title, value.Foundationyear, value.Activityfield);

            if (res == -1)
                return BadRequest();

            return Ok(res);
        }

        [HttpPut("/Departments/{departmentID:int}")]
        [Authorize(Roles = "Manager, Founder")]
        public async Task<IActionResult> Put(int departmentID, DepartmentUI value)
        {
            bool res;
            if (User.IsInRole("Manager"))
                res = _manager.UpdateDepartment(await GetCurEmployee(), departmentID, value.Title, value.Foundationyear, value.Activityfield);
            else
                res = _founder.UpdateDepartment(await GetCurEmployee(), departmentID, value.Title, value.Foundationyear, value.Activityfield);

            if (!res)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("/Departments/{departmentID:int}")]
        [Authorize(Roles = "Manager, Founder")]
        public async Task<IActionResult> Delete(int departmentID)
        {
            bool res;
            if (User.IsInRole("Manager"))
                res = await _manager.DeleteDepartment(await GetCurEmployee(), departmentID);
            else
                res = await _founder.DeleteDepartment(await GetCurEmployee(), departmentID);

            if (!res)
                return BadRequest();

            if (_manager.CheckEmployeeDepartment(await GetCurEmployee(), departmentID))
            {
                var identity = User.Identity as ClaimsIdentity;
                identity.RemoveClaim(User.Claims.Where(el => el.Type == ClaimTypes.Role).Single());

                var EmployeeClaim = User.Claims.Where(el => el.Type == "EmployeeID").SingleOrDefault();
                if (EmployeeClaim != null)
                    identity.RemoveClaim(EmployeeClaim);

                identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
            }

            return Ok();
        }
    }
}
