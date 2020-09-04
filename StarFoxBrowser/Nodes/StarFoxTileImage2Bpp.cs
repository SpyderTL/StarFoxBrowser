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
	public class StarFoxTileImage2Bpp : DataNode
	{
		public string Resource;
		public int TileOffset;
		public int TileLength;
		public int TileCount;
		public int MapOffset;
		public int MapLength;
		public Size MapSize;

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
				//stream.Position = 0x18b0a;
				stream.Position = 0x1d440;

				//var palette = Enumerable.Range(0, 16)
				var palette = Enumerable.Range(0, 256)
					.Select(n => reader.ReadUInt16())
					.Select(n => Color.FromArgb((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3))
					.ToArray();

				// Decompress Tile Data
				stream.Position = TileOffset;

				var compressedData = reader.ReadBytes(TileLength);

				Compression.CompressedData = compressedData;

				Compression.Decompress();

				var tileData = new byte[TileCount * 64];

				using (var stream2 = new MemoryStream(Compression.Data))
				using (var reader2 = new BinaryReader(stream2))
				{
					for (var tile = 0; tile < TileCount; tile++)
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

								tileData[(tile * 64) + (y * 8) + x] = (byte)value;
							}
						}
					}
				}

				// Decompress Map Data
				stream.Position = MapOffset;

				compressedData = reader.ReadBytes(MapLength);

				Compression.CompressedData = compressedData;

				Compression.Decompress();

				using (var stream2 = new MemoryStream(Compression.Data))
				using (var reader2 = new BinaryReader(stream2))
				{
					var bitmap = new Bitmap(MapSize.Width * 8, MapSize.Height * 8);

					for (var tileY = 0; tileY < MapSize.Height; tileY++)
					{
						for (var tileX = 0; tileX < MapSize.Width; tileX++)
						{
							var value = reader2.ReadUInt16();
							var tile = value & 0x03ff;
							var palette2 = (value >> 10) & 0x07;
							var priority = (value >> 13) & 0x01;
							var horizontalFlip = (value >> 14) & 0x01;
							var verticalFlip = (value >> 15) & 0x01;

							for (var y = 0; y < 8; y++)
							{
								for (var x = 0; x < 8; x++)
								{
									if (tile < TileCount)
									{
										var index = tileData[(tile * 64) + (y * 8) + x];

										if (horizontalFlip == 0)
										{
											if (verticalFlip == 0)
											{
												//bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + y, Color.FromArgb(index * 16, index * 16, index * 16));
												bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + y, palette[(palette2 * 16) + index]);
											}
											else
											{
												//bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + 7 - y, Color.FromArgb(index * 16, index * 16, index * 16));
												bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + 7 - y, palette[(palette2 * 16) + index]);
											}
										}
										else
										{
											if (verticalFlip == 0)
											{
												//bitmap.SetPixel((tileX * 8) + 7 - x, (tileY * 8) + y, Color.FromArgb(index * 16, index * 16, index * 16));
												bitmap.SetPixel((tileX * 8) + 7 - x, (tileY * 8) + y, palette[(palette2 * 16) + index]);
											}
											else
											{
												//bitmap.SetPixel((tileX * 8) + 7 - x, (tileY * 8) + 7 - y, Color.FromArgb(index * 16, index * 16, index * 16));
												bitmap.SetPixel((tileX * 8) + 7 - x, (tileY * 8) + 7 - y, palette[(palette2 * 16) + index]);
											}
										}
									}
									else
									{
										bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + y, Color.Black);
									}
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