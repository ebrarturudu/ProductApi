using FluentValidation;
using ProductApi.Application.Features.Products.Commands;

namespace ProductApi.Application.Validator
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            // Kuralları ProductDto üzerinden tanımlıyoruz
            RuleFor(x => x.ProductDto.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.")
                .MinimumLength(3).WithMessage("Ürün adı en az 3 karakter olmalıdır.");

            RuleFor(x => x.ProductDto.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

            RuleFor(x => x.ProductDto.Stock)
                .InclusiveBetween(0, 9999).WithMessage("Stok miktarı 0 ile 9999 arasında olmalıdır.");
        }
    }
}