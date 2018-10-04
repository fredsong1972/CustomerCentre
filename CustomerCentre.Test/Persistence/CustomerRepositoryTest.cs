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
    public class CustomerRepositoryTest
    {
        private DbContextOptions<CustomerCentreDbContext> _contextOptions;
        private CustomerCentreDbContext _appContext;
        private IOptions<DbContextSettings> _settings;
        private IDbContextFactory _dbContextFactory;
        private CustomerRepository _subject;
        private IList<Customer> _testData;
        private IList<Customer> _result;

        #region Facts

        [Fact]
        public void Get5OldestCustomersShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasCustomers(10))
                .When(x => WhenGet5OldestCustomersCalledAsync())
                .Then(x => ThenItShouldBe5CustomersAsExpected())
                .BDDfy();
        }

        [Fact]
        public void GetAllCustomersShouldSucceed()
        {
            this.Given(x => GivenADatabase("TestDb"))
                .Given(x => GivenTheDatabaseHasCustomers(10))
                .When(x => WhenGetAllCustomersCalledAsync())
                .Then(x => ThenItShouldBeAllCustomersAsExpected())
                .BDDfy();
        }
        #endregion

        #region Givens
        private void GivenADatabase(string context)
        {
            _contextOptions = MockDatabaseHelper.CreateNewContextOptions(context);
            _appContext = new CustomerCentreDbContext(_contextOptions);
            _settings = Substitute.For<IOptions<DbContextSettings>>();

            _settings.Value.Returns(new DbContextSettings { CustomerCentreConnectionString = "test" });
            _dbContextFactory = Substitute.For<IDbContextFactory>();
            _dbContextFactory.DbContext.Returns(_appContext);
            _subject = new CustomerRepository(_dbContextFactory, Substitute.For<ILogger>());
        }

        private void GivenTheDatabaseHasCustomers(int numberOfCustomers)
        {
            _testData = new List<Customer>();
            for (var item = 0; item < numberOfCustomers; item++)
            {
                _testData.Add(
                    new Customer()
                    {
                        Id = item + 1,
                        FirstName = $"Client{item}",
                        LastName = $"test",
                        DateOfBirth = new DateTime(1950 + item, 12, 11),
                        Email = $"Client{item}@test.com",
                        Modified = DateTimeOffset.UtcNow
                    }
                );
            }

            _appContext.Customers.AddRange(_testData);
            _appContext.SaveChanges();
        }

        #endregion

        #region Whens
        private async Task<bool> WhenGet5OldestCustomersCalledAsync()
        {
            _result = await _subject.Get5OldestCustomers();
            return true;
        }

        private async Task<bool> WhenGetAllCustomersCalledAsync()
        {
            _result = await _subject.GetAllCustomers();
            return true;
        }
        #endregion

        #region Thens

        private void ThenItShouldBe5CustomersAsExpected()
        {
            _result.Count.ShouldBe(5);
            var customers = _testData.OrderBy(x => x.DateOfBirth).Take(5).OrderBy(x => x.LastName);
            Assert.Equal(_result, customers);
        }

        private void ThenItShouldBeAllCustomersAsExpected()
        {
            _result.Count.ShouldBe(_testData.Count);
            var customers = _testData.OrderBy(x => x.LastName);
            Assert.Equal(_result, customers);
        }
        #endregion

    }
}
