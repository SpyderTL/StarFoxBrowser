using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class VertexList : DataNode
	{
		public Vertex[] Verteces;

		public override void Reload()
		{
			Nodes.Clear();
		}

		public override object GetProperties()
		{
			return Verteces;
		}
	}
}
