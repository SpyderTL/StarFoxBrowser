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
			new EventType { Value = 0xe0, Length = 0x01, Name = "0xe0" },
			new EventType { Value = 0xe1, Length = 0x01, Name = "0xe1" },
			new EventType { Value = 0xe2, Length = 0x02, Name = "0xe2" },
			new EventType { Value = 0xe3, Length = 0x03, Name = "0xe3" },
			new EventType { Value = 0xe4, Length = 0x00, Name = "0xe4" },
			new EventType { Value = 0xe5, Length = 0x01, Name = "0xe5" },
			new EventType { Value = 0xe6, Length = 0x02, Name = "0xe6" },
			new EventType { Value = 0xe7, Length = 0x01, Name = "0xe7" },
			new EventType { Value = 0xe8, Length = 0x02, Name = "0xe8" },
			new EventType { Value = 0xe9, Length = 0x01, Name = "0xe9" },
			new EventType { Value = 0xea, Length = 0x01, Name = "0xea" },
			new EventType { Value = 0xeb, Length = 0x03, Name = "0xeb" },
			new EventType { Value = 0xec, Length = 0x00, Name = "0xec" },
			new EventType { Value = 0xed, Length = 0x01, Name = "0xed" },
			new EventType { Value = 0xee, Length = 0x02, Name = "0xee" },
			new EventType { Value = 0xef, Length = 0x03, Name = "0xef" },
			new EventType { Value = 0xf0, Length = 0x01, Name = "0xf0" },
			new EventType { Value = 0xf1, Length = 0x03, Name = "0xf1" },
			new EventType { Value = 0xf2, Length = 0x03, Name = "0xf2" },
			new EventType { Value = 0xf3, Length = 0x00, Name = "0xf3" },
			new EventType { Value = 0xf4, Length = 0x01, Name = "0xf4" },
			new EventType { Value = 0xf5, Length = 0x03, Name = "0xf5" },
			new EventType { Value = 0xf6, Length = 0x00, Name = "0xf6" },
			new EventType { Value = 0xf7, Length = 0x03, Name = "0xf7" },
			new EventType { Value = 0xf8, Length = 0x03, Name = "0xf8" },
			new EventType { Value = 0xf9, Length = 0x03, Name = "0xf9" },
			new EventType { Value = 0xfa, Length = 0x01, Name = "0xfa" },
		};

		public struct EventType
		{
			public int Value;
			public int Length;
			public string Name;
		}
	}
}
