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
    public class ResponsibilityRepository : IResponsibilityRepository, IDisposable
    {
        private readonly transfersystemContext db;
        public ResponsibilityRepository(transfersystemContext curDb)
        {
            db = curDb;
        }
        public async Task<int> Add(Responsibility element)
        {
            ResponsibilityDB t = ResponsibilityConv.BltoDB(element);

            if (await db.Responsibilities.CountAsync() > 0)
                t.Responsibilityid = await db.Responsibilities.MaxAsync(comparer => comparer.Responsibilityid) + 1;
            else
                t.Responsibilityid = 1;

            try
            {
                await db.Responsibilities.AddAsync(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ResponsibilityAddException("ResponsibilityAdd", ex);
            }
            return t.Responsibilityid;
        }
        public async Task<List<Responsibility>> GetAll()
        {
            IQueryable<ResponsibilityDB> Responsibilities = db.Responsibilities;
            List<ResponsibilityDB> conv = await Responsibilities.ToListAsync();
            List<Responsibility> final = new List<Responsibility>();
            foreach (var m in conv)
            {
                final.Add(ResponsibilityConv.DBtoBL(m));
            }
            return final;
        }
        public async Task Update(Responsibility element)
        {
            ResponsibilityDB t = await db.Responsibilities.FindAsync(element.Responsibilityid);
            t.EmployeeID = element.Employee;
            t.ObjectiveID = element.Objective;
            t.Timespent = element.Timespent;
            try
            {
                db.Responsibilities.Update(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ResponsibilityUpdateException("ResponsibilityUpdate", ex);
            }
        }
        public async Task Delete(Responsibility element)
        {
            ResponsibilityDB t = ResponsibilityConv.BltoDB(element);
            if (t == null)
                return;

            try
            {
                db.Responsibilities.Remove(db.Responsibilities.Find(t.Responsibilityid));
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ResponsibilityDeleteException("ResponsibilityDelete", ex);
            }
        }
        public async Task<Responsibility> GetResponsibilityByID(int rid)
        {
            ResponsibilityDB e = await db.Responsibilities.FindAsync(rid);
            return e != null ? ResponsibilityConv.DBtoBL(e) : null;
        }
        public async Task<List<Responsibility>> GetResponsibilityByEmployee(int eid)
        {
            IQueryable<ResponsibilityDB> Responsibilities = db.Responsibilities.Where(needed => needed.EmployeeID == eid);
            List<ResponsibilityDB> conv = await Responsibilities.ToListAsync();
            List<Responsibility> final = new List<Responsibility>();
            foreach (var m in conv)
            {
                final.Add(ResponsibilityConv.DBtoBL(m));
            }
            return final;
        }
        public async Task<Responsibility> GetResponsibilityByObjectiveAndEmployee(int tid, int eid)
        {
            IQueryable<ResponsibilityDB> Responsibilities = db.Responsibilities.Where(needed => needed.ObjectiveID == tid && needed.EmployeeID == eid);
            List<ResponsibilityDB> conv = await Responsibilities.ToListAsync();
            List<Responsibility> final = new List<Responsibility>();
            foreach (var m in conv)
            {
                final.Add(ResponsibilityConv.DBtoBL(m));
            }
            return final.Count() > 0  ? final.First() : null;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
