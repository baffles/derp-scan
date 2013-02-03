using System;

namespace DerpScan.WiaWrapper.Exceptions
{
	public class WiaException : Exception
	{
		public WiaConstant ErrorCode { get; private set; }

		public WiaException(string message, WiaConstant errorCode, Exception innerException)
			: base(message, innerException)
		{
			ErrorCode = errorCode;
		}
	}
}
