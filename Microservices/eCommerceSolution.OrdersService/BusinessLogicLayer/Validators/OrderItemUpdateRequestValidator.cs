using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators
{
    public class OrderItemUpdateRequestValidator : AbstractValidator<OrderItemUpdateRequest>
    {
        public OrderItemUpdateRequestValidator()
        {
            RuleFor(temp=>temp.ProductID).NotEmpty();
            RuleFor(temp=>temp.UnitPrice).NotEmpty();
            RuleFor(temp=>temp.Quantity).NotEmpty();
            

            
        }
    }
}
