using FluentValidation;

namespace ChatAssistant.Application.Commands.CreateChatSession
{
    public class CreateChatSessionCommandValidator : AbstractValidator<CreateChatSessionCommand>
    {
        public CreateChatSessionCommandValidator()
        {
            RuleFor(x => x.RequestorConnectionId).NotEmpty().WithMessage("Requestor connection ID cannot be empty"); 
        }
    }
}
