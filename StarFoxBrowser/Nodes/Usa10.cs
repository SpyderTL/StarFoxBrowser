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

			palettes.Nodes.Add(new StarFoxPalette2 { Text = "Day", Resource = Resource, Offset = 0x90000 });

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

			levels.Nodes.Add(new StarFoxLevel { Text = "Training", Resource = Resource, Offset = 0x6eef8 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x68000 });

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