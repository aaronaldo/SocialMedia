using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SocialMediaDraft.Commom;
using SocialMediaDraft.Post.Infrastructure.Config;
using SocialMediaDraft.Post.Models;
using System.Data;

namespace SocialMediaDraft.Post.Infrastructure.Post
{
    internal class PostRepository : IPostRepository
    {
        public string _connectionString { get; set; }
        private readonly DbConfig _config;
        public PostRepository(IOptions<DbConfig> config)
        {
            _connectionString = config.Value.ConnectionString;
            _config = config.Value;
        }

        private const string GET_POST = @"
            SELECT
                p.Id,
                p.AuthorId,
                p.Title,
                p.Text
            FROM
                [dbo].Post p
        ";

        private const string GET_POST_LIKE_COUNT = @"
            SELECT
                count(*) count
            FROM
                [dbo].PostLike pl
            WHERE
                pl.PostId IN @ids
            ORDER BY
                pl.PostId
        ";

        private async Task<IList<PostModel>> InternalGetPosts(ModelKey<PostModel> key, string WHERE_CLAUSE)
        {
            using var conn = new SqlConnection(_connectionString);
            string sql = GET_POST + WHERE_CLAUSE;

            conn.Open();

            var posts = (await conn.QueryAsync<PostModel>(sql, key, commandTimeout: _config.CommandTimeout, commandType: CommandType.Text)).ToList();

            if (posts is null || posts.Count == 0)
            {
                return posts;
            }

            var postLikes = await conn.QueryAsync<PostLikeModel>(GET_POST_LIKE_COUNT, new { ids = posts.Select(p => p.Id) });
            AssignPostsAndLikes(posts, postLikes.ToList());

            return posts;
        }

        public async Task<PostModel> GetPost(PostPK pk)
        {
            const string WHERE_CLAUSE = " WHERE p.Id = @Id";

            var posts = await InternalGetPosts(pk, WHERE_CLAUSE);
            if (posts.Count > 1)
            {
                throw new Exception("Post PK must return only one post.");
            }

            return posts.First();
        }

        public async Task<IList<PostModel>> GetPostsByAuthor(AuthorKey key)
        {
            const string WHERE_CLAUSE = " WHERE p.AuthorId = @AuthorId";

            return await InternalGetPosts(key, WHERE_CLAUSE);
        }

        private void AssignPostsAndLikes(IList<PostModel> posts, IList<PostLikeModel> postLikes)
        {
            var currentPost = posts.First();
            ulong currentPostId = posts.First().Id;

            for (int i = 0; i < postLikes.Count; i++)
            {
                if (postLikes[i].PostId != currentPost.Id)
                {
                    currentPost = posts.FirstOrDefault(p => p.Id == postLikes[i].PostId);
                }

                currentPost.Likes++;
            }
        }
    }
}
