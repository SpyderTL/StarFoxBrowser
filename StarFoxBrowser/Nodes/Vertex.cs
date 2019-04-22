using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class Vertex
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public override string ToString()
		{
			return X.ToString() + ", " + Y.ToString() + ", " + Z.ToString();
		}
	}
}
