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
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x28042 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Inside Tunnel", Resource = Resource, Offset = 0x2b280 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x2b47c });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x2da0e });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x2da3f });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x68000 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Inside Tunnel", Resource = Resource, Offset = 0x6b2b3 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Black Hole", Resource = Resource, Offset = 0x6c8a9 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Out Of This World", Resource = Resource, Offset = 0x6c8e5 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Scramble", Resource = Resource, Offset = 0x6d068 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Corneria 1/2", Resource = Resource, Offset = 0x6d0bb });
			levels.Nodes.Add(new StarFoxLevel { Text = "Asteroid 1", Resource = Resource, Offset = 0x6d268 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Space Armada", Resource = Resource, Offset = 0x6d2c3 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Meteor", Resource = Resource, Offset = 0x6d586 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Venom 1/2 Space", Resource = Resource, Offset = 0x6d602 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Venom 1 Surface", Resource = Resource, Offset = 0x6d67f });
			levels.Nodes.Add(new StarFoxLevel { Text = "Sector X", Resource = Resource, Offset = 0x6df4c });
			levels.Nodes.Add(new StarFoxLevel { Text = "Titania", Resource = Resource, Offset = 0x6dfa7 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Sector Y", Resource = Resource, Offset = 0x6e02a });
			levels.Nodes.Add(new StarFoxLevel { Text = "Venom 1/2 Space", Resource = Resource, Offset = 0x6e085 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Highway", Resource = Resource, Offset = 0x6e0f5 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Asteroid 3", Resource = Resource, Offset = 0x6e62a });
			levels.Nodes.Add(new StarFoxLevel { Text = "Fortuna", Resource = Resource, Offset = 0x6e651 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Sector Z", Resource = Resource, Offset = 0x6e6d8 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Macbeth", Resource = Resource, Offset = 0x6e81c });
			levels.Nodes.Add(new StarFoxLevel { Text = "Venom 3 Space", Resource = Resource, Offset = 0x6e8b9 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Venom 3 Surface", Resource = Resource, Offset = 0x6e929 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Training", Resource = Resource, Offset = 0x6ee9a });

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