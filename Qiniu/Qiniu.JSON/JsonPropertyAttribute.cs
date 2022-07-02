using System;
using System.Runtime.CompilerServices;

namespace Qiniu.JSON
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : Attribute
	{
		[CompilerGenerated]
		private string _003CPropertyName_003Ek__BackingField;

		public string PropertyName
		{
			[CompilerGenerated]
			get
			{
				return _003CPropertyName_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CPropertyName_003Ek__BackingField = value;
			}
		}

		public JsonPropertyAttribute()
		{
		}

		public JsonPropertyAttribute(string propertyName)
		{
			PropertyName = propertyName;
		}
	}
}
