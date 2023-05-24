using books.Models.Entities;

namespace books.Models.ViewModels; 

class YazarListVM
{
    public int Id { get; set; }

    public string YazarAdi { get; set; }

    public string YazarSoyadi { get; set; }

    public int kitapSayisi { get; set; }

}