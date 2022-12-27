﻿using Microsoft.Extensions.Logging;
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
    public class DepartmentRepository : IDepartmentRepository, IDisposable
    {
        private readonly transfersystemContext db;
        public DepartmentRepository(transfersystemContext curDb)
        {
            db = curDb;
        }
        public async Task<int> Add(Department element)
        {
            DepartmentDB t = DepartmentConv.BltoDB(element);

            if (await db.Departments.CountAsync() > 0)
                t.Departmentid = await db.Departments.MaxAsync(comparer => comparer.Departmentid) + 1;
            else
                t.Departmentid = 1;

            try
            {
                await db.Departments.AddAsync(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DepartmentAddException("DepartmentAdd", ex);
            }
            return t.Departmentid;
        }
        public async Task<List<Department>> GetAll()
        {
            IQueryable<DepartmentDB> Departments = db.Departments;
            List<DepartmentDB> conv = await Departments.ToListAsync();
            List<Department> final = new List<Department>();
            foreach (var m in conv)
            {
                final.Add(DepartmentConv.DBtoBL(m));
            }
            return final;
        }
        public async Task Update(Department element)
        {
            DepartmentDB o = await db.Departments.FindAsync(element.Departmentid);

            if (o == null)
                return;

            o.Title = element.Title;
            o.CompanyID = element.Company;
            o.Foundationyear = element.Foundationyear;
            o.Activityfield = element.Activityfield;
            try
            {
                db.Departments.Update(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DepartmentUpdateException("DepartmentUpdate", ex);
            }
        }
        public async Task Delete(Department element)
        {
            DepartmentDB o = await db.Departments.FindAsync(element.Departmentid);
            if (o == null)
                return;

            try
            {
                db.Departments.Remove(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DepartmentDeleteException("DepartmentDelete", ex);
            }
        }
        public async Task<Department> GetDepartmentByID(int? departmentid)
        {
            if (departmentid == null)
                return null;

            DepartmentDB e = await db.Departments.FindAsync(departmentid);
            return e != null ? DepartmentConv.DBtoBL(e) : null;
        }
        public async Task<List<Department>> GetDepartmentsByCompany(int company)
        {

            IQueryable<DepartmentDB> Departments = db.Departments.Where(needed => needed.CompanyID == company);
            List<DepartmentDB> conv = await Departments.ToListAsync();
            List<Department> final = new List<Department>();
            foreach (var m in conv)
            {
                final.Add(DepartmentConv.DBtoBL(m));
            }
            return final;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
