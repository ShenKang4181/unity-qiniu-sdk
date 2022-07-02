using System.Collections.Generic;
using System.Text;
using Qiniu.Http;
using Qiniu.JSON;

namespace Qiniu.RS.Model
{
	public class BatchResult : HttpResult
	{
		public string Error
		{
			get
			{
				string result = null;
				if (base.Code != 200 && base.Code != 298)
				{
					Dictionary<string, string> obj = new Dictionary<string, string>();
					JsonHelper.Deserialize<Dictionary<string, string>>(base.Text, out obj);
					if (obj.ContainsKey("error"))
					{
						result = obj["error"];
					}
				}
				return result;
			}
		}

		public List<BatchInfo> Result
		{
			get
			{
				List<BatchInfo> obj = null;
				if ((base.Code == 200 || base.Code == 298) && !string.IsNullOrEmpty(base.Text))
				{
					JsonHelper.Deserialize<List<BatchInfo>>(base.Text, out obj);
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
				stringBuilder.AppendLine("result:");
				int num = 0;
				int count = Result.Count;
				foreach (BatchInfo item in Result)
				{
					stringBuilder.AppendFormat("#{0}/{1}\n", ++num, count);
					stringBuilder.AppendFormat("code: {0}\n", item.Code);
					stringBuilder.AppendFormat("data:\n{0}\n\n", item.Data);
				}
			}
			else if (!string.IsNullOrEmpty(Error))
			{
				stringBuilder.AppendFormat("Error: {0}\n", Error);
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
				foreach (KeyValuePair<string, string> item2 in base.RefInfo)
				{
					stringBuilder.AppendLine(string.Format("{0}: {1}", item2.Key, item2.Value));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
