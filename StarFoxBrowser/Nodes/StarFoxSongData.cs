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
						var songNode = new TreeNode(song.ToString("X2") + " " + songOffsets[song].ToString("X4"));

						if (songOffsets[song] != 0)
						{
							stream2.Position = songOffsets[song];

							var repeat = -1;
							var repeatPosition = -1L;

							var trackPositions = new List<long>();
							var trackOffsets = new List<int>();

							while (true)
							{
								var position = stream2.Position;
								var trackOffset = reader2.ReadUInt16();

								if (trackOffset == 0x83)
								{
									repeat = reader2.ReadUInt16();
									repeatPosition = position;
									break;
								}

								if (trackOffset == 0)
									break;

								trackPositions.Add(position);
								trackOffsets.Add(trackOffset);
							}

							for (var track = 0; track < trackOffsets.Count; track++)
							{
								var trackNode = new TreeNode(trackPositions[track].ToString("X4") + ": " + trackOffsets[track].ToString("X4"));

								stream2.Position = trackOffsets[track];

								var channelOffsets = new int[8];

								for (var channel = 0; channel < channelOffsets.Length; channel++)
									channelOffsets[channel] = reader2.ReadUInt16();

								for (var channel = 0; channel < channelOffsets.Length; channel++)
								{
									var channelNode = new TreeNode(channelOffsets[channel].ToString("X4"));

									// Read Channel
									stream2.Position = channelOffsets[channel];

									while (true)
									{
										var position = stream2.Position;

										var type = reader2.ReadByte();

										if (type == 0x00 || type == 0xC9 || type == 0x89)
										{
											channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2")));
											break;
										}
										else if (type >= 0xe0)
										{
											var data = reader2.ReadBytes(RomSongs.EventTypes[type - 0xe0].Length);

											channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": " + string.Join(" ", data.Select(x => x.ToString("X2")))));
										}
										else
										{
											channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2")));
										}
									}

									trackNode.Nodes.Add(channelNode);
								}

								songNode.Nodes.Add(trackNode);
							}

							if (repeat != -1)
							{
								var repeatNode = new TreeNode(repeatPosition.ToString("X4") + ": Repeat " + repeat.ToString("X4"));

								songNode.Nodes.Add(repeatNode);
							}
						}

						songsNode.Nodes.Add(songNode);
					}

					Nodes.Add(songsNode);
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
