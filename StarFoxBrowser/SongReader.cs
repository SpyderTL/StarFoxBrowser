using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class SongReader
	{
		public static int Position;
		public static EventTypes EventType;
		public static int TrackPosition;
		public static int RepeatCount;
		public static int LoopPosition;

		public static void Read()
		{
			var value = BitConverter.ToUInt16(Spc.Ram, Position);

			Position += 2;

			if (value == 0x0000)
				EventType = EventTypes.Stop;
			else if ((value & 0xff00) == 0)
			{
				if ((value & 0x0080) == 0)
				{
					EventType = EventTypes.Repeat;
					LoopPosition = BitConverter.ToUInt16(Spc.Ram, Position);
					RepeatCount = value;
					Position += 2;
				}
				else
				{
					EventType = EventTypes.Loop;
					LoopPosition = BitConverter.ToUInt16(Spc.Ram, Position);
					Position += 2;
				}
			}
			else
			{
				EventType = EventTypes.Track;
				TrackPosition = value;
			}
		}

		public enum EventTypes
		{
			Track,
			Repeat,
			Loop,
			Stop
		}
	}
}
