namespace DataAccessLayer.Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public byte[]? Avatar { get; set; }
        public string TotalTime { get; set; } = "0.00:00:00";

        public ICollection<History> Histories { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
