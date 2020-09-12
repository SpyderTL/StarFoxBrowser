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
	public class StarFoxSongPart : DataNode
	{
		public string Resource;
		public int Offset;
		public int Address;
		public int Size;

		public override void Reload()
		{
			Nodes.Clear();

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Offset;

				while (true)
				{
					var length = reader.ReadUInt16();

					if (length == 0)
						break;

					var destination = reader.ReadUInt16();

					var data = reader.ReadBytes(length);

					Nodes.Add(destination.ToString("X4") + " " + length + " bytes");
				}
			}
		}

		public override object GetProperties()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Offset;

				int destination = reader.ReadUInt16();
				int destination2 = reader.ReadUInt16();

				return new
				{
					Offset = Offset.ToString("X6"),
					Address = Address.ToString("X6"),
					Size = Size.ToString("X4"),
					Destination = destination.ToString("X4"),
					Destination2 = destination2.ToString("X4")
				};
			}
		}

		public class SongPart
		{
			public int Address;
			public int Size;

			public override string ToString()
			{
				return "Address; " + Address.ToString("X6") + " Size: " + Size.ToString("X4");
			}
		}
	}
}
