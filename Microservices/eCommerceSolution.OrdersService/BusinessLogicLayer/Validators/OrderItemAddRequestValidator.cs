using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators
{
    public class OrderItemAddRequestValidator : AbstractValidator<OrderItemAddRequest>
    {
        public OrderItemAddRequestValidator()
        {
            RuleFor(temp => temp.ProductID).NotEmpty().WithMessage("cannot be empty");
            RuleFor(temp => temp.UnitPrice).NotEmpty().WithMessage("cannot be empty").
                GreaterThan(0).WithMessage("cannot be 0");
            RuleFor(temp => temp.Quantity).NotEmpty().WithMessage("cannot be empty").GreaterThan(0);

        
            
        }
    }
}
