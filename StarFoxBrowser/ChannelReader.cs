using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class ChannelReader
	{
		public static int Position;
		public static int Value;
		public static EventTypes EventType;
		public static int Special;
		public static int Note;
		public static int Length;
		public static int Duration;
		public static int Velocity;
		public static int Call;
		public static int Repeat;
		public static int Tempo;
		public static int Pan;
		public static int Phase;

		public static void Read()
		{
			Value = Spc.Ram[Position++];

			if (Value == RomSongs.EndTrack)
				EventType = EventTypes.Stop;
			else if (Value < RomSongs.FirstNote)
			{
				Length = Value;

				if ((Spc.Ram[Position] & 0x80) != 0x00)
					EventType = EventTypes.Length;
				else
				{
					EventType = EventTypes.LengthDurationVelocity;

					Duration = Spc.Ram[Position] >> 4;
					Velocity = Spc.Ram[Position] & 0x0f;

					Position++;
				}
			}
			else if (Value < RomSongs.Tie)
			{
				EventType = EventTypes.Note;
				Note = Value - RomSongs.FirstNote + 24;
			}
			else if (Value == RomSongs.Tie)
				EventType = EventTypes.Tie;
			else if (Value == RomSongs.Rest)
				EventType = EventTypes.Rest;
			else if (Value < RomSongs.FirstEvent)
			{
				EventType = EventTypes.Percussion;
				Note = Value - RomSongs.FirstPercussion + 24;
			}
			else if (Value == 0xE1)
			{
				EventType = EventTypes.Pan;
				Pan = Spc.Ram[Position] & 0x2F;
				Phase = Spc.Ram[Position++] >> 6;
			}
			else if (Value == 0xE7)
			{
				EventType = EventTypes.Tempo;
				Tempo = Spc.Ram[Position++];
			}
			else if (Value == 0xEF)
			{
				EventType = EventTypes.Call;
				Call = BitConverter.ToUInt16(Spc.Ram, Position);
				Repeat = Spc.Ram[Position + 2];

				Position += 3;
			}
			else
			{
				EventType = EventTypes.Other;

				Position += RomSongs.EventTypes[Value - RomSongs.FirstEvent].Length;
			}
		}

		public enum EventTypes
		{
			Note,
			Tie,
			Rest,
			Length,
			LengthDurationVelocity,
			Pan,
			Percussion,
			Tempo,
			Call,
			Other,
			Stop,
		}
	}
}
