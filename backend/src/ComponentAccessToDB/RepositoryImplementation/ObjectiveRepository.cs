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
    public class ObjectiveRepository : IObjectiveRepository, IDisposable
    {
        private readonly transfersystemContext db;
        public ObjectiveRepository(transfersystemContext curDb)
        {
            db = curDb;
        }
        public async Task<int> Add(Objective element)
        {
            ObjectiveDB o = ObjectiveConv.BltoDB(element);

            if (await db.Objectives.CountAsync() > 0)
                o.Objectiveid = await db.Objectives.MaxAsync(comparer => comparer.Objectiveid) + 1;
            else
                o.Objectiveid = 1;

            try
            {
                await db.Objectives.AddAsync(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ObjectiveAddException("ObjectiveAdd", ex);
            }
            return o.Objectiveid;
        }
        public async Task<List<Objective>> GetAll()
        {
            IQueryable<ObjectiveDB> Objectives = db.Objectives;
            List<ObjectiveDB> conv = await Objectives.ToListAsync();
            List<Objective> final = new List<Objective>();
            foreach (var m in conv)
            {
                final.Add(ObjectiveConv.DBtoBL(m));
            }
            return final;
        }
        public async Task Update(Objective element)
        {
            ObjectiveDB o = await db.Objectives.FindAsync(element.Objectiveid);
            o.ParentTaskID = element.Parentobjective;
            o.Title = element.Title;
            o.CompanyID = element.Company;
            o.DepartmentID = element.Department;
            o.Termbegin = element.Termbegin;
            o.Termend = element.Termend;
            o.Estimatedtime = element.Estimatedtime;
            try
            {
                db.Objectives.Update(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ObjectiveUpdateException("ObjectiveUpdate", ex);
            }
        }
        public async Task Delete(Objective element)
        {
            ObjectiveDB o = await db.Objectives.FindAsync(element.Objectiveid);
            if (o == null)
                return;

            try
            {
                db.Objectives.Remove(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ObjectiveDeleteException("ObjectiveDelete", ex);
            }
        }
        public async Task<Objective> GetObjectiveByID(int? id)
        {
            ObjectiveDB oDB = await db.Objectives.FindAsync(id);
            if (oDB == null)
                return null;

            Objective o = ObjectiveConv.DBtoBL(oDB);
            return o;
        }
        public async Task<List<Objective>> GetObjectivesByTitle(string title)
        {
            IQueryable<ObjectiveDB> Objectives = db.Objectives.Where(needed => needed.Title == title);
            List<ObjectiveDB> conv = await Objectives.ToListAsync();
            List<Objective> final = new List<Objective>();
            foreach (var m in conv)
            {
                final.Add(ObjectiveConv.DBtoBL(m));
            }
            return final;
        }
        public async Task<List<Objective>> GetSubObjectives(int tid)
        {
            IQueryable<ObjectiveDB> Objectives = db.Objectives.Where(needed => needed.ParentTaskID == tid);
            List<ObjectiveDB> conv = await Objectives.ToListAsync();
            List<Objective> final = new List<Objective>();
            foreach (var m in conv)
            {
                final.Add(ObjectiveConv.DBtoBL(m));
            }
            return final;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
