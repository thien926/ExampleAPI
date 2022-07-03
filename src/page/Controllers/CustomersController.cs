using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using page.Entities;
using page.Models;
using System.Net.Http.Headers;


namespace page.Controllers
{
    public class CustomersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Customer> Customers = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Customers");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Customer>>();
                    Customers = readTask.Result;
                }
                else 
                {
                    Customers = Enumerable.Empty<Customer>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            Common();
            return View(Customers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            Customer Customer;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Customers/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Customer>();
                    Customer = readTask.Result;
                }
                else 
                {
                    Customer = null;
                }
            }
            if(Customer == null)
            {
                return NotFound();
            }
            Common();
            return View(Customer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            Customer Customer;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Customers/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Customer>();
                    Customer = readTask.Result;
                }
                else 
                {
                    Customer = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            if(Customer == null)
            {
                return NotFound();
            }

            IEnumerable<Tenant> Tenants = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Tenants");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Tenant>>();
                    Tenants = readTask.Result;
                }
                else 
                {
                    Tenants = Enumerable.Empty<Tenant>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            CreateCustomerViewModel result = new CreateCustomerViewModel
            {
                customer = Customer,
                tenants = Tenants
            };
            Common();
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, Customer Customer)
        {
            if(Id != Customer.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.urlAPI);
                    var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                    if(!String.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    var postTask = await client.PutAsJsonAsync("Customers/" + Id, Customer);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            IEnumerable<Tenant> Tenants = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Tenants");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Tenant>>();
                    Tenants = readTask.Result;
                }
                else 
                {
                    Tenants = Enumerable.Empty<Tenant>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            var createCustomerViewModel = new CreateCustomerViewModel{
                customer = Customer,
                tenants = Tenants
            };

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            Common();
            return View(createCustomerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            Customer Customer;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.DeleteAsync("Customers/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Customer>();
                    Customer = readTask.Result;
                }
                else 
                {
                    return NotFound();
                }
            }
            if(Customer == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? Id)
        {
            Customer Customer = new Customer();

            IEnumerable<Tenant> Tenants = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Tenants");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Tenant>>();
                    Tenants = readTask.Result;
                }
                else 
                {
                    Tenants = Enumerable.Empty<Tenant>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            CreateCustomerViewModel result = new CreateCustomerViewModel
            {
                customer = Customer,
                tenants = Tenants
            };
            Common();
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer Customer)
        {
            if(ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.urlAPI);
                    var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                    if(!String.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    var postTask = await client.PostAsJsonAsync("Customers", Customer);
                    
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            IEnumerable<Tenant> Tenants = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Tenants");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Tenant>>();
                    Tenants = readTask.Result;
                }
                else 
                {
                    Tenants = Enumerable.Empty<Tenant>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            var createCustomerViewModel = new CreateCustomerViewModel{
                customer = Customer,
                tenants = Tenants  
            };
            Common();
            return View(createCustomerViewModel);
        }

        public async void Common() 
        {
            User User;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("auth/getuser");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<User>();
                    User = readTask.Result;
                    // Console.WriteLine(User.Name);
                    ViewData["CurrentUser"] = User.Name;
                }
                else 
                {
                    User = null;
                }
            }
            
        }
    }
}