using books.Models.Entities;

namespace books.Models.ViewModels; 

public class KitapDetayVM
{
    public string KitapAdi { get; set; }
    public string Dil { get; set; }
    public string Resim { get; set; }
    public int SayfaSayisi { get; set; }
    public string YayinTarihi { get; set; }
    public string Ozet { get; set; }
    public KitapYazar Yazar { get; set; }
    public string YayineviAdi { get; set; }

    public List<Turler> KitapTurleri { get; set; }
}

public class KitapYazar
{
    public int Id { get; set; }
    public string YazarAdiSoyadi { get; set; }
}