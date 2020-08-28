using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace StarFoxBrowser.Nodes
{
	internal class StarFoxPath : DataNode
	{
		public string Resource;
		public int Path;

		public override object GetProperties() => new { Path };

		public override void Reload()
		{
			Nodes.Clear();

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = Usa10.PathTableAddress + (Path * 2);

				var offset = reader.ReadUInt16();

				stream.Position = Usa10.PathTableAddress + offset;

				var stage = 1;
				var read = true;

				while (read)
				{
					// Segment Start
					var start = reader.ReadByte();
					var one = reader.ReadByte();
					var two = reader.ReadByte();
					var three = reader.ReadByte();
					var levelAddress = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16) | 0x8000;
					var five = reader.ReadByte();
					var six = reader.ReadByte();

					var segment = new TreeNode("Stage " + stage + ": " + levelAddress.ToString("x6"));

					Nodes.Add(segment);

					while (true)
					{
						var character = reader.ReadInt16();

						if (character == -1)
							break;

						segment.Nodes.Add(character.ToString());
					}

					var type = reader.ReadByte();

					switch (type)
					{
						case 0:
							// End
							read = false;
							break;

						case 1:
							// Segment
							offset = reader.ReadUInt16();
							stream.Position = Usa10.PathTableAddress + offset;
							break;

						case 2:
							// Choice
							offset = reader.ReadUInt16();
							break;
					}

					stage++;
				}
			}
		}
	}
}