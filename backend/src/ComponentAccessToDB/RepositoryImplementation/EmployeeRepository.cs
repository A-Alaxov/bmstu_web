using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComponentBuisinessLogic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Runtime.CompilerServices;

namespace ComponentAccessToDB
{
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {
        private readonly transfersystemContext db;
        public EmployeeRepository(transfersystemContext curDb)
        {
            db = curDb;
        }
        public async Task<int> Add(Employee element)
        {
            EmployeeDB e = EmployeeConv.BltoDB(element);

            if (db.Employees.Count() > 0)
                e.Employeeid = await db.Employees.MaxAsync(comparer => comparer.Employeeid) + 1;
            else
                e.Employeeid = 1;

            try
            {
                await db.Employees.AddAsync(e);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new EmployeeAddException("EmployeeAdd", ex);
            }
            return e.Employeeid;
        }
        public async Task<List<Employee>> GetAll()
        {
            IQueryable<EmployeeDB> Employees = db.Employees;
            List<EmployeeDB> conv = await Employees.ToListAsync();
            List<Employee> final = new List<Employee>();
            foreach (var m in conv)
            {
                final.Add(EmployeeConv.DBtoBL(m));
            }
            return final;
        }
        public async Task Update(Employee element)
        {
            EmployeeDB e = await db.Employees.FindAsync(element.Employeeid);
            e.UserID = element.User_;
            e.CompanyID = element.Company;
            e.DepartmentID = element.Department;
            e.Permission_ = element.Permission_;
            try
            {
                db.Employees.Update(e);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new EmployeeUpdateException("EmployeeUpdate", ex);
            }
        }
        public async Task Delete(Employee element)
        {
            EmployeeDB e = EmployeeConv.BltoDB(element);
            if (e == null)
                return;

            try
            {
                db.Employees.Remove(db.Employees.Find(e.Employeeid));
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new EmployeeDeleteException("EmployeeDelete", ex);
            }
        }
        public async Task<Employee> GetEmployeeByID(int id)
        {
            EmployeeDB e = await db.Employees.FindAsync(id);
            return e != null ? EmployeeConv.DBtoBL(e) : null;
        }
        public async Task<List<Employee>> GetResponsibleEmployees(int tid)
        {
            IQueryable<ResponsibilityDB> Responsibilities = db.Responsibilities.Where(needed => needed.ObjectiveID == tid);
            List<ResponsibilityDB> tmp = await Responsibilities.ToListAsync();
            List<EmployeeDB> conv = new List<EmployeeDB>();
            foreach (var m in tmp)
            {
                IQueryable<EmployeeDB> Employees = db.Employees.Where(needed => needed.Employeeid == m.EmployeeID);
                conv.AddRange(await Employees.ToListAsync());
            }
            List<Employee> final = new List<Employee>();
            foreach (var m in conv)
            {
                final.Add(EmployeeConv.DBtoBL(m));
            }
            return final;
        }
        public async Task<Employee> GetEmployeeByWorkplace(string user, int cid, int? did)
        {
            IQueryable<EmployeeDB> Employees = db.Employees.Where(needed => needed.UserID == user && needed.CompanyID == cid && needed.DepartmentID == did); ;
            List<EmployeeDB> conv = await Employees.ToListAsync();
            List<Employee> final = new List<Employee>();
            foreach (var m in conv)
            {
                final.Add(EmployeeConv.DBtoBL(m));
            }
            return final.Count() > 0 ? final.First() : null;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
