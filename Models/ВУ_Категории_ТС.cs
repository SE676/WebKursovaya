using Azure;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKursovaya.Models
{
    public class ВУ_Категории_ТС
    {
        [Key]
        public int Код {  get; set; }

        [ForeignKey("ВУ")]
        public int Код_ВУ { get; set; }

        [ForeignKey("Категория")]
        [Display(Name = "Категория")]
        public int Код_Категории_ТС { get; set; }

        public ВУ? ВУ { get; set; }
        public Категории_ТС? Категория { get; set; }


        [NotMapped]
        public string ВУсн => $"{ВУ?.Серия_ВУ} {ВУ?.Номер_ВУ}";

    }
}
