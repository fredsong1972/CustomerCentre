using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerCentre.Models;

namespace CustomerCentre.Services
{
    public interface ICustomerService
    {
        Task<IList<Customer>> GetCustomers();
        Task<Customer> CreateCustomer(Customer customer);
        Task<IList<Customer>> Get5OldestCustomers();
    }
}
