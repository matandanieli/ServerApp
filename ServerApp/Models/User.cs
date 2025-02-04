namespace ServerApp.Models
{
    public class User
    {
        public int ID { get; set; }  //primary key of the table
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
