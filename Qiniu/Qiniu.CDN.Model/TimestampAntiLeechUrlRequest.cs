using System;
using System.Runtime.CompilerServices;
using Qiniu.Util;

namespace Qiniu.CDN.Model
{
	public class TimestampAntiLeechUrlRequest
	{
		[CompilerGenerated]
		private string _003COriginURL_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CHost_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CPath_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CFile_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CQuery_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CKey_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CTimestamp_003Ek__BackingField;

		public string RawUrl
		{
			get
			{
				if (!string.IsNullOrEmpty(OriginURL))
				{
					return OriginURL;
				}
				return Host + Path + File + Query;
			}
		}

		public string OriginURL
		{
			[CompilerGenerated]
			get
			{
				return _003COriginURL_003Ek__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				_003COriginURL_003Ek__BackingField = value;
			}
		}

		public string Host
		{
			[CompilerGenerated]
			get
			{
				return _003CHost_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CHost_003Ek__BackingField = value;
			}
		}

		public string Path
		{
			[CompilerGenerated]
			get
			{
				return _003CPath_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CPath_003Ek__BackingField = value;
			}
		}

		public string File
		{
			[CompilerGenerated]
			get
			{
				return _003CFile_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CFile_003Ek__BackingField = value;
			}
		}

		public string Query
		{
			[CompilerGenerated]
			get
			{
				return _003CQuery_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CQuery_003Ek__BackingField = value;
			}
		}

		public string Key
		{
			[CompilerGenerated]
			get
			{
				return _003CKey_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CKey_003Ek__BackingField = value;
			}
		}

		public string Timestamp
		{
			[CompilerGenerated]
			get
			{
				return _003CTimestamp_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CTimestamp_003Ek__BackingField = value;
			}
		}

		public TimestampAntiLeechUrlRequest()
		{
			OriginURL = "";
			Host = "";
			Path = "";
			File = "";
			Query = "";
			Key = "";
			Timestamp = "";
		}

		public TimestampAntiLeechUrlRequest(string url, string key, int expire)
		{
			OriginURL = url;
			string host;
			string path;
			string file;
			string query;
			UrlHelper.UrlSplit(url, out host, out path, out file, out query);
			Host = host;
			Path = path;
			File = file;
			Query = query;
			Key = key;
			SetLinkExpire(expire);
		}

		public void SetLinkExpire(int seconds)
		{
			Timestamp = UnixTimestamp.GetUnixTimestamp(seconds).ToString();
		}

		public void SetLinkExpire(DateTime dt)
		{
			Timestamp = UnixTimestamp.ConvertToTimestamp(dt).ToString();
		}
	}
}
