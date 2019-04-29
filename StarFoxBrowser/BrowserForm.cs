using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StarFoxBrowser.Nodes;

namespace StarFoxBrowser
{
	public partial class BrowserForm : Form
	{
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
					//pictureBox.Show();
				}
				else
				{
					//pictureBox.Hide();
					pictureBox.Image = null;
				}
			}
		}
	}
}
