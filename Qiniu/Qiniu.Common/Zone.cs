using System.Runtime.CompilerServices;

namespace Qiniu.Common
{
	public class Zone
	{
		[CompilerGenerated]
		private string _003CRsHost_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CRsfHost_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CApiHost_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CIovipHost_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CUpHost_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CUploadHost_003Ek__BackingField;

		public string RsHost
		{
			[CompilerGenerated]
			get
			{
				return _003CRsHost_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CRsHost_003Ek__BackingField = value;
			}
		}

		public string RsfHost
		{
			[CompilerGenerated]
			get
			{
				return _003CRsfHost_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CRsfHost_003Ek__BackingField = value;
			}
		}

		public string ApiHost
		{
			[CompilerGenerated]
			get
			{
				return _003CApiHost_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CApiHost_003Ek__BackingField = value;
			}
		}

		public string IovipHost
		{
			[CompilerGenerated]
			get
			{
				return _003CIovipHost_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CIovipHost_003Ek__BackingField = value;
			}
		}

		public string UpHost
		{
			[CompilerGenerated]
			get
			{
				return _003CUpHost_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CUpHost_003Ek__BackingField = value;
			}
		}

		public string UploadHost
		{
			[CompilerGenerated]
			get
			{
				return _003CUploadHost_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CUploadHost_003Ek__BackingField = value;
			}
		}

		public static Zone GetZone(ZoneID zoneId, bool useHTTPS = false)
		{
			switch (zoneId)
			{
			case ZoneID.CN_East:
				return ZONE_CN_East(useHTTPS);
			case ZoneID.CN_North:
				return ZONE_CN_North(useHTTPS);
			case ZoneID.CN_South:
				return ZONE_CN_South(useHTTPS);
			case ZoneID.US_North:
				return ZONE_US_North(useHTTPS);
			default:
				return ZONE_CN_East(useHTTPS);
			}
		}

		public static Zone ZONE_CN_East(bool useHTTPS)
		{
			if (useHTTPS)
			{
				Zone zone = new Zone();
				zone.RsHost = "https://rs.qbox.me";
				zone.RsfHost = "https://rsf.qbox.me";
				zone.ApiHost = "https://api.qiniu.com";
				zone.IovipHost = "https://iovip.qbox.me";
				zone.UpHost = "https://up.qbox.me";
				zone.UploadHost = "https://upload.qbox.me";
				return zone;
			}
			Zone zone2 = new Zone();
			zone2.RsHost = "http://rs.qbox.me";
			zone2.RsfHost = "http://rsf.qbox.me";
			zone2.ApiHost = "http://api.qiniu.com";
			zone2.IovipHost = "http://iovip.qbox.me";
			zone2.UpHost = "http://up.qiniu.com";
			zone2.UploadHost = "http://upload.qiniu.com";
			return zone2;
		}

		public static Zone ZONE_CN_North(bool useHTTPS)
		{
			if (useHTTPS)
			{
				Zone zone = new Zone();
				zone.RsHost = "https://rs-z1.qbox.me";
				zone.RsfHost = "https://rsf-z1.qbox.me";
				zone.ApiHost = "https://api-z1.qiniu.com";
				zone.IovipHost = "https://iovip-z1.qbox.me";
				zone.UpHost = "https://up-z1.qbox.me";
				zone.UploadHost = "https://upload-z1.qbox.me";
				return zone;
			}
			Zone zone2 = new Zone();
			zone2.RsHost = "http://rs-z1.qbox.me";
			zone2.RsfHost = "http://rsf-z1.qbox.me";
			zone2.ApiHost = "http://api-z1.qiniu.com";
			zone2.IovipHost = "http://iovip-z1.qbox.me";
			zone2.UpHost = "http://up-z1.qiniu.com";
			zone2.UploadHost = "http://upload-z1.qiniu.com";
			return zone2;
		}

		public static Zone ZONE_CN_South(bool useHTTPS)
		{
			if (useHTTPS)
			{
				Zone zone = new Zone();
				zone.RsHost = "https://rs-z2.qbox.me";
				zone.RsfHost = "https://rsf-z2.qbox.me";
				zone.ApiHost = "https://api-z2.qiniu.com";
				zone.IovipHost = "https://iovip-z2.qbox.me";
				zone.UpHost = "https://up-z2.qbox.me";
				zone.UploadHost = "https://upload-z2.qbox.me";
				return zone;
			}
			Zone zone2 = new Zone();
			zone2.RsHost = "http://rs-z2.qbox.me";
			zone2.RsfHost = "http://rsf-z2.qbox.me";
			zone2.ApiHost = "http://api-z2.qiniu.com";
			zone2.IovipHost = "http://iovip-z2.qbox.me";
			zone2.UpHost = "http://up-z2.qiniu.com";
			zone2.UploadHost = "http://upload-z2.qiniu.com";
			return zone2;
		}

		public static Zone ZONE_US_North(bool useHTTPS)
		{
			if (useHTTPS)
			{
				Zone zone = new Zone();
				zone.RsHost = "https://rs-na0.qbox.me";
				zone.RsfHost = "https://rsf-na0.qbox.me";
				zone.ApiHost = "https://api-na0.qiniu.com";
				zone.IovipHost = "https://iovip-na0.qbox.me";
				zone.UpHost = "https://up-na0.qbox.me";
				zone.UploadHost = "https://upload-na0.qbox.me";
				return zone;
			}
			Zone zone2 = new Zone();
			zone2.RsHost = "http://rs-na0.qbox.me";
			zone2.RsfHost = "http://rsf-na0.qbox.me";
			zone2.ApiHost = "http://api-na0.qiniu.com";
			zone2.IovipHost = "http://iovip-na0.qbox.me";
			zone2.UpHost = "http://up-na0.qiniu.com";
			zone2.UploadHost = "http://upload-na0.qiniu.com";
			return zone2;
		}
	}
}
