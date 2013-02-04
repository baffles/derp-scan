using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DerpScan.WiaWrapper
{
	internal class WiaPropertyMap
	{
		private Dictionary<WiaProperty, WIA.Property> propertyMap;

		public WiaPropertyMap(WIA.Device device)
		{
			propertyMap = BuildMap(device.Properties.Cast<WIA.Property>());
		}

		public WiaPropertyMap(WIA.Item item)
		{
			propertyMap = BuildMap(item.Properties.Cast<WIA.Property>());
		}

		protected Dictionary<WiaProperty, WIA.Property> BuildMap(IEnumerable<WIA.Property> properties)
		{
			return properties.ToDictionary(prop => (WiaProperty)prop.PropertyID);
		}

		public object this[WiaProperty property]
		{
			get { return propertyMap[property].get_Value(); }
			set { propertyMap[property].set_Value(value); }
		}

		public T GetValue<T>(WiaProperty property)
		{
			return (T)this[property];
		}

		public void SetValue<T>(WiaProperty property, T value)
		{
			this[property] = value;
		}
	}
}
