namespace SocialMediaDraft.Post.Infrastructure.Post
{
    public class PostDTO
    {
        public ulong Id { get; set; }
        public ulong AuthorId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
