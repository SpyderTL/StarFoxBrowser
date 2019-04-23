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
	public class StarFoxPalette2 : DataNode
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

				for(var xx = 0; xx < 256; xx++)
				{
				var type = reader.ReadByte();

					switch (type)
					{
						case 0x3e:
							var palette = reader.ReadByte();

							Nodes.Add("Palette: " + palette.ToString("X2"));
							break;

						case 0x3f:
							var color = reader.ReadByte();

							Nodes.Add("Color: " + color.ToString("X2"));
							break;

						case 0x40:
							var texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x41:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x42:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x43:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x44:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x45:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x46:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x47:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x48:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x49:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						case 0x4a:
							texture = reader.ReadByte();

							Nodes.Add("Texture 32x32: " + texture.ToString("X2"));
							break;

						default:
							if ((type & 0xf0) == 0x80)
							{
								var address = 0x10000 | type | (reader.ReadByte() << 8);

								Nodes.Add("Animation: " + address.ToString("X2"));
							}
							else
							{
								var material = reader.ReadByte();

								Nodes.Add("Material: " + type.ToString("X2") + " " + material.ToString("X2"));
							}
							break;
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
