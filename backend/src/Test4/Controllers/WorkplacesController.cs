using ComponentBuisinessLogic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyJira.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkplacesController : BaseController
    {
        IUserService _user;
        IEmployeeService _employee;

        public WorkplacesController(
            IUserService user,
            IEmployeeService employee,
            IUserRepository _userRep,
            IEmployeeRepository _employeeRep) :
            base(_userRep, _employeeRep)
        {
            _user = user;
            _employee = employee;
        }

        [Authorize]
        [HttpPost("/Workplaces/{employeeID:int}")]
        public async Task<IActionResult> ChooseWorkplace(int employeeID)
        {
            Employee employee = await _user.GetEmployeeByWorkplace(await GetCurUser(), employeeID);
            if (employee == null)
                return BadRequest();

            var identity = User.Identity as ClaimsIdentity;
            identity.RemoveClaim(User.Claims.Where(el => el.Type == ClaimTypes.Role).Single());

            var EmployeeClaim = User.Claims.Where(el => el.Type == "EmployeeID").SingleOrDefault();
            if (EmployeeClaim != null)
                identity.RemoveClaim(EmployeeClaim);

            if (employee.Permission_ == 0)
                identity.AddClaim(new Claim(ClaimTypes.Role, "Employee"));
            else if (employee.Permission_ == 1)
                identity.AddClaim(new Claim(ClaimTypes.Role, "Responsible"));
            else if (employee.Permission_ == 2)
                identity.AddClaim(new Claim(ClaimTypes.Role, "Manager"));
            else if (employee.Permission_ == 3)
                identity.AddClaim(new Claim(ClaimTypes.Role, "HR"));
            else if (employee.Permission_ == 4)
                identity.AddClaim(new Claim(ClaimTypes.Role, "Founder"));
            else
                return BadRequest();

            identity.AddClaim(new Claim("EmployeeID", employee.Employeeid.ToString()));

            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            WorkplaceView res = await _employee.GetWorkplace(employee);

            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWorkplaces()
        {
            Console.Write(Username);
            List<WorkplaceView> res = await _user.GetWorkplaces(await GetCurUser());

            return Ok(res);
        }

        [HttpGet("/Workplaces/Current")]
        [Authorize(Roles = "Employee, Manager, Responsible, HR, Founder")]
        public async Task<IActionResult> GetCurrentWorkplace()
        {
            WorkplaceView res = await _employee.GetWorkplace(await GetCurEmployee());

            return Ok(res);
        }
    }
}
