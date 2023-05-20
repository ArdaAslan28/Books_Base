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
    public IActionResult TurEkle()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TurEkle(TurlerVM gelenData)
    {
        Turler yeniTur = new Turler
        {
            TurAdi = gelenData.TurAdi,
            Sira = gelenData.Sira,
        };

            await db.AddAsync(yeniTur);
            await db.SaveChangesAsync();

        return Redirect("/Admin/Turler");
    }

    [Route("/Admin/TurSil/{turId?}")]
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
        /*TurlerVM silinecekTur = (from x in db.Turlers
        where x.TurAdi == gelenData.TurAdi
        where x.Sira == gelenData.Sira
                            select new TurlerVM
                            {
                                Id = x.Id,
                                TurAdi = x.TurAdi,
                                Sira = x.Sira
                            }).FirstOrDefault();*/

        Turler silinecekTur = new Turler
        {
            Id = gelenData.Id,
            TurAdi = gelenData.TurAdi,
            Sira = gelenData.Sira,
        };

            db.Remove(silinecekTur);
            db.SaveChangesAsync();

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
    public IActionResult UserEkle()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UserEkle(UserVM gelenData)
    {
        User yeniUser = new User
        {
            Username = gelenData.username,
            Password = gelenData.password,
        };

            await db.AddAsync(yeniUser);
            await db.SaveChangesAsync();

        return Redirect("/Admin/User");
    }

    [Route("/Admin/UserSil/{userId?}")]
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
        public  IActionResult UserSil(UserVM gelenData)
    {
        /*TurlerVM silinecekTur = (from x in db.Turlers
        where x.TurAdi == gelenData.TurAdi
        where x.Sira == gelenData.Sira
                            select new TurlerVM
                            {
                                Id = x.Id,
                                TurAdi = x.TurAdi,
                                Sira = x.Sira
                            }).FirstOrDefault();*/

        User silinecekUser = new User
        {
            Id = gelenData.Id,
            Username = gelenData.username,
            Password = gelenData.password,
        };

            db.Remove(silinecekUser);
            db.SaveChangesAsync();

        return Redirect("/Admin/User");
    }
}