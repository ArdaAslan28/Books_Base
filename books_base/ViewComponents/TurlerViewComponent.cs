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
    public class TurlerViewComponent : ViewComponent
    {
        private readonly KitapDbContext db = new KitapDbContext();

        public TurlerViewComponent(KitapDbContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var turler = await (from x in db.Turlers
                                
                                select new TurListVM
                                {
                                    Id = x.Id,
                                    Tur = x.TurAdi,
                                    kitapSayisi = (from k in db.Turlertokitaplars
                                                    where k.TurId == x.Id
                                                    select x).Count(),
                                }).ToListAsync();

            return View(turler);
        }
    }
}