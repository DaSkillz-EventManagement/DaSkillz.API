using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Tags.Commands.AddTag
{
    public class AddTagCommand : IRequest<APIResponse>
    {
        public string TagName { get; set; } = null!;

        public AddTagCommand(string tagName)
        {
            TagName = tagName;
        }
    }
}
