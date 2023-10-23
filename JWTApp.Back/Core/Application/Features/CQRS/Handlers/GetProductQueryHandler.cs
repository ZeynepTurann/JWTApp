using AutoMapper;
using JWTApp.Back.Core.Application.Dto;
using JWTApp.Back.Core.Application.Features.CQRS.Queries;
using JWTApp.Back.Core.Application.Interfaces;
using JWTApp.Back.Core.Domain;
using MediatR;

namespace JWTApp.Back.Core.Application.Features.CQRS.Handlers
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQueryRequest, ProductListDto>
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async  Task<ProductListDto> Handle(GetProductQueryRequest request, CancellationToken cancellationToken)
        {
          var product =  await _repository.GetByFilterAsync(x => x.Id == request.Id);
          return _mapper.Map<ProductListDto>(product);
        }
    }
}
