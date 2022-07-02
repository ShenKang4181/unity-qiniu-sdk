using System.Collections.Generic;
using System.Text;
using Qiniu.Http;
using Qiniu.JSON;

namespace Qiniu.RS.Model
{
	public class BucketResult : HttpResult
	{
		public BucketInfo Result
		{
			get
			{
				BucketInfo obj = null;
				if (base.Code == 200 && !string.IsNullOrEmpty(base.Text))
				{
					JsonHelper.Deserialize<BucketInfo>(base.Text, out obj);
				}
				return obj;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("code: {0}\n", base.Code);
			if (Result != null)
			{
				stringBuilder.AppendLine("bucket-info:");
				stringBuilder.AppendFormat("tbl={0}\n", Result.tbl);
				stringBuilder.AppendFormat("zone={0}\n", Result.zone);
				stringBuilder.AppendFormat("region={0}\n", Result.region);
				stringBuilder.AppendFormat("isGlobal={0}\n", Result.global);
				stringBuilder.AppendFormat("isLine={0}\n", Result.line);
			}
			else if (!string.IsNullOrEmpty(base.Text))
			{
				stringBuilder.AppendLine("text:");
				stringBuilder.AppendLine(base.Text);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("ref-code: {0}\n", base.RefCode);
			if (!string.IsNullOrEmpty(base.RefText))
			{
				stringBuilder.AppendLine("ref-text:");
				stringBuilder.AppendLine(base.RefText);
			}
			if (base.RefInfo != null)
			{
				stringBuilder.AppendFormat("ref-info:\n");
				foreach (KeyValuePair<string, string> item in base.RefInfo)
				{
					stringBuilder.AppendLine(string.Format("{0}: {1}", item.Key, item.Value));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
