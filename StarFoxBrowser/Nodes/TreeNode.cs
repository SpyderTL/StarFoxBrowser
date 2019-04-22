using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarFoxBrowser.Nodes
{
	public abstract class DataNode : TreeNode
	{
		public DataNode()
		{
			Nodes.Add("Loading...");
		}

		public abstract void Reload();

		public abstract object GetProperties();
	}
}
