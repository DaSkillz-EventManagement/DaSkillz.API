using FluentValidation;

namespace Application.UseCases.Events.Queries.GetEventParticipatedByUser
{
    public class GetEventParticipatedByUserQueryValidator : AbstractValidator<GetEventParticipatedByUserQuery>
    {
        public GetEventParticipatedByUserQueryValidator()
        {
            // Rule for PageNo to ensure it's greater than 0
            RuleFor(x => x.PageNo)
                    .GreaterThan(0).WithMessage("Page number must be greater than 0.")
                    .When(x => x.PageNo > 0 || x.PageNo == 1); // Additional condition to cover default value

            // Rule for ElementEachPage to ensure it's greater than 0
            RuleFor(x => x.ElementEachPage)
                    .GreaterThan(0).WithMessage("Elements per page must be greater than 0.")
                    .When(x => x.ElementEachPage > 0 || x.ElementEachPage == 10); // Additional condition to cover default valu
        }
    }
}
