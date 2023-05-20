using books.Models.Entities;

namespace books.Models.ViewModels; 

class YazanInfoVM
{
    public string YazarAdi { get; set; }

    public string YazarSoyadi { get; set; }

    public string yazarDogumTarihi { get; set; }
    public string yazarDogumYeri { get; set; }

    public string Cinsiyet { get; set; }

    public int kitapSayisi { get; set; }

}