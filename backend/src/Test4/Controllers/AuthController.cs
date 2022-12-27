using ComponentAccessToDB;
using ComponentBuisinessLogic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyJira.Models;

namespace MyJira.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private INotAuthService _notAuth;
        //private IEmployeeService _employee;

        public AuthController(INotAuthService notAuth,
            //IEmployeeService employee,
            IUserRepository _userRep,
            IEmployeeRepository _employeeRep) :
            base(_userRep, _employeeRep)
        {
            _notAuth = notAuth;
            //_employee = employee;
        }

        [HttpPost("/Auth/Register")]
        public IActionResult Register(UserUI value)
        {
            bool res = _notAuth.AddUser(value.Login, value.Password_, value.Name_, value.Surname);

            if (!res)
                return BadRequest();

            return Ok();
        }

        [HttpPost("/Auth/Login")]
        public async Task<IActionResult> Login(AuthUI val)
        {
            var user = await _notAuth.GetUserByLogin(val.Username);

            if (user != null && val.Password == user.Password_)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, val.Username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, val.Username));
                claims.Add(new Claim(ClaimTypes.Role, "User"));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("/Auth/Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        /*[HttpGet("/Test")]
        public async Task<IActionResult> Test()
        {
            return Ok(await _employee.GetAllObjectivesTest(null));
        }*/
    }
}
