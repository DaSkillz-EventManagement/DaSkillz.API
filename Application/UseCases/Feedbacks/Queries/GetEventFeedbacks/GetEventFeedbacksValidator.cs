using FluentValidation;

namespace Application.UseCases.Feedbacks.Queries.GetEventFeedbacks;

public class GetEventFeedbacksValidator : AbstractValidator<GetEventFeedbacksQueries>
{
    public GetEventFeedbacksValidator()
    {
        // Rule for PageNo to ensure it's greater than 0
        RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.")
                .When(x => x.Page > 0 || x.Page == 1); // Additional condition to cover default value

        // Rule for ElementEachPage to ensure it's greater than 0
        RuleFor(x => x.EachPage)
                .GreaterThan(0).WithMessage("Elements per page must be greater than 0.")
                .When(x => x.EachPage > 0 || x.EachPage == 10); // Additional condition to cover default value
    }
}
