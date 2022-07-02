using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qiniu.CDN.Model
{
	public class LogListInfo
	{
		[CompilerGenerated]
		private int _003CCode_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CError_003Ek__BackingField;

		[CompilerGenerated]
		private Dictionary<string, List<LogData>> _003CData_003Ek__BackingField;

		public int Code
		{
			[CompilerGenerated]
			get
			{
				return _003CCode_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCode_003Ek__BackingField = value;
			}
		}

		public string Error
		{
			[CompilerGenerated]
			get
			{
				return _003CError_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CError_003Ek__BackingField = value;
			}
		}

		public Dictionary<string, List<LogData>> Data
		{
			[CompilerGenerated]
			get
			{
				return _003CData_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CData_003Ek__BackingField = value;
			}
		}
	}
}
