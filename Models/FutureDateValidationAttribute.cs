using System.ComponentModel.DataAnnotations;

namespace WebKursovaya.Models
{
    public class FutureDateTimeValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                // Проверка на сегодня и позже
                if (dateTime.Date < DateTime.Today)
                    return false;

                // Проверка времени от 8:00 до 18:00
                TimeSpan startTime = new TimeSpan(8, 0, 0);
                TimeSpan endTime = new TimeSpan(18, 0, 0);

                TimeSpan visitTime = dateTime.TimeOfDay;

                // Проверка интервала времени (15 минут)
                if (visitTime < startTime || visitTime > endTime || visitTime.Minutes % 15 != 0)
                    return false;

                return true;
            }

            return false;
        }
    }
}
