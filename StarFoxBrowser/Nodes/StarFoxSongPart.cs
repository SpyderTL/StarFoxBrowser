using System;
using System.Collections.Generic;
using System.ComponentModel;
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

					var offset = (int)stream.Position;

					var data = reader.ReadBytes(length);

					Nodes.Add(offset.ToString("X6") + ": " + destination.ToString("X4") + "-" + (destination + length - 1).ToString("X4") + " " + length + " bytes");
				}
			}
		}

		public override object GetProperties()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Offset;

				var blocks = new List<SongDataBlock>();

				while (true)
				{
					var length = reader.ReadUInt16();

					if (length == 0)
						break;

					var destination = reader.ReadUInt16();

					var offset = (int)stream.Position;

					var data = reader.ReadBytes(length);

					blocks.Add(new SongDataBlock { Offset = offset, Length = length, Destination = destination, Data = data });
				}

				return new SongData
				{
					Offset = Offset.ToString("X6"),
					Address = Address.ToString("X6"),
					Size = Size.ToString("X4"),
					Blocks = blocks.ToArray()
				};
			}
		}

		public class SongData
		{
			public string Offset { get; set; }
			public string Address { get; set; }
			public string Size { get; set; }
			public SongDataBlock[] Blocks { get; set; }
		}

		public class SongDataBlock
		{
			public int Length { get; set; }
			public int Offset { get; set; }
			public int Destination { get; set; }
			public byte[] Data { get; set; }

			public override string ToString()
			{
				return Offset.ToString("X6") + ": " + Destination.ToString("X4") + "-" + (Destination + Length - 1).ToString("X4") + " " + Length + " bytes";
			}
		}
	}
}
