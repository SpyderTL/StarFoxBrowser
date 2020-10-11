using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class TrackReader
	{
		public static int Position;
		public static int[] Channels = new int[8];

		public static void Read()
		{
			for (var channel = 0; channel < 8; channel++)
			{
				var value = BitConverter.ToUInt16(Spc.Ram, Position);

				if ((value & 0xff00) == 0x0000)
					Channels[channel] = 0;
				else
					Channels[channel] = value;

				Position += 2;
			}
		}
	}
}
