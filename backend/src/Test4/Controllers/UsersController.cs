using ComponentAccessToDB;
using ComponentBuisinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyJira.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        INotAuthService _notAuth;
        IUserService _user;

        public UsersController(INotAuthService notAuth,
            IUserService user,
            IUserRepository _userRep,
            IEmployeeRepository _employeeRep) :
            base(_userRep, _employeeRep)
        {
            _notAuth = notAuth;
            _user = user;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            Console.Write(Username);
            UserView res = _user.GetUser(await GetCurUser());

            return Ok(res);
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Patch(UserUI value)
        {
            Console.Write(Username);
            UserView res = await _user.UpdateUser(await GetCurUser(), value.Name_, value.Surname);

            if (res == null)
                return BadRequest();

            return Ok(res);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            bool res = await _user.DeleteUser(await GetCurUser());

            if (!res)
                return BadRequest();

            return Ok();
        }
    }
}
