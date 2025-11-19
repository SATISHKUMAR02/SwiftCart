using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators
{
    public class OrderAddRequestValidator : AbstractValidator<OrderAddRequest>
    {
        public OrderAddRequestValidator()
        {
            RuleFor(temp=>temp.UserID).NotEmpty().WithMessage("User Id Cannot be empty");
            RuleFor(temp => temp.OrderDate).NotEmpty().WithMessage("Order date cannot be empty");
            RuleFor(temp => temp.OrderItems).NotEmpty().WithMessage("There should be atleast one product");

        }
    }
}
