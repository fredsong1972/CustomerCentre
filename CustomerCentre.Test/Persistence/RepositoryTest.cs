using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerCentre.Persistence.Config;
using CustomerCentre.Persistence.Models;
using CustomerCentre.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using Serilog;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace CustomerCentre.Test.Persistence
{
    public class RepositoryTest
    {
        private DbContextOptions<CustomerCentreDbContext> _contextOptions;
        private Customer _testData;
        private CustomerCentreDbContext _appContext;
        private IOptions<DbContextSettings> _settings;
        private IDbContextFactory _dbContextFactory;
        private Repository<Customer> _subject;
        private Customer _result;

        public RepositoryTest()
        {
            _testData = new Customer { Id = 100, FirstName="John", LastName="Smith", DateOfBirth = new DateTime(1961, 11, 01) };

        }
        #region Facts

        [Fact]
        public void CreateCustomerShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasCustomers(1))
                .When(x => WhenCreateIsCalledWithTheCustomerAsync(_testData))
                .Then(x => ThenItShouldReturnTheCustomer(_testData))
                .BDDfy();
        }

        [Fact]
        public void CreateCustomerShouldThrowException()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasACustomer(_testData))
                .When(x => WhenCreateSameIdIsCalledWithTheCustomerAsync(_testData))
                .Then(x => ThenItShouldBeSuccessful())
                .BDDfy();
        }

        [Fact]
        public void GetCustomerShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasACustomer(_testData))
                .When(x => WhenGetCalledWithTheCustomerIdAsync(_testData.Id))
                .Then(x => ThenItShouldReturnTheCustomer(_testData))
                .BDDfy();

        }

        [Fact]
        public void UpdateCustomerShouldSucceed()
        {
            var session = new Customer
            {
                Id = _testData.Id, FirstName = "Udpated", LastName = "Client", DateOfBirth = new DateTime(1970, 1, 1),
                Email = "updatedCleint@test.com"
            };
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasACustomer(_testData))
                .When(x => WhenUpdateCalledWithTheCustomerAsync(session))
                .Then(x => ThenItShouldReturnTheCustomer(session))
                .BDDfy();
        }

        [Fact]
        public void DeleteCustomerShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasACustomer(_testData))
                .When(x => WhenDeleteCalledWithTheCustomerIdAsync(_testData.Id))
                .Then(x => ThenItShouldBeNoExistCustomer())
                .BDDfy();
        }

        #endregion Facts

        #region Givens
        private void GivenADatabase(string context)
        {
            _contextOptions = MockDatabaseHelper.CreateNewContextOptions(context);
            _appContext = new CustomerCentreDbContext(_contextOptions);
            _settings = Substitute.For<IOptions<DbContextSettings>>();

            _settings.Value.Returns(new DbContextSettings { CustomerCentreConnectionString = "test" });
            _dbContextFactory = Substitute.For<IDbContextFactory>();
            _dbContextFactory.DbContext.Returns(_appContext);
            _subject = new Repository<Customer>(_dbContextFactory, Substitute.For<ILogger>());
        }

        private void GivenTheDatabaseHasCustomers(int numberOfCustomers)
        {
            var customers = new List<Customer>();
            for (var item = 0; item < numberOfCustomers; item++)
            {
                customers.Add(
                    new Customer()
                    {
                        Id = item +1,
                        FirstName = $"Client{item}",
                        LastName = $"test",
                        DateOfBirth = new DateTime(1950 + item, 12, 11),
                        Email = $"Client{item}@test.com",
                        Modified = DateTimeOffset.UtcNow
                    }
                );
            }

            _appContext.Customers.AddRange(customers);
            _appContext.SaveChanges();
        }

        private void GivenTheDatabaseHasACustomer(Customer customer)
        {
            _appContext.Customers.Add(customer);
            _appContext.SaveChanges();
        }


        #endregion

        #region Whens
        private async Task<bool> WhenCreateIsCalledWithTheCustomerAsync(Customer customer)
        {
            _result = await _subject.AddEntity(customer);
            return true;
        }

        private async Task WhenCreateSameIdIsCalledWithTheCustomerAsync(Customer customer)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _subject.AddEntity(customer));
        }

        private async Task<bool> WhenGetCalledWithTheCustomerIdAsync(int id)
        {
            _result = await _subject.GetEntity(id);
            return true;
        }

        private async Task<bool> WhenUpdateCalledWithTheCustomerAsync(Customer customer)
        {
            var entity = await _subject.GetEntity(customer.Id);
            entity.FirstName = customer.FirstName;
            entity.LastName = customer.LastName;
            entity.DateOfBirth = customer.DateOfBirth;
            entity.Email = customer.Email;
            entity.Modified = DateTimeOffset.UtcNow;
            _result = await _subject.UpdateEntity(entity);
            return true;
        }

        private async Task<bool> WhenDeleteCalledWithTheCustomerIdAsync(int id)
        {
            await _subject.DeleteEntity(id);
            return true;
        }

        #endregion

        #region Thens
        private void ThenItShouldReturnTheCustomer(Customer customer)
        {
            _result.Id.ShouldBe(customer.Id);
        }

        private void ThenItShouldBeNoExistCustomer()
        {
            _appContext.Customers.Count().ShouldBe(0);
        }

        private void ThenItShouldBeSuccessful()
        { }


        #endregion
    }
}
