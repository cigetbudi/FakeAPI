using FakeAPI.Domain.Entities;

namespace FakeAPI.Application.Common.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
}