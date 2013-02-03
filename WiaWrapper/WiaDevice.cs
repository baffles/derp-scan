using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DerpScan.WiaWrapper
{
	public class WiaDevice
	{
		public string DeviceID { get { return device.DeviceID; } }

		private WIA.Device device;

		#region Constructor/Acquire methods
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

		public static WiaDevice GetDeviceFromID(string deviceID)
		{
			var manager = new WIA.DeviceManager();

			foreach (WIA.DeviceInfo dev in manager.DeviceInfos)
				if (dev.DeviceID == deviceID)
					return new WiaDevice(dev.Connect());

			return null;
		}
		#endregion

		public object GetProperty(WiaProperty property)
		{
			return device.Properties.Cast<WIA.Property>().Single(prop => prop.PropertyID == (int)property).get_Value();
		}

		public void SetProperty(WiaProperty property, object value)
		{
			device.Properties.Cast<WIA.Property>().Single(prop => prop.PropertyID == (int)property).set_Value(value);
		}
	}
}
