using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using CustomerCentre.Models;
using CustomerCentre.Persistence.Repositories;
using CustomerEntity = CustomerCentre.Persistence.Models.Customer;
using Serilog;

namespace CustomerCentre.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// Get 5 oldest customers and order by last name
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Customer>> Get5OldestCustomers()
        {
            var entities = await _repository.Get5OldestCustomers();
            var customers = _mapper.Map<IList<CustomerEntity>, IList<Customer>>(entities);
            return customers;
        }
        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Customer>> GetCustomers()
        {
            var entities = await _repository.GetAllCustomers();
            var customers = _mapper.Map<IList<CustomerEntity>, IList<Customer>>(entities);
            return customers;
        }
        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<Customer> CreateCustomer(Customer customer)
        {
            var entity = _mapper.Map<Customer, CustomerEntity>(customer);
            entity = await _repository.AddEntity(entity);
            var newCustomer = _mapper.Map<CustomerEntity, Customer>(entity);
            return newCustomer;
        }
    }
}
