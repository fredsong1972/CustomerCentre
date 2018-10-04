using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CustomerCentre.Models;
using CustomerCentre.Persistence.Repositories;
using CustomerCentre.Services;
using NSubstitute;
using Serilog;
using TestStack.BDDfy;
using Xunit;

namespace CustomerCentre.Test.Services
{
    using CustomerEntity = CustomerCentre.Persistence.Models.Customer;

    public class CustomerServiceTest
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private CustomerEntity _testData;
        private Customer _testCustomer;
        private readonly CustomerService _subject;

        public CustomerServiceTest()
        {
            _repository = Substitute.For<ICustomerRepository>();
            _mapper = Substitute.For<IMapper>();
            _subject = new CustomerService(_repository, _mapper, Substitute.For<ILogger>());
        }

        #region Facts

        [Fact]
        public void Get5OldestCustomersShouldGet5OldestCustomers()
        {
            this.Given(x => GivenGetCustomersReturnsCustomers())
                .When(x => WhenGet5OldestCustomersIsCalledAsync())
                .Then(x => ThenItShouldCallGet5OldestCustomersMethodInDbActions())
                .BDDfy();
        }

        [Fact]
        public void GetCustomersShouldGetAllCustomers()
        {
            this.Given(x => GivenGetCustomersReturnsCustomers())
                .When(x => WhenGetCustomersIsCalledAsync())
                .Then(x => ThenItShouldCallGetAllCustomersMethodInDbActions())
                .BDDfy();
        }
        [Fact]
        public void CreateCustomersShouldAddEntity()
        {
            this.Given(x => GivenAddCustomerReturnsCustomerEntity())
                .When(x => WhenCreateCustomersIsCalledAsync())
                .Then(x => ThenItShouldCallAddEntityMethodInDbActions())
                .BDDfy();
        }

        #endregion

        #region Givens

        private void GivenGetCustomersReturnsCustomers()
        {
            _repository.Get5OldestCustomers()
                .Returns(new List<CustomerEntity>());

            _mapper.Map<IList<CustomerEntity>, IList <Customer>>(new List<CustomerEntity>()).ReturnsForAnyArgs(new List<Customer>());
        }

        private void GivenAddCustomerReturnsCustomerEntity()
        {
            _testData = new CustomerEntity();
            _testCustomer = new Customer();
            _repository.AddEntity(_testData).Returns(_testData);

            _mapper.Map<Customer, CustomerEntity>(_testCustomer).Returns(_testData);
        }

        #endregion

        #region Whens

        private async Task WhenGet5OldestCustomersIsCalledAsync()
        {
            await _subject.Get5OldestCustomers();
        }

        private async Task WhenGetCustomersIsCalledAsync()
        {
            await _subject.GetCustomers();
        }

        private async Task WhenCreateCustomersIsCalledAsync()
        {
            await _subject.CreateCustomer(_testCustomer);
        }

        #endregion

        #region Thens

        private void ThenItShouldCallGet5OldestCustomersMethodInDbActions()
        {
            _repository.Received().Get5OldestCustomers();

        }

        private void ThenItShouldCallGetAllCustomersMethodInDbActions()
        {
            _repository.Received().GetAllCustomers();

        }

        private void ThenItShouldCallAddEntityMethodInDbActions()
        {
            _repository.Received().AddEntity(_testData);

        }

        #endregion
    }
}
