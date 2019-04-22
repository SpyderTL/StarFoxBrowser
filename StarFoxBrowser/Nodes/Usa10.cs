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

			var levels = new TreeNode("Levels");

			levels.Nodes.Add(new StarFoxLevel { Text = "Training", Resource = Resource, Offset = 0x6eef8 });
			levels.Nodes.Add(new StarFoxLevel { Text = "Level", Resource = Resource, Offset = 0x68000 });

			Nodes.Add(models);
			Nodes.Add(levels);
		}

		public override object GetProperties()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = 0x18aca;

				var spacePalette = Enumerable.Range(0, 16)
					.Select(n => reader.ReadInt16())
					.Select(n => Color.FromArgb((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3))
					.ToArray();

				var nightPalette = Enumerable.Range(0, 16)
					.Select(n => reader.ReadInt16())
					.Select(n => Color.FromArgb((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3))
					.ToArray();

				var dayPalette = Enumerable.Range(0, 16)
					.Select(n => reader.ReadInt16())
					.Select(n => Color.FromArgb((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3))
					.ToArray();

				return new
				{
					SpacePalette = spacePalette,
					NightPalette = nightPalette,
					DayPalette = dayPalette,
				};
			}
		}
	}
}