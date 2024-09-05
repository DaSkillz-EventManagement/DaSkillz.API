using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.Events.Command
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(x => x.EventName)
            .NotEmpty().WithMessage("EventName is required!")
            .Length(3, 250).WithMessage("Event name must be between 3 and 250 characters!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Event Description is required!")
                .Length(3, 5000).WithMessage("Event Description must be between 3 and 5000 characters!");

            RuleFor(x => x.TagId)
                .NotNull().WithMessage("Tags are required!")
                .Must(t => t.Count > 0).WithMessage("At least one tag is required!");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.Now.Ticks).WithMessage("StartDate must be in the future!");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate!");

            RuleFor(x => x.Location)
                .NotNull().WithMessage("Location is required!");

            RuleFor(x => x.Capacity)
                .NotNull().WithMessage("Capacity is required!")
                .GreaterThan(0).WithMessage("Capacity must be greater than zero!");

            RuleFor(x => x.Ticket)
                .InclusiveBetween(0, 5000000).WithMessage("Maximum ticket price is 5 000 000!");
        }
    }
}
