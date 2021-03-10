using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop_DataAccess.Data;
using WebShop_DataAccess.Repository.IRepository;
using WebShop_Models;

namespace WebShop_DataAccess.Repository
{
     public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationType obj)
        {
            var objFromDb = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                
            }

        }
    }
}
