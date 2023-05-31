using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using books.Models;
using books.Models.Entities;
using books.Models.AdminViewModels;
using books.Models.ViewModels;

//
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
//

using System.Security.Claims;


namespace books.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly KitapDbContext db = new KitapDbContext();

    public AdminController(ILogger<HomeController> logger, KitapDbContext _db)
    {
        _logger = logger;
        db = _db;
    }

    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(UserVM postedData)
    {
        var user = (from x in db.Users
                    where x.Username == postedData.username && x.Password == postedData.password
                    select x).FirstOrDefault();

        if (user != null)
        {
            var claims = new List<Claim> {
                new Claim("user", user.Id.ToString()),
                new Claim("role", "admin"),
            };

            var ClaimsIdentity = new ClaimsIdentity(claims, "Cookies", "User", "Role");
            var claimsPrincipal = new ClaimsPrincipal(ClaimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            return Redirect("/admin");
        }
        else
        {
            TempData["NotFound"] = "Hatalı girdiniz.";
        }

        return View();
    }


    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/Admin");
    }

    [Route("/Admin/Turler")]
    public IActionResult Turler()
    {
        List<TurlerVM> Turler = (from x in db.Turlers
                                 select new TurlerVM
                                 {
                                     Id = x.Id,
                                     TurAdi = x.TurAdi,
                                     Sira = x.Sira
                                 }).ToList();
        return View(Turler);
    }


    [HttpGet]
    [Route("/Admin/TurForm/{turId?}")]
    public IActionResult TurForm(int? turId)
    {
        if (turId != null)
        {
            TurlerVM duzenlenecekTur = (from x in db.Turlers
                                        where x.Id == turId
                                        select new TurlerVM
                                        {
                                            Id = x.Id,
                                            Sira = x.Sira,
                                            TurAdi = x.TurAdi
                                        }).FirstOrDefault();

            ViewBag.Pagetitle = "Tür Düzenle";
            return View(duzenlenecekTur);
        }
        else if (turId == null)
        {
            ViewBag.Pagetitle = "Tür Ekle";
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TurForm(TurlerVM gelenData)
    {
        if (!ModelState.IsValid)
        {
            return View(gelenData);
        }
        if (gelenData.Id != 0)
        {
            Turler duzenlenecekTur = db.Turlers.Find(gelenData.Id);
            if (duzenlenecekTur != null)
            {
                duzenlenecekTur.Sira = gelenData.Sira;
                duzenlenecekTur.TurAdi = gelenData.TurAdi;
            }
        }
        else if (gelenData.Id == 0)
        {
            Turler yeniTur = new Turler
            {
                TurAdi = gelenData.TurAdi,
                Sira = gelenData.Sira,
            };

            await db.AddAsync(yeniTur);
        }
        await db.SaveChangesAsync();

        return Redirect("/Admin/Turler");
    }

    /*[Route("/Admin/TurSil/{turId?}")]
    [HttpGet]
        public IActionResult TurSil(int? turId)
    {
        TurlerVM tur = new TurlerVM();
        if (turId != null)
        {
            tur = (from x in db.Turlers
            where x.Id == turId
            select new TurlerVM
            {
                Id = x.Id,
                TurAdi = x.TurAdi,
                Sira = x.Sira,
            }
            ).FirstOrDefault();
        }
        return View(tur);
    }

    [HttpPost]
        public  IActionResult TurSil(TurlerVM gelenData)
    {
        TurlerVM silinecekTur = (from x in db.Turlers
        where x.TurAdi == gelenData.TurAdi
        where x.Sira == gelenData.Sira
                            select new TurlerVM
                            {
                                Id = x.Id,
                                TurAdi = x.TurAdi,
                                Sira = x.Sira
                            }).FirstOrDefault();

        Turler silinecekTur = new Turler
        {
            Id = gelenData.Id,
            TurAdi = gelenData.TurAdi,
            Sira = gelenData.Sira,
        };

            db.Remove(silinecekTur);
            db.SaveChangesAsync();

        return Redirect("/Admin/Turler");
    }*/

    [Route("/Admin/TurSil/{turId?}")]
    public async Task<IActionResult> TurSil(int turId)
    {
        Turler silinecekTur = db.Turlers.Find(turId);
        if (silinecekTur != null)
        {
            db.Turlers.Remove(silinecekTur);
            await db.SaveChangesAsync();
        }

        return Redirect("/Admin/Turler");
    }

    [Route("/Admin/User")]
    public IActionResult User()
    {
        List<UserVM> Users = (from x in db.Users
                              select new UserVM
                              {
                                  Id = x.Id,
                                  username = x.Username,
                                  password = x.Password,
                              }).ToList();
        return View(Users);
    }

    [HttpGet]
    [Route("/Admin/UserForm/{userId?}")]
    public IActionResult UserForm(int? userId)
    {
        if (userId != null)
        {
            UserVM duzenlenecekUser = (from x in db.Users
                                       where x.Id == userId
                                       select new UserVM
                                       {
                                           Id = x.Id,
                                           username = x.Username,
                                           password = x.Password,
                                       }).FirstOrDefault();

            ViewBag.Pagetitle = "User Düzenle";
            return View(duzenlenecekUser);
        }
        else if (userId == null)
        {
            ViewBag.Pagetitle = "User Ekle";
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserForm(UserVM gelenData)
    {
                if (!ModelState.IsValid)
        {
            return View(gelenData);
        }
        if (gelenData.Id != 0)
        {
            User duzenlenecekUser = db.Users.Find(gelenData.Id);
            if (duzenlenecekUser != null)
            {
                duzenlenecekUser.Username = gelenData.username;
                duzenlenecekUser.Password = gelenData.password;
            }
        }
        else if (gelenData.Id == 0)
        {
            User yeniUser = new User
            {
                Username = gelenData.username,
                Password = gelenData.password,
            };

            await db.AddAsync(yeniUser);
        }
        await db.SaveChangesAsync();

        return Redirect("/Admin/User");
    }

    /*[Route("/Admin/UserSil/{userId?}")]
    [HttpGet]
    public IActionResult UserSil(int? userId)
    {
        UserVM tur = new UserVM();
        if (userId != null)
        {
            tur = (from x in db.Users
                   where x.Id == userId
                   select new UserVM
                   {
                       Id = x.Id,
                       username = x.Username,
                       password = x.Password
                   }
            ).FirstOrDefault();
        }
        return View(tur);
    }


    [HttpPost]
    public async Task<IActionResult> UserSil(UserVM gelenData)
    {
        //TurlerVM silinecekTur = (from x in db.Turlers
        //where x.TurAdi == gelenData.TurAdi
        //where x.Sira == gelenData.Sira
        //                    select new TurlerVM
        //                    {
        //                        Id = x.Id,
        //                        TurAdi = x.TurAdi,
        //                        Sira = x.Sira
        //                    }).FirstOrDefault();

        User silinecekUser = new User
        {
            Id = gelenData.Id,
            Username = gelenData.username,
            Password = gelenData.password,
        };

        db.Remove(silinecekUser);
        await db.SaveChangesAsync();

        return Redirect("/Admin/User");
    }*/

        [Route("/Admin/UserSil/{userId?}")]
    public async Task<IActionResult> UserSil(int userId)
    {
        User silinecekUser = db.Users.Find(userId);
        if (silinecekUser != null)
        {
            db.Users.Remove(silinecekUser);
            await db.SaveChangesAsync();
        }

        return Redirect("/Admin/User");
    }
}