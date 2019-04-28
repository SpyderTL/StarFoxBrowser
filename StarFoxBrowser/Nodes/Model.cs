using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class Model : DataNode
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

							Nodes.Add(new VertexList { Text = "Vertex List", Verteces = verteces });
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

							Nodes.Add(new VertexList { Text = "Mirrored Vertex List", Verteces = verteces });
							break;

						case 0x1c:
							// Animation
							read = false;
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

							Nodes.Add(new VertexList { Text = "Vertex List (16-bit)", Verteces = verteces });
							break;

						case 0x0c:
							read = false;
							break;
					}
				}

				read = true;

				while (read)
				{
					var listType = reader.ReadByte();

					switch (listType)
					{
						case 0x30:
							// Triangle List
							var triangleCount = reader.ReadByte();

							var indeces = Enumerable.Range(0, triangleCount * 3)
								.Select(n => (int)reader.ReadByte())
								.ToArray();

							Nodes.Add(new IndexList { Text = "Index List", Indeces = indeces });

							break;

						case 0x14:
							// Face Group
							while (true)
							{
								var sideCount = reader.ReadByte();

								if (sideCount == 0xff)
									break;

								var faceNumber = reader.ReadByte();
								var colorNumber = reader.ReadByte();
								var normalX = reader.ReadSByte();
								var normalY = reader.ReadSByte();
								var normalZ = reader.ReadSByte();
								var vertex1 = reader.ReadByte();
								var vertex2 = reader.ReadByte();
								var vertex3 = reader.ReadByte();
							}
							break;

						default:
							// BSP Tree
							read = false;
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
