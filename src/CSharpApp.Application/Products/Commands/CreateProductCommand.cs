﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Commands
{
    public record CreateProductCommand(CreateProduct CreateProductDto): IRequest<Result<Product>>;
}
