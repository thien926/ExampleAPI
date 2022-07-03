using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using page.Entities;
using System.Net.Http;
using System.Diagnostics;
using page.Models;
using System.Net.Http.Headers;


namespace page.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login Login)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.urlAPI);
                var postTask = await client.PostAsJsonAsync("auth/login", Login);
                if (postTask.IsSuccessStatusCode)
                {
                    var readTask = postTask.Content.ReadAsStringAsync();
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Token", readTask.Result);
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            Common();
            return View(Login);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var Login = new Login();
            Common();
            return View(Login);
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