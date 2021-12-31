namespace BankTimeNET.models
{
    public class User
    {
        public string Dni { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Amount { get; set; }
        public bool Active { get; set; }

        public override string? ToString()
        {
            return $"USER: {Dni} - {Name}, {Username}, {Password}, {Amount}, {Active}";
        }
    }
}
