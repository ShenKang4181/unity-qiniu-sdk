using System;

namespace Qiniu.Common
{
	public class Config
	{
		public static Zone ZONE = Zone.GetZone(ZoneID.CN_East);

		public const string FUSION_API_HOST = "http://fusion.qiniuapi.com";

		public const string DFOP_API_HOST = "http://api.qiniu.com";

		public const string PILI_API_HOST = "http://pili.qiniuapi.com";

		public static void SetZone(ZoneID zoneId, bool useHTTPS)
		{
			if (zoneId != ZoneID.Invalid)
			{
				ZONE = Zone.GetZone(zoneId, useHTTPS);
				return;
			}
			throw new Exception("Invalid ZoneID");
		}

		public static void AutoZone(string accessKey, string bucket, bool useHTTPS)
		{
			ZoneID zoneID = ZoneHelper.QueryZone(accessKey, bucket);
			if (zoneID != ZoneID.Invalid)
			{
				SetZone(zoneID, useHTTPS);
			}
		}
	}
}
