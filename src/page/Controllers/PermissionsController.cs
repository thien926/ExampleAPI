
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
    public class PermissionsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Common();
            IEnumerable<Permission> permissions = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("permissions");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<Permission>>();
                    permissions = readTask.Result;
                }
                else 
                {
                    permissions = Enumerable.Empty<Permission>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(permissions);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            Permission permission;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("permissions/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Permission>();
                    permission = readTask.Result;
                }
                else 
                {
                    permission = null;
                }
            }
            if(permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            Permission permission;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("permissions/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Permission>();
                    permission = readTask.Result;
                }
                else 
                {
                    permission = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            if(permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, Permission permission)
        {
            Common();
            if(Id != permission.Id)
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
                    var postTask = await client.PutAsJsonAsync("permissions/" + Id, permission);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(permission);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            Permission permission;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.DeleteAsync("permissions/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Permission>();
                    permission = readTask.Result;
                }
                else 
                {
                    return NotFound();
                }
            }
            if(permission == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? Id)
        {
            Common();
            Permission permission = new Permission();
            return View(permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Permission permission)
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
                    var postTask = await client.PostAsJsonAsync("permissions", permission);
                    
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(permission);
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