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
	public class StarFoxSong : DataNode
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

				var flags = reader.ReadByte();

				while (true)
				{
					var address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);

					if (address == 0)
						break;

					var size = reader.ReadUInt16();

					Nodes.Add(new StarFoxSongPart { Text = "Address: " + address.ToString("x6") + " Size: " + size, Address = address, Size = size, Resource = Resource, Offset = ((address & 0xff0000) >> 1) | (address & 0x7fff) });
				}
			}
		}

		public override object GetProperties()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Offset;

				var flags = reader.ReadByte();

				var parts = new List<SongPart>();

				while (true)
				{
					var address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);

					if (address == 0)
						break;

					var size = reader.ReadUInt16();

					parts.Add(new SongPart { Address = address, Size = size });
				}

				return new
				{
					Offset,
					Flags = flags,
					Parts = parts.ToArray()
				};
			}
		}

		public class SongPart
		{
			public int Address;
			public int Size;

			public override string ToString() => "Address; " + Address.ToString("X6") + " Size: " + Size;
		}
	}
}
