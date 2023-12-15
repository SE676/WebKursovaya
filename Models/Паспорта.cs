using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebKursovaya.Models
{
    public class Паспорта
    {
        [Key]
        public int Код_паспорта { get; set; }

        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }

        [Display(Name = "Cерия паспорта")]
        //[MaxLength(4, ErrorMessage = "Поле 'Серия паспорта' должно содержать не более 4 символов")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Поле 'Серия паспорта' должно содержать только цифры")]
        [Required(ErrorMessage = "Поле 'Серия паспорта' обязательно для заполнения")]
        
        //public int Серия_паспорта { get; set; }

        public string Серия_паспорта { get; set; }
        [Display(Name = "Номер паспорта")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Поле 'Номер паспорта' должно содержать только цифры")]
        //[MaxLength(6, ErrorMessage = "Поле 'Номер паспорта' должно содержать не более 6 символов")]
        [Required(ErrorMessage = "Поле 'Номер паспорта' обязательно для заполнения")]
        //public int Номер_паспорта { get; set; }

        public string Номер_паспорта { get; set; }
        [Display(Name = "Дата выдачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата выдачи' обязательно для заполнения")]
        public DateTime Дата_выдачи {  get; set; }

        [Display(Name = "Кем выдан")]
        [Required(ErrorMessage = "Поле 'Кем выдан' обязательно для заполнения")]
        public string Кем_выдан { get; set; }

        [Display(Name = "Адрес прописки")]
        [Required(ErrorMessage = "Поле 'Адрес прописки' обязательно для заполнения")]
        public string Адрес_прописки {  get; set; }
        
        public Заявители? Заявитель { get; set; }

        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";

    }
}
