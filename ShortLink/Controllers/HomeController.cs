using Microsoft.AspNetCore.Mvc;
using ShortLink.Interfaces;
using ShortLink.Models;
using ShortLink.Repository;
using System;
using System.Diagnostics;
using System.Linq;

namespace ShortLink.Controllers
{
    public class HomeController : Controller
    {
        IRepository repo;

        public HomeController(IRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View(repo.GetAll());
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            Url url = repo.GetById(id.Value);
            if (url == null)
                return NotFound();

            return View(url);
        }

        [HttpPost]
        public IActionResult Edit(Url url)
        {
            if (ModelState.IsValid)
            {
                if (ShortLinkExtensions.CheckLink(url.LongUrl) && (repo.GetAll().FirstOrDefault(u => u.LongUrl == url.LongUrl) == null))
                {
                    url.ShortUrl = ShortLinkExtensions.GetShortUrl(url.LongUrl);
                    repo.Edit(url);
                    repo.Save();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ссылка, введенная Вами, некорректна или уже была добавленая ранее");
                }
            }

            return View(url);
        }


        [HttpGet]
        public IActionResult Create() => View(new Url { LongUrl = "", ShortUrl = "" });


        [HttpPost]
        public IActionResult Create(Url url)
        {
            if (ModelState.IsValid)
            {
                if (repo.GetAll().FirstOrDefault(u => u.LongUrl == url.LongUrl) == null)
                {
                    if (ShortLinkExtensions.CheckLink(url.LongUrl))
                    {
                        url.ShortUrl = ShortLinkExtensions.GetShortUrl(url.LongUrl);
                        url.DateCreate = DateTime.Now;
                        url.NumberFollowTheLink = 0;
                        repo.Create(url);
                        repo.Save();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ссылка, введенная Вами, некорректна");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Такая ссылка уже существует");
                }
            }
            return View(url);
        }

        public IActionResult RedirectToUrl(string link)
        {
            Url urlToRedirect = repo.GetAll().FirstOrDefault(u => u.ShortUrl == link);
            if (urlToRedirect != null)
            {
                urlToRedirect.NumberFollowTheLink++;
                repo.Edit(urlToRedirect);
                repo.Save();

                return Redirect(urlToRedirect.LongUrl);

            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            Url url = repo.GetById(id.Value);
            if (url != null)
            {
                repo.Delete(url);
                repo.Save();
            }
            else
                return NotFound();

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
