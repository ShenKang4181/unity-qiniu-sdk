using System;
using System.Collections.Generic;
using System.Text;
using Qiniu.Http;
using Qiniu.JSON;

namespace Qiniu.Common
{
	public class ZoneHelper
	{
		private static Dictionary<string, ZoneID> ZONE_DICT;

		public static ZoneID QueryZone(string accessKey, string bucket)
		{
			ZoneID zoneID = ZoneID.Invalid;
			try
			{
				string url = string.Format("https://uc.qbox.me/v1/query?ak={0}&bucket={1}", accessKey, bucket);
				HttpResult httpResult = new HttpManager(false).Get(url, null);
				if (httpResult.Code == 200)
				{
					ZoneInfo obj = null;
					if (JsonHelper.Deserialize<ZoneInfo>(httpResult.Text, out obj))
					{
						string key = obj.HTTP.UP[0];
						return ZONE_DICT[key];
					}
					throw new Exception("JSON Deserialize failed: " + httpResult.Text);
				}
				throw new Exception("code: " + httpResult.Code + ", text: " + httpResult.Text + ", ref-text:" + httpResult.RefText);
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("[{0}] QueryZone Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
				for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
				{
					stringBuilder.Append(ex2.Message + " ");
				}
				stringBuilder.AppendLine();
				throw new Exception(stringBuilder.ToString());
			}
		}

		static ZoneHelper()
		{
			Dictionary<string, ZoneID> dictionary = new Dictionary<string, ZoneID>();
			dictionary.Add("http://up.qiniu.com", ZoneID.CN_East);
			dictionary.Add("http://up-z1.qiniu.com", ZoneID.CN_North);
			dictionary.Add("http://up-z2.qiniu.com", ZoneID.CN_South);
			dictionary.Add("http://up-na0.qiniu.com", ZoneID.US_North);
			ZONE_DICT = dictionary;
		}
	}
}
