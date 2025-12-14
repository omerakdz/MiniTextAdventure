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
                PasswordHash = ComputeSha256("admin123"), 
                Role = "Admin"
            });
        }
        private static string ComputeSha256(string input)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
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
