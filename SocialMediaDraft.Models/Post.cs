using SocialMediaDraft.Commom;

namespace SocialMediaDraft.Post.Models
{
    public class PostModel
    {
        public ulong Id { get; set; }
        public ulong AuthorId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public ulong Likes { get; set; }
    }

    public class PostLikeModel
    {
        public ulong PostId { get; set; }
        public ulong UserId { get; set; }
    }

    #region Keys
    public class PostPK : ModelKey<PostModel>
    {
        public ulong Id { get; set; }
    }

    public class AuthorKey : ModelKey<PostModel>
    {
        public ulong AuthorId { get; set; }
    } 
    #endregion
}