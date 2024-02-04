using Microsoft.EntityFrameworkCore;
using WebKursovaya.Models;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Hosting;

namespace WebKursovaya.Data
{
    public class MyDBContext : DbContext
    {
        public DbSet<Документы_о_квалификации> Документы_о_Квалификации { get; set; }
        public DbSet<Посещения> Посещения { get; set; }

        public DbSet<TestAS> TestAS { get; set; }

        public MyDBContext(DbContextOptions<MyDBContext> options)
            : base(options)
        {
        }

        

        public DbSet<WebKursovaya.Models.Полы>? Полы { get; set; }

        public DbSet<WebKursovaya.Models.Заявители>? Заявители { get; set; }

        public DbSet<WebKursovaya.Models.Сотрудники>? Сотрудники { get; set; }

        public DbSet<WebKursovaya.Models.Должности>? Должности { get; set; }

        public DbSet<WebKursovaya.Models.Мед_организации>? Мед_организации { get; set; }

        public DbSet<WebKursovaya.Models.Экзамены>? Экзамены { get; set; }

        public DbSet<WebKursovaya.Models.Виды_статусов_ВУ>? Виды_статусов_ВУ { get; set; }

        public DbSet<WebKursovaya.Models.Виды_статусов_заявлений>? Виды_статусов_заявлений { get; set; }

        public DbSet<WebKursovaya.Models.Заявления>? Заявления { get; set; }

        public DbSet<WebKursovaya.Models.Формы_003_В_у>? Формы_003_В_у { get; set; }

        public DbSet<WebKursovaya.Models.ВУ>? ВУ { get; set; }

        public DbSet<WebKursovaya.Models.Автошколы>? Автошколы { get; set; }

        public DbSet<WebKursovaya.Models.Виды_услуг>? Виды_услуг { get; set; }

        public DbSet<WebKursovaya.Models.Запись_на_практический_экзамен>? Запись_на_практический_экзамен { get; set; }

        public DbSet<WebKursovaya.Models.Категории_ТС>? Категории_ТС { get; set; }

        public DbSet<WebKursovaya.Models.Справки_уплаты_госпошлины>? Справки_уплаты_госпошлины { get; set; }

        public DbSet<WebKursovaya.Models.Стоимость_госпошлины>? Стоимость_госпошлины { get; set; }

        public DbSet<WebKursovaya.Models.Паспорта>? Паспорта { get; set; }

        public DbSet<WebKursovaya.Models.ВУ_Категории_ТС>? ВУ_Категории_ТС { get; set; }

        public DbSet<WebKursovaya.Models.Внутренние_экзамены>? Внутренние_экзамены { get; set; }




    }


}