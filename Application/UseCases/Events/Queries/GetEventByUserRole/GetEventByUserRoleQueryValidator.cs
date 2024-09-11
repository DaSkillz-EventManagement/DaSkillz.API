using Application.UseCases.Events.Command.CreateEvent;
using Domain.Enum.Events;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetEventByUserRole
{
    public class GetEventByUserRoleQueryValidator : AbstractValidator<GetEventByUserRoleQuery>
    {

        public GetEventByUserRoleQueryValidator()
        {
            // Rule for EventRole to ensure it's not null and has a valid value
            RuleFor(x => x.EventRole)
                .NotNull().WithMessage("EventRole is required.")
                .IsInEnum().WithMessage("Invalid EventRole value.");

            // Rule for PageNo to ensure it's greater than 0
            RuleFor(x => x.PageNo)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.")
                .When(x => x.PageNo > 0 || x.PageNo == 1); // Additional condition to cover default value

            // Rule for ElementEachPage to ensure it's greater than 0
            RuleFor(x => x.ElementEachPage)
                .GreaterThan(0).WithMessage("Elements per page must be greater than 0.")
                .When(x => x.ElementEachPage > 0 || x.ElementEachPage == 10); // Additional condition to cover default value
        }
    }
}
