using MiniApiTextAdv.Models;


namespace MiniApiTextAdv.Services

{
    public class UserRepository
    {
        private readonly List<User> _users = new List<User>();

        public UserRepository()
        {           
            //ik heb deze user aangemaakt om te testen
            _users.Add(new User
            {
                Username = "admin",
                PasswordHash = "", 
                Role = "Admin"
            });
        }

        public User? GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public bool Exists(string username)
        {
            return _users.Any(u => u.Username == username);
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public void Update(User user)
        {
            
        }
    }
}
