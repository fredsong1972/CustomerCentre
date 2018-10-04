using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerCentre.Controllers;
using CustomerCentre.Models;
using CustomerCentre.Services;
using NSubstitute;
using Serilog;
using Shouldly;
using TestStack.BDDfy;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;

namespace CustomerCentre.Test.Controllers
{
    public class HomeControllerTest
    {
        private ICustomerService _service;
        private HomeController _controller;
        private Customer _testData;
        private object _result;

        #region Facts

        [Fact]
        public void IndexShouldReturnDefaultViewResult()
        {
            this.Given(x => GivenHomeControllerSetup())
                .And(x => GivenGetCustomersReturnsCustomers())
                .When(x => WhenIndexGetCalledAsync())
                .Then(x => ThenResultShouldBeViewResult())
                .BDDfy();
        }

        [Fact]
        public void List5OldestShouldReturnDefaultViewResult()
        {
            this.Given(x => GivenHomeControllerSetup())
                .And(x => GivenGetCustomersReturnsCustomers())
                .When(x => WhenList5OldestGetCalledAsync())
                .Then(x => ThenResultShouldBeViewResult())
                .BDDfy();
        }

        [Fact]
        public void CreateShouldReturnViewResult()
        {
            this.Given(x => GivenHomeControllerSetup())
                .When(x => WhenCreateGetCalled())
                .Then(x => ThenResultShouldBeViewResult())
                .BDDfy();
        }

        [Fact]
        public void CreateCustomerShouldReturnsCreatedResponse()
        {
            this.Given(x => GivenHomeControllerSetup())
                .And(x=>GivenCreateCustomerReturnsCustomer())
                .When(x => WhenPostCustomerToCreate())
                .Then(x => ThenResultShouldBeViewResult())
                .BDDfy();
        }

        [Fact]
        public void CreateInvalidCustomerShouldReturnsFailedResponse()
        {
            this.Given(x => GivenHomeControllerSetup())
                .And(x => GivenCreateCustomerThrowsException())
                .When(x => WhenPostCustomerToCreate())
                .Then(x => ThenResultShouldBeFailedResponse())
                .BDDfy();
        }
        #endregion

        #region Givens

        private void GivenHomeControllerSetup()
        {
            _service = Substitute.For<ICustomerService>();
            _controller = new HomeController(_service, Substitute.For<ILogger>());
        }

        private void GivenGetCustomersReturnsCustomers()
        {
            _service.GetCustomers().Returns(new List<Customer>());
        }
        private void GivenCreateCustomerReturnsCustomer()
        {
            _testData = new Customer();
            _service.CreateCustomer(_testData).Returns(_testData);
        }

        private void GivenCreateCustomerThrowsException()
        {
            _testData = new Customer();
            _service.CreateCustomer(_testData).ThrowsForAnyArgs(new Exception());
        }

        #endregion

        #region Whens

        private async Task WhenIndexGetCalledAsync()
        {
            _result = await _controller.Index();
        }

        private async Task WhenList5OldestGetCalledAsync()
        {
            _result = await _controller.List5Oldest();
        }
        private void WhenCreateGetCalled()
        {
            _result = _controller.Create();
        }

        private async Task WhenPostCustomerToCreate()
        {
            var customer = new Customer();
            _result = await _controller.Create(customer);
        }

        #endregion

        #region Thens

        private void ThenResultShouldBeViewResult()
        {
            var viewResult = Assert.IsType<ViewResult>(_result);
            Assert.Null(viewResult.ViewName);
        }

        private void ThenResultShouldBeCreatedResponse()
        {
            var viewResult = Assert.IsType<ViewResult>(_result);
            viewResult.ViewData.Values.Count.ShouldBe(1);
            var alert = Assert.IsType<Alert>(viewResult.ViewData.Values.FirstOrDefault());
            alert.Title.ShouldBe("Success!");
        }
        private void ThenResultShouldBeFailedResponse()
        {
            var viewResult = Assert.IsType<ViewResult>(_result);
            viewResult.ViewData.Values.Count.ShouldBe(1);
            var alert = Assert.IsType<Alert>(viewResult.ViewData.Values.FirstOrDefault());
            alert.Title.ShouldBe("Error!");
        }

        #endregion
    }
}
