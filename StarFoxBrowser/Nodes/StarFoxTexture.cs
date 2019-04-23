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

						bitmap.SetPixel(x, y, Color.FromArgb(value * 16, value * 16, value * 16));
					}
				}

				return bitmap;
			}
		}
	}
}
