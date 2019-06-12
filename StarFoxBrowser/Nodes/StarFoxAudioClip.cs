using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxAudioClip : DataNode
	{
		public string Resource;
		public int Offset;

		public override void Reload()
		{
			Nodes.Clear();
		}

		public override object GetProperties()
		{
			//using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			//using (var reader = new BinaryReader(stream))
			//{
			//	stream.Position = Offset;

			//	var palette = Enumerable.Range(0, 16)
			//		.Select(n => reader.ReadUInt16())
			//		.Select(n => Color.FromArgb((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3))
			//		.ToArray();

			//	return palette;
			//}

			return null;
		}
	}
}
