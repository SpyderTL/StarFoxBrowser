﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxSurface : DataNode
	{
		public string Resource;
		public int Offset;

		public override void Reload()
		{
			Nodes.Clear();

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Offset;

				for(var entry = 0; entry < 256; entry++)
				{
					var value = reader.ReadByte();
					var type = reader.ReadByte();

					if ((type & 0xf0) == 0x00)
					{
						// Lighting
						Nodes.Add(entry.ToString("X2") + " - " + type.ToString("X2") + " - Lighting Surface (" + value + ")");
					}
					else if ((type & 0x80) != 0x00)
					{
						// Animated
						var offset = (type & 0x0f) << 8 | value;

						Nodes.Add(new StarFoxAnimatedSurface { Text = entry.ToString("X2") + " - " + "8x - Animated Surface (0x" + offset.ToString("X2") + ")", Resource = Resource, Offset = 0x18000 + offset });
					}
					else
					{
						switch (type)
						{
							case 0x3e:
								// Dynamic Color
								Nodes.Add(entry.ToString("X2") + " - " + "3E - Dynamic Color (" + value + ")");
								break;

							case 0x3f:
								// Stipple Color
								Nodes.Add(entry.ToString("X2") + " - " + "3F - Stipple Color (" + (value & 0xf) + ", " + ((value & 0xf0) >> 4) + ")");
								break;

							case 0x40:
								// 32x32 Texture Flipped
								Nodes.Add(entry.ToString("X2") + " - " + "40 - 32x32 Texture Flipped (" + value.ToString("X2") + ")");
								break;

							case 0x41:
								// 64x64 Texture Flipped
								Nodes.Add(entry.ToString("X2") + " - " + "41 - 64x64 Texture Flipped (" + value.ToString("X2") + ")");
								break;

							case 0x42:
								// 8x8 Texture Flipped
								Nodes.Add(entry.ToString("X2") + " - " + "42 - 8x8 Texture Flipped (" + value.ToString("X2") + ")");
								break;

							case 0x43:
								// 64x16 Texture Flipped
								Nodes.Add("43 - 64x16 Texture Flipped (" + value.ToString("X2") + ")");
								break;

							case 0x44:
								// 32x8 Texture Flipped
								Nodes.Add(entry.ToString("X2") + " - " + "44 - 32x8 Texture Flipped (" + value.ToString("X2") + ")");
								break;

							case 0x45:
								// 32x8 Texture
								Nodes.Add(entry.ToString("X2") + " - " + "45 - 32x8 Texture (" + value.ToString("X2") + ")");
								break;

							case 0x46:
								// 64x64 Texture
								Nodes.Add(entry.ToString("X2") + " - " + "46 - 64x64 Texture (" + value.ToString("X2") + ")");
								break;

							case 0x47:
								// 16x8 Texture
								Nodes.Add(entry.ToString("X2") + " - " + "47 - 16x8 Texture (" + value.ToString("X2") + ")");
								break;

							case 0x48:
								// 32x32 Texture
								Nodes.Add(entry.ToString("X2") + " - " + "48 - 32x32 Texture (" + value.ToString("X2") + ")");
								break;

							case 0x49:
								// 64x64 Texture Polar Flipped
								Nodes.Add(entry.ToString("X2") + " - " + "49 - 64x64 Texture Polar Flipped (" + value.ToString("X2") + ")");
								break;

							case 0x4a:
								// 64x64 Texture Polar
								Nodes.Add(entry.ToString("X2") + " - " + "4a - 64x64 Texture Polar (" + value.ToString("X2") + ")");
								break;
						}
					}
				}
			}
		}

		public override object GetProperties()
		{
			return null;
		}
	}
}
