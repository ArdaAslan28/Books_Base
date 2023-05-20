using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using books.Models.Entities;
using books.Models.ViewModels;
using System.Threading.Tasks;

namespace books.ViewComponents
{
    public class YazarlarViewComponent : ViewComponent
    {
        private readonly KitapDbContext db = new KitapDbContext();

        public YazarlarViewComponent(KitapDbContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var yazarlar = await (from x in db.Yazarlars
                                  select new YazarListVM
                                  {
                                      Id = x.Id,
                                      YazarAdi = x.Adi,
                                      YazarSoyadi = x.Soyadi,
                                      kitapSayisi = (from k in db.Kitaplars
                                                     where k.YazarId == x.Id
                                                     select x).Count(),
                                  }).OrderByDescending(s => s.kitapSayisi).Take(10).ToListAsync();

            return View(yazarlar);
        }
    }
}