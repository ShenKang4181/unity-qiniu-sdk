using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qiniu.RS.Model
{
	public class ListInfo
	{
		[CompilerGenerated]
		private string _003CMarker_003Ek__BackingField;

		[CompilerGenerated]
		private List<FileDesc> _003CItems_003Ek__BackingField;

		[CompilerGenerated]
		private List<string> _003CCommonPrefixes_003Ek__BackingField;

		public string Marker
		{
			[CompilerGenerated]
			get
			{
				return _003CMarker_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CMarker_003Ek__BackingField = value;
			}
		}

		public List<FileDesc> Items
		{
			[CompilerGenerated]
			get
			{
				return _003CItems_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CItems_003Ek__BackingField = value;
			}
		}

		public List<string> CommonPrefixes
		{
			[CompilerGenerated]
			get
			{
				return _003CCommonPrefixes_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCommonPrefixes_003Ek__BackingField = value;
			}
		}
	}
}
