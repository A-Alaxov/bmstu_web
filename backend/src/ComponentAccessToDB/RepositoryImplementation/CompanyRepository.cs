using System;
using System.Collections.Generic;
using System.Linq;
using ComponentBuisinessLogic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ComponentAccessToDB
{
    public class CompanyRepository : ICompanyRepository, IDisposable
    {
        private readonly transfersystemContext db;
        public CompanyRepository(transfersystemContext curDb)
        {
            db = curDb;
        }
        public async Task<int> Add(Company element)
        {
            CompanyDB t = CompanyConv.BltoDB(element);

            if (await db.Companies.CountAsync() > 0)
                t.Companyid = await db.Companies.MaxAsync(comparer => comparer.Companyid) + 1;
            else
                t.Companyid = 1;

            try
            {
                await db.Companies.AddAsync(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new CompanyAddException("CompanyAdd", ex);
            }
            return t.Companyid;
        }
        public async Task<List<Company>> GetAll()
        {
            IQueryable<CompanyDB> Companys = db.Companies;
            List<CompanyDB> conv = await Companys.ToListAsync();
            List<Company> final = new List<Company>();
            foreach (var m in conv)
            {
                final.Add(CompanyConv.DBtoBL(m));
            }
            return final;
        }
        public async Task Update(Company element)
        {
            CompanyDB o = await db.Companies.FindAsync(element.Companyid);
            o.Title = element.Title;
            o.Foundationyear = element.Foundationyear;
            try
            {
                db.Companies.Update(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new CompanyUpdateException("CompanyUpdate", ex);
            }
        }
        public async Task Delete(Company element)
        {
            CompanyDB o = await db.Companies.FindAsync(element.Companyid);
            if (o == null)
                return;

            try
            {
                db.Companies.Remove(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new CompanyDeleteException("CompanyDelete", ex);
            }
        }
        public async Task<Company> GetCompanyByID(int companyid)
        {
            CompanyDB e = await db.Companies.FindAsync(companyid);
            return e != null ? CompanyConv.DBtoBL(e) : null;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
