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
    public class TenantsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Common();
            IEnumerable<Tenant> Tenants = null;
            var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
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
            return View(Tenants);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            Tenant Tenant;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Tenants/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Tenant>();
                    Tenant = readTask.Result;
                }
                else 
                {
                    Tenant = null;
                }
            }
            if(Tenant == null)
            {
                return NotFound();
            }

            return View(Tenant);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            Tenant Tenant;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Tenants/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Tenant>();
                    Tenant = readTask.Result;
                }
                else 
                {
                    Tenant = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            if(Tenant == null)
            {
                return NotFound();
            }

            return View(Tenant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, Tenant Tenant)
        {
            Common();
            if(Id != Tenant.Id)
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
                    var postTask = await client.PutAsJsonAsync("Tenants/" + Id, Tenant);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(Tenant);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            Tenant Tenant;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.DeleteAsync("Tenants/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Tenant>();
                    Tenant = readTask.Result;
                }
                else 
                {
                    return NotFound();
                }
            }
            if(Tenant == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? Id)
        {
            Common();
            Tenant Tenant = new Tenant();
            return View(Tenant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tenant Tenant)
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
                    var postTask = await client.PostAsJsonAsync("Tenants", Tenant);
                    
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(Tenant);
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