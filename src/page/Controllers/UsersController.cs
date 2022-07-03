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
    public class UsersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Common();
            IEnumerable<User> Users = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Users");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<User>>();
                    Users = readTask.Result;
                }
                else 
                {
                    Users = Enumerable.Empty<User>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(Users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            User User;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Users/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<User>();
                    User = readTask.Result;
                }
                else 
                {
                    User = null;
                }
            }
            if(User == null)
            {
                return NotFound();
            }
            return View(User);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            User User;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Users/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<User>();
                    User = readTask.Result;
                }
                else 
                {
                    User = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            if(User == null)
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

            IEnumerable<Role> Roles = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Roles");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Role>>();
                    Roles = readTask.Result;
                }
                else 
                {
                    Roles = Enumerable.Empty<Role>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            CreateUserViewModel result = new CreateUserViewModel
            {
                user = User,
                roles = Roles,
                tenants = Tenants
            };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, User User)
        {
            Common();
            if(Id != User.Id)
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
                    var postTask = await client.PutAsJsonAsync("Users/" + Id, User);
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

            IEnumerable<Role> Roles = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Roles");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Role>>();
                    Roles = readTask.Result;
                }
                else 
                {
                    Roles = Enumerable.Empty<Role>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            var createUserViewModel = new CreateUserViewModel{
                user = User,
                tenants = Tenants,
                roles = Roles
            };

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(createUserViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            User User;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.DeleteAsync("Users/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<User>();
                    User = readTask.Result;
                }
                else 
                {
                    return NotFound();
                }
            }
            if(User == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Common();
            User User = new User();

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

            IEnumerable<Role> Roles = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Roles");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Role>>();
                    Roles = readTask.Result;
                }
                else 
                {
                    Roles = Enumerable.Empty<Role>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            CreateUserViewModel result = new CreateUserViewModel
            {
                user = User,
                tenants = Tenants,
                roles = Roles
            };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User User)
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
                    var postTask = await client.PostAsJsonAsync("Users", User);
                    
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

            IEnumerable<Role> Roles = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Roles");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Role>>();
                    Roles = readTask.Result;
                }
                else 
                {
                    Roles = Enumerable.Empty<Role>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            var createUserViewModel = new CreateUserViewModel{
                user = User,
                roles = Roles,
                tenants = Tenants  
            };
            return View(createUserViewModel);
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