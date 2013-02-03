using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DerpScan.WiaWrapper.Exceptions
{
	public class WiaNoDeviceAvailableException : WiaException
	{
		public WiaNoDeviceAvailableException(Exception innerException)
			: base("No WIA devices available", WiaConstant.WIA_S_NO_DEVICE_AVAILABLE, innerException)
		{
		}
	}
}
