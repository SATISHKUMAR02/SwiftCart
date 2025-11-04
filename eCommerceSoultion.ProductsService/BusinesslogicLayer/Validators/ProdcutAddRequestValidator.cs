using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinesslogicLayer.DTO;
using FluentValidation;

namespace BusinesslogicLayer.Validators
{
    public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>

    {
        public ProductAddRequestValidator()
        {
            RuleFor(temp=>temp.ProductName).NotEmpty().WithMessage("Cannot be empty")
                .MinimumLength(5);
            RuleFor(temp => temp.CategoryOptions).IsInEnum().WithMessage("cannot be blank");
            RuleFor(temp => temp.UnitPrice).InclusiveBetween(0, double.MaxValue).WithMessage("inclusive");
            RuleFor(temp => temp.quantity).InclusiveBetween(0, int.MaxValue).WithMessage("inclusive");
            
        }

    }
}
