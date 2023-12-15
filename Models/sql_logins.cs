namespace WebKursovaya.Models
{
    public class sql_logins
    {
        public string name { get; set; }
        //public int principal_id { get; set; }
        //public int sid { get; set; }
        //public string type { get; set; }
        //public string type_desc { get; set; }
        //public byte[] is_disabled { get; set; }
        //public DateTime create_date { get; set; }
        //public DateTime modify_date { get; set; }
        public string password_hash { get; set; }

    }
}
