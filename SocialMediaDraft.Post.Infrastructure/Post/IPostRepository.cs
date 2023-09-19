using SocialMediaDraft.Post.Models;

namespace SocialMediaDraft.Post.Infrastructure.Post
{
    public interface IPostRepository
    {
        Task<PostModel> GetPost(PostPK pk);
        Task<IList<PostModel>> GetPostsByAuthor(AuthorKey key);
    }
}
