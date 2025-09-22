using FluentValidation;

namespace ESourcing.Order.Application.Features.Orders.Commands.Create;

public class OrderCreateCommandValidator : AbstractValidator<OrderCreateCommand>
{
    public OrderCreateCommandValidator()
    {
        RuleFor(v => v.SellerUserName)
            .EmailAddress()
            .NotEmpty();

        RuleFor(v => v.ProductId)
            .NotEmpty();
    }
}