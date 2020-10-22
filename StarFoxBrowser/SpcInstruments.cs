using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class SpcInstruments
	{
		public static List<int> BlockAddresses;
		public static List<int> BlockLengths;
		public static List<int> BlockOffsets;

		public static readonly Dictionary<string, int> Instruments = new Dictionary<string, int>
		{
			{ "Orchestra Hit", 55 },
			{ "Orchestra Hit 2", 55 },
			{ "Orchestra Hit 3", 55 },
			//{ "Overdrive Guitar", 29 },
			{ "Overdrive Guitar", 30 },
			{ "Haunting Music", 53 },
			//{ "Haunting Music", 86 },
			//{ "Haunting Music", 54 },
			{ "Bass Guitar", 33 },
			{ "Bass Guitar 2", 34 },
			{ "Synth Lead", 80 },
			{ "Synth Lead 2", 81 },
			//{ "Synth Lead 3", 82 },
			{ "Synth Lead 3", 68 },
			{ "Tom", 117 },
			{ "Strings", 48 },
			{ "Timpani", 47 },
			{ "Beep", 83 },
			{ "Beep 2", 84 },
			{ "Beep 3", 85 },
			{ "Beep 4", 86 },
			{ "Beep 5", 87 },
			{ "Beep 6", 88 },
			{ "Beep 7", 82 },
			{ "Horns", 56 },
			{ "Horns 2", 60 },
			//{ "Horns 3", 59 },
			{ "Horns 3", 58 },
			{ "Horns 4", 57 },
			//{ "Horns 5", 61 },
			{ "Horns 5", 49 },
			{ "French Horns", 60 },
			//{ "Calliope", 82 },
			{ "Calliope", 83 },
			{ "Mechanical", 117 },
			{ "Mechanical 2", 117 },
			{ "Mechanical 3", 117 },
			{ "Flute", 73 },
			{ "Synth Voice", 85 },
			{ "Halo Pad", 94 },
			{ "Whistle", 78 },
			{ "Oboe", 68 },
			{ "Chiff", 83 },
			{ "Strings Hit", 45 },
			{ "Flute 2", 72 },
			{ "Noise 5", 127 },
			{ "Choir", 52 },
			{ "Click 2", 81 }
		};

		public static readonly Dictionary<string, int> Drums = new Dictionary<string, int>
		{
			{ "Snare", 40 },
			{ "Snare 2", 38 },
			{ "Bass Drum", 36 },
			//{ "Overdrive Guitar", 35 },
			{ "Click", 31 },
			//{ "Click 2", 37 },
			{ "Click 3", 31 },
		};
	}
}
