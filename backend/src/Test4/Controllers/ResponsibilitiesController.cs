using ComponentAccessToDB;
using ComponentBuisinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyJira.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResponsibilitiesController : BaseController
    {
        IResponsibleService _responsible;
        IManagerService _manager;
        IFounderService _founder;

        public ResponsibilitiesController(
            IResponsibleService responsible,
            IManagerService manager,
            IFounderService founder,
            IUserRepository _userRep,
            IEmployeeRepository _employeeRep) :
            base(_userRep, _employeeRep)
        {
            _responsible = responsible;
            _manager = manager;
            _founder = founder;
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Responsible, Founder")]
        public async Task<IActionResult> Post(ResponsibilityUI value)
        {
            bool res;
            if (User.IsInRole("Responsible"))
                res = await _responsible.AddResponsibility(await GetCurEmployee(), value.Employee, value.Objective, TimeSpan.FromTicks(value.Timeamount));
            else if (User.IsInRole("Manager"))
                res = await _manager.AddResponsible(await GetCurEmployee(), value.Employee, value.Objective);
            else
                res = await _founder.AddResponsible(await GetCurEmployee(), value.Employee, value.Objective);

            if (!res)
                return BadRequest();

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Responsible")]
        public async Task<IActionResult> Put(ResponsibilityUI value)
        {
            bool res = await _responsible.AddResponsibility(await GetCurEmployee(), value.Employee, value.Objective, TimeSpan.FromTicks(value.Timeamount));

            if (!res)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("/Resposibilities/{resposibilitiID:int}")]
        [Authorize(Roles = "Manager, Responsible, Founder")]
        public async Task<IActionResult> Delete(int resposibilitiID)
        {
            bool res;
            if (User.IsInRole("Responsible"))
                res = await _responsible.DeleteResponsibility(await GetCurEmployee(), resposibilitiID);
            else if (User.IsInRole("Manager"))
                res = await _manager.DeleteResponsibility(await GetCurEmployee(), resposibilitiID);
            else
                res = await _founder.DeleteResponsibility(await GetCurEmployee(), resposibilitiID);

            if (!res)
                return BadRequest();

            return Ok();
        }
    }
}
