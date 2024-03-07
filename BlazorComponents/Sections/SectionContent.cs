using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorComponents
{
	/// <summary>
	/// See gist.github.com/SteveSandersonMS/4f08efe2ad32178add12bfa3eb6e4559
	/// </summary>
	public class SectionContent : IComponent, IDisposable
	{
		private SectionRegistry _registry;

		[Parameter] public string Name { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }

		public void Attach(RenderHandle renderHandle)
		{
			_registry = SectionRegistry.GetRegistry(renderHandle);
		}

		public Task SetParametersAsync(ParameterView parameters)
		{
			parameters.SetParameterProperties(this);
			_registry.SetContent(Name, ChildContent);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			if (!string.IsNullOrEmpty(Name)) _registry.SetContent(Name, null);
		}
	}
}