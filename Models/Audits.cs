namespace WebKursovaya.Models
{
    public class Audits
    {
        public DateTime event_time { get; set; }
        public string server_principal_name { get; set;}
        public string database_principal_name { get; set; }
        public string database_name { get; set; }
        public string object_name { get; set; }

        public string statement {  get; set; }

        public string succeeded { get; set; }
    }
}
