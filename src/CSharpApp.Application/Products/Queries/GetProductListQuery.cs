using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Queries
{
    public record GetProductListQuery(): IRequest<IReadOnlyCollection<Product>>;
}
