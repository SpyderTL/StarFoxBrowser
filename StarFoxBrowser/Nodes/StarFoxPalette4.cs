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
	public class StarFoxPalette4 : DataNode
	{
		public string Resource;
		public int Offset;
		public int Length;

		public override void Reload()
		{
			Nodes.Clear();
		}

		public override object GetProperties()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Offset;

				Compression.CompressedData = reader.ReadBytes(Length);

				Compression.Decompress();

				using (var stream2 = new MemoryStream(Compression.Data))
				using (var reader2 = new BinaryReader(stream2))
				{
					var palette = Enumerable.Range(0, 2560)
						.Select(x => reader2.ReadUInt16())
						.Select(x => Color.FromArgb((x & 0x1f) << 3, (x >> 5 & 0x1f) << 3, (x >> 10) << 3))
						.ToArray();

					return palette;
				}
			}
		}
	}
}