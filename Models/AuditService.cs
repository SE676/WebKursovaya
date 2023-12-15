namespace WebKursovaya.Models
{
    public class AuditService: IAuditService
    {
        private readonly UserContext _dbContext;
        public AuditService(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogAction(string userName, string userRole, string action, string tableName)
        {
            var auditLog = new Log
            {
                Имя_пользователя = userName,
                Действие = action,
                Таблица = tableName,
                Дата = DateTime.Now,
                Роль = userRole
            };

            _dbContext.Logs.Add(auditLog);
            _dbContext.SaveChanges();
        }
    }
}

