using System.IO;
using System.Reflection;

namespace StarFoxBrowser.Nodes
{
	public class StarFoxCourse : DataNode
	{
		public int Path { get; set; }
		public ushort Offset { get; set; }
		public ushort Count { get; set; }
		public string Resource { get; set; }

		public override object GetProperties() => new { Path, Offset, Count };

		public override void Reload()
		{
			Nodes.Clear();

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = 0x28000 + Offset;

				for (var level = 0; level < Count; level++)
				{
					var offset = reader.ReadUInt16();
					var bank = reader.ReadByte();

					Nodes.Add("Level " + level + ": " + ((bank << 16) + offset + 0x8000).ToString("x6"));
				}
			}
		}
	}
}