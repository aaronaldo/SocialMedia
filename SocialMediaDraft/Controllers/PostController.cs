using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaDraft.Post.API.Requests;
using SocialMediaDraft.Post.Infrastructure.Post;
using SocialMediaDraft.Post.Models;

namespace SocialMediaDraft.API.Controllers
{

    [Produces("application/json")]
    [Route("api/post")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("get")]
        public async Task<ActionResult<IList<PostModel>>> GetPostByIds([FromBody] IList<ulong> ids)
        {
            if (ids is null || !ids.Any())
                return BadRequest();

            IList<PostModel> result = new List<PostModel>();

            var tasks = new List<Task<IList<PostModel>>>();

            foreach (var id in ids)
            {
                tasks.Add(_mediator.Send(new PostMediator.Query(new PostPK { Id = id })));
            }

            return Ok(tasks.Select(t => t.Result));
        }

        [HttpPost]
        [Route("createPost")]
        public async Task<ActionResult<IList<PostModel>>> CreatePost([FromBody] IList<PostRequest> inputs)
        {
            if (inputs is null || !inputs.Any())
                return BadRequest();

            IList<PostModel> result = new List<PostModel>();

            var tasks = new List<Task<IList<PostModel>>>();

            foreach (PostRequest input in inputs)
            {
                tasks.Add(_mediator.Send(new PostMediator.Command(PostRequestConverter.ToModel(input))));
            }

            return Ok(tasks.Select(t => t.Result));
        }


    }
}
