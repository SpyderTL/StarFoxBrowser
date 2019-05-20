using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using StarFoxBrowser.Models;
using StarFoxBrowser.Nodes;

namespace StarFoxBrowser
{
	public partial class BrowserForm : Form
	{
		Model SelectedModel = null;

		public BrowserForm()
		{
			InitializeComponent();

			treeView.Nodes.Add(new Usa10
			{
				Text = "Star Fox 1.0 (USA)",
				Resource = "StarFoxBrowser.Resources.StarFoxUsa10.bin"
			});
		}

		private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
		{
			if (e.Node is DataNode)
				((Nodes.DataNode)e.Node).Reload();
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node is DataNode)
			{
				propertyGrid.SelectedObject = ((DataNode)e.Node).GetProperties();

				if (propertyGrid.SelectedObject is Bitmap)
				{
					pictureBox.Image = (Bitmap)propertyGrid.SelectedObject;
					panel.Hide();
					SelectedModel = null;
				}
				else if (propertyGrid.SelectedObject is Model)
				{
					pictureBox.Image = null;
					SelectedModel = (Model)propertyGrid.SelectedObject;
					DrawModel();
					panel.Show();
				}
				else
				{
					pictureBox.Image = null;
					panel.Hide();
					SelectedModel = null;
				}
			}
			else
				propertyGrid.SelectedObject = null;
		}

		private void DrawModel()
		{
			var d3d = new Direct3D();

			var device = new Device(d3d, 0, DeviceType.Hardware, panel.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters(panel.ClientSize.Width, panel.ClientSize.Height));

			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, SharpDX.Color.Black, 1.0f, 0);
			device.BeginScene();

			//device.VertexDeclaration = SelectedModel.VertexDeclaration;
			//device.SetStreamSource(0, SelectedModel.VertexBuffer, 0, 12);

			//device.DrawIndexedUserPrimitives(

			device.EndScene();
			device.Present();

			device.Dispose();
			d3d.Dispose();
		}
	}
}
