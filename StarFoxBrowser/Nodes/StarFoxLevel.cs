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
					var position = stream.Position;

					position = (((position & 0xff8000) << 1) | (position & 0x007fff)) + 0x8000;

					var entryType = reader.ReadByte();

					switch (entryType)
					{
						case 0x00:
							// 3D Object (16-bit vector) (8-bit lookup table index)
							int timer = reader.ReadUInt16();
							int x = reader.ReadInt16();
							int y = -reader.ReadInt16();
							int z = reader.ReadInt16();
							int objectIndex = reader.ReadByte();
							int behaviorIndex = reader.ReadByte();
							int behaviorID = Usa10.BehaviorIndexes[behaviorIndex];
							var behaviorName = Usa10.BehaviorNames[behaviorID];
							var objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(Usa10.ObjectIndexes[objectIndex], out objectName);

							Nodes.Add(position.ToString("X6") + " 00 - 3D Object: " + objectIndex.ToString("X2") + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behaviorIndex.ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x02:
							// End Level
							Nodes.Add(position.ToString("X6") + " 02 - End Level");
							read = false;
							break;

						case 0x04:
							// Loop Segment
							int loop = reader.ReadUInt16() + 0x8000;
							int times = reader.ReadUInt16();

							Nodes.Add(position.ToString("X6") + " 04 - Loop Segment: " + loop.ToString("X4") + " (Times " + times + ")");
							break;

						case 0x08:
							// No Op
							Nodes.Add(position.ToString("X6") + " 08 - No Op");
							break;

						case 0x0a:
							// Group Object
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = -reader.ReadInt16();
							z = reader.ReadInt16();
							var objectID = reader.ReadUInt16();
							var behaviorAddress = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							var groupAddress = reader.ReadByte() | reader.ReadByte() << 8;
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(objectID, out objectName);

							var group = Nodes.Add(position.ToString("X6") + " 0a - Group Object: " + objectID.ToString("X4") + " " + objectName + " (" + x + ", " + y + ", " + z + ") Timer: " + timer);

							position = stream.Position;

							//stream.Position = groupAddress & 0x058000;
							stream.Position = groupAddress | 0x028000;

							var read2 = true;

							while (read2)
							{
								var position2 = (((stream.Position & 0xff8000) << 1) | (stream.Position & 0x007fff)) + 0x8000;

								entryType = reader.ReadByte();

								switch (entryType)
								{
									case 0x00:
										// Load Object
										var data2 = reader.ReadBytes(2);
										x = reader.ReadInt16();
										y = reader.ReadInt16();
										z = reader.ReadInt16();
										objectID = reader.ReadUInt16();
										behaviorAddress = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
										objectName = "Unknown";
										Usa10.ModelNames.TryGetValue(objectID, out objectName);

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Load Object " + objectID.ToString("X4") + " " + objectName + " (" + x + ", " + y + ", " + z + ")");
										break;

									case 0x02:
										// Unkown
										data2 = reader.ReadBytes(5);

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Unknown");
										break;

									case 0x04:
										// Stop
										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Stop");
										read2 = false;
										break;

									case 0x06:
										// Load Object
										data2 = reader.ReadBytes(2);
										x = reader.ReadInt16();
										y = reader.ReadInt16();
										z = reader.ReadInt16();
										objectID = reader.ReadUInt16();
										behaviorAddress = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
										objectName = "Unknown";
										Usa10.ModelNames.TryGetValue(objectID, out objectName);

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Load Object " + objectID.ToString("X4") + " " + objectName + " (" + x + ", " + y + ", " + z + ")");
										break;

									case 0x08:
										// Jump
										data2 = reader.ReadBytes(2);
										var address2 = reader.ReadUInt16() | 0x058000;

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Jump " + address2.ToString("X4"));
										break;

									case 0x0a:
										// No Op
										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - No Op");
										data2 = reader.ReadBytes(2);
										break;

									case 0x0c:
										// Count
										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Count");
										data2 = reader.ReadBytes(4);
										break;

									case 0x0e:
										// Branch
										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Branch");
										data2 = reader.ReadBytes(7);
										break;

									default:
										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Unknown Abort");
										read2 = false;
										break;
								}
							}

							stream.Position = position;
							break;

						case 0x0c:
							// Random Object Data
							int address = reader.ReadInt32();
							Nodes.Add(position.ToString("X6") + " 0c - Random Object Data (" + address.ToString("X8") + ")");
							break;

						case 0x0e:
							// Show Stage Number
							Nodes.Add(position.ToString("X6") + " 0e - Show Stage Number");
							break;

						case 0x10:
							// Initialize Level
							var level = reader.ReadUInt16();
							Nodes.Add(position.ToString("X6") + " 10 - Initialize Level: " + level.ToString("X4"));
							break;

						case 0x12:
							// Z Timer (16-bit)
							timer = reader.ReadUInt16();
							Nodes.Add(position.ToString("X6") + " 12 - Z-Timer: " + timer);
							break;

						case 0x14:
							// Change Music
							var music = reader.ReadByte();
							Nodes.Add(position.ToString("X6") + " 14 - Change Music: " + music.ToString("X2"));
							break;

						case 0x16:
							// Disable Surface Point Grid
							Nodes.Add(position.ToString("X6") + " 16 - Disable Surface Point Grid");
							break;

						case 0x18:
							// Enable Surface Point Grid
							Nodes.Add(position.ToString("X6") + " 18 - Enable Surface Point Grid");
							break;

						case 0x1A:
							// Enable Point Stars
							Nodes.Add(position.ToString("X6") + " 1A - Enable Point Stars");
							break;

						case 0x1C:
							// Unknown
							var unknown = reader.ReadByte();
							Nodes.Add(position.ToString("X6") + " 1C - Unknown " + unknown.ToString("X2"));
							break;

						case 0x1E:
							// Enable Background Rotation
							Nodes.Add(position.ToString("X6") + " 1E - Enable Background Rotation");
							break;

						case 0x20:
							// Disable Background Rotation
							Nodes.Add(position.ToString("X6") + " 20 - Disable Background Rotation");
							break;

						case 0x22:
							// Unknown
							Nodes.Add(position.ToString("X6") + " 22 - Unknown");
							break;

						case 0x24:
							// Unknown
							Nodes.Add(position.ToString("X6") + " 24 - Unknown");
							break;

						case 0x26:
							// 3D Object With Rotation
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = -reader.ReadInt16();
							z = reader.ReadInt16();
							objectIndex = reader.ReadByte();
							behaviorIndex = reader.ReadByte();
							var rotationZ = reader.ReadByte();
							behaviorID = Usa10.BehaviorIndexes[behaviorIndex];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(Usa10.ObjectIndexes[objectIndex], out objectName);

							Nodes.Add(position.ToString("X6") + " 26 - 3D Object: " + objectIndex + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Rotation: " + rotationZ + " Timer: " + timer + " Behavior: " + behaviorIndex.ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x28:
							// Warp w/ Return
							var warp = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16 | 0x8000;
							Nodes.Add(position.ToString("X6") + " 28 - Warp and Return: " + warp.ToString("X6"));
							break;

						case 0x2a:
							// Return From Warp
							Nodes.Add(position.ToString("X6") + " 2a - Return From Warp");
							read = false;
							break;

						case 0x2c:
							// Conditional Warp
							int condition = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);

							address = reader.ReadUInt16();

							Nodes.Add(position.ToString("X6") + " 2c - Conditional Warp: " + address.ToString("X4") + " Condition: " + condition.ToString("X6"));
							break;

						case 0x2e:
							// Warp
							warp = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16) | 0x8000;
							Nodes.Add(position.ToString("X6") + " 2e - Warp: " + warp.ToString("X6"));
							read = false;
							break;

						case 0x30:
							// Set X Rotation
							x = reader.ReadByte();

							Nodes.Add(position.ToString("X6") + " 30 - Set X Rotation: " + x);
							break;

						case 0x32:
							// Set Y Rotation
							y = reader.ReadByte();

							Nodes.Add(position.ToString("X6") + " 32 - Set Y Rotation: " + y);
							break;

						case 0x34:
							// Set Z Rotation
							z = reader.ReadByte();

							Nodes.Add(position.ToString("X6") + " 34 - Set Z Rotation: " + z);
							break;

						case 0x36:
							// Set Byte Property
							var offset = reader.ReadUInt16();
							int value = reader.ReadSByte();
							Nodes.Add(position.ToString("X6") + " 36 - Set Byte Property: Offset " + offset.ToString("X4") + " Value: " + value);
							break;

						case 0x38:
							// Set Short Property
							offset = reader.ReadUInt16();
							value = reader.ReadByte() | reader.ReadByte() << 8;
							Nodes.Add(position.ToString("X6") + " 36 - Set Short Property: Offset " + offset.ToString("X4") + " Value: " + value.ToString("X4"));
							break;

						case 0x3a:
							// Set Long Property
							offset = reader.ReadUInt16();
							value = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							Nodes.Add(position.ToString("X6") + " 36 - Set Long Property: Offset " + offset.ToString("X4") + " Value: " + value.ToString("X6"));
							break;

						case 0x3c:
							// Set Byte Extended Property
							offset = reader.ReadUInt16();
							value = reader.ReadByte();
							Nodes.Add(position.ToString("X6") + " 36 - Set Byte Extended Property: Offset " + offset.ToString("X4") + " Value: " + value);
							break;

						case 0x3e:
							// Set Short Extended Property
							offset = reader.ReadUInt16();
							value = reader.ReadByte() | (reader.ReadByte() << 8);
							Nodes.Add(position.ToString("X6") + " 36 - Set Short Extended Property: Offset " + offset.ToString("X4") + " Value: " + value.ToString("X4"));
							break;

						case 0x40:
							// Set Long Extended Property
							offset = reader.ReadUInt16();
							value = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							Nodes.Add(position.ToString("X6") + " 36 - Set Long Extended Property: Offset " + offset.ToString("X4") + " Value: " + value.ToString("X6"));
							break;

						case 0x42:
							// Fade From Black (Slow)
							Nodes.Add(position.ToString("X6") + " 42 - Fade From Black (Slow)");
							break;

						case 0x44:
							// Fade To Black (Slow)
							Nodes.Add(position.ToString("X6") + " 44 - Fade To Black (Slow)");
							break;

						case 0x46:
							reader.ReadBytes(6);
							Nodes.Add(position.ToString("X6") + " 46 - Unknown");
							break;

						case 0x48:
							reader.ReadBytes(5);
							Nodes.Add(position.ToString("X6") + " 48 - Move Previous Object By Parent Coordinates");
							break;

						case 0x4a:
							reader.ReadBytes(3);
							Nodes.Add(position.ToString("X6") + " 4a - Unknown");
							break;

						case 0x4c:
							Nodes.Add(position.ToString("X6") + " 4c - Unknown");
							break;

						case 0x4e:
							Nodes.Add(position.ToString("X6") + " 4e - Fade From Black (Fast)");
							break;

						case 0x50:
							Nodes.Add(position.ToString("X6") + " 50 - Fade To Black (Fast)");
							break;

						case 0x52:
							Nodes.Add(position.ToString("X6") + " 52 - Disable Screen");
							break;

						case 0x54:
							Nodes.Add(position.ToString("X6") + " 54 - Enable Screen");
							break;

						case 0x56:
							Nodes.Add(position.ToString("X6") + " 56 - Disable Foreground Rotation");
							break;

						case 0x58:
							Nodes.Add(position.ToString("X6") + " 58 - Enable Foreground Rotation");
							break;

						case 0x5a:
							Nodes.Add(position.ToString("X6") + " 5a - Palette Change Previous Object");
							break;

						case 0x5c:
							// Set SNES RAM Byte
							value = reader.ReadByte();
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add(position.ToString("X6") + " 5c - Set SNES RAM Byte (Value: " + value + " Address: " + address.ToString("X6")  + ")");
							break;

						case 0x5e:
							// Set SNES RAM Short
							value = reader.ReadByte() | reader.ReadByte() << 8;
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add(position.ToString("X6") + " 5c - Set SNES RAM Short (Value: " + value.ToString("X4") + " Address: " + address.ToString("X6") + ")");
							break;

						case 0x60:
							// Set SNES RAM Long
							value = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add(position.ToString("X6") + " 5c - Set SNES RAM Short (Value: " + value.ToString("X6") + " Address: " + address.ToString("X6") + ")");
							break;

						case 0x62:
							// Unknown
							value = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add(position.ToString("X6") + " 62 - Unknown");
							break;

						case 0x64:
							// Load Preset
							value = reader.ReadByte();
							Nodes.Add(position.ToString("X6") + " 64 - Load Preset: " + value.ToString("X2"));
							break;

						case 0x66:
							// Initialize Preset
							value = reader.ReadByte();
							Nodes.Add(position.ToString("X6") + " 66 - Initialize Preset: " + value.ToString("X2"));
							break;

						case 0x68:
							// Unknown
							reader.ReadBytes(5);
							Nodes.Add(position.ToString("X6") + " 68 - Unknown");
							break;

						case 0x6a:
							// Unknown
							reader.ReadBytes(5);
							Nodes.Add(position.ToString("X6") + " 6a - Unknown");
							break;

						case 0x6c:
							// Unknown
							Nodes.Add(position.ToString("X6") + " 6c - Unknown");
							break;

						case 0x6e:
							// Unknown
							Nodes.Add(position.ToString("X6") + " 6e - Unknown");
							break;

						case 0x70:
							// 3D Object (8-bit vector) (8-bit lookup table index)
							timer = reader.ReadByte() * 16;
							x = reader.ReadSByte() * 4;
							y = -reader.ReadSByte() * 4;
							z = reader.ReadByte() * 16;
							objectIndex = reader.ReadByte();
							var behavior = reader.ReadBytes(1);
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(Usa10.ObjectIndexes[objectIndex], out objectName);

							Nodes.Add(position.ToString("X6") + " 70 - 3D Object: " + objectIndex.ToString("X2") + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behavior[0].ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x72:
							// 3D Object (8-bit vector) (8-bit lookup table index)
							timer = reader.ReadByte() * 16;
							x = reader.ReadSByte() * 4;
							y = -reader.ReadSByte() * 4;
							z = reader.ReadByte() * 16;
							behaviorID = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							behaviorName = Usa10.BehaviorNames[behaviorID];

							Nodes.Add(position.ToString("X6") + " 72 - 3D Object: No Model (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behaviorID.ToString("X6") + " (" + behaviorName + ")");
							break;

						case 0x74:
							// 3D Object Behavior (16-bit vector) (8-bit behavior lookup index)
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = -reader.ReadInt16();
							z = reader.ReadInt16();
							behavior = reader.ReadBytes(1);               // TODO: Find lookup table
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							objectIndex = Usa10.BehaviorObjects[behavior[0]];
							var behaviorObject = Usa10.ObjectIndexes[objectIndex];
							objectName = "None";
							Usa10.ModelNames.TryGetValue(behaviorObject, out objectName);

							Nodes.Add(position.ToString("X6") + " 74 - 3D Object: Behavior Default (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behavior[0].ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x76:
							// 3D Object (8-bit vector)
							timer = reader.ReadByte() * 16;
							x = reader.ReadSByte() * 4;
							y = -reader.ReadSByte() * 4;
							z = reader.ReadByte() * 4;
							behavior = reader.ReadBytes(1);
							behaviorID = Usa10.BehaviorIndexes[behavior[0]];
							behaviorName = Usa10.BehaviorNames[behaviorID];
							objectIndex = Usa10.BehaviorObjects[behavior[0]];
							behaviorObject = Usa10.ObjectIndexes[objectIndex];
							objectName = "None";
							Usa10.ModelNames.TryGetValue(behaviorObject, out objectName);

							Nodes.Add(position.ToString("X6") + " 76 - 3D Object: Behavior Default (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behavior[0].ToString("X2") + " (" + behaviorName + ")");
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

							Nodes.Add(position.ToString("X6") + " 78 - Native Code (" + length + " bytes)");

							break;

						case 0x7a:
							address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							Nodes.Add(position.ToString("X6") + " 7a - Transition: " + address.ToString("X6"));
							break;

						case 0x82:
							// Unknown
							var data = reader.ReadBytes(1);
							Nodes.Add(position.ToString("X6") + " 82 - Unknown: " + string.Join(" ", data.Select(d => d.ToString("X2"))));
							break;

						case 0x84:
							// Change Palette
							Nodes.Add(position.ToString("X6") + " 84 - Change Palette");
							break;

						case 0x86:
							// 3D Object (16-bit vector) (16-bit address)
							//reader.ReadBytes(13);
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = -reader.ReadInt16();
							z = reader.ReadInt16();
							objectID = reader.ReadUInt16();
							behavior = reader.ReadBytes(3);

							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(objectID, out objectName);

							behaviorID = behavior[0] | (behavior[1] << 8) | (behavior[2] << 16);
							behaviorName = "Unknown";

							Usa10.BehaviorNames.TryGetValue(behaviorID, out behaviorName);

							Nodes.Add(position.ToString("X6") + " 86 - 3D Object: " + objectID.ToString("X4") + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + string.Join(" ", behavior.Reverse().Select(d => d.ToString("X2"))) + " (" + behaviorName + ")");
							break;

						case 0x88:
							// Change Music
							value = reader.ReadByte();
							Nodes.Add(position.ToString("X6") + " 88 - Set Music:" + value);
							break;

						case 0x8a:
							// Z Timer (8-bit)
							value = reader.ReadByte() << 4;
							Nodes.Add(position.ToString("X6") + " 8a - Z-Timer: " + value);
							break;

						case 0x8c:
							// Set Event Property
							value = reader.ReadByte() | reader.ReadByte() << 8;

							Nodes.Add(position.ToString("X6") + " 8c - Set Event Property:" + value.ToString("X4"));
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