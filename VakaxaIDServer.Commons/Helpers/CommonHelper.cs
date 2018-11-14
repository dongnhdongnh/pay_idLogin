using System;

namespace VakaxaIDServer.Commons.Helpers
{
	public static class CommonHelper
	{
		public static string GenerateUuid()
		{
			return Guid.NewGuid().ToString();
		}

		public static long GetUnixTimestamp()
		{
			return UnixTimestamp.ToUnixTimestamp(DateTime.UtcNow);
		}
		
	}
}