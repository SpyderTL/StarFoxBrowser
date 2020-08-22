using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxObject : DataNode
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

				var verteces = reader.ReadUInt16();
				var bank = reader.ReadByte();
				var faces = reader.ReadUInt16();
				var zPosition = reader.ReadInt16();
				var scale = reader.ReadSByte();
				var collisionInformation = reader.ReadUInt16();
				var sizeX = reader.ReadInt16();
				var sizeY = reader.ReadInt16();
				var sizeZ = reader.ReadInt16();
				var alignment = reader.ReadInt16();
				var material = reader.ReadUInt16();
				var id1 = reader.ReadUInt16();
				var id2 = reader.ReadUInt16();
				var id3 = reader.ReadUInt16();
				var id4 = reader.ReadUInt16();

				if (verteces != 0)
				{
					Nodes.Add(new StarFoxModel { Text = "Space", Resource = Resource, VertexOffset = ((bank - 1) * 0x8000) + verteces, FaceOffset = ((bank - 1) * 0x8000) + faces, PaletteOffset = 0x18aca, MaterialOffset = 0x10000 + material, Scale = scale });
					Nodes.Add(new StarFoxModel { Text = "Night", Resource = Resource, VertexOffset = ((bank - 1) * 0x8000) + verteces, FaceOffset = ((bank - 1) * 0x8000) + faces, PaletteOffset = 0x18aea, MaterialOffset = 0x10000 + material, Scale = scale });
					Nodes.Add(new StarFoxModel { Text = "Day", Resource = Resource, VertexOffset = ((bank - 1) * 0x8000) + verteces, FaceOffset = ((bank - 1) * 0x8000) + faces, PaletteOffset = 0x18b0a, MaterialOffset = 0x10000 + material, Scale = scale });
				}
			}
		}

		public override object GetProperties()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Offset;

				var verteces = reader.ReadUInt16();
				var bank = reader.ReadByte();
				var faces = reader.ReadUInt16();
				var zPosition = reader.ReadInt16();
				var scale = reader.ReadSByte();
				var collisionInformation = reader.ReadUInt16();
				var sizeX = reader.ReadInt16();
				var sizeY = reader.ReadInt16();
				var sizeZ = reader.ReadInt16();
				var alignment = reader.ReadInt16();
				var material = reader.ReadUInt16();
				var id1 = reader.ReadUInt16();
				var id2 = reader.ReadUInt16();
				var id3 = reader.ReadUInt16();
				var id4 = reader.ReadUInt16();

				return new
				{
					Verteces = verteces,
					Bank = bank,
					Faces = faces,
					ZPosition = zPosition,
					Scale = scale,
					CollisionInformation = collisionInformation,
					SizeX = sizeX,
					SizeY = sizeY,
					SizeZ = sizeZ,
					Alignment = alignment,
					Material = material,
					ID1 = id1,
					ID2 = id2,
					ID3 = id3,
					ID4 = id4
				};
			}
		}
	}
}
