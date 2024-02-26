using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorComponents
{
	public partial class CategorisedSelect<TObject, TCategory, TText>
	{
		#region Parameters

		[Parameter]
		public TObject Value
		{
			get => _value;
			set
			{
				_value = value;
				if (BoundValueMatches(value) == false) ValueChanged.InvokeAsync(value);
			}
		}
		[Parameter] public EventCallback<TObject> ValueChanged { get; set; }
		/// <summary>
		/// The available choices for the select options.
		/// </summary>
		[Parameter][EditorRequired] public List<TObject> Options { get; set; }
		/// <summary>
		/// Function for determining the categories the select should be split into.
		/// </summary>
		[Parameter][EditorRequired] public Func<TObject, TCategory> CategoryFunction { get; set; }
		/// <summary>
		/// Function for determining the text that should be displayed within the select options.
		/// </summary>
		[Parameter][EditorRequired] public Func<TObject, TText> DisplayTextFunction { get; set; }
		/// <summary>
		/// Function for determining which option is currently bound to the select value.
		/// </summary>
		[Parameter][EditorRequired] public Func<TObject, bool> BoundValueEqualityComparisonFunction { get; set; }
		[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

		#endregion

		#region Properties and Fields

		private TObject _value;
		private List<TCategory> _categories { get; set; } = new();
		private bool _boundValueNotInList { get; set; }
		private List<KeyValuePair<int, TObject>> _keyValueOptionList { get; set; } = new();

		#endregion

		#region Methods

		protected override void OnInitialized()
		{
			_categories = Options.Select(CategoryFunction.Invoke).Distinct().ToList();

			for (var i = 0; i < Options.Count; i++) _keyValueOptionList.Add(new KeyValuePair<int, TObject>(i, Options[i]));

			if (Options.All(x => BoundValueMatches(x) == false)) _boundValueNotInList = true;
		}

		protected int? GetCurrentlyBoundValueKey()
		{
			var matchingValue = _keyValueOptionList.FirstOrDefault(x => BoundValueMatches(x.Value));
			if (matchingValue.Equals(default(KeyValuePair<int, TObject>))) return null;

			return matchingValue.Key;
		}

		protected bool BoundValueMatches(TObject value)
		{
			if (Equals(value, default)) return false;
			return BoundValueEqualityComparisonFunction.Invoke(value);
		}

		protected void OnChange(ChangeEventArgs e)
		{
			if (int.TryParse(e.Value?.ToString(), out var selectedKeyOfValue) == false) return;

			Value = _keyValueOptionList.First(x => x.Key == selectedKeyOfValue).Value;
		}

		protected List<KeyValuePair<int, TObject>> GetOptionsForCategory(TCategory category) => _keyValueOptionList.Where(x => EqualityComparer<TCategory>.Default.Equals(CategoryFunction.Invoke(x.Value), category)).ToList();

		#endregion
	}
}