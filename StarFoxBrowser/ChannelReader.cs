﻿using System;
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
		public static int Instrument;
		public static int PercussionInstrumentOffset;
		public static int Volume;
		public static int Fade;
		public static int Transpose;
		public static int Tuning;
		public static int PitchSlide;
		public static int Delay;

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
				Note = Value - RomSongs.FirstNote;

				if (Spc.Ram[Position] != 0xf9)
				{
					Delay = 0;
					Duration = 0;
					PitchSlide = 0;
				}
				else
				{
					Position++;

					Delay = Spc.Ram[Position++];
					Duration = Spc.Ram[Position++];
					PitchSlide = Spc.Ram[Position++] - RomSongs.FirstNote;
				}
			}
			else if (Value == RomSongs.Tie)
				EventType = EventTypes.Tie;
			else if (Value == RomSongs.Rest)
				EventType = EventTypes.Rest;
			else if (Value < RomSongs.FirstEvent)
			{
				EventType = EventTypes.Percussion;
				Note = Value - RomSongs.FirstPercussion;
			}
			else if (Value == 0xE0)
			{
				EventType = EventTypes.Instrument;
				Instrument = Spc.Ram[Position++];
			}
			else if (Value == 0xE1)
			{
				EventType = EventTypes.Pan;
				Pan = Spc.Ram[Position] & 0x2F;
				Phase = Spc.Ram[Position++] >> 6;
			}
			else if (Value == 0xE2)
			{
				EventType = EventTypes.PanFade;
				Fade = Spc.Ram[Position++];
				Pan = Spc.Ram[Position++] & 0x2F;
			}
			else if (Value == 0xE5)
			{
				EventType = EventTypes.MasterVolume;
				Volume = Spc.Ram[Position++];
			}
			else if (Value == 0xE6)
			{
				EventType = EventTypes.MasterVolumeFade;
				Fade = Spc.Ram[Position++];
				Volume = Spc.Ram[Position++];
			}
			else if (Value == 0xE7)
			{
				EventType = EventTypes.Tempo;
				Tempo = Spc.Ram[Position++];
			}
			else if (Value == 0xE8)
			{
				EventType = EventTypes.TempoFade;
				Fade = Spc.Ram[Position++];
				Tempo = Spc.Ram[Position++];
			}
			else if (Value == 0xEA)
			{
				EventType = EventTypes.Transpose;
				Transpose = (sbyte)Spc.Ram[Position++];
			}
			else if (Value == 0xED)
			{
				EventType = EventTypes.Volume;
				Volume = Spc.Ram[Position++];
			}
			else if (Value == 0xEE)
			{
				EventType = EventTypes.VolumeFade;
				Fade = Spc.Ram[Position++];
				Volume = Spc.Ram[Position++];
			}
			else if (Value == 0xEF)
			{
				EventType = EventTypes.Call;
				Call = BitConverter.ToUInt16(Spc.Ram, Position);
				Repeat = Spc.Ram[Position + 2];

				Position += 3;
			}
			else if (Value == 0xF4)
			{
				EventType = EventTypes.Tuning;
				Tuning = Spc.Ram[Position++];
			}
			else if (Value == 0xF9)
			{
				EventType = EventTypes.PitchSlide;
				Delay = Spc.Ram[Position++];
				Duration = Spc.Ram[Position++];
				PitchSlide = Spc.Ram[Position++];
			}
			else if (Value == 0xFA)
			{
				EventType = EventTypes.PercussionInstrumentOffset;
				PercussionInstrumentOffset = Spc.Ram[Position++];
			}
			else
			{
				EventType = EventTypes.Other;

				Position += RomSongs.EventTypes[Value - RomSongs.FirstEvent].Length;

				System.Diagnostics.Debug.WriteLine("Unknown Event: " + Value.ToString("X2"));
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
			PanFade,
			Percussion,
			Tempo,
			TempoFade,
			Call,
			Other,
			Stop,
			Instrument,
			PercussionInstrumentOffset,
			Volume,
			VolumeFade,
			Transpose,
			Tuning,
			PitchSlide,
			MasterVolume,
			MasterVolumeFade,
		}
	}
}
