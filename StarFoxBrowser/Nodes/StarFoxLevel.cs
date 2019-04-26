using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxLevel : DataNode
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

				var read = true;

				while (read)
				{
					var entryType = reader.ReadByte();

					switch (entryType)
					{
						case 0x00:
							// 3D Object (16-bit vector) (8-bit lookup table index)
							//reader.ReadBytes(10);
							var z = reader.ReadInt16();
							var x = reader.ReadInt16();
							var y = reader.ReadInt16();
							var timer = reader.ReadUInt16();
							var objectNumber = reader.ReadByte();       // Lookup object address in table at 0x2848
							var end2 = reader.ReadByte();

							Nodes.Add("00 - 3D Object: " + objectNumber + " (" + x + ", " + y + ", " + z + ") Timer: " + timer);
							break;

						case 0x02:
							// End Level
							Nodes.Add("End Level");
							read = false;
							break;

						case 0x04:
							// Loop Segment
							reader.ReadBytes(4);
							Nodes.Add("04 - Loop Segment");
							break;

						case 0x06:
							reader.ReadBytes(176);
							Nodes.Add("06 - Unknown");
							break;

						case 0x0a:
							reader.ReadBytes(15);
							Nodes.Add("0a - Random Object");
							break;

						case 0x0c:
							reader.ReadBytes(4);
							Nodes.Add("0c - Unknown");
							break;

						case 0x0e:
							// Show Stage Number
							Nodes.Add("0e - Show Stage Number");
							break;

						case 0x10:
							// Initialize Level
							reader.ReadBytes(2);
							Nodes.Add("10 - Initialize Level");
							break;

						case 0x12:
							// Z Timer (16-bit)
							reader.ReadUInt16();
							Nodes.Add("12 - Z-Timer");
							break;

						case 0x14:
							// Change Music
							reader.ReadBytes(1);
							Nodes.Add("14 - Change Music");
							break;

						case 0x26:
							reader.ReadBytes(3);
							Nodes.Add("26 - Data");
							break;

						case 0x28:
							// Warp w/ Return
							reader.ReadBytes(3);
							Nodes.Add("28 - Warp and Return");
							break;

						case 0x2a:
							// Return From Warp
							Nodes.Add("2a - Return From Warp");
							break;

						case 0x2c:
							// Warp If Condition
							reader.ReadBytes(5);
							//unknown = reader.ReadBytes(10);
							Nodes.Add("2c - Warp If True");
							break;

						case 0x2e:
							// Warp
							reader.ReadBytes(3);
							Nodes.Add("2e - Warp");
							break;

						case 0x36:
							// Rotate Previous Object
							//reader.ReadBytes(3);
							var mode = reader.ReadByte();
							var angle = reader.ReadUInt16();
							Nodes.Add("36 - Rotate Previous Object (Mode: " + mode + " Angle: " + angle + ")");
							break;

						case 0x38:
							reader.ReadBytes(4);
							Nodes.Add("38 - Define Active Area For Previous Object");
							break;

						case 0x3c:
							reader.ReadBytes(3);
							Nodes.Add("3c - Offset Z-Position");
							break;

						case 0x3e:
							reader.ReadBytes(4);
							Nodes.Add("3e - Unknown");
							break;

						case 0x42:
							Nodes.Add("42 - Unknown");
							break;

						case 0x44:
							// Fade Out
							Nodes.Add("44 - Fade Out");
							break;

						case 0x48:
							reader.ReadBytes(5);
							Nodes.Add("48 - Move Previous Object By Parent Coordinates");
							break;

						case 0x4a:
							reader.ReadBytes(3);
							Nodes.Add("4a - Attach Parent To Previous Object");
							break;

						case 0x4c:
							Nodes.Add("4c - Unknown");
							break;

						case 0x4e:
							Nodes.Add("4e - Unknown");
							break;

						case 0x50:
							Nodes.Add("50 - Unknown");
							break;

						case 0x5a:
							Nodes.Add("5a - Palette Change Previous Object");
							break;

						case 0x5c:
							//reader.ReadBytes(4);
							var value = reader.ReadByte();
							var address = reader.ReadBytes(3);

							Nodes.Add("5c - Set SNES RAM (Value: " + value + " Address: " + string.Join(string.Empty, address.Select(v => v.ToString("X2"))) + ")");
							break;

						case 0x5e:
							// Screen Transition
							reader.ReadBytes(5);
							Nodes.Add("5e - Screen Transition");
							break;

						case 0x64:
							// Reset Camera
							reader.ReadBytes(1);
							Nodes.Add("64 - Reset Camera");
							break;

						case 0x6a:
							// Unknown
							reader.ReadBytes(5);
							Nodes.Add("6a - Unknown");
							break;

						case 0x70:
							// 3D Object (8-bit vector) (8-bit lookup table index)
							//reader.ReadBytes(6);
							z = reader.ReadByte();
							x = reader.ReadSByte();
							y = reader.ReadSByte();
							timer = reader.ReadUInt16();
							objectNumber = reader.ReadByte();
							//	//var behavior = reader.ReadByte();
							Nodes.Add("70 - 3D Object: " + objectNumber + " (" + x + ", " + y + ", " + z + ") Timer: " + timer);
							break;

						case 0x74:
							// 3D Object Behavior (16-bit vector) (8-bit table lookup index)
							reader.ReadBytes(9);
							//	z = reader.ReadInt16();
							//	x = reader.ReadInt16();
							//	y = reader.ReadInt16();
							//	timer = reader.ReadUInt16();
							//	var behavior = reader.ReadByte();               // TODO: Find lookup table
							Nodes.Add("74 - 3D Object");
							break;

						case 0x76:
							// 3D Object (16-bit vector)
							reader.ReadBytes(5);
							//	//z = reader.ReadByte();
							//	//x = reader.ReadSByte();
							//	//y = reader.ReadSByte();
							//	//timer = reader.ReadUInt16();
							//	//var objectAddress = reader.ReadUInt16();
							//	//var behavior2 = reader.ReadBytes(4);

							//	//var color = reader.ReadByte();
							//	//end = reader.ReadByte();
							Nodes.Add("76 - 3D Object");
							break;

						case 0x78:
							// SNES Code
							var start = stream.Position;
							var end = stream.Position;
							var branches = new List<long>();

							while (true)
							{
								if (branches.Contains(stream.Position & 0x7fff))
								{
									end = stream.Position - 1;
									break;
								}

								if (reader.ReadByte() == 0xa2)
									branches.Add(reader.ReadUInt16());
							}

							var length = end - start;

							Nodes.Add("78 - Native Code (" + length + " bytes)");

							break;

						case 0x7a:
							reader.ReadBytes(3);
							Nodes.Add("7a - Transition");
							break;

						case 0x82:
							// Unknown
							reader.ReadBytes(1);
							Nodes.Add("82 - Unknown");
							break;

						case 0x84:
							// Change Palette
							Nodes.Add("84 - Change Palette");
							break;

						case 0x86:
							// 3D Object (16-bit vector) (16-bit address)
							reader.ReadBytes(13);
							//	z = reader.ReadInt16();
							//	x = reader.ReadInt16();
							//	y = reader.ReadInt16();
							//	timer = reader.ReadUInt16();
							//	var objectAddress = reader.ReadUInt16();
							//	var behavior2 = reader.ReadBytes(4);

							//	var color = reader.ReadByte();
							//	end = reader.ReadByte();
							Nodes.Add("86 - 3D Object");
							break;

						case 0x8a:
							// Z Timer (8-bit)
							value = reader.ReadByte();
							Nodes.Add("8a - Z-Timer:" + value);
							break;

						case 0x8c:
							var behavior = reader.ReadUInt16();
							Nodes.Add("8c - Attach Behavior To Previous Object:" + behavior.ToString("X4"));
							break;

						default:
							read = false;
							//System.Diagnostics.Debugger.Break();
							Nodes.Add(entryType.ToString("x2") + " - Unknown Abort");
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