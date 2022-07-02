using System.Runtime.CompilerServices;

namespace Qiniu.Common
{
	internal class ZoneInfo
	{
		[CompilerGenerated]
		private string _003CTTL_003Ek__BackingField;

		[CompilerGenerated]
		private OBulk _003CHTTP_003Ek__BackingField;

		[CompilerGenerated]
		private OBulk _003CHTTPS_003Ek__BackingField;

		public string TTL
		{
			[CompilerGenerated]
			get
			{
				return _003CTTL_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CTTL_003Ek__BackingField = value;
			}
		}

		public OBulk HTTP
		{
			[CompilerGenerated]
			get
			{
				return _003CHTTP_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CHTTP_003Ek__BackingField = value;
			}
		}

		public OBulk HTTPS
		{
			[CompilerGenerated]
			get
			{
				return _003CHTTPS_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CHTTPS_003Ek__BackingField = value;
			}
		}
	}
}
