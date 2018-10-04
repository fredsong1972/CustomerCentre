using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomerCentre.Models;
using CustomerCentre.Services;
using Serilog;

namespace CustomerCentre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;
        public HomeController(ICustomerService customerService, ILogger logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        /// <summary>
        /// List view
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var model = await _customerService.GetCustomers();
            return View(model);
        }
        /// <summary>
        /// 5 Oldest Customers View
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> List5Oldest()
        {
            var model = await _customerService.Get5OldestCustomers();
            return View(model);
        }
        /// <summary>
        /// Create View
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customer = await _customerService.CreateCustomer(customer);
                    ViewBag.Alert = new Alert {CssClassName = "text-info", Title = "Success!", Message = "The customer has been created successfully."};
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Unhandled Exception");
                    ViewBag.Alert = new Alert { CssClassName = "text-danger", Title = "Error!", Message = "Failed to create customer." };
                }
            }
            return View(customer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
