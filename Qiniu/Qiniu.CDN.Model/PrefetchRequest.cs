using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Qiniu.CDN.Model
{
	public class PrefetchRequest
	{
		[CompilerGenerated]
		private List<string> _003CUrls_003Ek__BackingField;

		public List<string> Urls
		{
			[CompilerGenerated]
			get
			{
				return _003CUrls_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CUrls_003Ek__BackingField = value;
			}
		}

		public PrefetchRequest()
		{
			Urls = new List<string>();
		}

		public PrefetchRequest(IList<string> urls)
		{
			if (urls != null)
			{
				Urls = new List<string>(urls);
			}
			else
			{
				Urls = new List<string>();
			}
		}

		public void AddUrls(IList<string> urls)
		{
			if (urls == null)
			{
				return;
			}
			foreach (string url in urls)
			{
				if (!Urls.Contains(url))
				{
					Urls.Add(url);
				}
			}
		}

		public string ToJsonStr()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{ ");
			stringBuilder.Append("\"urls\":[");
			if (Urls != null)
			{
				if (Urls.Count == 1)
				{
					stringBuilder.AppendFormat("\"{0}\"", Urls[0]);
				}
				else
				{
					for (int i = 0; i < Urls.Count; i++)
					{
						if (i < Urls.Count - 1)
						{
							stringBuilder.AppendFormat("\"{0}\",", Urls[i]);
						}
						else
						{
							stringBuilder.AppendFormat("\"{0}\"", Urls[i]);
						}
					}
				}
			}
			stringBuilder.Append("] }");
			return stringBuilder.ToString();
		}
	}
}
