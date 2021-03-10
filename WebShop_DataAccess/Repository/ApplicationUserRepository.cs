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
     public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       
    }
}
