namespace SocialMediaDraft.User.Models
{
    public class User
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
