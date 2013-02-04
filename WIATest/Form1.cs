using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WIATest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				var d = DerpScan.WiaWrapper.WiaDevice.ShowSelectionDialog();
				if (d != null)
				{
					MessageBox.Show("got dev");
					MessageBox.Show(d.DeviceName);

					pictureBox1.Image = d.Scan();
				}
				else
					MessageBox.Show("no selection");
			}
			catch (DerpScan.WiaWrapper.Exceptions.WiaNoDeviceAvailableException ex)
			{
				MessageBox.Show("no devices");
			}
		}
	}
}
