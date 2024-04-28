using FluentValidation;

namespace DataSanitization.model
{
    public class TestFluent
    {
        public string Id { get; set; }
    }

    public class TestFluentValidator : AbstractValidator<TestFluent>
    {
        public TestFluentValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Requires a product order ID")
                .MaximumLength(100)
                .WithMessage("Requires a product order ID with maximum 100 characters")
                .Matches(@"^[a-zA-Z0-9]+$")
                .WithMessage("Xss character(s) detected");
        }
    }
}
