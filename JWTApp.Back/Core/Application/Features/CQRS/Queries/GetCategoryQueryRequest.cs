using JWTApp.Back.Core.Application.Dto;
using MediatR;

namespace JWTApp.Back.Core.Application.Features.CQRS.Queries
{
    public class GetCategoryQueryRequest:IRequest<CategoryListDto?>
    {
        public GetCategoryQueryRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

    }
}
