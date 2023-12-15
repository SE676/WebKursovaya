using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models

{
    public class Полы
    {
        [Key]
        public int Код_пола {  get; set; }
        [Required(ErrorMessage = "Поле 'Пол' обязательно для заполнения")]
        public string Пол { get; set; }
    }
}
