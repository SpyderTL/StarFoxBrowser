using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxModel : DataNode
	{
		public string Resource;
		public int Offset;

		public override void Reload()
		{
			Nodes.Clear();

			if (Offset <= 0)
				return;

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Offset;

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

							Nodes.Add(new Animation { Text = "1C - Animation", Offsets = frameOffsets  });
							break;

						case 0x20:
							// Jump
							var offset = reader.ReadInt16();

							Nodes.Add("20 - Jump (" + offset.ToString() + ")");
							break;

						case 0x34:
							// Vertex List (short)
							vertexCount = reader.ReadByte();

							verteces = Enumerable.Range(0, vertexCount)
								.Select(n => new Vertex
								{
									X = reader.ReadInt16(),
									Y = reader.ReadInt16(),
									Z = reader.ReadInt16()
								}).ToArray();

							Nodes.Add(new VertexList { Text = "34 - Vertex List (16-bit)", Verteces = verteces });
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

							Nodes.Add("28 - BSP Tree Leaf (" + faceGroupOffset +")");
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

						default:
							read = false;
							break;
					}
				}
			}
		}

		public override object GetProperties()
		{
			return new Models.Model();
		}
	}
}
