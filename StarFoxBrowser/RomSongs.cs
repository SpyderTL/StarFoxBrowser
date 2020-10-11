using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class RomSongs
	{
		public static EventType[] EventTypes = new EventType[]
		{
			new EventType { Value = 0xe0, Length = 0x01, Name = "SetInstrument" },
			new EventType { Value = 0xe1, Length = 0x01, Name = "Pan" },
			new EventType { Value = 0xe2, Length = 0x02, Name = "PanFade" },
			new EventType { Value = 0xe3, Length = 0x03, Name = "VibratoOn" },
			new EventType { Value = 0xe4, Length = 0x00, Name = "VibratoOff" },
			new EventType { Value = 0xe5, Length = 0x01, Name = "MasterVolume" },
			new EventType { Value = 0xe6, Length = 0x02, Name = "MasterVolumeFade" },
			new EventType { Value = 0xe7, Length = 0x01, Name = "SetTempo" },
			new EventType { Value = 0xe8, Length = 0x02, Name = "TempoFade" },
			new EventType { Value = 0xe9, Length = 0x01, Name = "GlobalTranspose" },
			new EventType { Value = 0xea, Length = 0x01, Name = "Transpose" },
			new EventType { Value = 0xeb, Length = 0x03, Name = "TremoloOn" },
			new EventType { Value = 0xec, Length = 0x00, Name = "TremoloOff" },
			new EventType { Value = 0xed, Length = 0x01, Name = "Volume" },
			new EventType { Value = 0xee, Length = 0x02, Name = "VolumeFade" },
			new EventType { Value = 0xef, Length = 0x03, Name = "CallSubroutine" },
			new EventType { Value = 0xf0, Length = 0x01, Name = "VibratoFade" },
			new EventType { Value = 0xf1, Length = 0x03, Name = "PitchEnvelopeTo" },
			new EventType { Value = 0xf2, Length = 0x03, Name = "PitchEnvelopeFrom" },
			new EventType { Value = 0xf3, Length = 0x00, Name = "PitchEnvelopeOff" },
			new EventType { Value = 0xf4, Length = 0x01, Name = "Tuning" },
			new EventType { Value = 0xf5, Length = 0x03, Name = "EchoVolume" },
			new EventType { Value = 0xf6, Length = 0x00, Name = "EchoOff" },
			new EventType { Value = 0xf7, Length = 0x03, Name = "EchoParameters" },
			new EventType { Value = 0xf8, Length = 0x03, Name = "EchoFade" },
			new EventType { Value = 0xf9, Length = 0x03, Name = "PitchSlide" },
			new EventType { Value = 0xfa, Length = 0x01, Name = "PercussionBase" }
		};

		public const int EndTrack = 0x00;
		public const int FirstNote = 0x80;
		public const int LastNote = 0xC7;
		public const int Tie = 0xC8;
		public const int Rest = 0xC9;
		public const int FirstPercussion = 0xCA;
		public const int LastPercussion = 0xDF;
		public const int FirstEvent = 0xE0;
		public const int LastEvent = 0xFA;

		public struct EventType
		{
			public int Value;
			public int Length;
			public string Name;
		}
	}
}
