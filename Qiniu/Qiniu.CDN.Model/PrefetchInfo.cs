using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qiniu.CDN.Model
{
	public class PrefetchInfo
	{
		[CompilerGenerated]
		private int _003CCode_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CError_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CRequestId_003Ek__BackingField;

		[CompilerGenerated]
		private List<string> _003CInvalidUrls_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CQuotaDay_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CSurplusaDay_003Ek__BackingField;

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

		public string RequestId
		{
			[CompilerGenerated]
			get
			{
				return _003CRequestId_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CRequestId_003Ek__BackingField = value;
			}
		}

		public List<string> InvalidUrls
		{
			[CompilerGenerated]
			get
			{
				return _003CInvalidUrls_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CInvalidUrls_003Ek__BackingField = value;
			}
		}

		public int QuotaDay
		{
			[CompilerGenerated]
			get
			{
				return _003CQuotaDay_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CQuotaDay_003Ek__BackingField = value;
			}
		}

		public int SurplusaDay
		{
			[CompilerGenerated]
			get
			{
				return _003CSurplusaDay_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CSurplusaDay_003Ek__BackingField = value;
			}
		}
	}
}
