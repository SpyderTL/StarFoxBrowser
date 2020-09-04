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
	public class StarFoxTileSet2Bpp : DataNode
	{
		public string Resource;
		public int Offset;
		public int Length;
		public Size Size;

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

				// Decompress Data
				var compressedData = reader.ReadBytes(Length);

				Compression.CompressedData = compressedData;

				Compression.Decompress();

				using (var stream2 = new MemoryStream(Compression.Data))
				using (var reader2 = new BinaryReader(stream2))
				{
					var bitmap = new Bitmap(Size.Width * 8, Size.Height * 8);

					for (var tileY = 0; tileY < Size.Height; tileY++)
					{
						for (var tileX = 0; tileX < Size.Width; tileX++)
						{
							var data = new int[64];

							// Plane Row 0
							for (var row = 0; row < 8; row++)
							{
								// Plane 0
								var plane0 = reader2.ReadByte();

								// Plane 1
								var plane1 = reader2.ReadByte();

								data[(row * 8) + 0] |= ((plane0 >> 7) & 0x01) | ((plane1 >> 6) & 0x02);
								data[(row * 8) + 1] |= ((plane0 >> 6) & 0x01) | ((plane1 >> 5) & 0x02);
								data[(row * 8) + 2] |= ((plane0 >> 5) & 0x01) | ((plane1 >> 4) & 0x02);
								data[(row * 8) + 3] |= ((plane0 >> 4) & 0x01) | ((plane1 >> 3) & 0x02);
								data[(row * 8) + 4] |= ((plane0 >> 3) & 0x01) | ((plane1 >> 2) & 0x02);
								data[(row * 8) + 5] |= ((plane0 >> 2) & 0x01) | ((plane1 >> 1) & 0x02);
								data[(row * 8) + 6] |= ((plane0 >> 1) & 0x01) | ((plane1 >> 0) & 0x02);
								data[(row * 8) + 7] |= ((plane0 >> 0) & 0x01) | ((plane1 << 1) & 0x02);
							}

							for (var y = 0; y < 8; y++)
							{
								for (var x = 0; x < 8; x++)
								{
									var value = data[(y * 8) + x];

									bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + y, Color.FromArgb(value * 64, value * 64, value * 64));
									//bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + y, palette[value]);
								}
							}
						}
					}

					return bitmap;
				}
			}
		}
	}
}