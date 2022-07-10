using ComponentDemo.Models;
using FluentValidation;
using System;

namespace ComponentDemo
{
	public class DateModelValidator : AbstractValidator<DateModel>
	{
		/// <summary>
		/// Validator example.
		/// </summary>
		public DateModelValidator()
		{
			RuleFor(x => x.Date).GreaterThan(DateTime.Today).WithMessage("Test");
			RuleFor(x => x.Date2).NotEmpty().WithMessage("Test 2");
		}
	}
}
