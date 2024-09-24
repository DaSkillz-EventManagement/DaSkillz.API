using MediatR;

namespace Application.UseCases.Payment.Commands.Callback
{
    public class CallbackCommand : IRequest<object>
    {
        public string? data { get; set; }
        public string? mac { get; set; }

        public CallbackCommand(string? data, string? mac)
        {
            this.data = data;
            this.mac = mac;
        }
    }
}
