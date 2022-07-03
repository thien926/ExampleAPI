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
    public class RolesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Common();
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
            return View(Roles);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            Role Role;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Roles/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Role>();
                    Role = readTask.Result;
                }
                else 
                {
                    Role = null;
                }
            }
            if(Role == null)
            {
                return NotFound();
            }

            if(Role.Detail != null)
            {
                string[] arr = Role.Detail.Split("&");
                string res = "";
                foreach (var item in arr)
                {
                    if(item != "")
                    {
                        // Console.WriteLine(item);
                        res += item.Split(" ")[1] + " ";
                    }
                }
                Role.Detail = res;
            }

            return View(Role);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? Id)
        {
            Common();
            if(Id == null)
            {
                return NotFound();
            }
            Role Role;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.GetAsync("Roles/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Role>();
                    Role = readTask.Result;
                }
                else 
                {
                    Role = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            if(Role == null)
            {
                return NotFound();
            }

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

            CreateRoleViewModel result = new CreateRoleViewModel
            {
                role = Role,
                permissions = permissions
            };
            return View(result);

            // return View(Role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, Role Role)
        {
            Common();
            if(Id != Role.Id)
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
                    var postTask = await client.PutAsJsonAsync("Roles/" + Id, Role);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

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
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            var createRoleViewModel = new CreateRoleViewModel();
            createRoleViewModel.role = Role;
            createRoleViewModel.permissions = permissions;
            return View(createRoleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            Role Role;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                if(!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage  responseTask = await client.DeleteAsync("Roles/" + Id);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<Role>();
                    Role = readTask.Result;
                }
                else 
                {
                    return NotFound();
                }
            }
            if(Role == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? Id)
        {
            Common();
            Role Role = new Role();
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

            CreateRoleViewModel result = new CreateRoleViewModel
            {
                role = Role,
                permissions = permissions
            };
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role Role)
        {
            Common();
            IEnumerable<Permission> permissions = null;
            if(ModelState.IsValid)
            {
                // var Role = createRoleViewModel.role;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.urlAPI);
                    var token = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Token");
                    if(!String.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    var postTask = await client.PostAsJsonAsync("Roles", Role);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
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
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            var createRoleViewModel = new CreateRoleViewModel();
            createRoleViewModel.role = Role;
            createRoleViewModel.permissions = permissions;
            return View(createRoleViewModel);
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