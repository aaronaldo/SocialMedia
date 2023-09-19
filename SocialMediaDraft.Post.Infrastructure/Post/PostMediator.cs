using MediatR;
using SocialMediaDraft.Commom;
using SocialMediaDraft.Post.Models;

namespace SocialMediaDraft.Post.Infrastructure.Post
{
    public class PostMediator
    {
        public class Query : IRequest<IList<PostModel>>
        {
            public ModelKey<PostModel> Key { get; }
            public Query(ModelKey<PostModel> key)
            {
                Key = key;
            }
        }

        public class QueryHandler : IRequestHandler<Query, IList<PostModel>>
        {
            private readonly IPostRepository _repository;

            public QueryHandler(IPostRepository repository)
            {
                _repository = repository;
            }

            public async Task<IList<PostModel>> Handle(Query request, CancellationToken _)
            {
                IList<PostModel> result;

                if (request.Key is PostPK pk)
                {
                    result = new List<PostModel>() { await _repository.GetPost(pk) };
                }
                else if (request.Key is AuthorKey key)
                {
                    result = await _repository.GetPostsByAuthor(key);
                }
                else
                {
                    throw new Exception($"ModelKey not implemented: {request.Key.GetType().Name}");
                }

                return result;
            }
        }

        public class Command : IRequest<IList<PostModel>>
        {
            public PostModel Post { get; set; }
            public Command(PostModel post)
            {
                Post = post;
            }
        }

        public class CommandHandler : IRequestHandler<Command, IList<PostModel>>
        {
            public Task<IList<PostModel>> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
