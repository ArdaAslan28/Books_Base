using System.ComponentModel.DataAnnotations;
using books.Models.Entities;

namespace books.Models.AdminViewModels; 

public class UserVM
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Kullanıcı adı boş geçilmez!")]
    [MinLength(3, ErrorMessage = "Kullanıcı adı en az 3 karakter olmalıdır!")]
    public string username { get; set; }
    [Required(ErrorMessage = "Şifre alanı boş geçilmez!")]
    public string password { get; set; }
}