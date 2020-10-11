using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxSongData : DataNode
	{
		public string Resource;
		public int Offset;

		public override void Reload()
		{
			Nodes.Clear();

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				// Read Song Parts
				var partsNode = new TreeNode("Parts");
				var partOffsets = new List<int>();

				stream.Position = Offset;

				var flags = reader.ReadByte();

				while (true)
				{
					var address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
					var offset = ((address & 0xff0000) >> 1) | (address & 0x7fff);

					if (address == 0)
						break;

					var size = reader.ReadUInt16();

					partsNode.Nodes.Add(new StarFoxSongPart { Text = "Address: " + address.ToString("x6") + " Size: " + size, Address = address, Size = size, Resource = Resource, Offset = offset });

					partOffsets.Add(offset);
				}

				Nodes.Add(partsNode);

				// Read Parts
				var spc = new byte[0x10000];

				partOffsets = new int[] { 0x0C0000 }.Concat(partOffsets).ToList();

				for (var part = 0; part < partOffsets.Count; part++)
				{
					stream.Position = partOffsets[part];

					while (true)
					{
						var length = reader.ReadUInt16();

						if (length == 0)
							break;

						var destination = reader.ReadUInt16();

						var offset = (int)stream.Position;

						var data = reader.ReadBytes(length);

						Array.Copy(data, 0, spc, destination, data.Length);
					}
				}

				// Read Songs
				var songsNode = new TreeNode("Songs");

				using (var stream2 = new MemoryStream(spc))
				using (var reader2 = new BinaryReader(stream2))
				{
					stream2.Position = 0xfdc0;

					var songOffsets = new int[20];

					for (var song = 0; song < songOffsets.Length; song++)
						songOffsets[song] = reader2.ReadUInt16();

					for (var song = 0; song < songOffsets.Length; song++)
					{
						var songNode = new StarFoxSong { Text = song.ToString("X2") + " " + songOffsets[song].ToString("X4"), Offset = songOffsets[song], DataOffset = Offset, Resource = Resource };

						songsNode.Nodes.Add(songNode);
					}
				}

				Nodes.Add(songsNode);

				// Read Voices
				var voicesNode = new TreeNode("Voices");

				using (var stream2 = new MemoryStream(spc))
				using (var reader2 = new BinaryReader(stream2))
				{
					stream2.Position = 0x3d00;

					for (var voice = 0; voice < 55; voice++)
					{
						var sample = reader2.ReadByte();
						var envelope = reader2.ReadByte();
						var envelope2 = reader2.ReadByte();
						var gain = reader2.ReadByte();
						var echo = reader2.ReadUInt16();

						voicesNode.Nodes.Add(voice.ToString("X2") + ": " + sample.ToString("X2"));
					}
				}

				Nodes.Add(voicesNode);

				// Read Samples
				var samplesNode = new TreeNode("Samples");

				using (var stream2 = new MemoryStream(spc))
				using (var reader2 = new BinaryReader(stream2))
				{
					stream2.Position = 0x3c00;

					for (var voice = 0; voice < 0x40; voice++)
					{
						var start = reader2.ReadUInt16();
						var loop = reader2.ReadUInt16();

						samplesNode.Nodes.Add(voice.ToString("X2") + ": " + start.ToString("X4") + " - " + loop.ToString("X4"));
					}
				}

				Nodes.Add(samplesNode);
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
