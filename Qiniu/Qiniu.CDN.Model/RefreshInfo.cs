using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qiniu.CDN.Model
{
	public class RefreshInfo
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
		private List<string> _003CInvalidDirs_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CUrlQuotaDay_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CUrlSurplusaDay_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CDirQuotaDay_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CDirSurplusaDay_003Ek__BackingField;

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

		public List<string> InvalidDirs
		{
			[CompilerGenerated]
			get
			{
				return _003CInvalidDirs_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CInvalidDirs_003Ek__BackingField = value;
			}
		}

		public int UrlQuotaDay
		{
			[CompilerGenerated]
			get
			{
				return _003CUrlQuotaDay_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CUrlQuotaDay_003Ek__BackingField = value;
			}
		}

		public int UrlSurplusaDay
		{
			[CompilerGenerated]
			get
			{
				return _003CUrlSurplusaDay_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CUrlSurplusaDay_003Ek__BackingField = value;
			}
		}

		public int DirQuotaDay
		{
			[CompilerGenerated]
			get
			{
				return _003CDirQuotaDay_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDirQuotaDay_003Ek__BackingField = value;
			}
		}

		public int DirSurplusaDay
		{
			[CompilerGenerated]
			get
			{
				return _003CDirSurplusaDay_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDirSurplusaDay_003Ek__BackingField = value;
			}
		}
	}
}
