using ComponentDemo.Models;
using FluentValidation;
using System;

namespace ComponentDemo
{
	public class TestModelValidator : AbstractValidator<TestModel>
	{
		/// <summary>
		/// Validator example.
		/// </summary>
		public TestModelValidator()
		{
			RuleFor(x => x.Date).GreaterThan(DateTime.Today).WithMessage("Test");
			RuleFor(x => x.Date2).NotEmpty().WithMessage("Test 2");
			RuleFor(x => x.MyInt).NotEmpty().WithMessage("Test 3");
			RuleFor(x => x.MyNullableDecimal).Must(x => Math.Round(x.Value, 2) == x).When(x => x.MyNullableDecimal != null).WithMessage("Test 4");
			RuleFor(x => x.MyNullableInt).GreaterThan(5).WithMessage("Test 5");
			RuleFor(x => x.MyDecimal).InclusiveBetween(10, 20).WithMessage("Test 6");
		}
	}
}
