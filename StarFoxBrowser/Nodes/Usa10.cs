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

			var images = new TreeNode("Images");

			foreach (var image in ImageNames)
				images.Nodes.Add(new StarFoxImage { Text = image.Value, Resource = Resource, Offset = image.Key, Size = ImageSizes[image.Key] });

			var materials = new TreeNode("Materials");

			materials.Nodes.Add(new StarFoxSurface { Text = "0000", Resource = Resource, Offset = 0x10000 });
			materials.Nodes.Add(new StarFoxSurface { Text = "0500", Resource = Resource, Offset = 0x10500 });
			materials.Nodes.Add(new StarFoxSurface { Text = "1400", Resource = Resource, Offset = 0x11400 });
			materials.Nodes.Add(new StarFoxSurface { Text = "2800", Resource = Resource, Offset = 0x12800 });
			materials.Nodes.Add(new StarFoxSurface { Text = "2a00", Resource = Resource, Offset = 0x12a00 });
			materials.Nodes.Add(new StarFoxSurface { Text = "3001", Resource = Resource, Offset = 0x13001 });
			materials.Nodes.Add(new StarFoxSurface { Text = "5000", Resource = Resource, Offset = 0x15000 });
			materials.Nodes.Add(new StarFoxSurface { Text = "6e00", Resource = Resource, Offset = 0x16e00 });
			materials.Nodes.Add(new StarFoxSurface { Text = "7001", Resource = Resource, Offset = 0x17001 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8004", Resource = Resource, Offset = 0x18004 });
			materials.Nodes.Add(new StarFoxSurface { Text = "800c", Resource = Resource, Offset = 0x1800c });
			materials.Nodes.Add(new StarFoxSurface { Text = "807c", Resource = Resource, Offset = 0x1807c });
			materials.Nodes.Add(new StarFoxSurface { Text = "81d8", Resource = Resource, Offset = 0x181d8 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8213", Resource = Resource, Offset = 0x18213 });
			materials.Nodes.Add(new StarFoxSurface { Text = "82ed", Resource = Resource, Offset = 0x182ed });
			materials.Nodes.Add(new StarFoxSurface { Text = "83c1", Resource = Resource, Offset = 0x183c1 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8421", Resource = Resource, Offset = 0x18421 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8481", Resource = Resource, Offset = 0x18481 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8520", Resource = Resource, Offset = 0x18520 });
			materials.Nodes.Add(new StarFoxSurface { Text = "852c", Resource = Resource, Offset = 0x1852c });
			materials.Nodes.Add(new StarFoxSurface { Text = "852e", Resource = Resource, Offset = 0x1852e });
			materials.Nodes.Add(new StarFoxSurface { Text = "8530", Resource = Resource, Offset = 0x18530 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8532", Resource = Resource, Offset = 0x18532 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8534", Resource = Resource, Offset = 0x18534 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8536", Resource = Resource, Offset = 0x18536 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8538", Resource = Resource, Offset = 0x18538 });
			materials.Nodes.Add(new StarFoxSurface { Text = "853a", Resource = Resource, Offset = 0x1853a });
			materials.Nodes.Add(new StarFoxSurface { Text = "853c", Resource = Resource, Offset = 0x1853c });
			materials.Nodes.Add(new StarFoxSurface { Text = "853e", Resource = Resource, Offset = 0x1853e });
			materials.Nodes.Add(new StarFoxSurface { Text = "8540", Resource = Resource, Offset = 0x18540 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8542", Resource = Resource, Offset = 0x18542 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8544", Resource = Resource, Offset = 0x18544 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8546", Resource = Resource, Offset = 0x18546 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8548", Resource = Resource, Offset = 0x18548 });
			materials.Nodes.Add(new StarFoxSurface { Text = "854c", Resource = Resource, Offset = 0x1854c });
			materials.Nodes.Add(new StarFoxSurface { Text = "854e", Resource = Resource, Offset = 0x1854e });
			materials.Nodes.Add(new StarFoxSurface { Text = "8552", Resource = Resource, Offset = 0x18552 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8554", Resource = Resource, Offset = 0x18554 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8556", Resource = Resource, Offset = 0x18556 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8558", Resource = Resource, Offset = 0x18558 });
			materials.Nodes.Add(new StarFoxSurface { Text = "855a", Resource = Resource, Offset = 0x1855a });
			materials.Nodes.Add(new StarFoxSurface { Text = "855c", Resource = Resource, Offset = 0x1855c });
			materials.Nodes.Add(new StarFoxSurface { Text = "855e", Resource = Resource, Offset = 0x1855e });
			materials.Nodes.Add(new StarFoxSurface { Text = "8560", Resource = Resource, Offset = 0x18560 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8562", Resource = Resource, Offset = 0x18562 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8566", Resource = Resource, Offset = 0x18566 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8568", Resource = Resource, Offset = 0x18568 });
			materials.Nodes.Add(new StarFoxSurface { Text = "856a", Resource = Resource, Offset = 0x1856a });
			materials.Nodes.Add(new StarFoxSurface { Text = "856e", Resource = Resource, Offset = 0x1856e });
			materials.Nodes.Add(new StarFoxSurface { Text = "8572", Resource = Resource, Offset = 0x18572 });
			materials.Nodes.Add(new StarFoxSurface { Text = "857d", Resource = Resource, Offset = 0x1857d });
			materials.Nodes.Add(new StarFoxSurface { Text = "8583", Resource = Resource, Offset = 0x18583 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8585", Resource = Resource, Offset = 0x18585 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8590", Resource = Resource, Offset = 0x18590 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8594", Resource = Resource, Offset = 0x18594 });
			materials.Nodes.Add(new StarFoxSurface { Text = "85a4", Resource = Resource, Offset = 0x185a4 });
			materials.Nodes.Add(new StarFoxSurface { Text = "85af", Resource = Resource, Offset = 0x185af });
			materials.Nodes.Add(new StarFoxSurface { Text = "85bd", Resource = Resource, Offset = 0x185bd });
			materials.Nodes.Add(new StarFoxSurface { Text = "8873", Resource = Resource, Offset = 0x18873 });
			materials.Nodes.Add(new StarFoxSurface { Text = "8899", Resource = Resource, Offset = 0x18899 });
			materials.Nodes.Add(new StarFoxSurface { Text = "88d0", Resource = Resource, Offset = 0x188d0 });


			//surfaces.Nodes.Add(new StarFoxSurface { Text = "Space", Resource = Resource, Offset = 0x181f0 });
			//surfaces.Nodes.Add(new StarFoxSurface { Text = "Space", Resource = Resource, Offset = 0x18213 });
			//surfaces.Nodes.Add(new StarFoxSurface { Text = "Day", Resource = Resource, Offset = 0x182ed });
			//surfaces.Nodes.Add(new StarFoxSurface { Text = "Night", Resource = Resource, Offset = 0x183c1 });
			//surfaces.Nodes.Add(new StarFoxSurface { Text = "Night", Resource = Resource, Offset = 0x18481 });
			//surfaces.Nodes.Add(new StarFoxSurface { Text = "Night", Resource = Resource, Offset = 0x18af1 });

			var palettes = new TreeNode("Palettes");

			palettes.Nodes.Add(new StarFoxPalette { Text = "Unknown", Resource = Resource, Offset = 0x18a6a });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Unknown", Resource = Resource, Offset = 0x18a8a });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Unknown", Resource = Resource, Offset = 0x18aaa });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Space", Resource = Resource, Offset = 0x18aca });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Night", Resource = Resource, Offset = 0x18aea });
			palettes.Nodes.Add(new StarFoxPalette { Text = "Day", Resource = Resource, Offset = 0x18b0a });

			//palettes.Nodes.Add(new StarFoxPalette2 { Text = "3D", Resource = Resource, Offset = 0x90000 });

			var models = new TreeNode("Models");

			var indexedModels = new TreeNode("Indexed");

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = ModelTableAddress;

				for (int x = 0; x < 255; x++)
				{
					var objectID = reader.ReadUInt16();

					ObjectIndexes[x] = objectID;

					indexedModels.Nodes.Add(x.ToString("X2") + ": " + objectID.ToString("X4"));
				}
			}

			models.Nodes.Add(indexedModels);

			foreach (var model in ModelNames)
				models.Nodes.Add(new StarFoxObject { Text = model.Key.ToString("X4") + " " + model.Value, Resource = Resource, Offset = model.Key - 0x8000 });

			var behaviors = new TreeNode("Behaviors");

			var indexedBehaviors = new TreeNode("Indexed");

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				stream.Position = BehaviorTableAddress;

				for (int x = 0; x < 255; x++)
				{
					var objectIndex = reader.ReadByte();
					var behavior = reader.ReadBytes(3);
					var behaviorID = behavior[0] | (behavior[1] << 8) | (behavior[2] << 16);

					indexedBehaviors.Nodes.Add(x.ToString("X2") + ": " + string.Join(string.Empty, behavior.Reverse().Select(y => y.ToString("X2"))) + " Object: " + objectIndex.ToString("X2"));

					BehaviorIndexes[x] = behaviorID;
					BehaviorObjects[x] = objectIndex;
				}
			}

			behaviors.Nodes.Add(indexedBehaviors);

			//models.Nodes.Add(new Behavior { Text = "", Resource = Resource, Offset = 0x2840 });

			var textures = new TreeNode("Textures");

			textures.Nodes.Add(new StarFoxTexture { Text = "Texture", Resource = Resource, Offset = 0x90000, Page = 0 });
			textures.Nodes.Add(new StarFoxTexture { Text = "Texture", Resource = Resource, Offset = 0x90000, Page = 1 });

			var levels = new TreeNode("Levels");

			levels.Nodes.Add(new StarFoxLevel { Text = "050042", Resource = Resource, Offset = 0x28042 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0503b2", Resource = Resource, Offset = 0x283b2 });
			levels.Nodes.Add(new StarFoxLevel { Text = "050691", Resource = Resource, Offset = 0x28691 });
			levels.Nodes.Add(new StarFoxLevel { Text = "050774", Resource = Resource, Offset = 0x28774 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0507da", Resource = Resource, Offset = 0x287da });
			levels.Nodes.Add(new StarFoxLevel { Text = "050ce5", Resource = Resource, Offset = 0x28ce5 });
			levels.Nodes.Add(new StarFoxLevel { Text = "051380", Resource = Resource, Offset = 0x29380 });
			levels.Nodes.Add(new StarFoxLevel { Text = "05198a", Resource = Resource, Offset = 0x2998a });
			levels.Nodes.Add(new StarFoxLevel { Text = "0520ad", Resource = Resource, Offset = 0x2a0ad });
			levels.Nodes.Add(new StarFoxLevel { Text = "053280", Resource = Resource, Offset = 0x2b280 });
			levels.Nodes.Add(new StarFoxLevel { Text = "053488", Resource = Resource, Offset = 0x2b488 });
			levels.Nodes.Add(new StarFoxLevel { Text = "053b7c", Resource = Resource, Offset = 0x2bb7c });
			levels.Nodes.Add(new StarFoxLevel { Text = "053e53", Resource = Resource, Offset = 0x2be53 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0542fb", Resource = Resource, Offset = 0x2c2fb });
			levels.Nodes.Add(new StarFoxLevel { Text = "054301", Resource = Resource, Offset = 0x2c301 });
			levels.Nodes.Add(new StarFoxLevel { Text = "055885", Resource = Resource, Offset = 0x2d885 });
			levels.Nodes.Add(new StarFoxLevel { Text = "055a0e", Resource = Resource, Offset = 0x2da0e });
			levels.Nodes.Add(new StarFoxLevel { Text = "055a3f", Resource = Resource, Offset = 0x2da3f });
			levels.Nodes.Add(new StarFoxLevel { Text = "055bae", Resource = Resource, Offset = 0x2dbae });
			levels.Nodes.Add(new StarFoxLevel { Text = "055d53", Resource = Resource, Offset = 0x2dd53 });
			levels.Nodes.Add(new StarFoxLevel { Text = "055eed", Resource = Resource, Offset = 0x2deed });
			levels.Nodes.Add(new StarFoxLevel { Text = "05607a", Resource = Resource, Offset = 0x2e07a });
			levels.Nodes.Add(new StarFoxLevel { Text = "0561f1", Resource = Resource, Offset = 0x2e1f1 });
			levels.Nodes.Add(new StarFoxLevel { Text = "05672b", Resource = Resource, Offset = 0x2e72b });
			levels.Nodes.Add(new StarFoxLevel { Text = "056f16", Resource = Resource, Offset = 0x2ef16 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d0000 Scramble + Corneria", Resource = Resource, Offset = 0x68000 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d012d", Resource = Resource, Offset = 0x6812d });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d0920", Resource = Resource, Offset = 0x68920 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d10ad", Resource = Resource, Offset = 0x690ad });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d195d", Resource = Resource, Offset = 0x6995d });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d28a7", Resource = Resource, Offset = 0x6a8a7 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d2b67", Resource = Resource, Offset = 0x6ab67 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d2ce5", Resource = Resource, Offset = 0x6ace5 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d2e09", Resource = Resource, Offset = 0x6ae09 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d2f49", Resource = Resource, Offset = 0x6af49 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d317c", Resource = Resource, Offset = 0x6b17c });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d32b3", Resource = Resource, Offset = 0x6b2b3 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d34fb", Resource = Resource, Offset = 0x6b4fb });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d35d3", Resource = Resource, Offset = 0x6b5d3 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d3c57", Resource = Resource, Offset = 0x6bc57 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d492e", Resource = Resource, Offset = 0x6c92e });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d4c27", Resource = Resource, Offset = 0x6cc27 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d4da3", Resource = Resource, Offset = 0x6cda3 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d4f1d", Resource = Resource, Offset = 0x6cf1d });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d697e", Resource = Resource, Offset = 0x6e97e });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d6b76", Resource = Resource, Offset = 0x6eb76 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d6d02", Resource = Resource, Offset = 0x6ed02 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d6d21", Resource = Resource, Offset = 0x6ed21 });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d72dd", Resource = Resource, Offset = 0x6f2dd });
			levels.Nodes.Add(new StarFoxLevel { Text = "0d752d", Resource = Resource, Offset = 0x6f52d });
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


			var audioClips = new TreeNode("Audio Clips");
			
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c3a64", Resource = Resource, Offset = 0xc3a64 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c3fc5", Resource = Resource, Offset = 0xc3fc5 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c4589", Resource = Resource, Offset = 0xc4589 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c512c", Resource = Resource, Offset = 0xc512c });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c5ebe", Resource = Resource, Offset = 0xc5ebe });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c6d31", Resource = Resource, Offset = 0xc6d31 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c7547", Resource = Resource, Offset = 0xc7547 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c775a", Resource = Resource, Offset = 0xc775a });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c7f0d", Resource = Resource, Offset = 0xc7f0d });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c877d", Resource = Resource, Offset = 0xc877d });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c8fc9", Resource = Resource, Offset = 0xc8fc9 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c9ad3", Resource = Resource, Offset = 0xc9ad3 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "c9d42", Resource = Resource, Offset = 0xc9d42 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "cd4aa", Resource = Resource, Offset = 0xcd4aa });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "cebe7", Resource = Resource, Offset = 0xcebe7 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "cffca", Resource = Resource, Offset = 0xcffca });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "d1282", Resource = Resource, Offset = 0xd1282 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "d12b6", Resource = Resource, Offset = 0xd12b6 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "d2add", Resource = Resource, Offset = 0xd2add });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "d4f1c", Resource = Resource, Offset = 0xd4f1c });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "d6995", Resource = Resource, Offset = 0xd6995 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "d7727", Resource = Resource, Offset = 0xd7727 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "d791f", Resource = Resource, Offset = 0xd791f });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "da6ba", Resource = Resource, Offset = 0xda6ba });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "dc3fb", Resource = Resource, Offset = 0xdc3fb });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "dcef3", Resource = Resource, Offset = 0xdcef3 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "e1bec", Resource = Resource, Offset = 0xe1bec });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "e2b76", Resource = Resource, Offset = 0xe2b76 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "e3974", Resource = Resource, Offset = 0xe3974 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "e5c8a", Resource = Resource, Offset = 0xe5c8a });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "e6f35", Resource = Resource, Offset = 0xe6f35 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "e7b25", Resource = Resource, Offset = 0xe7b25 });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "e958d", Resource = Resource, Offset = 0xe958d });
			audioClips.Nodes.Add(new StarFoxAudioClip { Text = "ea73b", Resource = Resource, Offset = 0xea73b });

			Nodes.Add(images);
			Nodes.Add(materials);
			Nodes.Add(palettes);
			Nodes.Add(models);
			Nodes.Add(behaviors);
			Nodes.Add(textures);
			Nodes.Add(levels);
			Nodes.Add(audioClips);

			//ExportAudioClips();
			//ExportImages();
			//ExportAllImages();
		}

		private void ExportImages()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				// Load Palette
				stream.Position = 0x18b0a;

				var palette = Enumerable.Range(0, 16)
					.Select(n => reader.ReadUInt16())
					.Select(n => Color.FromArgb((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3))
					.ToArray();

				foreach (var image in ImageNames)
				{
					stream.Position = image.Key;

					var fileName = image.Value + ".bmp";

					var imageSize = ImageSizes[image.Key];

					var bitmap = new Bitmap(imageSize.Width * 8, imageSize.Height * 8);

					for (var tileX = 0; tileX < imageSize.Width; tileX++)
					{
						for (var tileY = 0; tileY < imageSize.Height; tileY++)
						{
							var data = new int[64];

							// Plane Row 0
							for (var row = 0; row < 8; row++)
							{
								// Plane 0
								var plane0 = reader.ReadByte();

								// Plane 1
								var plane1 = reader.ReadByte();

								data[(row * 8) + 0] |= ((plane0 >> 7) & 0x01) | ((plane1 >> 6) & 0x02);
								data[(row * 8) + 1] |= ((plane0 >> 6) & 0x01) | ((plane1 >> 5) & 0x02);
								data[(row * 8) + 2] |= ((plane0 >> 5) & 0x01) | ((plane1 >> 4) & 0x02);
								data[(row * 8) + 3] |= ((plane0 >> 4) & 0x01) | ((plane1 >> 3) & 0x02);
								data[(row * 8) + 4] |= ((plane0 >> 3) & 0x01) | ((plane1 >> 2) & 0x02);
								data[(row * 8) + 5] |= ((plane0 >> 2) & 0x01) | ((plane1 >> 1) & 0x02);
								data[(row * 8) + 6] |= ((plane0 >> 1) & 0x01) | ((plane1 >> 0) & 0x02);
								data[(row * 8) + 7] |= ((plane0 >> 0) & 0x01) | ((plane1 << 1) & 0x02);
							}

							// Plane Row 1
							for (var row = 0; row < 8; row++)
							{
								// Plane 2
								var plane2 = reader.ReadByte();

								// Plane 3
								var plane3 = reader.ReadByte();

								data[(row * 8) + 0] |= ((plane2 >> 5) & 0x04) | ((plane3 >> 4) & 0x08);
								data[(row * 8) + 1] |= ((plane2 >> 4) & 0x04) | ((plane3 >> 3) & 0x08);
								data[(row * 8) + 2] |= ((plane2 >> 3) & 0x04) | ((plane3 >> 2) & 0x08);
								data[(row * 8) + 3] |= ((plane2 >> 2) & 0x04) | ((plane3 >> 1) & 0x08);
								data[(row * 8) + 4] |= ((plane2 >> 1) & 0x04) | ((plane3 >> 0) & 0x08);
								data[(row * 8) + 5] |= ((plane2 >> 0) & 0x04) | ((plane3 << 1) & 0x08);
								data[(row * 8) + 6] |= ((plane2 << 1) & 0x04) | ((plane3 << 2) & 0x08);
								data[(row * 8) + 7] |= ((plane2 << 2) & 0x04) | ((plane3 << 3) & 0x08);
							}

							for (var y = 0; y < 8; y++)
							{
								for (var x = 0; x < 8; x++)
								{
									var value = data[(y * 8) + x];

									//bitmap.SetPixel(x, y, Color.FromArgb(value * 16, value * 16, value * 16));
									bitmap.SetPixel((tileX * 8) + x, (tileY * 8) + y, palette[value]);
								}
							}
						}

						bitmap.Save(fileName);

					}
				}
			}
		}

		private void ExportAllImages()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				// Load Palette
				stream.Position = 0x18b0a;

				var palette = Enumerable.Range(0, 16)
					.Select(n => reader.ReadUInt16())
					.Select(n => Color.FromArgb((n & 0x1f) << 3, (n >> 5 & 0x1f) << 3, (n >> 10) << 3))
					.ToArray();

				stream.Position = 0xb0000;

				while (stream.Position < 0xc0000)
				{
					//var fileName = stream.Position.ToString("D8") + ".bmp";
					//var fileName = stream.Position.ToString("X6") + ".bmp";
					var fileName = stream.Position.ToString("D8") + " - " + stream.Position.ToString("X6") + ".bmp";
					var data = new int[64];

					// Plane Row 0
					for (var row = 0; row < 8; row++)
					{
						// Plane 0
						var plane0 = reader.ReadByte();

						// Plane 1
						var plane1 = reader.ReadByte();

						data[(row * 8) + 0] |= ((plane0 >> 7) & 0x01) | ((plane1 >> 6) & 0x02);
						data[(row * 8) + 1] |= ((plane0 >> 6) & 0x01) | ((plane1 >> 5) & 0x02);
						data[(row * 8) + 2] |= ((plane0 >> 5) & 0x01) | ((plane1 >> 4) & 0x02);
						data[(row * 8) + 3] |= ((plane0 >> 4) & 0x01) | ((plane1 >> 3) & 0x02);
						data[(row * 8) + 4] |= ((plane0 >> 3) & 0x01) | ((plane1 >> 2) & 0x02);
						data[(row * 8) + 5] |= ((plane0 >> 2) & 0x01) | ((plane1 >> 1) & 0x02);
						data[(row * 8) + 6] |= ((plane0 >> 1) & 0x01) | ((plane1 >> 0) & 0x02);
						data[(row * 8) + 7] |= ((plane0 >> 0) & 0x01) | ((plane1 << 1) & 0x02);
					}

					// Plane Row 1
					for (var row = 0; row < 8; row++)
					{
						// Plane 2
						var plane2 = reader.ReadByte();

						// Plane 3
						var plane3 = reader.ReadByte();

						data[(row * 8) + 0] |= ((plane2 >> 5) & 0x04) | ((plane3 >> 4) & 0x08);
						data[(row * 8) + 1] |= ((plane2 >> 4) & 0x04) | ((plane3 >> 3) & 0x08);
						data[(row * 8) + 2] |= ((plane2 >> 3) & 0x04) | ((plane3 >> 2) & 0x08);
						data[(row * 8) + 3] |= ((plane2 >> 2) & 0x04) | ((plane3 >> 1) & 0x08);
						data[(row * 8) + 4] |= ((plane2 >> 1) & 0x04) | ((plane3 >> 0) & 0x08);
						data[(row * 8) + 5] |= ((plane2 >> 0) & 0x04) | ((plane3 << 1) & 0x08);
						data[(row * 8) + 6] |= ((plane2 << 1) & 0x04) | ((plane3 << 2) & 0x08);
						data[(row * 8) + 7] |= ((plane2 << 2) & 0x04) | ((plane3 << 3) & 0x08);
					}

					var bitmap = new Bitmap(8, 8);

					for (var y = 0; y < 8; y++)
					{
						for (var x = 0; x < 8; x++)
						{
							var value = data[(y * 8) + x];

							//bitmap.SetPixel(x, y, Color.FromArgb(value * 16, value * 16, value * 16));
							bitmap.SetPixel(x, y, palette[value]);
						}
					}

					bitmap.Save(fileName);
				}
			}
		}

		private void ExportAudioClips()
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resource))
			using (var reader = new BinaryReader(stream))
			{
				foreach (var clip in AudioClipNames)
				{
					stream.Position = clip.Key;

					var stream2 = File.Create(clip.Key.ToString("X2") + ".wav");
					var writer = new BinaryWriter(stream2);

					writer.Write(Encoding.ASCII.GetBytes("RIFF"));
					var fileLengthPosition = stream2.Position;
					writer.Write(0);
					writer.Write(Encoding.ASCII.GetBytes("WAVE"));

					writer.Write(Encoding.ASCII.GetBytes("fmt "));
					writer.Write(16);
					writer.Write((ushort)1);
					writer.Write((ushort)1);
					//writer.Write(32000);
					//writer.Write(64000);
					writer.Write(8000);
					writer.Write(32000);
					writer.Write((ushort)4);
					writer.Write((ushort)16);

					writer.Write(Encoding.ASCII.GetBytes("data"));
					var dataLengthPosition = stream2.Position;
					writer.Write(0);

					var data2 = new List<short>();
					//var index = 0;

					while (true)
					{
						var header = reader.ReadByte();

						var range = header >> 4;
						var filter = (header >> 2) & 0x3;
						var loop = (header & 0x2) != 0;
						var end = (header & 0x1) != 0;

						var data = reader.ReadBytes(8);
						var samples = data.SelectMany(x => new int[] { SignedNibbleToInt(x >> 4), SignedNibbleToInt(x & 0xf) }).ToArray();
						//var samples = data.SelectMany(x => new int[] { SignedNibbleToInt(x & 0xf), SignedNibbleToInt(x >> 4) }).ToArray();

						foreach (var sample in samples)
						{
							var value = sample << range;

							switch (filter)
							{
								case 1:
									if (data2.Count > 0)
										value += (int)(data2[data2.Count - 1] * (15.0f / 16.0f));
									break;

								case 2:
									if (data2.Count > 1)
										value += (int)((data2[data2.Count - 1] * (61.0f / 32.0f)) - (data2[data2.Count - 2] * (15.0f / 16.0f)));
									break;

								case 3:
									if (data2.Count > 1)
										value += (int)((data2[data2.Count - 1] * (115.0f / 64.0f)) - (data2[data2.Count - 2] * (13.0f / 16.0f)));
									break;
							}

							data2.Add((short)value);
						}

						if (end)
							break;
					}

					foreach (var value in data2)
						writer.Write(value);

					writer.Flush();

					var fileLength = (int)stream2.Position;

					stream2.Position = dataLengthPosition;

					writer.Write(fileLength - 36);

					writer.Flush();

					stream2.Position = fileLengthPosition;

					writer.Write(fileLength - 8);

					writer.Flush();
					writer.Close();
					writer.Dispose();
				}
			}
		}

		private int SignedNibbleToInt(int value)
		{
			if (value < 8)
				return value;

			return value - 16;
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

				var coprocessorEnable = reader.ReadUInt16();
				var breakHandler = reader.ReadUInt16();
				var abortHandler = reader.ReadUInt16();
				var nonMaskableInterruptHandler = reader.ReadUInt16();
				var resetHandler = reader.ReadUInt16();
				var irqHandler = reader.ReadUInt16();

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
					Checksum = checksum,

					CoprocessorEnable = coprocessorEnable,
					BreakHandler = breakHandler,
					AbortHandler = abortHandler,
					NonmaskableInterruptHandler = nonMaskableInterruptHandler,
					ResetHandler = resetHandler,
					IrqHandler = irqHandler
				};
			}
		}

		public static readonly int LevelTableAddress = 0x28000;
		public static readonly int BehaviorTableAddress = 0x2840;
		public static readonly int ModelTableAddress = 0x264b;

		public static readonly int[] ObjectIndexes = new int[255];
		public static readonly int[] BehaviorIndexes = new int[255];
		public static readonly byte[] BehaviorObjects = new byte[255];

		public static readonly Dictionary<int, string> ModelNames = new Dictionary<int, string>
		{
			{ 0xAC15, "Unknown" },
			{ 0xAC31, "Unknown" },
			{ 0xAC4D, "Andross Square" },
			{ 0xAC69, "Black Hole Sprite" },
			{ 0xAC85, "Andross Cube" },
			{ 0xACA1, "Unknown" },
			{ 0xACBD, "Unknown" },
			{ 0xACD9, "Unknown" },
			{ 0xACF5, "Unknown" },
			{ 0xAD11, "Unknown" },
			{ 0xAD2D, "Explosion" },
			{ 0xAD49, "Unknown" },
			{ 0xAD65, "Unknown" },
			{ 0xAD81, "Unknown" },
			{ 0xAD9D, "Unknown" },
			{ 0xADB9, "Unknown" },
			{ 0xADD5, "Unknown" },
			{ 0xADF1, "Unknown" },
			{ 0xAE0D, "Unknown" },
			{ 0xAE29, "Unknown" },
			{ 0xAE45, "Unknown" },
			{ 0xAE61, "Unknown" },
			{ 0xAE7D, "Unknown" },
			{ 0xAE99, "Unknown" },
			{ 0xAEB5, "Unknown" },
			{ 0xAED1, "Unknown" },
			{ 0xAEED, "Unknown" },
			{ 0xAF09, "Unknown" },
			{ 0xAF25, "Unknown" },
			{ 0xAF41, "Unknown" },
			{ 0xAF5D, "Unknown" },
			{ 0xAF79, "Unknown" },
			{ 0xAF95, "Unknown" },
			{ 0xAFB1, "Unknown" },
			{ 0xAFCD, "Big Asteroid" },
			{ 0xAFE9, "Unknown" },
			{ 0xB005, "Unknown" },
			{ 0xB021, "Unknown" },
			{ 0xB03D, "Unknown" },
			{ 0xB059, "Unknown" },
			{ 0xB075, "Unknown" },
			{ 0xB091, "Unknown" },
			{ 0xB0AD, "Unknown" },
			{ 0xB0C9, "Nova bomb" },
			{ 0xB0E5, "Nova bomb 2" },
			{ 0xB101, "Explo 2" },
			{ 0xB11D, "Big Explo 3" },
			{ 0xB139, "Big Explo" },
			{ 0xB155, "Big Explo" },
			{ 0xB171, "Big Explo" },
			{ 0xB18D, "Big Explo" },
			{ 0xB1A9, "Big Explo" },
			{ 0xB1C5, "Big Explo" },
			{ 0xB1E1, "nothing?" },
			{ 0xB1FD, "nothing?" },
			{ 0xB219, "nothing?" },
			{ 0xB235, "”GAME”" },
			{ 0xB251, "”OVER”" },
			{ 0xB26D, "Alien pilot" },
			{ 0xB289, "Sparks" },
			{ 0xB2A5, "Nothing?" },
			{ 0xB2C1, "Flashing antenna" },
			{ 0xB2DD, "Bigger Flashing antenna" },
			{ 0xB2F9, "Bigger Flashing antenna" },
			{ 0xB315, "Flashing antenna" },
			{ 0xB331, "White morphing fire" },
			{ 0xB34D, "Flashing yellow morphing fire" },
			{ 0xB369, "Flashing blue morphing fire" },
			{ 0xB385, "Flashing flat blue morphing shot" },
			{ 0xB3A1, "Weird flat asteroid sprite (unused?)" },
			{ 0xB3BD, "Same but bigger" },
			{ 0xB3D9, "Same but even bigger" },
			{ 0xB3F5, "Walker with leg broken" },
			{ 0xB411, "Walker with leg missing" },
			{ 0xB42D, "Walker walking" },
			{ 0xB449, "Growing column" },
			{ 0xB465, "Bee!" },
			{ 0xB481, "Weird antenna" },
			{ 0xB49D, "Antenna top" },
			{ 0xB4DA, "Hexagonal multi-missile head" },
			{ 0xB4F6, "Slamming outdoor wall" },
			{ 0xB533, "Slamming outdoor wall" },
			{ 0xB54F, "Slamming outdoor wall" },
			{ 0xB56B, "Flat four pronged enemy" },
			{ 0xB587, "Pyramid" },
			{ 0xB5A3, "Flat far away ship" },
			{ 0xB5DB, "Unknown" },
			{ 0xB5FC, "Unknown" },
			{ 0xB601, "Caterpillar head" },
			{ 0xB61D, "Caterpillar part" },
			{ 0xB643, "Cloaking Ship" },
			{ 0xB67B, "Unknown" },
			{ 0xB680, "Caterpillar part" },
			{ 0xB69C, "Caterpillar head part" },
			{ 0xB6B8, "Shadow" },
			{ 0xB6D4, "blue red pill" },
			{ 0xB6F0, "mini ship" },
			{ 0xB728, "mini ship 2" },
			{ 0xB744, "turret" },
			{ 0xB7B4, "Tank track" },
			{ 0xB77C, "Tank track Corner" },
			{ 0xB798, "Tank track Corner" },
			{ 0xB7B9, "mine?" },
			{ 0xB7D5, "shaded cone spike" },
			{ 0xB7F1, "Launcher hovercraft" },
			{ 0xB80D, "Exploding mine part" },
			{ 0xB829, "Exploding mine part" },
			{ 0xB845, "falling column orange" },
			{ 0xB882, "falling column blue" },
			{ 0xB89E, "column orange (A3 B8)" },
			{ 0xB8BF, "Mid level ring" },
			{ 0xB8DB, "Magic door! (unused in game)" },
			{ 0xB8F7, "Plasma Hydra Boss arms" },
			{ 0xB913, "Plasma Hydra Boss arms bulging" },
			{ 0xB92F, "Plasma Hydra Boss body" },
			{ 0xB94B, "Plasma Hydra animated claws" },
			{ 0xBA7F, "neck/tail for Monarch Dodora Boss" },
			{ 0xB983, "Monarch Dodora Boss head" },
			{ 0xB99F, "Monarch Dodora body animation" },
			{ 0xB9BB, "Monarch Dodora Boss tip of tail" },
			{ 0xB9D7, "mini bird" },
			{ 0xB9F3, "Egg" },
			{ 0xBA0F, "Egg broken top" },
			{ 0xBA2B, "Egg broken bottom" },
			{ 0xBA47, "bird boss flapping wing" },
			{ 0xBA63, "bird boss flapping wing" },
			{ 0xBA84, "Motorist" },
			{ 0xBAA0, "Large missile" },
			{ 0xBABC, "dart" },
			{ 0xBAD8, "Big blue Bonus Archway" },
			{ 0xBAF4, "Bonus vertical doorway" },
			{ 0xBB10, "Horiz Arrow sliding door" },
			{ 0xBB2C, "Random poly" },
			{ 0xBB48, "scramble line part" },
			{ 0xBB64, "scramble arches" },
			{ 0xBB80, "scramble tunel" },
			{ 0xBB9C, "Big walker" },
			{ 0xBBB8, "Asteroid flat + enemy" },
			{ 0xBBD4, "Weird vent?" },
			{ 0xBBF0, "Dancing Insector head" },
			{ 0xBC0C, "Dancing Insector Boss leg" },
			{ 0xBC28, "Dancing Insector leg" },
			{ 0xBC44, "Dancing Insector top" },
			{ 0xBC60, "Parrot (Bonus OOTD bird)" },
			{ 0xBC7C, "flashing flat blue hexagon" },
			{ 0xBC98, "Shadow small high poly arwing" },
			{ 0xBCB4, "Small high poly arwing" },
			{ 0xBCD0, "Big high poly arwing" },
			{ 0xBCEC, "Lava erruption" },
			{ 0xBD08, "Slot Machine" },
			{ 0xBD24, "Slot Machine Arm" },
			{ 0xBD40, "Training Ring (Flashing blue)" },
			{ 0xBD5C, "Andross Cube + pyramids" },
			{ 0xBD78, "Tower top" },
			{ 0xBD94, "Yellow-red flashing shot" },
			{ 0xBDB0, "Rock formation (irregular gray shape)" },
			{ 0xBDCC, "Four pronged space thing" },
			{ 0xBDE8, "Diafragm Door" },
			{ 0xBE04, "Rock reverse faces" },
			{ 0xBE20, "Rock reverse faces and broken" },
			{ 0xBE3C, "Rock Crusher Boss part" },
			{ 0xBE58, "Rock Crusher Boss flashing dimple" },
			{ 0xBE74, "Rock Crusher Boss rotating part" },
			{ 0xBE90, "Spinning Core bumpers" },
			{ 0xBEAC, "Electric thing under Spinning Core Boss" },
			{ 0xBEC8, "Spinning Core Boss top" },
			{ 0xBEE4, "Spinning Core flaps up" },
			{ 0xBF00, "Spinning Core flaps down" },
			{ 0xBF1C, "Spinning Core flaps parts closed" },
			{ 0xBF38, "Spinning Core flaps parts closed" },
			{ 0xBF54, "Black lines" },
			{ 0xBF70, "lines" },
			{ 0xBF8C, "lines" },
			{ 0xBFA8, "lines" },
			{ 0xBFC4, "lines" },
			{ 0xBFE0, "lines" },
			{ 0xBFFC, "mess" },
			{ 0xC018, "Armada Battleship entry" },
			{ 0xC034, "Atomic Base entry" },
			{ 0xC050, "Atomic Base entry zoom" },
			{ 0xC06C, "Weird black/orange button" },
			{ 0xC088, "Weird portal" },
			{ 0xC0A4, "Weird portal 2" },
			{ 0xC0C0, "Weird portal wire frame" },
			{ 0xC0DC, "Weird portal wire frame 2" },
			{ 0xC0F8, "Flashing yellow stripe" },
			{ 0xC114, "Black rectangle" },
			{ 0xC130, "Gray rectangle" },
			{ 0xC14C, "Black large rectangle" },
			{ 0xC168, "Gray rectangle" },
			{ 0xC184, "Gray rectangle smaller" },
			{ 0xC1A0, "Gray rectangle large" },
			{ 0xC1BC, "Inside black tunnel" },
			{ 0xC1D8, "Inside gray tunnel" },
			{ 0xC1F4, "little tower top" },
			{ 0xC210, "box with flashing top" },
			{ 0xC22C, "Butterfly" },
			{ 0xC248, "Attack carrier right launcher" },
			{ 0xC264, "Attack carrier middle part" },
			{ 0xC280, "Attack carrier low poly middle" },
			{ 0xC29C, "Attack carrier left shield" },
			{ 0xC2B8, "A-carrier top left closing" },
			{ 0xC2D4, "A-carrier down left closing" },
			{ 0xC2F0, "A-carrier mini-ships" },
			{ 0xC30C, "Yellow flying enemy" },
			{ 0xC328, "Blue flying enemy" },
			{ 0xC344, "Enemy wing" },
			{ 0xC360, "wireframe shielded Arwing" },
			{ 0xC37C, "wireframe Arwing no right wing" },
			{ 0xC398, "wireframe Arwing no left wing" },
			{ 0xC3B4, "wireframe Arwing no wings" },
			{ 0xC3D0, "Interlocking doors opening" },
			{ 0xC3EC, "Antenna dish" },
			{ 0xC408, "Antenna base" },
			{ 0xC424, "Pylone" },
			{ 0xC440, "Pylone 2" },
			{ 0xC45C, "Twin flasher/repair enemy" },
			{ 0xC478, "Twin flasher item" },
			{ 0xC494, "Nova bomb item" },
			{ 0xC4B0, "Shield item" },
			{ 0xC4CC, "Wing Repair item" },
			{ 0xC4E8, "unused (filled cube shield)" },
			{ 0xC504, "mine, blue/red" },
			{ 0xC520, "Repairing Arwing" },
			{ 0xC53C, "Hexagonal mini Hover tank" },
			{ 0xC558, "Enemy with some wireframe" },
			{ 0xC574, "Morphing boss part (round room)" },
			{ 0xC590, "Atomic Base core opening" },
			{ 0xC5AC, "Atomic Base core opening 2" },
			{ 0xC5C8, "Half monolyte (white)" },
			{ 0xC5E4, "Atomic base targets" },
			{ 0xC600, "Atomic base electric arcs" },
			{ 0xC61C, "Atomic Base beaks" },
			{ 0xC638, "Exploded polys" },
			{ 0xC654, "Slot flashing button" },
			{ 0xC670, "Slot disabled button" },
			{ 0xC68C, "A-Carrier2 rotating heads" },
			{ 0xC6A8, "A-Carrier2 main head" },
			{ 0xC6C4, "A-Carrier2 tank base" },
			{ 0xC6E0, "Armada ship core stand" },
			{ 0xC6FC, "Armada ship core" },
			{ 0xC718, "Unknown boss or portal part" },
			{ 0xC734, "A-Carrier2 tank middle" },
			{ 0xC750, "A-Carrier2 tank right" },
			{ 0xC76C, "A-Carrier2 tank left" },
			{ 0xC788, "Phantron transformation/jumping animation" },
			{ 0xC7A4, "Phantron closed" },
			{ 0xC7C0, "Mountain with antennas" },
			{ 0xC7DC, "Manned pod ship" },
			{ 0xC7F8, "Winged fat ship" },
			{ 0xC814, "Phantron leg" },
			{ 0xC830, "Phantron leg" },
			{ 0xC84C, "Wingling tail enemy" },
			{ 0xC868, "Small Bonus Archway" },
			{ 0xC884, "Long cannon tank" },
			{ 0xC8A0, "Mothership part" },
			{ 0xC8BC, "Octogonal tunnel blue" },
			{ 0xC8D8, "Rectangle tunnel orange" },
			{ 0xC8F4, "Letter D orange" },
			{ 0xC910, "Letter D blue" },
			{ 0xC92C, "Scenery White box" },
			{ 0xC948, "Scenery small White box" },
			{ 0xC964, "Scenery White high rise box" },
			{ 0xC980, "Galactic rider door" },
			{ 0xC99C, "Galactic rider" },
			{ 0xC9B8, "Base shooting red/blue rings" },
			{ 0xC9D4, "Metal Crusher right" },
			{ 0xC9F0, "Metal Crusher left" },
			{ 0xCA0C, "Metal Crusher again" },
			{ 0xCA28, "Space Truck" },
			{ 0xCA44, "Boss three prong opening top" },
			{ 0xCA60, "Boss three prong opening top" },
			{ 0xCA7C, "Hexa orange with black top" },
			{ 0xCA98, "Atomic base part" },
			{ 0xCAB4, "Web" },
			{ 0xCAD0, "Yellow flat octogon" },
			{ 0xCAEC, "White prism" },
			{ 0xCB08, "Vertical Orange white column" },
			{ 0xCB24, "Big mountain with entry" },
			{ 0xCB40, "Fake flat mountain entry" },
			{ 0xCB5C, "Long orange tunnel" },
			{ 0xCB78, "Orange/gray tunnel" },
			{ 0xCB94, "Professor Hanger" },
			{ 0xCBB0, "Vertical opening interlock doors" },
			{ 0xCBCC, "Mountain part orange" },
			{ 0xCBE8, "Mountain part blue" },
			{ 0xCC04, "Mountain part white" },
			{ 0xCC20, "Mountain part 2 orange" },
			{ 0xCC3C, "Mountain part 2 blue" },
			{ 0xCC58, "Mountain part 2 white" },
			{ 0xCC74, "Mountain part 3 orange" },
			{ 0xCC90, "Unknown" },
			{ 0xCCAC, "Unknown" },
			{ 0xCCC8, "Mountain part 4 orange" },
			{ 0xCCE4, "Unknown" },
			{ 0xCD00, "Unknown" },
			{ 0xCD1C, "Mountain part 5 orange" },
			{ 0xCD38, "Unknown" },
			{ 0xCD54, "Unknown" },
			{ 0xCD70, "Mountain part 6 orange" },
			{ 0xCD8C, "Unknown" },
			{ 0xCDA8, "Unknown" },
			{ 0xCDC4, "Helicopter" },
			{ 0xCDE0, "Metal Crusher mines" },
			{ 0xCDFC, "Mountain part 7 orange" },
			{ 0xCE18, "Little mountain blue" },
			{ 0xCE34, "Little mountain white" },
			{ 0xCE50, "Flying Fish" },
			{ 0xCE6C, "Growing green leaf" },
			{ 0xCE88, "Blossoming flower" },
			{ 0xCEA4, "Water Dragon/Flower part" },
			{ 0xCEC0, "Water Dragon head" },
			{ 0xCEDC, "Flower/Dragon part?" },
			{ 0xCEF8, "Flower part" },
			{ 0xCF14, "Flower part" },
			{ 0xCF30, "Two sides Blue square" },
			{ 0xCF4C, "Small Flower" },
			{ 0xCF68, "Big Flower" },
			{ 0xCF84, "Flower part?" },
			{ 0xCFA0, "Flower part" },
			{ 0xCFBC, "Spider Boss head" },
			{ 0xCFD8, "Spider Boss leg" },
			{ 0xCFF4, "Spider Boss leg 2" },
			{ 0xD010, "Big vertical Entryway" },
			{ 0xD02C, "Long Octo Tunnel" },
			{ 0xD048, "Diafragm Door Blue" },
			{ 0xD064, "Interlocking Doors" },
			{ 0xD080, "?? part" },
			{ 0xD09C, "Vertical Creature" },
			{ 0xD0B8, "Flashing White thing" },
			{ 0xD0D4, "Flashing White Monolyte" },
			{ 0xD0F0, "Pale Monolyte" },
			{ 0xD10C, "Andross Face Squares" },
			{ 0xD128, "Andross Face Squares big" },
			{ 0xD144, "Andross Face morph" },
			{ 0xD160, "Andross Face morph 2" },
			{ 0xD17C, "Prof Hanger part" },
			{ 0xD198, "Vertical Tunnel Door" },
			{ 0xD1B4, "Tunnel part" },
			{ 0xD1D0, "Tunnel part orange" },
			{ 0xD1EC, "Tunnel part orange" },
			{ 0xD208, "Tunnel part orange" },
			{ 0xD224, "Tunnel part" },
			{ 0xD240, "Tunnel part orange" },
			{ 0xD25C, "Tunnel twisting part" },
			{ 0xD278, "Orange Electronic Cube" },
			{ 0xD294, "Orange Electronic Column" },
			{ 0xD2B0, "Orange Electronic Column" },
			{ 0xD2CC, "nothing?" },
			{ 0xD2E8, "Shadow arwing" },
			{ 0xD304, "Normal arwing" },
			{ 0xD320, "Normal arwing" },
			{ 0xD33C, "Normal arwing" },
			{ 0xD358, "Shadow arwing no right wing" },
			{ 0xD374, "Arwing no right wing" },
			{ 0xD390, "Shadow arwing no left wing" },
			{ 0xD3AC, "Arwing no left wing" },
			{ 0xD3C8, "Shadow arwing no wings" },
			{ 0xD3E4, "Arwing no wings" },
			{ 0xD400, "Simple White building" },
			{ 0xD41C, "Buildings (4) one blue high" },
			{ 0xD438, "Buildings (3) one orange high" },
			{ 0xD454, "Buildings (4) all high rise" },
			{ 0xD470, "Building with Fox logo" },
			{ 0xD48C, "Great Commander Boss" },
			{ 0xD4A8, "Great Commander Head" },
			{ 0xD4C4, "Great Commander Head" },
			{ 0xD4E0, "Great Commander trunk with rotating arms" },
			{ 0xD4FC, "Great Commander base with opening vent" },
			{ 0xD518, "Great Commander whole, not animated" },
			{ 0xD534, "Great Commander part" },
			{ 0xD550, "Great Commander left arm" },
			{ 0xD56C, "Great Commander right arm" },
			{ 0xD588, "Opening Great Commander vent" },
			{ 0xD5A4, "Opening Great Commander vent" },
			{ 0xD5C0, "Explosion crater" },
			{ 0xD5DC, "Explosion crater" },
			{ 0xD5F8, "Unused?" },
			{ 0xD614, "Unused?" },
			{ 0xD630, "Some horizontal beam" },
			{ 0xD64C, "Some horizontal beam" },
			{ 0xD668, "Beveled White box" },
			{ 0xD684, "Beveled White square" },
			{ 0xD6A0, "Beveled White beam" },
			{ 0xD6BC, "Beveled White beam" },
			{ 0xD6D8, "Beveled White square" },
			{ 0xD6F4, "Beveled White beam" },
			{ 0xD710, "Rising Gray beam" },
			{ 0xD72C, "Rising arch" },
			{ 0xD748, "Arwing Cockpit" },
			{ 0xD764, "Big low poly arwing" },
			{ 0xD780, "Big arwing (no wings)" },
			{ 0xD79C, "Big arwing (no R wings)" },
			{ 0xD7B8, "Big arwing (no L wings)" },
			{ 0xD7D4, "PHANTRON Kicking" },
			{ 0xD7F0, "PHANTRON part" },
			{ 0xD80C, "Dropped by some boss" },
			{ 0xD828, "Rotating Artsy thing" },
			{ 0xD844, "Rotating Artsy thing" },
			{ 0xD860, "Box thing" },
			{ 0xD87C, "Square thing" },
			{ 0xD898, "Box thing" },
			{ 0xD8B4, "Box thing" },
			{ 0xD8D0, "Box thing high" },
			{ 0xD8EC, "Blue beam white ends" },
			{ 0xD908, "Hexagonal blue beam" },
			{ 0xD924, "Enemy ship" },
			{ 0xD940, "Enemy ship half" },
			{ 0xD95C, "Enemy Cargo ship" },
			{ 0xD978, "Enemy Cargo ship" },
			{ 0xD994, "Enemy Cargo ship" },
			{ 0xD9B0, "Enemy Cargo ship   Low poly" },
			{ 0xD9CC, "Enemy Cargo ship   Low poly" },
			{ 0xD9E8, "Enemy Cargo ship   Low poly" },
			{ 0xDA04, "Far away ships (flat)" },
			{ 0xDA20, "Interlocking doors" },
			{ 0xDA3C, "Interlocking corner doors" },
			{ 0xDA58, "Interlocking diagonal doors" },
			{ 0xDA74, "Interlocking diagonal doors (s)" },
			{ 0xDA90, "Enemy flapping wings and tail" },
			{ 0xDAAC, "Boss poly part?" },
			{ 0xDAC8, "Boss poly part triangle" },
			{ 0xDAE4, "Boss poly part triangle" },
			{ 0xDB00, "Boss poly part triangle" },
			{ 0xDB1C, "Rising orange beam in tunnel" },
			{ 0xDB38, "Enemy ship" },
			{ 0xDB54, "Growing missile" },
			{ 0xDB70, "Main enemy ship (orange)" },
			{ 0xDB8C, "Mini walker" },
			{ 0xDBA8, "Ringo enemy" },
			{ 0xDBC4, "Enemy with rotating wings" },
			{ 0xDBE0, "Growing beam with flashing top" },
			{ 0xDBFC, "Sliding door in tunnel" },
			{ 0xDC18, "Coins" },
			{ 0xDC34, "Flashing Slot Maching knob" },
			{ 0xDC50, "Slot Machine Fruit texture" },
			{ 0xDC6C, "Hexa tower with flashing top" },
			{ 0xDC88, "Origami 1" },
			{ 0xDCA4, "Paper plane" },
			{ 0xDCC0, "Warning beam (yellow/black)" },
			{ 0xDCDC, "Tunnel side Textured Blocks" },
			{ 0xDCF8, "Road side Textured Blocks" },
			{ 0xDD14, "nothing?" },
			{ 0xDD30, "Shootable cone" },
			{ 0xDD4C, "High rise with flashing corner" },
			{ 0xDD68, "Big low poly Arwing (2)" },
			{ 0xDD84, "Corneria Base" },
			{ 0xDDA0, "Flat long rectangle" },
			{ 0xDDBC, "Random polys" },
			{ 0xDDF4, "Part some ship (DD08)" },
			{ 0xDE10, "Micro Walker" },
			{ 0xDE2C, "Nano Walker" },
			{ 0xDE48, "Machine Gun ship" },
			{ 0xDE64, "Machine Gun ship Part (gun)" },
			{ 0xDE80, "Machine Gun ship Part body" },
			{ 0xDE9C, "Space missile mines" },
			{ 0xDEB8, "Shieldable enemy" },
			{ 0xDED4, "Shielded enemy" },
			{ 0xDEF0, "Shielded enemy 2" },
			{ 0xDF0C, "Shielded enemy cloaking" },
			{ 0xDF28, "Big launcher tank" },
			{ 0xDF44, "Libellule" },
			{ 0xDF60, "Other smaller Libellule" },
			{ 0xDF7C, "Flashing wireframe beam" },
			{ 0xDF98, "Flashing beam" },
			{ 0xDFB4, "Flashing beam" },
			{ 0xDFD0, "Flashing beam" },
			{ 0xDFEC, "Flashing beam" },
			{ 0xE008, "Flashing beam" },
			{ 0xE024, "Flashing beam" },
			{ 0xE040, "Flashing beam" },
			{ 0xE05C, "Flashing beam" },
			{ 0xE078, "Flashing beam" },
			{ 0xE094, "Flashing beam" },
			{ 0xE0B0, "Flashing beam" },
			{ 0xE0CC, "Flashing beam" },
			{ 0xE0E8, "Flashing beam" },
			{ 0xE104, "Flashing beam (wireframe)" },
			{ 0xE120, "Flashing beam" },
			{ 0xE13C, "Flashing beam (wireframe)" },
			{ 0xE158, "Flashing beam" },
			{ 0xE174, "Flashing beam (wireframe)" },
			{ 0xE190, "Flashing beam" },
			{ 0xE1AC, "Ship Entryway (with antennas)" },
			{ 0xE1C8, "Ship Entryway part?" },
			{ 0xE1E4, "Space half arch" },
			{ 0xE200, "Space half arch big" },
			{ 0xE21C, "Space other half arch big" },
			{ 0xE238, "Rotating pod enemy orange" },
			{ 0xE254, "Rotating pod enemy white" },
			{ 0xE270, "Rotating pod enemy white" },
			{ 0xE28C, "Rotating pod enemy blue" },
			{ 0xE2A8, "Rotating pod enemy blue" },
			{ 0xE2C4, "Medium Walker" },
			{ 0xE2E0, "Mini Volcano" },
			{ 0xE2FC, "Big Volcano" },
			{ 0xE318, "Portable hole" },
			{ 0xE334, "Cool missile" },
			{ 0xE350, "Mini Vertical Enemy" },
			{ 0xE36C, "Main enemy 2 (orange)" },
			{ 0xE388, "Main enemy 3 big wings" },
			{ 0xE3A4, "Small yellow stingray" },
			{ 0xE3C0, "Small blue stingray" },
			{ 0xE3DC, "Big blue stingray" },
			{ 0xE3F8, "Big red stingray" },
			{ 0xE414, "Crystal in ground" },
			{ 0xE430, "Crystal in ground (vertical)" },
			{ 0xE44C, "Flying crystal" },
			{ 0xE468, "Hovercar (road level)" },
			{ 0xE484, "Skii Truck  (road level)" },
			{ 0xE4A0, "Unknown" },
			{ 0xE4BC, "Some Enemy part" },
			{ 0xE4D8, "Energy yellow ring" },
			{ 0xE4F4, "Mini Fish" },
			{ 0xE510, "Other Machine gun enemy" },
			{ 0xE52C, "Swimming creature" },
			{ 0xE548, "White flat stripe" },
			{ 0xE564, "Flat thing" },
			{ 0xE580, "White arrow" },
			{ 0xE59C, "Tunnel with blue doors" },
			{ 0xE5B8, "Orange tunnel" },
			{ 0xE5D4, "Tunnel with blue doors 2" },
			{ 0xE5F0, "Federation friend ship" },
			{ 0xE60C, "Federation friend ship (half)" },
			{ 0xE628, "Mothership part 2" },
			{ 0xE644, "Mothership center part" },
			{ 0xE660, "Bonus bird" },
			{ 0xE67C, "Whale" },
			{ 0xE698, "Rotating solid beam with direction arrows" },
			{ 0xE6B4, "Andross 2 Evil Face" },
			{ 0xE6D0, "UFO" },
			{ 0xE6EC, "Small yellow tank" },
			{ 0xE708, "Small blue tank" },
			{ 0xE724, "Weird asteroid" },
			{ 0xE740, "Tunnel with yellow door" },
			{ 0xE75C, "Tunnel with yellow door" },
			{ 0xE778, "Morphing ground thing" },
			{ 0xE794, "Bonus doors (yellow)" },
			{ 0xE7B0, "Letter ”E” orange" },
			{ 0xE7CC, "Letter ”E” blue" },
			{ 0xE7E8, "Letter ”E” gray" },
			{ 0xE804, "Letter ”T” orange" },
			{ 0xE820, "Letter ”T” blue" },
			{ 0xE83C, "Letter ”T” gray" },
			{ 0xE858, "Letter ”N” orange" },
			{ 0xE874, "Letter ”H” blue" },
			{ 0xE890, "Letter ”H” gray" }
		};

		public static readonly Dictionary<int, string> BehaviorNames = new Dictionary<int, string>
		{
			{ 0x000000, "Unknown" },
			{ 0x005800, "Rotate Z-Axis Clockwise" },
			{ 0x006200, "Rotate Z-Axis Counter Clockwise" },
			{ 0x00F99F, "Unknown" },
			{ 0x00FA62, "Vertical Sliding Door w/ Arrows" },
			{ 0x00FAA5, "Big Asteroid" },
			{ 0x00FABA, "Exploding Core Ship" },
			{ 0x034C00, "Shoot 5 Rings" },
			{ 0x048004, "Training Ring" },
			{ 0x04800E, "UFO 2" },
			{ 0x048022, "Point Far Away, Up/Down" },
			{ 0x04EE15, "Giant Mountain Entry" },
			{ 0x04F667, "Unknown" },
			{ 0x04F870, "Interlocking Vertical Sliding Door Entry" },
			{ 0x04F8F9, "Tunnel Part (Orange)" },
			{ 0x04F8FF, "UFO" },
			{ 0x04F9C0, "Interlocking Vertical Sliding Door Entry 2" },
			{ 0x050000, "Unknown" },
			{ 0x050005, "Unknown" },
			{ 0x068593, "Unknown" },
			{ 0x068599, "Unknown" },
			{ 0x0685AF, "Solid Object" },
			{ 0x0685B9, "Walker" },
			{ 0x0685D9, "Active Volcano" },
			{ 0x0685F9, "Flying Fish" },
			{ 0x06862D, "Rising Beam" },
			{ 0x06864B, "Unknown" },
			{ 0x068656, "Unknown" },
			{ 0x068738, "Camera Flies In From Above" },
			{ 0x06873F, "Tunnel/Corneria Base" },
			{ 0x06A4EE, "High Poly Arwing Fly By" },
			{ 0x06BE05, "Arwing Fly Forward" },
			{ 0x06BE53, "Arwing Mimic Player" },
			{ 0x06BEA1, "Arwing Mimic Player 2" },
			{ 0x06BF90, "Black Tunnel Entry" },
			{ 0x06BFDF, "Arwing Bank Right Fly Away" },
			{ 0x06C02E, "Arwing Bank Left Fly Away" },
			{ 0x06C196, "Arwing Moving" },
			{ 0x06C1FA, "Arwing Moving 2" },
			{ 0x06C25E, "Arwing Moving 3" },
			{ 0x06C2BE, "Arwing Mimic Player 3" },
			{ 0x06C2C8, "Arwing Fly Away" },
			{ 0x06C30E, "Arwing Moving 4" },
			{ 0x06C318, "Unknown" },
			{ 0x06C35E, "Arwing Moving 5" },
			{ 0x06C368, "Unknown" },
			{ 0x06C4CE, "Arwing Moving 6" },
			{ 0x06C52D, "Arwing Mimic Player 4" },
			{ 0x06C58C, "Arwing Mimic Player 5" },
			{ 0x06C70A, "Arwing Mimic Player 6" },
			{ 0x06C75D, "Arwing Bank Right Fly Up" },
			{ 0x06C7B0, "Arwing Bank Left Fly Up" },
			{ 0x06C920, "Arwing Fly Forward 2" },
			{ 0x06C9F3, "Arwing Right Left Up" },
			{ 0x06CAC6, "Arwing Left Right Up" },
			{ 0x06CC0B, "Arwing Fly Right Up" },
			{ 0x06CC5E, "Arwing Bank Right Fly Forward" },
			{ 0x06CCB1, "Arwing Bank Left Fly Forward" },
			{ 0x06CE18, "Little Mountain (Blue)" },
			{ 0x06CE75, "Arwing Bank Right Fly Left" },
			{ 0x06CED2, "Arwing Bank Left Fly Right Up" },
			{ 0x06D2A5, "Andross Mask" },
			{ 0x06D30B, "Vertical Opening Doors" },
			{ 0x06D470, "Door Open Ground" },
			{ 0x06D7C0, "End Level w/ Music and Camera" },
			{ 0x06D9C9, "Unknown" },
			{ 0x06DB45, "Diaphram Door (Blue)" },
			{ 0x06DD51, "Camera Top Down" },
			{ 0x06E831, "Unknown" },
			{ 0x06F96F, "Unknown" },
			{ 0x078004, "White Arrow" },
			{ 0x078137, "Unknown" },
			{ 0x078507, "Solid Object 2" },
			{ 0x078530, "Long Octo Tunnel" },
			{ 0x0785BD, "Door Ground" },
			{ 0x078673, "Shootable Orange Asteroid" },
			{ 0x078CA3, "Mothership Center" },
			{ 0x07919C, "Unknown" },
			{ 0x079815, "Jumping Water Creature" },
			{ 0x0799C3, "Unknown" },
			{ 0x079B0F, "Atomic Base Boss" },
			{ 0x07AAD1, "Unknown" },
			{ 0x07B848, "Monarch Dodora Boss" },
			{ 0x07CF7C, "Tower Top Cannon" },
			{ 0x07DEAB, "Mini Ski Truck" },
			{ 0x08816E, "Object Explodes" },
			{ 0x088BBE, "Attacking 3-Leg Pod" },
			{ 0x089B15, "Destructor Boss" },
			{ 0x089CC1, "Diamond Hovering" },
			{ 0x089E1F, "Armada Cargo Ship" },
			{ 0x08A035, "Armada Cargo Ship w/ Entry" },
			{ 0x08A258, "Red Diamond" },
			{ 0x08A299, "Unknown" },
			{ 0x08A386, "Rock Crusher Boss" },
			{ 0x08A5EA, "Spinning Core Boss" },
			{ 0x08A6DB, "Unknown" },
			{ 0x08A8F8, "Atomic Base Core" },
			{ 0x08AB37, "Unknown" },
			{ 0x08AC8B, "Antenna Dish Base" },
			{ 0x08AE0F, "Metal Smasher Boss" },
			{ 0x08B176, "Enemy Ship (Static)" },
			{ 0x08B194, "Rotating Antenna Dish" },
			{ 0x08B19E, "Enemy Up Down Far" },
			{ 0x08B1CB, "Enemy Loop" },
			{ 0x08B1DB, "Enemy Up Down Away" },
			{ 0x08B539, "Tower Top Cannon" },
			{ 0x08B56F, "Enemy Up Down Away 2" },
			{ 0x08B69F, "Tank Track" },
			{ 0x08B7C1, "Checkpoint Ring" },
			{ 0x08B93F, "Cloaking Enemy" },
			{ 0x08BE2C, "Black Hole Item Box" },
			{ 0x08BF70, "Cannon Shooting Missles" },
			{ 0x08C0E9, "Unknown" },
			{ 0x08C2AD, "Smoking Missle" },
			{ 0x08C777, "Exploding Column (Blue)" },
			{ 0x08C858, "Flying Fish" },
			{ 0x08CAD1, "Tank Shooting Missles" },
			{ 0x08CB12, "Unknown" },
			{ 0x08CCDB, "Catepillar Part" },
			{ 0x08CFE8, "Unknown" },
			{ 0x08D075, "Armada Entry" },
			{ 0x08D249, "Hovering Box" },
			{ 0x08D414, "Enemy Path" },
			{ 0x08D556, "Horizonal Beam (Static)" },
			{ 0x08D5F0, "Nova Bomb Item" },
			{ 0x08D85A, "Hexagonal Tank Shooting" },
			{ 0x08DB39, "Extra Credit" },
			{ 0x08DDEC, "Shield Item" },
			{ 0x08E32A, "CLoaking Enemy 2" },
			{ 0x08E602, "Four Pronged Enemy Fly Away" },
			{ 0x08E8AE, "Unknown" },
			{ 0x08EACC, "Unknown" },
			{ 0x08EBB2, "Unknown" },
			{ 0x08F145, "Gray Tunnel" },
			{ 0x08F15D, "Space Arch Right" },
			{ 0x08F18B, "Unknown" },
			{ 0x08F193, "Unknown" },
			{ 0x08F19B, "Unknown" },
			{ 0x08F1A3, "Unknown" },
			{ 0x08F1F7, "Blue Arch Door Open/Close" },
			{ 0x08F200, "Falco: The Attack Carrier Will Be Mine" },
			{ 0x08F206, "Unknown" },
			{ 0x098AA4, "Big Walker Walking AWay" },
			{ 0x098BC6, "Slamming Wall Right" },
			{ 0x099108, "Unknown" },
			{ 0x099171, "Slamming Wall Right 2" },
			{ 0x09917A, "Slamming Wall Left" },
			{ 0x099529, "Unknown" },
			{ 0x0995A1, "Unknown" },
			{ 0x099686, "Bonus Archway" },
			{ 0x0997DA, "Enemy (Catepillar)" },
			{ 0x099997, "Water Dragon" },
			{ 0x0999A9, "Asteroid w/ Face (Black Hole Warp)" },
			{ 0x099A70, "Flower Growing Right" },
			{ 0x099A78, "Flower Growing" },
			{ 0x099B37, "Highway Car Turning Left" },
			{ 0x09AA4F, "Unknown" },
			{ 0x09B259, "Unknown" },
			{ 0x09B271, "Plasma Hydra Boss" },
			{ 0x09B31D, "Great Commander Boss" },
			{ 0x09C160, "Phantron Boss 2" },
			{ 0x09C54F, "Galactic Rider" },
			{ 0x09C562, "Motorist" },
			{ 0x09CC60, "Blade Barrier Base" },
			{ 0x09D77F, "Unknown" },
			{ 0x09E128, "Unknown" },
			{ 0x09F72B, "Professor Hangar Boss" },
			{ 0x09F737, "Triangle (Static)" },
			{ 0x09F743, "Triangle 2" },
			{ 0x09F74F, "Triangle 3" },
			{ 0x09F75B, "Unknown" },
			{ 0x09F763, "Triangle 4" },
			{ 0x09F767, "Unknown" },
			{ 0x09F856, "Unknown" },
			{ 0x0A0000, "Unknown" },
			{ 0x0A8004, "Ring Enemy" },
			{ 0x0A8035, "Attack Carrier Boss" },
			{ 0x0A8489, "Pylon" },
			{ 0x0A8631, "Unknown" },
			{ 0x0A8790, "Great Commander Boss 2" },
			{ 0x0A87AA, "Invisible Enemy" },
			{ 0x0A87C4, "Unknown" },
			{ 0x0A881B, "Unknown" },
			{ 0x0A88AC, "Invisible Enemy 2" },
			{ 0x0A8972, "Unknown" },
			{ 0x0A8A84, "White Tank" },
			{ 0x0A8AD4, "Unknown" },
			{ 0x0A8CFC, "Unknown" },
			{ 0x0A8F35, "Orange Tank Reversing/Shooting" },
			{ 0x0A9029, "Unknown" },
			{ 0x0A90E9, "Unknown" },
			{ 0x0A9211, "Deferred Behavior" },
			{ 0x0A9216, "Warp Yellow Ring" },
			{ 0x0A921B, "Warp Yellow Ring 2" },
			{ 0x0A9F67, "Buildings" },
			{ 0x0AA197, "Unknown" },
			{ 0x0AA4EF, "Orange Enemy Fly Away" },
			{ 0x0AAF28, "Enemy Ship Evasive" },
			{ 0x0AB01C, "Big Cargo Ship" },
			{ 0x0AB304, "Big Cargo Ship 2" },
			{ 0x0AB34C, "Nano Walker Shooting/Jumping" },
			{ 0x0AB50A, "Cargo Ships (Far)" },
			{ 0x0AB5FD, "Rising Orange Bar" },
			{ 0x0AB69C, "Interlocking Diagonal Doors" },
			{ 0x0AB721, "Normal Arwing" },
			{ 0x0AB727, "Unknown" },
			{ 0x0ABB1F, "Unknown" },
			{ 0x0ABB91, "Big Tank w/ Launcher" },
			{ 0x0ABBFA, "Unknown" },
			{ 0x0ABF4D, "THE END" },
			{ 0x0ABF53, "Machine Gun Ship Left/Right" },
			{ 0x0AC24B, "Machine Gun Ship Right/Left" },
			{ 0x0AC359, "Space Missile/Mine" },
			{ 0x0AC697, "Shieldable Enemy" },
			{ 0x0ACC0D, "Atomic Base Entry" },
			{ 0x0ACC54, "Horizontal Beam Up/Down" },
			{ 0x0ACF37, "Unknown" },
			{ 0x0ACF7B, "Horizontal Beam" },
			{ 0x0ACFD3, "Horizontal Beam 2" },
			{ 0x0AD189, "Horizontal Beam 3" },
			{ 0x0AD251, "Horizontal Beam 4" },
			{ 0x0AD417, "Ship Entry w/ Antenna" },
			{ 0x0AD44B, "Ship Entry" },
			{ 0x0AD4D0, "Rotating Blue Pod" },
			{ 0x0AD567, "Mini Walker Shooting" },
			{ 0x0AD5F1, "Rotating White Pod Shooting" },
			{ 0x0AD5F7, "Twin Blaster Item" },
			{ 0x0AD707, "Rotating Orange Pod Shooting" },
			{ 0x0AD76B, "Unknown" },
			{ 0x0ADAC6, "Invisible Enemy" },
			{ 0x0ADD08, "Active Volcano Flipped" },
			{ 0x0ADE06, "Asteroid w/ Enemy" },
			{ 0x0AE08A, "Small Water Dragon" },
			{ 0x0AE131, "Highway Car" },
			{ 0x0AE233, "Highway Car 2" },
			{ 0x0AE38F, "Highway Car 3" },
			{ 0x0AE478, "Highway Car 4" },
			{ 0x0AE5B5, "Interlocking Doors" },
			{ 0x0AE5F5, "Mini Ski Truck" },
			{ 0x0AE67F, "Unknown" },
			{ 0x0AE8B7, "Unknown" },
			{ 0x0AE9E8, "Unknown" },
			{ 0x0AEEDE, "Unknown" },
			{ 0x0AF0EC, "Hit Sound" },
			{ 0x0AF17D, "Shootable Horizontal Sliding Doors" },
			{ 0x0AF235, "Shootable Orange Asteroid" },
			{ 0x0AF7A6, "Warp Yellow Ring" },
			{ 0x0BA23D, "Unknown" },
			{ 0x0BADB9, "Lock Camera" },
			{ 0x0BAFF5, "Unknown" },
			{ 0x0BB29B, "Unknown" },
			{ 0x0BEBB3, "Phantron Boss Loop" },
			{ 0x15AC15, "Unknown" },
			{ 0x1FF600, "Bonus Counter" },
			{ 0xD80045, "Unknown" }
		};

		public static readonly Dictionary<int, string> AudioClipNames = new Dictionary<int, string>
		{
			{ 0xc292a, "Unknown" },
			{ 0xc2b22, "Unknown" },
			{ 0xc2f00, "Unknown" },
			{ 0xc36c3, "Unknown" },
			{ 0xc3a64, "Unknown" },
			{ 0xc3fc5, "Unknown" },
			{ 0xc4589, "Unknown" },
			{ 0xc512c, "Unknown" },
			{ 0xc5ebe, "Unknown" },
			{ 0xc6482, "Unknown" },
			{ 0xc64e5, "Unknown" },
			{ 0xc679a, "Unknown" },
			{ 0xc6d31, "Unknown" },
			{ 0xc7547, "Unknown" },
			{ 0xc775a, "Unknown" },
			{ 0xc7f0d, "Unknown" },
			{ 0xc877d, "Unknown" },
			{ 0xc8b1c, "Unknown" },
			{ 0xc8fc9, "Unknown" },
			{ 0xc9572, "Unknown" },
			{ 0xc976a, "Unknown" },
			{ 0xc9ad3, "Unknown" },
			{ 0xc9cd6, "Unknown" },
			{ 0xc9d0c, "Unknown" },
			{ 0xc9d42, "Unknown" },
			{ 0xcb08f, "Unknown" },
			{ 0xcb74f, "Unknown" },
			{ 0xcc505, "Unknown" },
			{ 0xcc5d4, "Unknown" },
			{ 0xcd26a, "Unknown" },
			{ 0xcd2a0, "Unknown" },
			{ 0xcd4aa, "Unknown" },
			{ 0xcdb34, "Unknown" },
			{ 0xcdc15, "Unknown" },
			{ 0xcdd08, "Unknown" },
			{ 0xce233, "Unknown" },
			{ 0xce272, "Unknown" },
			{ 0xce767, "Unknown" },
			{ 0xceaf4, "Unknown" },
			{ 0xceb4f, "Unknown" },
			{ 0xcebe7, "Unknown" },
			{ 0xcf163, "Unknown" },
			{ 0xcf1c6, "Unknown" },
			{ 0xcf661, "Unknown" },
			{ 0xcf886, "Unknown" },
			{ 0xcffca, "Unknown" },
			{ 0xd10e0, "Unknown" },
			{ 0xd1282, "Unknown" },
			{ 0xd12b6, "Unknown" },
			{ 0xd2add, "Unknown" },
			{ 0xd4f1c, "Unknown" },
			{ 0xd6995, "Unknown" },
			{ 0xd7727, "Unknown" },
			{ 0xd791f, "Unknown" },
			{ 0xda6ba, "Unknown" },
			{ 0xdb606, "Unknown" },
			{ 0xdc3fb, "Unknown" },
			{ 0xdcef3, "Unknown" },
			{ 0xe1bec, "Unknown" },
			{ 0xe2b76, "Unknown" },
			{ 0xe3974, "Unknown" },
			{ 0xe5c8a, "Unknown" },
			{ 0xe6f35, "Unknown" },
			{ 0xe79af, "Unknown" },
			{ 0xe7a7e, "Unknown" },
			{ 0xe7b25, "Unknown" },
			{ 0xe958d, "Unknown" },
			{ 0xec3c7, "Unknown" },
			{ 0xec50b, "Unknown" },
			{ 0xed2d3, "Unknown" },
			{ 0xed324, "Unknown" },
			{ 0xed540, "Unknown" },
			{ 0xea73b, "Unknown" },
			{ 0xeed82, "Unknown" },
			{ 0xeefa7, "Unknown" },
		};

		public static readonly Dictionary<int, string> ImageNames = new Dictionary<int, string>
		{
			{ 0xbb9c0, "Communication" },
			{ 0xbbc40, "Communication 2" },
			{ 0xbbec0, "Communication 3" },
			{ 0xbc140, "Communication 4" },
			{ 0xbc3c0, "Communication 5" },
			{ 0xbc640, "Fox" },
			{ 0xbc8c0, "Fox 2" },
			{ 0xbcb40, "Peppy" },
			{ 0xbcdc0, "Peppy 2" },
			{ 0xbd040, "Falco" },
			{ 0xbd2c0, "Falco 2" },
			{ 0xbd540, "Slippy" },
			{ 0xbd7c0, "Slippy 2" },
			{ 0xbda40, "Pepper" },
			{ 0xbdcc0, "Pepper 2" },
			{ 0xbdf40, "Andross" },
			{ 0xbe1c0, "Andross 2" },
			{ 0xbe440, "Communication 6" },
			{ 0x0208b, "Map Sprites" }
		};

		public static readonly Dictionary<int, Size> ImageSizes = new Dictionary<int, Size>
		{
			{ 0xbb9c0, new Size(4, 5) },
			{ 0xbbc40, new Size(4, 5) },
			{ 0xbbec0, new Size(4, 5) },
			{ 0xbc140, new Size(4, 5) },
			{ 0xbc3c0, new Size(4, 5) },
			{ 0xbc640, new Size(4, 5) },
			{ 0xbc8c0, new Size(4, 5) },
			{ 0xbcb40, new Size(4, 5) },
			{ 0xbcdc0, new Size(4, 5) },
			{ 0xbd040, new Size(4, 5) },
			{ 0xbd2c0, new Size(4, 5) },
			{ 0xbd540, new Size(4, 5) },
			{ 0xbd7c0, new Size(4, 5) },
			{ 0xbda40, new Size(4, 5) },
			{ 0xbdcc0, new Size(4, 5) },
			{ 0xbdf40, new Size(4, 5) },
			{ 0xbe1c0, new Size(4, 5) },
			{ 0xbe440, new Size(4, 5) },
			{ 0x0208b, new Size(30, 1) }
		};
	}
}