using JWTApp.Back.Core.Application.Dto;
using MediatR;

namespace JWTApp.Back.Core.Application.Features.CQRS.Queries
{
    public class CheckUserQueryRequest:IRequest<CheckUserResponseDto>
    {
        public string Username { get; set; } = null!;   //default value cannot be null
        public string Password { get; set; } = null!;   // we can write String.Empty 
    }
}
