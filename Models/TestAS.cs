using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebKursovaya.Models
{
    public class TestAS
    {
        [Key]
        public int Код_внутреннего_экзамена { get; set; }

        [Display(Name = "Название автошколы")]
        [Required(ErrorMessage = "Поле 'Название автошколы' обязательно для заполнения")]
        public string Название_автошколы {  get; set; }
        public string Фамилия { get; set; }
      
        public string Имя { get; set; }
        
        public string Отчество { get; set; }
        [Display(Name = "Дата сдачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Дата_сдачи {get; set; }

        [Display(Name = "Результат сдачи")]
        public string Результат_сдачи { get; set; }

        
    }
}
