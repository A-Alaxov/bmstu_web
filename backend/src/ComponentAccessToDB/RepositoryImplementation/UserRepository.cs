using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComponentBuisinessLogic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ComponentAccessToDB
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly transfersystemContext db;
        public UserRepository(transfersystemContext curDb)
        {
            db = curDb;
        }
        public async Task<int> Add(User element)
        {
            UserDB t = UserConv.BltoDB(element);
            Console.WriteLine(db);

            try
            {
                await db.Users.AddAsync(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserAddException("UserAdd", ex);
            }
            return -1;
        }
        public async Task<List<User>> GetAll()
        {
            IQueryable<UserDB> Users = db.Users;
            List<UserDB> conv = await Users.ToListAsync();
            List<User> final = new List<User>();
            foreach (var m in conv)
            {
                final.Add(UserConv.DBtoBL(m));
            }
            return final;
        }
        public async Task Update(User element)
        {
            UserDB o = await db.Users.FindAsync(element.Login);
            o.Name_ = element.Name_;
            o.Surname = element.Surname;
            try
            {
                db.Users.Update(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserUpdateException("UserUpdate", ex);
            }
        }
        public async Task Delete(User element)
        {
            UserDB o = await db.Users.FindAsync(element.Login);
            if (o == null)
                return;

            try
            {
                db.Users.Remove(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserDeleteException("UserDelete", ex);
            }
        }
        public async Task<User> GetUserByLogin(string login)
        {
            UserDB e = db.Users.Find(login);
            return e != null ? UserConv.DBtoBL(e) : null;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
