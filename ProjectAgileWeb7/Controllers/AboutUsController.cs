using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectAgileWeb7.Models;

namespace ProjectAgileWeb7.Controllers
{
    public class AboutUsController : Controller
    { 
        public IActionResult Index()
        {
            IEnumerable<AboutUs> facilities = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");
                //HTTP GET
                var responseTask = client.GetAsync("AboutUs");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<AboutUs>>();
                    readTask.Wait();

                    facilities = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    facilities = Enumerable.Empty<AboutUs>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            Console.WriteLine(facilities);
            return View(facilities);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AboutUs facilities)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<AboutUs>("AboutUs", facilities);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(facilities);
        }
        //public ActionResult Edit(int id)
        //{
        //    AboutUs facility = null;

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://localhost:44328/api/");
        //        //HTTP GET
        //        var responseTask = client.GetAsync("AboutUs/" + id.ToString());
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<AboutUs>();
        //            readTask.Wait();

        //            facility = readTask.Result;
        //        }
        //    }
        //    return View(facility);
        //}
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("AboutUs/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
