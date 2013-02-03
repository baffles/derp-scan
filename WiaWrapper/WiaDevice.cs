using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DerpScan.WiaWrapper
{
	public class WiaDevice
	{
		private WIA.Device device;

		public WiaDevice(WIA.Device device)
		{
			this.device = device;
		}

		public static WiaDevice ShowSelectionDialog()
		{
			var dialog = new WIA.CommonDialog();

			try
			{
				var d = dialog.ShowSelectDevice(WIA.WiaDeviceType.ScannerDeviceType, true);
				if (d != null)
					return new WiaDevice(d);
			}
			catch (COMException e)
			{
				switch ((WiaConstant)e.ErrorCode)
				{
					case WiaConstant.WIA_S_NO_DEVICE_AVAILABLE:
						throw new Exceptions.WiaNoDeviceAvailableException(e);

					default:
						throw new Exceptions.WiaException("Error selecting device", (WiaConstant)e.ErrorCode, e);
				}
			}

			return null;
		}
	}
}
