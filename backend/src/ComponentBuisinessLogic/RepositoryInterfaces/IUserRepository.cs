﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IUserRepository : CrudRepository<User>
    {
        Task<User> GetUserByLogin(string login);
    }
}
