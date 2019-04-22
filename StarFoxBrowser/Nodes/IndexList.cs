using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class IndexList : DataNode
	{
		public int[] Indeces;

		public override void Reload()
		{
			Nodes.Clear();
		}

		public override object GetProperties()
		{
			return Indeces;
		}
	}
}
