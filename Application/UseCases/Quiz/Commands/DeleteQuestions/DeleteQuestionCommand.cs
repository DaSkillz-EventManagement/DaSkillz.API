using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quizs.Commands.DeleteQuestions;

public class DeleteQuestionCommand: IRequest<APIResponse>
{
    public List<Guid> QuestionID { get; set; } = new List<Guid>();
    public DeleteQuestionCommand(List<Guid> questionID)
    {
        QuestionID = questionID;
    }
}
