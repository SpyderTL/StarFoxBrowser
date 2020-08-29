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
	public class StarFoxFont2 : DataNode
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
				stream.Position = Offset;

				var bitmap = new Bitmap(80 * 16, 16);

				for (var character = 0; character < 80; character++)
				{
					for (var row = 0; row < 16; row++)
					{
						var value = reader.ReadByte();

						for (var column = 0; column < 8; column++)
						{
							bitmap.SetPixel((character * 16) + column + 8, row, (value & (1 << (7 - column))) == 0 ? Color.Black : Color.White);
						}

						value = reader.ReadByte();

						for (var column = 0; column < 8; column++)
						{
							bitmap.SetPixel((character * 16) + column, row, (value & (1 << (7 - column))) == 0 ? Color.Black : Color.White);
						}
					}
				}

				return bitmap;
			}
		}
	}
}