using System;
using System.Collections.Generic;
using System.Text;

namespace Qiniu.Util
{
	public class StringHelper
	{
		public static string Join(IList<string> array, string sep)
		{
			if (array == null || sep == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int count = array.Count;
			for (int i = 0; i < count; i++)
			{
				stringBuilder.Append(array[i]);
				stringBuilder.Append(sep);
			}
			return stringBuilder.ToString(0, stringBuilder.Length - sep.Length);
		}

		public static string JsonJoin(IList<string> array)
		{
			if (array == null || array.Count == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int count = array.Count;
			for (int i = 0; i < count; i++)
			{
				stringBuilder.AppendFormat("\"{0}\",", array[i]);
			}
			return stringBuilder.ToString(0, stringBuilder.Length - 1);
		}

		public static string UrlEncode(string text)
		{
			return Uri.EscapeDataString(text);
		}

		public static string UrlFormEncode(Dictionary<string, string> values)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> value in values)
			{
				stringBuilder.AppendFormat("{0}={1}&", Uri.EscapeDataString(value.Key), Uri.EscapeDataString(value.Value));
			}
			string text = stringBuilder.ToString();
			return text.Substring(0, text.Length - 1);
		}
	}
}
