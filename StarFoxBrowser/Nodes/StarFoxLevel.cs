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

					int value;
					int address;
					byte condition;

					switch (entryType)
					{
						case 0x00:
							// 3D Object (16-bit vector) (8-bit lookup table index) (OBJ)
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

						case 0x06:
							// DEB
							Nodes.Add(position.ToString("X6") + " 06 - DEB??");
							break;

						case 0x08:
							// No Op
							Nodes.Add(position.ToString("X6") + " 08 - No Op");
							break;

						case 0x0a:
							// Mother Object
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = -reader.ReadInt16();
							z = reader.ReadInt16();
							var objectID = reader.ReadUInt16();
							var behaviorAddress = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							var groupAddress = reader.ReadByte() | reader.ReadByte() << 8;
							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(objectID, out objectName);

							var groupAddress2 = groupAddress | 0x058000;

							var group = Nodes.Add(position.ToString("X6") + " 0a - Mother Object: " + objectID.ToString("X4") + " " + objectName + " (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Group: " + groupAddress2.ToString("X6") + " Behavior: " + behaviorAddress.ToString("X6"));

							position = stream.Position;

							//stream.Position = groupAddress | 0x058000;
							stream.Position = groupAddress | 0x028000;

							var read2 = true;

							while (read2)
							{
								var position2 = (((stream.Position & 0xff8000) << 1) | (stream.Position & 0x007fff)) + 0x8000;

								entryType = reader.ReadByte();

								switch (entryType)
								{
									case 0x00:
										// Child Object
										timer = reader.ReadUInt16();
										x = reader.ReadInt16();
										y = reader.ReadInt16();
										z = reader.ReadInt16();
										objectID = reader.ReadUInt16();
										behaviorAddress = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
										objectName = "Unknown";
										Usa10.ModelNames.TryGetValue(objectID, out objectName);

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Child Object " + objectID.ToString("X4") + " " + objectName + " (" + x + ", " + y + ", " + z + ") Timer: " + timer.ToString("X4"));
										break;

									case 0x02:
										// Loop
										timer = reader.ReadUInt16();
										address = reader.ReadInt16();
										times = reader.ReadByte();

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Loop: " + address.ToString("X4") + " Times: " + times + " Timer: " + timer.ToString("X4"));
										break;

									case 0x04:
										// Stop
										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Stop");
										read2 = false;
										break;

									case 0x06:
										// Child Object (Random Position)
										timer = reader.ReadUInt16();
										x = reader.ReadInt16();					// X-Position Mask
										y = reader.ReadInt16();					// Y-Position Mask
										z = reader.ReadInt16();					// Z-Position Mask
										objectID = reader.ReadUInt16();
										behaviorAddress = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
										objectName = "Unknown";
										Usa10.ModelNames.TryGetValue(objectID, out objectName);

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Child Object " + objectID.ToString("X4") + " " + objectName + " Random(+/-" + x + ", +/-" + y + ", +/-" + z + ") Timer: " + timer.ToString("X4"));
										break;

									case 0x08:
										// Goto
										timer = reader.ReadUInt16();
										var address2 = reader.ReadUInt16() | 0x058000;

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Goto " + address2.ToString("X4") + " Timer: " + timer.ToString("X4"));
										read2 = false;
										break;

									case 0x0a:
										// Wait
										timer = reader.ReadUInt16();
										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Wait Timer: " + timer.ToString("X4"));
										break;

									case 0x0c:
										// Count
										timer = reader.ReadUInt16();
										objectID = reader.ReadUInt16();
										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Count Objects: " + objectID + " Timer: " + timer.ToString("X4"));
										break;

									case 0x0e:
										// Branch
										timer = reader.ReadUInt16();
										value = reader.ReadInt16();
										address = reader.ReadInt16();
										condition = reader.ReadByte();

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Branch: " + address.ToString("X4") + " Value: " + value + " Condition: " + condition + " Timer: " + timer.ToString("X4"));
										break;

									case 0x10:
										// Set
										timer = reader.ReadUInt16();
										var type = reader.ReadByte();
										address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
										value = reader.ReadInt16();

										group.Nodes.Add(position2.ToString("X6") + " " + entryType.ToString("X2") + " - Set: Type: " + type + " Address: " + address.ToString("X6") + " Value: " + value.ToString("X4") + " Timer: " + timer.ToString("X4"));
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
							// Remove Object
							timer = reader.ReadUInt16();
							objectID = reader.ReadUInt16();
							Nodes.Add(position.ToString("X6") + " 0c - Remove Object: " + objectID.ToString("X4") + " Timer: " + timer.ToString("X4"));
							break;

						case 0x0e:
							// Set Stage
							Nodes.Add(position.ToString("X6") + " 0e - Set Stage");
							break;

						case 0x10:
							// Set Background
							var level = reader.ReadUInt16();
							Nodes.Add(position.ToString("X6") + " 10 - Set Background: " + level.ToString("X4"));
							break;

						case 0x12:
							// Wait (16-bit)
							timer = reader.ReadUInt16();
							Nodes.Add(position.ToString("X6") + " 12 - Wait: " + timer);
							break;

						case 0x14:
							// Set Background Music
							var music = reader.ReadByte();
							Nodes.Add(position.ToString("X6") + " 14 - Set Background Music: " + music.ToString("X2"));
							break;

						case 0x16:
							// Disable Ground Dots
							Nodes.Add(position.ToString("X6") + " 16 - Disable Ground Dots");
							break;

						case 0x18:
							// Enable Ground Dots
							Nodes.Add(position.ToString("X6") + " 18 - Enable Ground Dots");
							break;

						case 0x1A:
							// Space Dust
							Nodes.Add(position.ToString("X6") + " 1A - Space Dust");
							break;

						case 0x1C:
							// Set Other Music
							var unknown = reader.ReadByte();
							Nodes.Add(position.ToString("X6") + " 1C - Set Other Music: " + unknown.ToString("X2"));
							break;

						case 0x1E:
							// Enable Background Vertical Rotation
							Nodes.Add(position.ToString("X6") + " 1E - Enable Background Vertical Rotation");
							break;

						case 0x20:
							// Disable Background Vertical Rotation
							Nodes.Add(position.ToString("X6") + " 20 - Disable Background Vertical Rotation");
							break;

						case 0x22:
							// Enable Background Horizontal Rotation
							Nodes.Add(position.ToString("X6") + " 22 - Enable Background Hoizontal Rotation");
							break;

						case 0x24:
							// Disable Background Horizontal Rotation
							Nodes.Add(position.ToString("X6") + " 24 - Disable Background Horizontal Rotation");
							break;

						case 0x26:
							// 3D Object With Z Rotation
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

							Nodes.Add(position.ToString("X6") + " 26 - 3D Object w/ Z Rotation: " + objectIndex + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Rotation: " + rotationZ + " Timer: " + timer + " Behavior: " + behaviorIndex.ToString("X2") + " (" + behaviorName + ")");
							break;

						case 0x28:
							// Call Subroutine
							var warp = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16 | 0x8000;
							Nodes.Add(position.ToString("X6") + " 28 - Call Subroutine: " + warp.ToString("X6"));
							break;

						case 0x2a:
							// Return From Subroiutine
							Nodes.Add(position.ToString("X6") + " 2a - Return From Subroutine");
							read = false;
							break;

						case 0x2c:
							// Branch
							value = reader.ReadByte() | (reader.ReadByte() << 8);
							condition = reader.ReadByte();
							address = reader.ReadUInt16();

							Nodes.Add(position.ToString("X6") + " 2c - Branch: " + address.ToString("X4") + " Value: " + value  + " Condition: " + condition.ToString("X4"));
							break;

						case 0x2e:
							// GOTO
							warp = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16) | 0x8000;
							Nodes.Add(position.ToString("X6") + " 2e - Goto: " + warp.ToString("X6"));
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
							value = reader.ReadSByte();
							Nodes.Add(position.ToString("X6") + " 36 - Set Byte Property: Offset " + offset.ToString("X4") + " Value: " + value);
							break;

						case 0x38:
							// Set Short Property
							offset = reader.ReadUInt16();
							value = reader.ReadInt16();
							Nodes.Add(position.ToString("X6") + " 36 - Set Short Property: Offset " + offset.ToString("X4") + " Value: " + value);
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
							value = reader.ReadInt16();
							Nodes.Add(position.ToString("X6") + " 36 - Set Short Extended Property: Offset " + offset.ToString("X4") + " Value: " + value);
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
							// SET AL VAR PTR BYTE
							reader.ReadBytes(6);
							Nodes.Add(position.ToString("X6") + " 46 - SET AL VAR PTR BYTE");
							break;

						case 0x48:
							// SET AL VAR PTR WORD
							reader.ReadBytes(5);
							Nodes.Add(position.ToString("X6") + " 48 - SET AL VAR PTR WORD");
							break;

						case 0x4a:
							// SET VAR OBJ
							reader.ReadBytes(3);
							Nodes.Add(position.ToString("X6") + " 4a - Set Var Obj");
							break;

						case 0x4c:
							// Wait For Fade
							Nodes.Add(position.ToString("X6") + " 4c - Wait For Fade");
							break;

						case 0x4e:
							// Set Q Fade Up
							Nodes.Add(position.ToString("X6") + " 4e - Fade From Black (Fast)");
							break;

						case 0x50:
							// Set Q Fade Down
							Nodes.Add(position.ToString("X6") + " 50 - Fade To Black (Fast)");
							break;

						case 0x52:
							Nodes.Add(position.ToString("X6") + " 52 - Disable Screen");
							break;

						case 0x54:
							Nodes.Add(position.ToString("X6") + " 54 - Enable Screen");
							break;

						case 0x56:
							Nodes.Add(position.ToString("X6") + " 56 - Disable Z Rotation");
							break;

						case 0x58:
							Nodes.Add(position.ToString("X6") + " 58 - Enable Z Rotation");
							break;

						case 0x5a:
							// Map Special
							Nodes.Add(position.ToString("X6") + " 5a - Map Special");
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

							Nodes.Add(position.ToString("X6") + " 5c - Set SNES RAM Long (Value: " + value.ToString("X6") + " Address: " + address.ToString("X6") + ")");
							break;

						case 0x62:
							// Set Background Slow (check fields)
							value = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;
							address = reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16;

							Nodes.Add(position.ToString("X6") + " 62 - Set Background Slow: " + value.ToString("X6") + " " + address.ToString("X6"));
							break;

						case 0x64:
							// Wait Set Background
							Nodes.Add(position.ToString("X6") + " 64 - Wait For Set Background");
							break;

						case 0x66:
							// Set Background Info
							Nodes.Add(position.ToString("X6") + " 66 - Set Background Info");
							break;

						case 0x68:
							// ADD AL VAR PTR BYTE
							reader.ReadBytes(5);
							Nodes.Add(position.ToString("X6") + " 68 - ADD AL VAR PTR BYTE");
							break;

						case 0x6a:
							// ADD AL VAR PTR WORD
							reader.ReadBytes(5);
							Nodes.Add(position.ToString("X6") + " 6a - ADD AL VAR PTR WORD");
							break;

						case 0x6c:
							// Fade To Sea
							Nodes.Add(position.ToString("X6") + " 6c - Fade To Sea");
							break;

						case 0x6e:
							// Fade To Ground
							Nodes.Add(position.ToString("X6") + " 6e - Fade To Ground");
							break;

						case 0x70:
							// 3D Object (8-bit vector) (8-bit lookup table index) (Q OBJ)
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
							// 3D Object (8-bit vector) (8-bit lookup table index) (OBJ 8)
							timer = reader.ReadByte() * 16;
							x = reader.ReadSByte() * 4;
							y = -reader.ReadSByte() * 4;
							z = reader.ReadByte() * 16;
							behaviorID = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							behaviorName = Usa10.BehaviorNames[behaviorID];

							Nodes.Add(position.ToString("X6") + " 72 - 3D Object: No Model (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behaviorID.ToString("X6") + " (" + behaviorName + ")");
							break;

						case 0x74:
							// 3D Object Behavior (16-bit vector) (8-bit behavior lookup index) (D OBJ)
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
							// 3D Object (8-bit vector) (Q OBJ 2)
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
							// Call Function (CODE JSL)
							address = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
							Nodes.Add(position.ToString("X6") + " 7a - Call Function: " + address.ToString("X6"));
							break;

						case 0x7c:
							// JMP VAR LESS
							break;

						case 0x7e:
							// JMP VAR GREATER
							break;

						case 0x80:
							// JMP VAR EQ
							break;

						case 0x82:
							// Send Message
							var data = reader.ReadBytes(1);
							Nodes.Add(position.ToString("X6") + " 82 - SendMessage: " + string.Join(" ", data.Select(d => d.ToString("X2"))));
							break;

						case 0x84:
							// Special Color
							Nodes.Add(position.ToString("X6") + " 84 - Special Color");
							break;

						case 0x86:
							// 3D Object (16-bit vector) (16-bit address) (N OBJ)
							timer = reader.ReadUInt16();
							x = reader.ReadInt16();
							y = -reader.ReadInt16();
							z = reader.ReadInt16();
							objectID = reader.ReadUInt16();
							behaviorAddress = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);

							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(objectID, out objectName);

							behaviorName = "Unknown";
							Usa10.BehaviorNames.TryGetValue(behaviorAddress, out behaviorName);

							Nodes.Add(position.ToString("X6") + " 86 - 3D Object: " + objectID.ToString("X4") + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behaviorAddress.ToString("X6") + " (" + behaviorName + ")");
							break;

						case 0x88:
							// MQN OBJ (8-bit)
							timer = reader.ReadByte() * 16;
							x = reader.ReadSByte() * 4;
							y = -reader.ReadSByte() * 4;
							z = reader.ReadByte() * 16;
							objectID = reader.ReadUInt16();
							behaviorAddress = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);

							objectName = "Unknown";
							Usa10.ModelNames.TryGetValue(objectID, out objectName);

							behaviorName = "Unknown";
							Usa10.BehaviorNames.TryGetValue(behaviorAddress, out behaviorName);

							Nodes.Add(position.ToString("X6") + " 88 - 3D Object: " + objectID.ToString("X4") + " (" + objectName + ") (" + x + ", " + y + ", " + z + ") Timer: " + timer + " Behavior: " + behaviorAddress.ToString("X6") + " (" + behaviorName + ")");
							break;

						case 0x8a:
							// Wait (8-bit)
							value = reader.ReadByte() << 4;
							Nodes.Add(position.ToString("X6") + " 8a - Wait: " + value);
							break;

						case 0x8c:
							// Set Path
							value = reader.ReadByte() | reader.ReadByte() << 8;

							Nodes.Add(position.ToString("X6") + " 8c - Set Path:" + value.ToString("X4"));
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