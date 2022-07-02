using System;

namespace Qiniu.Util
{
	public class UnixTimestamp
	{
		private static DateTime dtBase = new DateTime(1970, 1, 1).ToLocalTime();

		private const long TICK_BASE = 10000000L;

		public static long GetUnixTimestamp(long secondsAfterNow)
		{
			return DateTime.Now.AddSeconds(secondsAfterNow).ToLocalTime().Subtract(dtBase)
				.Ticks / 10000000;
		}

		public static long ConvertToTimestamp(DateTime dt)
		{
			return dt.Subtract(dtBase).Ticks / 10000000;
		}

		public static DateTime ConvertToDateTime(string timestamp)
		{
			long value = long.Parse(timestamp) * 10000000;
			return dtBase.AddTicks(value);
		}

		public static DateTime ConvertToDateTime(long timestamp)
		{
			long value = timestamp * 10000000;
			return dtBase.AddTicks(value);
		}
	}
}
