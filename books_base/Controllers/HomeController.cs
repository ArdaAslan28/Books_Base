using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using books.Models;
using books.Models.Entities;
using books.Models.ViewModels;

namespace books.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly KitapDbContext db = new KitapDbContext();

    public HomeController(ILogger<HomeController> logger, KitapDbContext _db)
    {
        _logger = logger;
        db = _db;
    }

    public IActionResult Index()
    {
        //var kitaps = (from x in db.Kitaplars
        //select x).ToList();

        List<IndexVM> kitaplar = (from x in db.Kitaplars
                                  orderby x.YayinTarihi descending
                                  select new IndexVM
                                  {
                                      Id = x.Id,
                                      KitapAdi = x.Adi,
                                      Resim = x.Resim,
                                      YayinTarihi = x.YayinTarihi.ToShortDateString()
                                  }).Take(12).ToList();


        return View(kitaplar);
    }

    [Route("/Kitaplar")]
    [Route("/Kitaplar/Tur/{turId?}")]
    public IActionResult Kitaplar(int? yazarId, int? turId)
    {
        List<IndexVM> kitaplar = new List<IndexVM>();
        if (yazarId == null && turId == null)
        {
            kitaplar = (from x in db.Kitaplars
                        orderby x.YayinTarihi descending
                        select new IndexVM
                        {
                            Id = x.Id,
                            KitapAdi = x.Adi,
                            Resim = x.Resim,
                            YayinTarihi = x.YayinTarihi.ToShortDateString()
                        }).ToList();

            ViewBag.Pagetile = String.Format(("Tüm kitaplar ({0})"), kitaplar.Count());
        }
        else if (turId != null)
        {
            kitaplar = (from x in db.Turlertokitaplars
                        join k in db.Kitaplars on x.KitapId equals k.Id
                        where x.TurId == turId
                        select new IndexVM
                        {
                            Id = k.Id,
                            KitapAdi = k.Adi,
                            Resim = k.Resim,
                            YayinTarihi = k.YayinTarihi.ToShortDateString()
                        }).ToList();
            var tur = db.Turlers.Find(turId);
            var turAdi = tur.TurAdi;
            ViewBag.Pagetile = String.Format(("{0} türündeki kitaplar ({1})"), turAdi, kitaplar.Count());
        }
        return View(kitaplar);
    }

    [Route("/Kitaplar/Yazar/{yazarId?}")]

    public IActionResult YazarKitaplar(int? yazarId)
    {

        List<IndexVM> kitaplar = new List<IndexVM>();
        if (yazarId != null)
        {
            kitaplar = (from x in db.Kitaplars
                        orderby x.YayinTarihi descending
                        where x.YazarId == yazarId
                        select new IndexVM
                        {
                            Id = x.Id,
                            KitapAdi = x.Adi,
                            Resim = x.Resim,
                            YayinTarihi = x.YayinTarihi.ToShortDateString()
                        }).ToList();


            ViewBag.yazar = (from x in db.Yazarlars
            where x.Id == yazarId
            select new YazanInfoVM
            {
                YazarAdi = x.Adi,
                YazarSoyadi = x.Soyadi,
                yazarDogumTarihi = x.DogumTarihi.ToShortDateString(),
                yazarDogumYeri = x.DogumYeri,
                Cinsiyet = x.Cinsiyeti == true ? "Erkek" : "Kadın",
                kitapSayisi = kitaplar.Count()
            }
            ).FirstOrDefault();
            
            /*var yazar = db.Yazarlars.Find(yazarId);
            var yazarAdi = yazar.Adi + " " + yazar.Soyadi;
            var yazarDogumTarihi = yazar.DogumTarihi;
            var yazarCinsiyet = yazar.Cinsiyeti == true ? "Erkek" : "Kadın";
            var yazarDogumYeri = yazar.DogumYeri;
           // ViewBag.Pagetile = String.Format(("{0}'ın kitapları ({1})"), yazarAdi, kitaplar.Count());
            ViewBag.PageBookCount = kitaplar.Count().ToString();
            ViewBag.yazarAdi = yazarAdi;
            ViewBag.yazarDogumTarihi = yazarDogumTarihi;
            ViewBag.yazarCinsiyet = yazarCinsiyet;
            ViewBag.yazarDogumYeri = yazarDogumYeri;*/

        }
        return View(kitaplar);
    }

    [Route("/Kitap/{id}")]
    public IActionResult KitapDetay(int id)
    {
        KitapDetayVM aktifKitap = (from x in db.Kitaplars
                                   where x.Id == id
                                   join y in db.Yazarlars on x.YazarId equals y.Id
                                   join d in db.Dillers on x.DilId equals d.Id
                                   join ye in db.Yayinevleris on x.YayineviId equals ye.Id
                                   select new KitapDetayVM
                                   {
                                       KitapAdi = x.Adi,
                                       Dil = d.DilAdi,
                                       Ozet = x.Ozet,
                                       Resim = x.Resim,
                                       SayfaSayisi = x.SayfaSayisi,
                                       YayinTarihi = x.YayinTarihi.ToShortDateString(),
                                       Yazar = new KitapYazar
                                       {
                                           Id = y.Id,
                                           YazarAdiSoyadi = y.Adi + " " + y.Soyadi,
                                       },
                                       YayineviAdi = ye.YayineviAdi,
                                       KitapTurleri = (from t in db.Turlertokitaplars
                                                       join m in db.Turlers on t.TurId equals m.Id
                                                       where t.KitapId == x.Id
                                                       select new Turler
                                                       {
                                                           Id = t.TurId,
                                                           TurAdi = m.TurAdi,
                                                           Sira = m.Sira
                                                       }).ToList()
                                   }).FirstOrDefault();

        return View(aktifKitap);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
