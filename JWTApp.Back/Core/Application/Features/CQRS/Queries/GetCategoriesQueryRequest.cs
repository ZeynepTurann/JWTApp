using JWTApp.Back.Core.Application.Dto;
using MediatR;

namespace JWTApp.Back.Core.Application.Features.CQRS.Queries
{
    public class GetCategoriesQueryRequest:IRequest<List<CategoryListDto>>
    {
    }
}
