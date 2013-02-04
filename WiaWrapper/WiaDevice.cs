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
		private WiaPropertyMap properties;

		#region Properties
		public string DeviceID { get { return device.DeviceID; } }
		public string DeviceName { get { return properties.GetValue<string>(WiaProperty.WIA_DIP_DEV_NAME); } }
		#endregion

		#region Constructor/Acquire methods
		public WiaDevice(WIA.Device device)
		{
			this.device = device;

			properties = new WiaPropertyMap(device);
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

		/*internal object GetProperty(WiaProperty property)
		{
			return device.Properties.Cast<WIA.Property>().Single(prop => prop.PropertyID == (int)property).get_Value();
		}

		internal void SetProperty(WiaProperty property, object value)
		{
			device.Properties.Cast<WIA.Property>().Single(prop => prop.PropertyID == (int)property).set_Value(value);
		}*/

		public System.Drawing.Image Scan()
		{
			var dialog = new WIA.CommonDialog();

			try
			{
				//do
				{
					//!!properties[WiaProperty.WIA_IPS_DOCUMENT_HANDLING_SELECT] = 2;

					var item = (WIA.Item)device.Items[1];

					var itemProperties = new WiaPropertyMap(item);
					itemProperties[WiaProperty.WIA_IPA_DATATYPE] = 3;
					itemProperties[WiaProperty.WIA_IPS_XRES] = 200;
					itemProperties[WiaProperty.WIA_IPS_YRES] = 200;
					//new Size(8500, 11000)
					var pageWidth = 8500 * 200 / 1000;
					var pageHeight = 11000 * 200 / 1000;
					var pageMaxWidth = properties.GetValue<int>(WiaProperty.WIA_DPS_HORIZONTAL_BED_SIZE) * 200 / 1000;
					var pageMaxHeight = properties.GetValue<int>(WiaProperty.WIA_DPS_VERTICAL_BED_SIZE) * 200 / 1000;

					var horizontalPos = pageMaxWidth - pageWidth;
					pageWidth = Math.Min(pageWidth, pageMaxWidth);
					pageHeight = Math.Min(pageHeight, pageMaxHeight);

					itemProperties[WiaProperty.WIA_IPS_XEXTENT] = pageWidth;
					itemProperties[WiaProperty.WIA_IPS_YEXTENT] = pageHeight;
					itemProperties[WiaProperty.WIA_IPS_XPOS] = horizontalPos;

					
					/*var img = (WIA.ImageFile)dialog.ShowTransfer(item, WiaImageFormat.WiaImgFmt_BMP.ToString());
					img.*/
					var img = (WIA.ImageFile)dialog.ShowTransfer(item, /*WiaImageFormat.WiaImgFmt_BMP.ToString()*/"{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}", false);
					System.Drawing.Image image;
					using (var stream = new System.IO.MemoryStream((byte[])img.FileData.get_BinaryData()))
						image = System.Drawing.Image.FromStream(stream);
					return image;
				}
			}
			finally
			{
			}
		}
	}
}
