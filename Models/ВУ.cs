using Azure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKursovaya.Models
{
    public class ВУ
    {
        [Key]
        public int Код_ВУ { get; set; }


        [ForeignKey("Виды_статусов_ВУ")]
        public int Код_статуса_ВУ { get; set; }


        [ForeignKey("Заявитель")]
        public int Код_заявителя { get; set; }

        [Display(Name = "Серия ВУ")]
        //[MaxLength(4, ErrorMessage = "Поле 'Серия ВУ' должно содержать не более 4 символов")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Поле 'Серия ВУ' должно содержать только цифры")]
        [Required(ErrorMessage = "Поле 'Серия ВУ' обязательно для заполнения")]
        //public int Серия_ВУ { get; set; }
        public string Серия_ВУ { get; set; }

        [Display(Name = "Номер ВУ")]
        //[MaxLength(6, ErrorMessage = "Поле 'Номер ВУ' должно содержать не более 6 символов")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Поле 'Номер ВУ' должно содержать только цифры")]
        [Required(ErrorMessage = "Поле 'Номер ВУ' обязательно для заполнения")]
        //public int Номер_ВУ { get; set; }

        public string Номер_ВУ { get; set; }

        [Display(Name = "Дата выдачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата выдачи' обязательно для заполнения")]
        public DateTime Дата_выдачи { get; set; }

        [Display(Name = "Дата окончания срока действия")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле 'Дата окончания срока действия' обязательно для заполнения")]
        public DateTime Дата_окончания_срока_действия { get; set; }


        public Заявители? Заявитель { get; set; }

        [Display(Name = "Статус ВУ")]
        public Виды_статусов_ВУ? Виды_статусов_ВУ { get; set; }

        [NotMapped]
        public string ВУсн => $"{Серия_ВУ} {Номер_ВУ}";


        [NotMapped]
        public string ЗаявительФИО => $"{Заявитель?.Фамилия} {Заявитель?.Имя} {Заявитель?.Отчество}";

    }
}
