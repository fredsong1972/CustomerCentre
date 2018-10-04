using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerCentre.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CustomerCentre.Persistence.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbContextFactory dbContextFactory, ILogger logger) : base(dbContextFactory, logger)
        {
        }

        /// <summary>
        /// Get 5 oldest customer and order by last name
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Customer>> Get5OldestCustomers()
        {
            var list = await DbContext.Customers.OrderBy(x => x.DateOfBirth).Take(5).OrderBy(x=>x.LastName).ToListAsync();
            return list;
        }
        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Customer>> GetAllCustomers()
        {
            var list = await DbContext.Customers.OrderBy(x => x.LastName).ToListAsync();
            return list;
        }
    }
}
