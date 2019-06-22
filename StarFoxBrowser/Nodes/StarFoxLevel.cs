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

				while (read && stream.Position != stream.Length)
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
							var objectIndex = reader.ReadByte();
							var behaviorIndex = reader.ReadByte();
							var behaviorID = Usa10.BehaviorIndexes[behaviorIndex];
							var behaviorName = Usa10.BehaviorNames[behaviorID];
							var behaviorObject = Usa10.BehaviorObjects[behaviorIndex];
							var objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(behaviorObject, out objectName);

							Nodes.Add("00 - 3D Object: " + objectIndex + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behaviorIndex.ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x02:
							// End Level
							Nodes.Add("End Level");
							read = false;
							break;

						case 0x04:
							// Loop Segment
							//reader.ReadBytes(4);
							var loop = reader.ReadUInt16();
							var times = reader.ReadUInt16();

							Nodes.Add("04 - Loop Segment: " + loop + " (Times " + times + ")");
							break;

						case 0x0a:
							//reader.ReadBytes(15);
							z = reader.ReadInt16();
							x = reader.ReadInt16();
							y = reader.ReadInt16();
							timer = reader.ReadUInt16();
							var objectID = reader.ReadUInt16();
							var group = reader.ReadBytes(5);

							Nodes.Add("0a - Random Object: " + objectID.ToString("X4") + " (" + x + ", " + y + ", " + z + ")");
							break;

						case 0x0c:
							reader.ReadBytes(4);
							Nodes.Add("0c - Random Object Data (4 bytes)");
							break;

						case 0x0e:
							// Show Stage Number
							Nodes.Add("0e - Show Stage Number");
							break;

						case 0x10:
							// Initialize Level
							var level = reader.ReadUInt16();
							Nodes.Add("10 - Initialize Level: " + level.ToString("X4"));
							break;

						case 0x12:
							// Z Timer (16-bit)
							reader.ReadUInt16();
							Nodes.Add("12 - Z-Timer");
							break;

						case 0x14:
							// Change Music
							var music = reader.ReadByte();
							Nodes.Add("14 - Change Music: " + music.ToString("X2"));
							break;

						case 0x26:
							reader.ReadBytes(11);
							Nodes.Add("26 - Data");
							break;

						case 0x28:
							// Warp w/ Return
							var warp = reader.ReadBytes(3);
							Nodes.Add("28 - Warp and Return: " + string.Join(string.Empty, warp.Reverse().Select(v => v.ToString("X2"))));
							break;

						case 0x2a:
							// Return From Warp
							Nodes.Add("2a - Return From Warp");
							read = false;
							break;

						case 0x2c:
							// Warp If Condition
							reader.ReadBytes(5);
							//unknown = reader.ReadBytes(10);
							Nodes.Add("2c - Warp If True");
							break;

						case 0x2e:
							// Warp
							warp = reader.ReadBytes(3);
							Nodes.Add("2e - Warp: " + string.Join(string.Empty, warp.Reverse().Select(v => v.ToString("X2"))));
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
							x = reader.ReadByte();
							y = reader.ReadByte();
							z = reader.ReadByte();

							Nodes.Add("3c - Offset Z-Position (" + x + ", " + y + ", " + z + ")");
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

							Nodes.Add("5c - Set SNES RAM (Value: " + value + " Address: " + string.Join(string.Empty, address.Reverse().Select(v => v.ToString("X2"))) + ")");
							break;

						case 0x5e:
							// Screen Transition
							var data = reader.ReadBytes(5);
							Nodes.Add("5e - Screen Transition: " + string.Join(" ", data.Select(v => v.ToString("X2"))));
							break;

						case 0x64:
							// Reset Camera
							value = reader.ReadByte();
							Nodes.Add("64 - Reset Camera: " + value.ToString("X2"));
							break;

						case 0x6a:
							// Unknown
							reader.ReadBytes(5);
							Nodes.Add("6a - Unknown");
							break;

						case 0x6c:
							// Unknown
							Nodes.Add("6c - Unknown");
							break;

						case 0x6e:
							// Unknown
							Nodes.Add("6e - Unknown");
							break;

						case 0x70:
							// 3D Object (8-bit vector) (8-bit lookup table index)
							//reader.ReadBytes(6);
							timer = reader.ReadByte();
							z = reader.ReadByte();
							x = reader.ReadSByte();
							y = reader.ReadSByte();
							objectIndex = reader.ReadByte();
							var behavior = reader.ReadBytes(1);
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							behaviorObject = Usa10.BehaviorObjects[behavior[0]];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(behaviorObject, out objectName);

							Nodes.Add("70 - 3D Object: " + objectIndex + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + string.Join(string.Empty, behavior.Select(v => v.ToString("X2"))));
							break;

						case 0x74:
							// 3D Object Behavior (16-bit vector) (8-bit behavior lookup index)
							//reader.ReadBytes(9);
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = reader.ReadInt16();
							z = reader.ReadInt16();
							behavior = reader.ReadBytes(1);               // TODO: Find lookup table
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							behaviorObject = Usa10.BehaviorObjects[behavior[0]];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(behaviorObject, out objectName);

							Nodes.Add("74 - 3D Object: Behavior Default (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + string.Join(string.Empty, behavior.Select(v => v.ToString("X2"))));
							break;

						case 0x76:
							// 3D Object (8-bit vector)
							//reader.ReadBytes(5);
							z = reader.ReadByte();
							x = reader.ReadSByte();
							y = reader.ReadSByte();
							//timer = reader.ReadUInt16();
							timer = reader.ReadByte();
							behavior = reader.ReadBytes(1);
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							behaviorObject = Usa10.BehaviorObjects[behavior[0]];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(behaviorObject, out objectName);

							Nodes.Add("76 - 3D Object: Behavior Default (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + string.Join(" ", behavior.Reverse().Select(v => v.ToString("X2"))) + " (" + behaviorName + ")");
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
							data = reader.ReadBytes(3);
							Nodes.Add("7a - Transition: " + string.Join(" ", data.Select(d => d.ToString("X2"))));
							break;

						case 0x82:
							// Unknown
							data = reader.ReadBytes(1);
							Nodes.Add("82 - Unknown: " + string.Join(" ", data.Select(d => d.ToString("X2"))));
							break;

						case 0x84:
							// Change Palette
							Nodes.Add("84 - Change Palette");
							break;

						case 0x86:
							// 3D Object (16-bit vector) (16-bit address)
							//reader.ReadBytes(13);
							z = reader.ReadInt16();
							x = reader.ReadInt16();
							y = reader.ReadInt16();
							timer = reader.ReadUInt16();
							objectID = reader.ReadUInt16();
							behavior = reader.ReadBytes(3);

							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(objectID, out objectName);

							behaviorID = behavior[0] | (behavior[1] << 8) | (behavior[2] << 16);
							behaviorName = "Unknown";

							Usa10.BehaviorNames.TryGetValue(behaviorID, out behaviorName);

							Nodes.Add("86 - 3D Object: " + objectID.ToString("X4") + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + string.Join(" ", behavior.Reverse().Select(d => d.ToString("X2"))) + " (" + behaviorName + ")");
							break;

						case 0x8a:
							// Z Timer (8-bit)
							value = reader.ReadByte();
							Nodes.Add("8a - Z-Timer:" + value);
							break;

						case 0x8c:
							behavior = reader.ReadBytes(2);
							Nodes.Add("8c - Attach Behavior To Previous Object:" + string.Join(" ", behavior.Select(d => d.ToString("X2"))));
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