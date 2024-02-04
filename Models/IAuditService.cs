namespace WebKursovaya.Models
{
    public interface IAuditService
    {
        void LogAction(string userId, string userRole, string action, string tableName);

    }
}
