using MediatR;

namespace JWTApp.Back.Core.Application.Features.CQRS.Commands
{
    public class DeleteProductCommandRequest:IRequest
    {
        public DeleteProductCommandRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
