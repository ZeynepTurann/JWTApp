using JWTApp.Back.Core.Application.Features.CQRS.Commands;
using JWTApp.Back.Core.Application.Interfaces;
using JWTApp.Back.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JWTApp.Back.Core.Application.Features.CQRS.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest>
    {
        private readonly IRepository<Product> _repository;

        public UpdateProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetByIdAsync(request.Id);
            if (data != null)
            {
                data.Name = request.Name;
                data.Price = request.Price;
                data.Stock = request.Stock;
                data.CategoryId = request.CategoryId;
                await   _repository.UpdateAsync(data);
            }
            return Unit.Value;
        }
    }
}
