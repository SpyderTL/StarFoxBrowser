using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxModel : DataNode
	{
		public string Resource;
		public int VertexOffset;
		public int FaceOffset;
		public int PaletteOffset;
		public int MaterialOffset;

		public override void Reload()
		{
			Nodes.Clear();

			if (VertexOffset <= 0)
				return;

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = VertexOffset;

				var read = true;

				while (read)
				{
					var listType = reader.ReadByte();

					switch (listType)
					{
						case 0x04:
							// Vertex List (sbyte)
							var vertexCount = reader.ReadByte();

							var verteces = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadSByte(),
									Y = reader.ReadSByte(),
									Z = reader.ReadSByte()
								}).ToArray();

							Nodes.Add(new VertexList { Text = "04 - Vertex List", Verteces = verteces });
							break;

						case 0x08:
							// Vertex List (short)
							vertexCount = reader.ReadByte();

							verteces = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadInt16(),
									Y = reader.ReadInt16(),
									Z = reader.ReadInt16()
								}).ToArray();

							Nodes.Add(new VertexList { Text = "08 - Vertex List (16-bit)", Verteces = verteces });
							break;

						case 0x38:
							// Mirrored Vertex List (sbyte)
							vertexCount = reader.ReadByte();

							verteces = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadSByte(),
									Y = reader.ReadSByte(),
									Z = reader.ReadSByte()
								}).ToArray();

							Nodes.Add(new VertexList { Text = "38 - Mirrored Vertex List", Verteces = verteces });
							break;

						case 0x1c:
							// Animation
							var frameCount = reader.ReadByte();
							var frameOffsets = Enumerable.Range(0, frameCount)
								.Select(n => reader.ReadInt16())
								.ToArray();

							Nodes.Add(new Animation { Text = "1C - Animation", Offsets = frameOffsets });
							break;

						case 0x20:
							// Jump
							var offset = reader.ReadInt16();

							Nodes.Add("20 - Jump (" + offset.ToString() + ")");
							break;

						case 0x34:
							// Mirrored Vertex List (short)
							vertexCount = reader.ReadByte();

							verteces = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadInt16(),
									Y = reader.ReadInt16(),
									Z = reader.ReadInt16()
								}).ToArray();

							Nodes.Add(new VertexList { Text = "34 - Mirrored Vertex List (16-bit)", Verteces = verteces });
							break;

						case 0x0c:
							Nodes.Add("0C - End Vertex List");
							break;

						case 0x30:
							// Triangle List
							var triangleCount = reader.ReadByte();

							var indeces = Enumerable.Range(0, triangleCount)
								.Select(n => new int[] {
									reader.ReadByte(),
									reader.ReadByte(),
									reader.ReadByte()
								})
								.ToArray();

							Nodes.Add(new IndexList { Text = "30 - Triangle List", Indeces = indeces });
							break;

						case 0x3c:
							// Start BSP Tree
							Nodes.Add("3C - Start BSP Tree");
							break;

						case 0x28:
							// BSP Tree Node
							var triangle = reader.ReadByte();
							var faceGroupOffset = reader.ReadUInt16();
							var branchOffset = reader.ReadByte();

							Nodes.Add(new BspTreeNode { Text = "28 - BSP Tree Node", Triangle = triangle, FaceGroupOffset = faceGroupOffset, BranchOffset = branchOffset });
							break;

						case 0x44:
							// BSP Tree Leaf
							faceGroupOffset = reader.ReadUInt16();

							Nodes.Add("28 - BSP Tree Leaf (" + faceGroupOffset + ")");
							break;

						case 0x14:
							// Face Group
							var faceGroup = new System.Windows.Forms.TreeNode("14 - Face Group");

							while (true)
							{
								vertexCount = reader.ReadByte();

								if (vertexCount == 0xff || vertexCount == 0xfe)
									break;

								var faceNumber = reader.ReadByte();
								var colorNumber = reader.ReadByte();
								var normalX = reader.ReadSByte();
								var normalY = reader.ReadSByte();
								var normalZ = reader.ReadSByte();

								var vertices = Enumerable.Range(0, vertexCount)
									.Select(n => (int)reader.ReadByte())
									.ToArray();

								faceGroup.Nodes.Add(new Face { Text = "Face (" + vertexCount + ")", FaceNumber = faceNumber, ColorNumber = colorNumber, NormalX = normalX, NormalY = normalY, NormalZ = normalZ, Vertices = vertices });
							}

							Nodes.Add(faceGroup);
							break;

						case 0x00:
							// End Face Data
							Nodes.Add("00 - End Face Data");
							read = false;
							break;

						//case 0x01:
						//Nodes.Add("00 - End Face Data");
						//	break;

						case 0x0a:
							Nodes.Add(listType.ToString("X2") + " - UNKNOWN");
							break;

						case 0x0b:
							Nodes.Add(listType.ToString("X2") + " - UNKNOWN");
							break;

						case 0x40:
							Nodes.Add(listType.ToString("X2") + " - UNKNOWN");
							break;

						case 0x78:
							Nodes.Add(listType.ToString("X2") + " - UNKNOWN");
							break;

						default:
							//read = false;
							Nodes.Add(listType.ToString("X2") + " - UNKNOWN");
							break;
					}
				}
			}
		}

		public override object GetProperties()
		{
			if (VertexOffset <= 0)
				return null;

			var vectors = new List<Vector4>();
			var colorFaces = new List<Models.ColorFace>();
			var texture1Faces = new List<Models.TextureFace>();
			var texture2Faces = new List<Models.TextureFace>();

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				// Load Colors
				stream.Position = PaletteOffset;

				var palette = Enumerable.Range(0, 16)
					.Select(n => reader.ReadUInt16())
					.Select(n => new Vector4((n & 0x1f) / (float)0x1f, (n >> 5 & 0x1f) / (float)0x1f, (n >> 10) / (float)0x1f, 1.0f))
					.ToArray();

				// Load Lighting
				stream.Position = 0x18e0a;

				var lighting = Enumerable.Range(0, 400)
					.Select(n => reader.ReadByte())
					.Select(n => (palette[n & 0x0f] + palette[n >> 4]) * 0.5f)
					.ToArray();

				// Load Dynamic
				stream.Position = 0x18b8a;

				var dynamic = Enumerable.Range(0, 32)
					.Select(n => reader.ReadByte())
					.Select(n => (palette[n & 0x0f] + palette[n >> 4]) * 0.5f)
					.ToArray();

				// Load Texture Addresses
				stream.Position = 0x18918;

				var textureAddresses = Enumerable.Range(0, 0x80)
					.Select(n => reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 16)
					.Select(x => ((x >> 16) * 0x8000) | (x & 0x7fff))
					.ToArray();

				var texturePositions = textureAddresses
					.Select(x => new Vector2(((x - 0x90000) % 256) / 256.0f, ((x - 0x90000) / 256) / 256.0f))
					.ToArray();

				// Materials
				stream.Position = MaterialOffset;

				var colors = new Vector4[109];
				var textures = new Vector2[109][];
				var texturePages = new byte?[109];
				var frame = (int)(DateTime.Now.TimeOfDay.TotalSeconds * 15.0d);

				for (var entry = 0; entry < 109; entry++)
				{
					var value = reader.ReadByte();
					var type = reader.ReadByte();

					if ((type & 0xf0) == 0x00)
					{
						// Lighting
						colors[entry] = lighting[value];
					}
					else if ((type & 0x80) != 0x00)
					{
						// Animated
						var offset = (type & 0x0f) << 8 | value;

						var position = reader.BaseStream.Position;

						reader.BaseStream.Position = 0x18000 + offset;

						var frameCount = reader.ReadByte();

						if (frameCount != 0)
							reader.BaseStream.Seek((frame % frameCount) * 2, SeekOrigin.Current);

						var value2 = reader.ReadByte();
						var type2 = reader.ReadByte();

						switch (type2)
						{
							case 0x3e:
								// Dynamic Color
								colors[entry] = dynamic[value2];
								break;

							case 0x3f:
								// Stipple Color
								colors[entry] = (palette[value2 & 0xf] + palette[value2 >> 4]) * 0.5f;
								break;

							case 0x40:
								// 32x32 Texture Flipped
								var width = 32;
								var height = 32;
								var page = value2 >> 7;
								var index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x41:
								// 64x64 Texture Flipped
								width = 64;
								height = 64;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x42:
								// 8x8 Texture Flipped
								width = 8;
								height = 8;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x43:
								// 64x16 Texture Flipped
								width = 64;
								height = 16;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x44:
								// 32x8 Texture Flipped
								width = 64;
								height = 16;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x45:
								// 32x8 Texture
								width = 32;
								height = 8;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x46:
								// 64x64 Texture
								width = 64;
								height = 64;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x47:
								// 16x8 Texture
								width = 16;
								height = 8;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x48:
								// 32x32 Texture
								width = 32;
								height = 32;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x49:
								// 64x64 Texture Polar Flipped
								width = 64;
								height = 64;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x4a:
								// 64x64 Texture Polar
								width = 64;
								height = 64;
								page = value2 >> 7;
								index = value2 & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							default:
								colors[entry] = palette[14];
								break;
						}

						reader.BaseStream.Position = position;
					}
					else
					{
						switch (type)
						{
							case 0x3e:
								// Dynamic Color
								colors[entry] = dynamic[value];
								break;

							case 0x3f:
								// Stipple Color
								colors[entry] = (palette[value & 0xf] + palette[value >> 4]) * 0.5f;
								break;

							case 0x40:
								// 32x32 Texture Flipped
								var width = 32;
								var height = 32;
								var page = value >> 7;
								var index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x41:
								// 64x64 Texture Flipped
								width = 64;
								height = 64;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x42:
								// 8x8 Texture Flipped
								width = 8;
								height = 8;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x43:
								// 64x16 Texture Flipped
								width = 64;
								height = 16;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x44:
								// 32x8 Texture Flipped
								width = 64;
								height = 16;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x45:
								// 32x8 Texture
								width = 32;
								height = 8;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x46:
								// 64x64 Texture
								width = 64;
								height = 64;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x47:
								// 16x8 Texture
								width = 16;
								height = 8;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x48:
								// 32x32 Texture
								width = 32;
								height = 32;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x49:
								// 64x64 Texture Polar Flipped
								width = 64;
								height = 64;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;

							case 0x4a:
								// 64x64 Texture Polar
								width = 64;
								height = 64;
								page = value >> 7;
								index = value & 0x7f;
								textures[entry] = new Vector2[]
								{
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (0 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (0 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
									new Vector2(texturePositions[index].X + (1 * (width / 256.0f)), texturePositions[index].Y + (1 * (height / 256.0f))),
								};
								texturePages[entry] = (byte)page;
								break;
						}
					}
				}

				stream.Position = VertexOffset;

				var read = true;

				while (read)
				{
					var listType = reader.ReadByte();

					switch (listType)
					{
						case 0x04:
							// Vertex List (sbyte)
							var vertexCount = reader.ReadByte();

							var vertices = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadSByte(),
									Y = reader.ReadSByte(),
									Z = reader.ReadSByte()
								}).ToArray();

							vectors.AddRange(vertices.Select(x => new Vector4(x.X, -x.Y, -x.Z, 1)));
							break;

						case 0x08:
							// Vertex List (short)
							vertexCount = reader.ReadByte();

							vertices = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadInt16(),
									Y = reader.ReadInt16(),
									Z = reader.ReadInt16()
								}).ToArray();

							vectors.AddRange(vertices.Select(x => new Vector4(x.X, -x.Y, -x.Z, 1)));
							break;


						case 0x38:
							// Mirrored Vertex List (sbyte)
							vertexCount = reader.ReadByte();

							vertices = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadSByte(),
									Y = reader.ReadSByte(),
									Z = reader.ReadSByte()
								}).ToArray();

							foreach (var vertex in vertices)
							{
								vectors.Add(new Vector4(vertex.X, -vertex.Y, -vertex.Z, 1));
								vectors.Add(new Vector4(-vertex.X, -vertex.Y, -vertex.Z, 1));
							}
							break;

						case 0x1c:
							// Animation
							var frameCount = reader.ReadByte();

							reader.BaseStream.Seek((frame % frameCount) * 2, SeekOrigin.Current);

							var offset = reader.ReadInt16();

							reader.BaseStream.Seek(offset - 1, SeekOrigin.Current);
							break;

						case 0x20:
							// Jump
							offset = reader.ReadInt16();
							reader.BaseStream.Seek(offset - 1, SeekOrigin.Current);
							break;

						case 0x34:
							// Mirrored Vertex List (short)
							vertexCount = reader.ReadByte();

							vertices = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadInt16(),
									Y = reader.ReadInt16(),
									Z = reader.ReadInt16()
								}).ToArray();

							foreach (var vertex in vertices)
							{
								vectors.Add(new Vector4(vertex.X, -vertex.Y, -vertex.Z, 1));
								vectors.Add(new Vector4(-vertex.X, -vertex.Y, -vertex.Z, 1));
							}
							break;

						case 0x0c:
							read = false;
							break;

						default:
							read = false;
							break;
					}
				}

				stream.Position = FaceOffset;

				read = true;

				while (read)
				{
					var listType = reader.ReadByte();

					switch (listType)
					{
						case 0x30:
							// Triangle List
							var triangleCount = reader.ReadByte();

							var indeces = Enumerable.Range(0, triangleCount)
								.Select(n => new int[] {
									reader.ReadByte(),
									reader.ReadByte(),
									reader.ReadByte()
								})
								.ToArray();
							break;

						case 0x3c:
							// Start BSP Tree
							break;

						case 0x28:
							// BSP Tree Node
							var triangle = reader.ReadByte();
							var faceGroupOffset = reader.ReadUInt16();
							var branchOffset = reader.ReadByte();
							break;

						case 0x44:
							// BSP Tree Leaf
							faceGroupOffset = reader.ReadUInt16();
							break;

						case 0x14:
							// Face Group
							while (true)
							{
								var vertexCount = reader.ReadByte();

								if (vertexCount == 0xff || vertexCount == 0xfe)
									break;

								var faceNumber = reader.ReadByte();
								var colorNumber = reader.ReadByte();
								var normalX = reader.ReadSByte();
								var normalY = reader.ReadSByte();
								var normalZ = reader.ReadSByte();

								var indices = Enumerable.Range(0, vertexCount)
									.Select(n => (int)reader.ReadByte())
									.ToArray();

								switch (vertexCount)
								{
									case 1:
										colorFaces.Add(new Models.ColorFace
										{
											Indices = indices,
											PrimitiveCount = 1,
											PrimitiveType = SharpDX.Direct3D9.PrimitiveType.PointList,
											Vertices = vectors.Select(x => new Models.Vertex
											{
												Position = x,
												Color = colors[colorNumber]
											}).ToArray()
										});
										break;

									case 2:
										colorFaces.Add(new Models.ColorFace
										{
											Indices = indices,
											PrimitiveCount = 1,
											PrimitiveType = SharpDX.Direct3D9.PrimitiveType.LineList,
											Vertices = vectors.Select(x => new Models.Vertex
											{
												Position = x,
												Color = colors[colorNumber]
											}).ToArray()
										});
										break;

									default:
										if (texturePages[colorNumber] == null)
										{
											colorFaces.Add(new Models.ColorFace
											{
												Indices = indices,
												PrimitiveCount = vertexCount - 2,
												PrimitiveType = SharpDX.Direct3D9.PrimitiveType.TriangleFan,
												Vertices = vectors.Select(x => new Models.Vertex
												{
													Position = x,
													Color = colors[colorNumber]
												}).ToArray()
											});
										}
										else if (texturePages[colorNumber] == 0)
										{
											texture1Faces.Add(new Models.TextureFace
											{
												Indices = Enumerable.Range(0, indices.Length).ToArray(),
												//Indices = indices,
												PrimitiveCount = vertexCount - 2,
												PrimitiveType = SharpDX.Direct3D9.PrimitiveType.TriangleFan,
												Vertices = indices.Select((x, i) => new Models.TextureVertex
												{
													Position = vectors[x % vectors.Count],
													TexturePosition = textures[colorNumber][i]
												}).ToArray()
												//Vertices = vectors.Select((vector, index) => new Models.TextureVertex
												//{
												//	Position = vector,
												//	TexturePosition = textures[colorNumber][index % 4]
												//}).ToArray()
											});
										}
										else
										{
											texture2Faces.Add(new Models.TextureFace
											{
												Indices = Enumerable.Range(0, indices.Length).ToArray(),
												//Indices = indices,
												PrimitiveCount = vertexCount - 2,
												PrimitiveType = SharpDX.Direct3D9.PrimitiveType.TriangleFan,
												Vertices = indices.Select((x, i) => new Models.TextureVertex
												{
													Position = vectors[x % vectors.Count],
													TexturePosition = textures[colorNumber][i]
												}).ToArray()
												//Vertices = vectors.Select((vector, index) => new Models.TextureVertex
												//{
												//	Position = vector,
												//	TexturePosition = textures[colorNumber][index % 4]
												//}).ToArray()
											});
										}
										break;
								}
							}
							break;

						case 0x40:
							break;

						case 0x00:
							// End Face Data
							read = false;
							break;

						default:
							read = false;
							break;
					}
				}

				return new Models.Model
				{
					ColorFaces = colorFaces.ToArray(),
					Texture1Faces = texture1Faces.ToArray(),
					Texture2Faces = texture2Faces.ToArray()
				};
			}
		}

		//private readonly Vector2[] TexturePositions = new Vector2[]
		//{
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	// Fox Logo
		//	new Vector2(6 * (32.0f / 256), 0 * (32.0f / 256)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	// Nova Bomb 1
		//	new Vector2(1 * (32.0f / 256), 3 * (32.0f / 256)),
		//	// Nova Bomb 2
		//	new Vector2(2 * (32.0f / 256), 3 * (32.0f / 256)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	// Nova Bomb 3
		//	new Vector2(4 * (32.0f / 256), 3 * (32.0f / 256)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	// Andross
		//	new Vector2(3 * (64.0f / 256), 0 * (64.0f / 256)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	// Black Hole
		//	new Vector2(2 * (32.0f / 256), 5 * (32.0f / 256)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//	new Vector2(0 * (256 / 32), 0 * (256 / 32)),
		//};
	}
}
