using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarFoxBrowser.Nodes
{
	public class Usa10 : DataNode
	{
		public string Resource;

		public override void Reload()
		{
			Nodes.Clear();

			var palettes = new TreeNode("Palettes");

			palettes.Nodes.Add(new StarFoxPalette { Text = "Unknown", Resource = Resource, Offset = 0x18a6a });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Unknown", Resource = Resource, Offset = 0x18a8a });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Unknown", Resource = Resource, Offset = 0x18aaa });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Space", Resource = Resource, Offset = 0x18aca });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Night", Resource = Resource, Offset = 0x18aea });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Day", Resource = Resource, Offset = 0x18b0a });

			//palettes.Nodes.Add(new StarFoxPalette2 { Text = "3D", Resource = Resource, Offset = 0x90000 });

			var models = new TreeNode("Models");

			models.Nodes.Add(new StarFoxObject { Text = "Arwing (Low Poly)", Resource = Resource, Offset = 0x5320 });
			models.Nodes.Add(new StarFoxObject { Text = "Main Enemy", Resource = Resource, Offset = 0x6388 });
			models.Nodes.Add(new StarFoxObject { Text = "Gray Archway", Resource = Resource, Offset = 0x4868 });
			models.Nodes.Add(new StarFoxObject { Text = "Blue Archway", Resource = Resource, Offset = 0x3ad8 });
			models.Nodes.Add(new StarFoxObject { Text = "Yellow Ring", Resource = Resource, Offset = 0x64d8 });
			models.Nodes.Add(new StarFoxObject { Text = "Blue Ring", Resource = Resource, Offset = 0x3d40 });
			models.Nodes.Add(new StarFoxObject { Text = "Silver Ring", Resource = Resource, Offset = 0x38bf });
			models.Nodes.Add(new StarFoxObject { Text = "Wing Repair", Resource = Resource, Offset = 0x44cc });
			models.Nodes.Add(new StarFoxObject { Text = "Weapon Upgrade", Resource = Resource, Offset = 0x4478 });

			var textures = new TreeNode("Textures");

			textures.Nodes.Add(new StarFoxTexture { Text = "Texture", Resource = Resource, Offset = 0x90000, Page = 0 });
			textures.Nodes.Add(new StarFoxTexture { Text = "Texture", Resource = Resource, Offset = 0x90000, Page = 1 });

			var levels = new TreeNode("Levels");
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d068 });

			levels.Nodes.Add(new StarFoxLevel { Text = "Training", Resource = Resource, Offset = 0x6eef8 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x2b283 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x2b47f });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x2da11 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x2da42 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6b2b6 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6c8ae });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6c8ea });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d072 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d0d2 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d26d });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d2c8 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d30a });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d313 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d328 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d331 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d346 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d42a });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d58b });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d607 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d684 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6d729 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6dac6 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6db84 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6dbe1 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6dc97 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6dd70 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6ddd0 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6df51 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6dfac });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6dffb });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e01a });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e02f });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e08a });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e0fa });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e19b });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e3d9 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e439 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e5fb });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e658 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e6dd });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e7b2 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e821 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e8be });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e92e });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e966 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Corneria", Resource = Resource, Offset = 0x6d124 });
			//levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6dd70 });

			//levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x6e3cf });

			Nodes.Add(palettes);
			Nodes.Add(models);
			Nodes.Add(textures);
			Nodes.Add(levels);
		}

		public override object GetProperties()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = 0x7fc0;

				var title = Encoding.ASCII.GetString(reader.ReadBytes(21));
				var romLayout = reader.ReadByte();
				var romType = reader.ReadByte();
				var romSize = reader.ReadByte();
				var sramSize = reader.ReadByte();
				var licenseID = reader.ReadUInt16();
				var version = reader.ReadByte();
				var checksumCompliment = reader.ReadUInt16();
				var checksum = reader.ReadUInt16();

				return new
				{
					Title = title,
					RomLayout = romLayout,
					RomType = romType,
					RomSize = romSize,
					SramSize = sramSize,
					LicenseID = licenseID,
					Version = version,
					ChecksumCompliment = checksumCompliment,
					Checksum = checksum
				};
			}
		}
	}
}