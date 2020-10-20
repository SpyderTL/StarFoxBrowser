using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxSong : DataNode
	{
		public string Resource;
		public int Offset;
		public int DataOffset;

		public override object GetProperties()
		{
			return new Properties
			{
				Resource = Resource,
				Offset = Offset,
				DataOffset = DataOffset
			};
		}

		public override void Reload()
		{
			Nodes.Clear();

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				// Read Song Parts
				var partOffsets = new List<int>();

				stream.Position = DataOffset;

				var flags = reader.ReadByte();

				while (true)
				{
					var address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
					var offset = ((address & 0xff0000) >> 1) | (address & 0x7fff);

					if (address == 0)
						break;

					var size = reader.ReadUInt16();

					partOffsets.Add(offset);
				}

				// Read Parts
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

						Array.Copy(data, 0, Spc.Ram, destination, data.Length);
					}
				}

				// Read Song
				using (var stream2 = new MemoryStream(Spc.Ram))
				using (var reader2 = new BinaryReader(stream2))
				{
					stream2.Position = Offset;

					var loop = -1;
					var loopPosition = -1L;

					var stopPosition = -1L;

					var trackPositions = new List<long>();
					var trackOffsets = new List<int>();
					var trackRepeat = new List<int>();

					while (true)
					{
						var position = stream2.Position;
						var trackOffset = reader2.ReadUInt16();

						if (trackOffset == 0x0000)
						{
							stopPosition = position;
							break;
						}

						if ((trackOffset & 0xff00) == 0)
						{
							if ((trackOffset & 0x0080) == 0)
							{
								var offset = reader2.ReadUInt16();

								trackPositions.Add(position);
								trackOffsets.Add(offset);
								trackRepeat.Add(trackOffset);

								continue;
							}
							else
							{
								loop = reader2.ReadUInt16();
								loopPosition = position;
								break;
							}
						}

						trackPositions.Add(position);
						trackOffsets.Add(trackOffset);
						trackRepeat.Add(0);
					}

					for (var track = 0; track < trackOffsets.Count; track++)
					{
						if (trackRepeat[track] != 0)
						{
							var repeatNode = new TreeNode(trackPositions[track].ToString("X4") + ": Repeat " + trackOffsets[track].ToString("X4") + " " + (trackRepeat[track] + 1) + " times");

							Nodes.Add(repeatNode);

							continue;
						}

						var trackNode = new TreeNode(trackPositions[track].ToString("X4") + ": Track " + track + " " + trackOffsets[track].ToString("X4"));

						stream2.Position = trackOffsets[track];

						var channelOffsets = new int[8];

						for (var channel = 0; channel < channelOffsets.Length; channel++)
							channelOffsets[channel] = reader2.ReadUInt16();

						for (var channel = 0; channel < channelOffsets.Length; channel++)
						{
							var channelNode = new TreeNode(channelOffsets[channel].ToString("X4") + ": Channel " + channel);

							// Read Channel
							stream2.Position = channelOffsets[channel];

							while (true)
							{
								var position = stream2.Position;

								var type = reader2.ReadByte();

								if (type == 0x00)
								{
									channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2")));
									break;
								}
								else if (type == 0xc8)
								{
									channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Tie"));
								}
								else if (type == 0xc9)
								{
									channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Rest"));
								}
								else if (type == 0xe0)
								{
									var name = RomSongs.EventTypes[type - 0xe0].Name;
									var data = reader2.ReadBytes(RomSongs.EventTypes[type - 0xe0].Length);

									channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": " + name + " " + string.Join(" ", data.Select(x => x.ToString("X2")))));
								}
								else if (type == 0xef)
								{
									var name = RomSongs.EventTypes[type - 0xe0].Name;
									var start = reader2.ReadUInt16();
									var loop2 = reader2.ReadByte();
									var next = stream2.Position;

									stream2.Position = start;

									var loopNode = new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": " + name + " " + start.ToString("X4") + " " + loop2 + " times");

									while (true)
									{
										position = stream2.Position;
										type = reader2.ReadByte();

										if (type == 0x00)
										{
											loopNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2")));
											break;
										}
										else if (type == 0xc8)
										{
											loopNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Tie"));
										}
										else if (type == 0xc9)
										{
											loopNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Rest"));
										}
										else if (type == 0xe0)
										{
											var name2 = RomSongs.EventTypes[type - 0xe0].Name;
											var data2 = reader2.ReadBytes(RomSongs.EventTypes[type - 0xe0].Length);

											loopNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": " + name2 + " " + string.Join(" ", data2.Select(x => x.ToString("X2")))));
										}
										else if (type >= 0xe1 && type < 0xfb)
										{
											var name2 = RomSongs.EventTypes[type - 0xe0].Name;
											var data2 = reader2.ReadBytes(RomSongs.EventTypes[type - 0xe0].Length);

											loopNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": " + name2 + " " + string.Join(" ", data2.Select(x => x.ToString("X2")))));
										}
										else if (type >= 0x80 && type < 0xca)
										{
											var node = new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Note " + (type - 0x80).ToString("X2"));
											node.ForeColor = Color.Blue;

											loopNode.Nodes.Add(node);
										}
										else if (type >= 0xca && type < 0xe0)
										{
											var node = new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Percussion " + (type - 0xca).ToString("X2"));
											node.ForeColor = Color.Purple;

											loopNode.Nodes.Add(node);
										}
										else if (type >= 0x01 && type < 0x80)
										{
											loopNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Length " + type.ToString()));

											var data2 = reader2.ReadByte();

											if ((data2 & 0x80) == 0)
												loopNode.Nodes.Add(new TreeNode((position + 1).ToString("X4") + ": " + data2.ToString("X2") + ": Duration " + (data2 >> 4).ToString() + " Velocity " + (data2 & 0x0f).ToString()));
											else
												stream2.Seek(-1, SeekOrigin.Current);
										}
										else
										{
											loopNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ""));
										}
									}

									stream2.Position = next;

									channelNode.Nodes.Add(loopNode);
								}
								else if (type >= 0xe1 && type < 0xfb)
								{
									var name = RomSongs.EventTypes[type - 0xe0].Name;
									var data = reader2.ReadBytes(RomSongs.EventTypes[type - 0xe0].Length);

									channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": " + name + " " + string.Join(" ", data.Select(x => x.ToString("X2")))));
								}
								else if (type >= 0x80 && type < 0xca)
								{
									var node = new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Note " + (type - 0x80).ToString("X2"));
									node.ForeColor = Color.Blue;

									channelNode.Nodes.Add(node);
								}
								else if (type >= 0xca && type < 0xe0)
								{
									var node = new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Percussion " + (type - 0x80).ToString("X2"));
									node.ForeColor = Color.Purple;

									channelNode.Nodes.Add(node);
								}
								else if (type >= 0x01 && type < 0x80)
								{
									channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ": Length " + type.ToString()));

									var data2 = reader2.ReadByte();

									if ((data2 & 0x80) == 0)
										channelNode.Nodes.Add(new TreeNode((position + 1).ToString("X4") + ": " + data2.ToString("X2") + ": Duration " + (data2 >> 4).ToString() + " Velocity " + (data2 & 0x0f).ToString()));
									else
										stream2.Seek(-1, SeekOrigin.Current);
								}
								else
								{
									channelNode.Nodes.Add(new TreeNode(position.ToString("X4") + ": " + type.ToString("X2") + ""));
								}

								if (channel < channelOffsets.Length - 1 && stream2.Position == channelOffsets[channel + 1])
									break;
							}

							trackNode.Nodes.Add(channelNode);
						}

						Nodes.Add(trackNode);
					}

					if (loop != -1)
					{
						var repeatNode = new TreeNode(loopPosition.ToString("X4") + ": Repeat " + loop.ToString("X4"));

						Nodes.Add(repeatNode);
					}

					if (stopPosition != -1L)
					{
						var stopNode = new TreeNode(stopPosition.ToString("X4") + ": Stop");

						Nodes.Add(stopNode);
					}
				}
			}
		}

		public class Properties : PropertiesBase
		{
			public string Resource;
			public int Offset;
			public int DataOffset;

			public override IEnumerable<DesignerVerb> Commands
			{
				get
				{
					yield return new DesignerVerb("Play", Play);
					yield return new DesignerVerb("Export", Export);
				}
			}

			private void Play(object sender, EventArgs e)
			{
				SongPlayerForm.Close();

				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
				using (var reader = new BinaryReader(stream))
				{
					stream.Position = DataOffset;

					var flags = reader.ReadByte();

					var partOffsets = new List<int>
					{
						0x0C0000,
						0x0C28C2,
						0x0C9C8E,
						0x0DA6A2,
					};

					while (true)
					{
						var address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
						var offset = ((address & 0xff0000) >> 1) | (address & 0x7fff);

						if (address == 0)
							break;

						var size = reader.ReadUInt16();

						partOffsets.Add(offset);
					}

					SpcInstruments.BlockAddresses = new List<int>();
					SpcInstruments.BlockLengths = new List<int>();
					SpcInstruments.BlockOffsets = new List<int>();

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

							Array.Copy(data, 0, Spc.Ram, destination, data.Length);

							SpcInstruments.BlockAddresses.Add(destination);
							SpcInstruments.BlockLengths.Add(length);
							SpcInstruments.BlockOffsets.Add(offset);
						}
					}

					SongReader.Position = Offset;

					SongPlayerForm.Show();
				}
			}

			private void Export(object sender, EventArgs e)
			{
			}
		}
	}
}