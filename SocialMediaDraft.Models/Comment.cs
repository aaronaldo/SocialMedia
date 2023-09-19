namespace SocialMediaDraft.Models
{
    public class CommentModel
    {
        public ulong Id { get; set; }
        public ulong AuthorId { get; set; }
        public string Text { get; set; }
        public ulong Likes { get; set; }
    }
}