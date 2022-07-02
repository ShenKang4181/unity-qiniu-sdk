using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Qiniu.Util;

namespace Qiniu.CDN.Model
{
	public class LogListRequest
	{
		[CompilerGenerated]
		private string _003CDay_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CDomains_003Ek__BackingField;

		public string Day
		{
			[CompilerGenerated]
			get
			{
				return _003CDay_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDay_003Ek__BackingField = value;
			}
		}

		public string Domains
		{
			[CompilerGenerated]
			get
			{
				return _003CDomains_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDomains_003Ek__BackingField = value;
			}
		}

		public LogListRequest()
		{
			Day = "";
			Domains = "";
		}

		public LogListRequest(string day, string domains)
		{
			Day = day;
			Domains = domains;
		}

		public LogListRequest(string day, IList<string> domains)
		{
			if (string.IsNullOrEmpty(day))
			{
				Day = "";
			}
			else
			{
				Day = day;
			}
			if (domains == null)
			{
				Domains = "";
				return;
			}
			List<string> list = new List<string>();
			foreach (string domain in domains)
			{
				if (!list.Contains(domain))
				{
					list.Add(domain);
				}
			}
			if (list.Count > 0)
			{
				Domains = StringHelper.Join(list, ";");
			}
			else
			{
				Domains = "";
			}
		}

		public string ToJsonStr()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{ ");
			stringBuilder.AppendFormat("\"day\":\"{0}\", ", Day);
			stringBuilder.AppendFormat("\"domains\":\"{0}\"", Domains);
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}
	}
}
