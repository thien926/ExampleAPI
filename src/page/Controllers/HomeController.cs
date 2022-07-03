using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using page.Models;
using System.Net.Http.Headers;
using page.Entities;
namespace page.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Common();
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
            }
            else 
            {
                User = null;
            }
        }

        return View(User);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        SessionHelper.SetObjectAsJson(HttpContext.Session, "Token", "");
        return new JsonResult("ok");
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
