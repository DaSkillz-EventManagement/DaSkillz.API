using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Tags.Commands.DeleteTag
{
    public class DeleteTagCommand : IRequest<APIResponse>
    {
        public int tagId { get; set; }

        public DeleteTagCommand(int tagId)
        {
            this.tagId = tagId;
        }
    }
}
