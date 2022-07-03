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
    public class FormTypesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Common();
            IEnumerable<FormType> FormTypes = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("FormTypes");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<FormType>>();
                    FormTypes = readTask.Result;
                }
                else 
                {
                    FormTypes = Enumerable.Empty<FormType>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(FormTypes);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            FormType FormType;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("FormTypes/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<FormType>();
                    FormType = readTask.Result;
                }
                else 
                {
                    FormType = null;
                }
            }
            if(FormType == null)
            {
                return NotFound();
            }

            return View(FormType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            FormType FormType;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("FormTypes/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<FormType>();
                    FormType = readTask.Result;
                }
                else 
                {
                    FormType = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            if(FormType == null)
            {
                return NotFound();
            }

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

            CreateFormTypeViewModel result = new CreateFormTypeViewModel
            {
                formType = FormType,
                customers = Customers
            };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, FormType FormType)
        {
            Common();
            if(Id != FormType.Id)
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
                    var postTask = await client.PutAsJsonAsync("FormTypes/" + Id, FormType);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

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

            CreateFormTypeViewModel result = new CreateFormTypeViewModel
            {
                formType = FormType,
                customers = Customers
            };

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            FormType FormType;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.DeleteAsync("FormTypes/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<FormType>();
                    FormType = readTask.Result;
                }
                else 
                {
                    return NotFound();
                }
            }
            if(FormType == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? Id)
        {
            Common();
            FormType FormType = new FormType();

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

            CreateFormTypeViewModel result = new CreateFormTypeViewModel
            {
                formType = FormType,
                customers = Customers
            };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FormType FormType)
        {
            Common();
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
                    var postTask = await client.PostAsJsonAsync("FormTypes", FormType);
                    
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

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

            CreateFormTypeViewModel result = new CreateFormTypeViewModel
            {
                formType = FormType,
                customers = Customers
            };

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(result);
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