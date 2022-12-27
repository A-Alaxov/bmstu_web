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
    public class CompaniesController : BaseController
    {
        IUserService _user;
        IFounderService _founder;

        public CompaniesController(
            IUserService user,
            IFounderService founder,
            IUserRepository _userRep,
            IEmployeeRepository _employeeRep) :
            base(_userRep, _employeeRep)
        {
            _user = user;
            _founder = founder;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CompanyUI value)
        {
            Console.Write(Username);
            WorkplaceView res = await _user.AddCompany(await GetCurUser(), value.Title, value.Foundationyear);

            if (res == null)
                return BadRequest();

            return Ok(res);
        }

        [HttpPut]
        [Authorize(Roles = "Founder")]
        public async Task<IActionResult> Put(CompanyUI value)
        {
            WorkplaceView res = await _founder.UpdateCompany(await GetCurEmployee(), value.Title, value.Foundationyear);

            if (res == null)
                return BadRequest();

            return Ok(res);
        }

        [HttpDelete]
        [Authorize(Roles = "Founder")]
        public async Task<IActionResult> Delete()
        {
            bool res = await _founder.DeleteCompany(await GetCurEmployee());

            if (!res)
                return BadRequest();

            var identity = User.Identity as ClaimsIdentity;
            identity.RemoveClaim(User.Claims.Where(el => el.Type == ClaimTypes.Role).Single());

            var EmployeeClaim = User.Claims.Where(el => el.Type == "EmployeeID").SingleOrDefault();
            if (EmployeeClaim != null)
                identity.RemoveClaim(EmployeeClaim);

            identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return Ok();
        }
    }
}
