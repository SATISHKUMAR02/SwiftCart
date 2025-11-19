using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators
{
    public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
    {
        public OrderUpdateRequestValidator()
        {
            RuleFor(temp=>temp.OrderID).NotEmpty();
            RuleFor(temp=>temp.UserID).NotEmpty();
            RuleFor(temp=>temp.OrderItems).NotEmpty();
            RuleFor(temp=>temp.OrderDate).NotEmpty();
        }
    }
}
