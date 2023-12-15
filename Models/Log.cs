using System.ComponentModel.DataAnnotations;
namespace WebKursovaya.Models
{
    public class Log
    {
        [Key]
        public int ID_записи { get; set; }
        public string Имя_пользователя { get; set; }

        public string Действие { get; set; }

        public string Таблица { get; set; }

        public DateTime Дата { get; set; }

        public string? Роль { get; set; }
    }
}

