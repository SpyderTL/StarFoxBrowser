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
							int timer = reader.ReadUInt16();
							var x = reader.ReadInt16();
							var y = reader.ReadInt16();
							var z = reader.ReadInt16();
							var objectIndex = reader.ReadByte();
							var behaviorIndex = reader.ReadByte();
							var behaviorID = Usa10.BehaviorIndexes[behaviorIndex];
							var behaviorName = Usa10.BehaviorNames[behaviorID];
							var objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(Usa10.ObjectIndexes[objectIndex], out objectName);

							Nodes.Add("00 - 3D Object: " + objectIndex + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behaviorIndex.ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x02:
							// End Level
							Nodes.Add("02 - End Level");
							read = false;
							break;

						case 0x04:
							// Loop Segment
							var loop = reader.ReadUInt16();
							var times = reader.ReadUInt16();

							Nodes.Add("04 - Loop Segment: " + loop + " (Times " + times + ")");
							break;

						case 0x08:
							// No Op
							Nodes.Add("08 - No Op");
							break;

						case 0x0a:
							// Group Object
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = reader.ReadInt16();
							z = reader.ReadInt16();
							var objectID = reader.ReadUInt16();
							var behaviorAddress = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							var groupAddress = reader.ReadByte() | reader.ReadByte() << 8;
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(objectID, out objectName);

							Nodes.Add("0a - Group Object: " + objectID.ToString("X4") + " " + objectName + " (" + x + ", " + y + ", " + z + ")");
							break;

						case 0x0c:
							// Random Object Data
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
							timer = reader.ReadUInt16();
							Nodes.Add("12 - Z-Timer: " + timer);
							break;

						case 0x14:
							// Change Music
							var music = reader.ReadByte();
							Nodes.Add("14 - Change Music: " + music.ToString("X2"));
							break;

						case 0x16:
							// Disable Surface Point Grid
							Nodes.Add("16 - Disable Surface Point Grid");
							break;

						case 0x18:
							// Enable Surface Point Grid
							Nodes.Add("18 - Enable Surface Point Grid");
							break;

						case 0x1A:
							// Enable Point Stars
							Nodes.Add("1A - Enable Point Stars");
							break;

						case 0x1C:
							// Unknown
							var unknown = reader.ReadByte();
							Nodes.Add("1C - Unknown " + unknown.ToString("X2"));
							break;

						case 0x1E:
							// Enable Background Rotation
							Nodes.Add("1E - Enable Background Rotation");
							break;

						case 0x20:
							// Disable Background Rotation
							Nodes.Add("20 - Disable Background Rotation");
							break;

						case 0x22:
							// Unknown
							Nodes.Add("22 - Unknown");
							break;

						case 0x24:
							// Unknown
							Nodes.Add("24 - Unknown");
							break;

						case 0x26:
							// 3D Object With Rotation
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = reader.ReadInt16();
							z = reader.ReadInt16();
							objectIndex = reader.ReadByte();
							behaviorIndex = reader.ReadByte();
							var rotationZ = reader.ReadByte();
							behaviorID = Usa10.BehaviorIndexes[behaviorIndex];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(Usa10.ObjectIndexes[objectIndex], out objectName);

							Nodes.Add("26 - 3D Object: " + objectIndex + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Rotation: " + rotationZ + " Timer: " + timer + " Behavior: " + behaviorIndex.ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x28:
							// Warp w/ Return
							var warp = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							Nodes.Add("28 - Warp and Return: " + warp.ToString("X6"));
							break;

						case 0x2a:
							// Return From Warp
							Nodes.Add("2a - Return From Warp");
							read = false;
							break;

						case 0x2c:
							// Call Native Function
							var address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);

							var condition = reader.ReadUInt16();

							Nodes.Add("2c - Conditional Warp: " + address.ToString("X6") + " Condition: " + condition.ToString("X4"));
							break;

						case 0x2e:
							// Warp
							warp = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							Nodes.Add("2e - Warp: " + warp.ToString("X6"));
							read = false;
							break;

						case 0x30:
							// Set X Rotation
							x = reader.ReadByte();

							Nodes.Add("30 - Set X Rotation: " + x);
							break;

						case 0x32:
							// Set Y Rotation
							y = reader.ReadByte();

							Nodes.Add("32 - Set Y Rotation: " + y);
							break;

						case 0x34:
							// Set Z Rotation
							z = reader.ReadByte();

							Nodes.Add("34 - Set Z Rotation: " + z);
							break;

						case 0x36:
							// Set Byte Property
							var offset = reader.ReadUInt16();
							var value = reader.ReadByte();
							Nodes.Add("36 - Set Byte Property: Offset " + offset.ToString("X4") + " Value: " + value);
							break;

						case 0x38:
							// Set Short Property
							offset = reader.ReadUInt16();
							var value2 = reader.ReadByte() | reader.ReadByte() << 8;
							Nodes.Add("36 - Set Short Property: Offset " + offset.ToString("X4") + " Value: " + value2.ToString("X4"));
							break;

						case 0x3a:
							// Set Long Property
							offset = reader.ReadUInt16();
							var value3 = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							Nodes.Add("36 - Set Long Property: Offset " + offset.ToString("X4") + " Value: " + value3.ToString("X6"));
							break;

						case 0x3c:
							// Set Byte Extended Property
							offset = reader.ReadUInt16();
							value = reader.ReadByte();
							Nodes.Add("36 - Set Byte Extended Property: Offset " + offset.ToString("X4") + " Value: " + value);
							break;

						case 0x3e:
							// Set Short Extended Property
							offset = reader.ReadUInt16();
							value2 = reader.ReadByte() | (reader.ReadByte() << 8);
							Nodes.Add("36 - Set Short Extended Property: Offset " + offset.ToString("X4") + " Value: " + value2.ToString("X4"));
							break;

						case 0x40:
							// Set Long Extended Property
							offset = reader.ReadUInt16();
							value3 = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							Nodes.Add("36 - Set Long Extended Property: Offset " + offset.ToString("X4") + " Value: " + value3.ToString("X6"));
							break;

						case 0x42:
							// Fade From Black (Slow)
							Nodes.Add("42 - Fade From Black (Slow)");
							break;

						case 0x44:
							// Fade To Black (Slow)
							Nodes.Add("44 - Fade To Black (Slow)");
							break;

						case 0x46:
							reader.ReadBytes(6);
							Nodes.Add("46 - Unknown");
							break;

						case 0x48:
							reader.ReadBytes(5);
							Nodes.Add("48 - Move Previous Object By Parent Coordinates");
							break;

						case 0x4a:
							reader.ReadBytes(3);
							Nodes.Add("4a - Unknown");
							break;

						case 0x4c:
							Nodes.Add("4c - Unknown");
							break;

						case 0x4e:
							Nodes.Add("4e - Fade From Black (Fast)");
							break;

						case 0x50:
							Nodes.Add("50 - Fade To Black (Fast)");
							break;

						case 0x52:
							Nodes.Add("52 - Disable Screen");
							break;

						case 0x54:
							Nodes.Add("54 - Enable Screen");
							break;

						case 0x56:
							Nodes.Add("56 - Disable Foreground Rotation");
							break;

						case 0x58:
							Nodes.Add("58 - Enable Foreground Rotation");
							break;

						case 0x5a:
							Nodes.Add("5a - Palette Change Previous Object");
							break;

						case 0x5c:
							// Set SNES RAM Byte
							value = reader.ReadByte();
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add("5c - Set SNES RAM Byte (Value: " + value + " Address: " + address.ToString("X6")  + ")");
							break;

						case 0x5e:
							// Set SNES RAM Short
							value2 = reader.ReadByte() | reader.ReadByte() << 8;
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add("5c - Set SNES RAM Short (Value: " + value2.ToString("X4") + " Address: " + address.ToString("X6") + ")");
							break;

						case 0x60:
							// Set SNES RAM Long
							value3 = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add("5c - Set SNES RAM Short (Value: " + value3.ToString("X6") + " Address: " + address.ToString("X6") + ")");
							break;

						case 0x62:
							// Unknown
							value3 = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add("62 - Unknown");
							break;

						case 0x64:
							// Load Preset
							value = reader.ReadByte();
							Nodes.Add("64 - Load Preset: " + value.ToString("X2"));
							break;

						case 0x66:
							// Initialize Preset
							value = reader.ReadByte();
							Nodes.Add("66 - Initialize Preset: " + value.ToString("X2"));
							break;

						case 0x68:
							// Unknown
							reader.ReadBytes(5);
							Nodes.Add("68 - Unknown");
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
							timer = reader.ReadByte() << 4;
							z = reader.ReadByte();
							x = reader.ReadSByte();
							y = reader.ReadSByte();
							objectIndex = reader.ReadByte();
							var behavior = reader.ReadBytes(1);
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							objectIndex = Usa10.BehaviorObjects[behavior[0]];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(Usa10.ObjectIndexes[objectIndex], out objectName);

							Nodes.Add("70 - 3D Object: " + objectIndex + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behavior[0].ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x74:
							// 3D Object Behavior (16-bit vector) (8-bit behavior lookup index)
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = reader.ReadInt16();
							z = reader.ReadInt16();
							behavior = reader.ReadBytes(1);               // TODO: Find lookup table
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							objectIndex = Usa10.BehaviorObjects[behavior[0]];
							var behaviorObject = Usa10.ObjectIndexes[objectIndex];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(behaviorObject, out objectName);

							Nodes.Add("74 - 3D Object: Behavior Default (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behavior[0].ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x76:
							// 3D Object (8-bit vector)
							timer = reader.ReadByte() << 4;
							x = reader.ReadSByte();
							y = reader.ReadSByte();
							z = reader.ReadByte();
							behavior = reader.ReadBytes(1);
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							objectIndex = Usa10.BehaviorObjects[behavior[0]];
							behaviorObject = Usa10.ObjectIndexes[objectIndex];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(behaviorObject, out objectName);

							Nodes.Add("76 - 3D Object: Behavior Default (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behavior[0].ToString("X2") + " (" + behaviorName + ")");
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
							address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							Nodes.Add("7a - Transition: " + address.ToString("X6"));
							break;

						case 0x82:
							// Unknown
							var data = reader.ReadBytes(1);
							Nodes.Add("82 - Unknown: " + string.Join(" ", data.Select(d => d.ToString("X2"))));
							break;

						case 0x84:
							// Change Palette
							Nodes.Add("84 - Change Palette");
							break;

						case 0x86:
							// 3D Object (16-bit vector) (16-bit address)
							//reader.ReadBytes(13);
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = reader.ReadInt16();
							z = reader.ReadInt16();
							objectID = reader.ReadUInt16();
							behavior = reader.ReadBytes(3);

							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(objectID, out objectName);

							behaviorID = behavior[0] | (behavior[1] << 8) | (behavior[2] << 16);
							behaviorName = "Unknown";

							Usa10.BehaviorNames.TryGetValue(behaviorID, out behaviorName);

							Nodes.Add("86 - 3D Object: " + objectID.ToString("X4") + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + string.Join(" ", behavior.Reverse().Select(d => d.ToString("X2"))) + " (" + behaviorName + ")");
							break;

						case 0x88:
							// Change Music
							value = reader.ReadByte();
							Nodes.Add("88 - Set Music:" + value);
							break;

						case 0x8a:
							// Z Timer (8-bit)
							value2 = reader.ReadByte() << 4;
							Nodes.Add("8a - Z-Timer: " + value2);
							break;

						case 0x8c:
							// Set Event
							value2 = reader.ReadByte() | reader.ReadByte() << 8;
							Nodes.Add("8c - Attach Behavior To Previous Object:" + value2.ToString("X4"));
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