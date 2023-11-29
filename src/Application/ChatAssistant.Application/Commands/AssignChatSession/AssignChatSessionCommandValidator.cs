using FluentValidation;

namespace ChatAssistant.Application.Commands.AssignChatSession
{
    internal class AssignChatSessionCommandValidator: AbstractValidator<AssignChatSessionCommand>
    {
        public AssignChatSessionCommandValidator()
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage("The message cannot be empty");
        }
    }
}
