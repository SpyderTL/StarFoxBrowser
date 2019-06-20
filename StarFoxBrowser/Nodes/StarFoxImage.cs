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
	public class StarFoxImage : DataNode
	{
		public string Resource;
		public int Offset;

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

				var bitmap = new Bitmap(32, 40);

				for (var tileX = 0; tileX < 4; tileX++)
				{
					for (var tileY = 0; tileY < 5; tileY++)
					{
						var data = new int[64];

						// Plane Row 0
						for (var row = 0; row < 8; row++)
						{
							// Plane 0
							var plane0 = reader.ReadByte();

							// Plane 1
							var plane1 = reader.ReadByte();

							data[(row * 8) + 0] |= ((plane0 >> 7) & 0x01) | ((plane1 >> 6) & 0x02);
							data[(row * 8) + 1] |= ((plane0 >> 6) & 0x01) | ((plane1 >> 5) & 0x02);
							data[(row * 8) + 2] |= ((plane0 >> 5) & 0x01) | ((plane1 >> 4) & 0x02);
							data[(row * 8) + 3] |= ((plane0 >> 4) & 0x01) | ((plane1 >> 3) & 0x02);
							data[(row * 8) + 4] |= ((plane0 >> 3) & 0x01) | ((plane1 >> 2) & 0x02);
							data[(row * 8) + 5] |= ((plane0 >> 2) & 0x01) | ((plane1 >> 1) & 0x02);
							data[(row * 8) + 6] |= ((plane0 >> 1) & 0x01) | ((plane1 >> 0) & 0x02);
							data[(row * 8) + 7] |= ((plane0 >> 0) & 0x01) | ((plane1 << 1) & 0x02);
						}

						// Plane Row 1
						for (var row = 0; row < 8; row++)
						{
							// Plane 2
							var plane2 = reader.ReadByte();

							// Plane 3
							var plane3 = reader.ReadByte();

							data[(row * 8) + 0] |= ((plane2 >> 5) & 0x04) | ((plane3 >> 4) & 0x08);
							data[(row * 8) + 1] |= ((plane2 >> 4) & 0x04) | ((plane3 >> 3) & 0x08);
							data[(row * 8) + 2] |= ((plane2 >> 3) & 0x04) | ((plane3 >> 2) & 0x08);
							data[(row * 8) + 3] |= ((plane2 >> 2) & 0x04) | ((plane3 >> 1) & 0x08);
							data[(row * 8) + 4] |= ((plane2 >> 1) & 0x04) | ((plane3 >> 0) & 0x08);
							data[(row * 8) + 5] |= ((plane2 >> 0) & 0x04) | ((plane3 << 1) & 0x08);
							data[(row * 8) + 6] |= ((plane2 << 1) & 0x04) | ((plane3 << 2) & 0x08);
							data[(row * 8) + 7] |= ((plane2 << 2) & 0x04) | ((plane3 << 3) & 0x08);
						}

						for (var y = 0; y < 8; y++)
						{
							for (var x = 0; x < 8; x++)
							{
								var value = data[(y * 8) + x];

								//bitmap.SetPixel(x, y, Color.FromArgb(value * 16, value * 16, value * 16));
								bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + y, palette[value]);
							}
						}
					}
				}

				return bitmap;
			}
		}
	}
}