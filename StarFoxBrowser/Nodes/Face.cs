using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class Face : DataNode
	{
		public byte FaceNumber;
		public byte ColorNumber;
		public sbyte NormalX;
		public sbyte NormalY;
		public sbyte NormalZ;
		public int[] Vertices;

		public override void Reload()
		{
			Nodes.Clear();
		}

		public override object GetProperties()
		{
			return new { FaceNumber, ColorNumber, NormalX, NormalY, NormalZ, Vertices };
		}
	}
}
