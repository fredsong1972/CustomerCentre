using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CustomerCentre.Persistence.Models;

namespace CustomerCentre.Persistence.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IList<Customer>> Get5OldestCustomers();
        Task<IList<Customer>> GetAllCustomers();
    }
}
