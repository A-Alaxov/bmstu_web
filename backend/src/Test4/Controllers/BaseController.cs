using ComponentBuisinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyJira.Controllers
{
    public class BaseController : ControllerBase
    {
        private IUserRepository userRep;
        private IEmployeeRepository employeeRep;

        private string _username;
        private User _curUser;
        private Employee _curEmployee;

        public BaseController(
            IUserRepository _userRep,
            IEmployeeRepository _employeeRep)
        {
            userRep = _userRep;
            employeeRep = _employeeRep;
        }

        protected string Username
        {
            get
            {
                if (_username == null)
                {
                    _username = User.Identity.Name;
                }
                return _username;
            }
        }

        protected async Task<User> GetCurUser()
        {
            if (_curUser == null)
            {
                _curUser = await userRep.GetUserByLogin(_username);
            }
            return _curUser;
        }

        protected async Task<Employee> GetCurEmployee()
        {
            if (_curEmployee == null)
            {
                var identity = User.Identity as ClaimsIdentity;

                int eid = int.Parse(identity.FindFirst("EmployeeID").Value);
                _curEmployee = await employeeRep.GetEmployeeByID(eid);
            }
            return _curEmployee;
        }
    }
}
