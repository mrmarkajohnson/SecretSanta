using FluentValidation;
using Xunit;

namespace UnitTests.ValidationTests;

public sealed class TestNotNullOrEmpty
{
    public TestNotNullOrEmpty()
    {
    }

    [Fact]
    public void ZeroInteger()
    {
        var testClass = new TestClass();
        var validator = new TestValidator();
        var result = validator.Validate(testClass);

        Assert.False(result.IsValid); // TODO: Make this more accurate
    }

    public sealed class TestClass
    {
        public int IntVal { get; set; }
        public int? NullIntVal { get; set; }
        public int? ZeroNullIntVal { get; set; } = 0;
        public string StringVal { get; set; } = string.Empty;
        public string? NullStringVal { get; set; }
        public string EmptyNullStringVal { get; set; } = string.Empty;
        // TODO: Add decimals, dates etc.
    }

    public sealed class TestValidator : AbstractValidator<TestClass>
    {
        public TestValidator()
        {
            RuleFor(x => x.IntVal).NotNullOrEmpty();
            RuleFor(x => x.NullIntVal).NotNullOrEmpty();
            RuleFor(x => x.ZeroNullIntVal).NotNullOrEmpty();
            RuleFor(x => x.StringVal).NotNullOrEmpty();
            RuleFor(x => x.NullStringVal).NotNullOrEmpty();
            RuleFor(x => x.EmptyNullStringVal).NotNullOrEmpty();
        }
    }
}
