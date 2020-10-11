using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class SfcFile
	{
		public static void Load(string path)
		{
			var data = File.ReadAllBytes(path);

			ReadLoRom(data);
		}

		public static void Read(Stream stream)
		{
			var data = new byte[stream.Length];

			stream.Read(data, 0, data.Length);

			ReadLoRom(data);
		}

		private static void ReadLoRom(byte[] data)
		{
			for (var bank = 0; bank < 0x20; bank++)
			{
				Array.Copy(data, bank * 0x8000, Snes.Ram, (bank * 0x10000) + 0x8000, 0x8000);
				//Array.Copy(data, bank * 0x8000, Snes.Ram, (bank * 0x10000) + 0x808000, 0x8000);
			}
		}
	}
}
