using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class Стоимость_госпошлины
    {
        [Key]
        public int Код_стоимости { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата' обязательно для заполнения")]
        public DateTime Дата { get; set; }
        
        [Required(ErrorMessage = "Поле 'Стоимость' обязательно для заполнения")]
        public string Стоимость { get; set; }
    }
}
