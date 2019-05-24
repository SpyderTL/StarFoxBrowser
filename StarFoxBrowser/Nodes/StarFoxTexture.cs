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
	public class StarFoxTexture : DataNode
	{
		public string Resource;
		public int Offset;
		public int Page;

		public override void Reload()
		{
			Nodes.Clear();
		}

		public override object GetProperties()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				// Load Palette
				stream.Position = 0x18b0a;

				var palette = Enumerable.Range(0, 16)
					.Select(n => reader.ReadUInt16())
					.Select(n => Color.FromArgb((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3))
					.ToArray();

				stream.Position = Offset;

				var bitmap = new Bitmap(256, 256);

				for (var y = 0; y < 256; y++)
				{
					for (var x = 0; x < 256; x++)
					{
						var value = reader.ReadByte();

						if (Page == 0)
							value >>= 4;
						else
							value &= 0x0f;

						//bitmap.SetPixel(x, y, Color.FromArgb(value * 16, value * 16, value * 16));
						bitmap.SetPixel(x, y, palette[value]);
					}
				}

				return bitmap;
			}
		}
	}
}
