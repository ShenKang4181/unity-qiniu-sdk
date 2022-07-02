using System.Runtime.CompilerServices;
using System.Text;

namespace Qiniu.CDN.Model
{
	public class FluxRequest
	{
		[CompilerGenerated]
		private string _003CStartDate_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CEndDate_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CGranularity_003Ek__BackingField;

		[CompilerGenerated]
		private string _003CDomains_003Ek__BackingField;

		public string StartDate
		{
			[CompilerGenerated]
			get
			{
				return _003CStartDate_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CStartDate_003Ek__BackingField = value;
			}
		}

		public string EndDate
		{
			[CompilerGenerated]
			get
			{
				return _003CEndDate_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CEndDate_003Ek__BackingField = value;
			}
		}

		public string Granularity
		{
			[CompilerGenerated]
			get
			{
				return _003CGranularity_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CGranularity_003Ek__BackingField = value;
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

		public FluxRequest()
		{
			StartDate = "";
			EndDate = "";
			Granularity = "";
			Domains = "";
		}

		public FluxRequest(string startDate, string endDate, string granularity, string domains)
		{
			StartDate = startDate;
			EndDate = endDate;
			Granularity = granularity;
			Domains = domains;
		}

		public string ToJsonStr()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{ ");
			stringBuilder.AppendFormat("\"startDate\":\"{0}\", ", StartDate);
			stringBuilder.AppendFormat("\"endDate\":\"{0}\", ", EndDate);
			stringBuilder.AppendFormat("\"granularity\":\"{0}\", ", Granularity);
			stringBuilder.AppendFormat("\"domains\":\"{0}\"", Domains);
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}
	}
}
