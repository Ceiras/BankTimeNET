namespace BankTimeNET.models
{
    public class User
    {
        public User(string dni, string name, string? password, int amount, bool active)
        {
            Dni = dni;
            Name = name;
            Password = password;
            Amount = amount;
            Active = active;
        }

        public int Id { get; set; }
        public string Dni { get; set; }
        public string Name { get; set; }
        public string? Password { get; set; }
        public int Amount { get; set; }
        public bool Active { get; set; }
        public virtual Bank? Bank { get; set; }
    }
}
