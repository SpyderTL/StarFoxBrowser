using System;
using System.Linq;

namespace StarFoxBrowser
{
	public static class MidiPlayer
	{
		public static int[] MidiChannels;
		public static int[] MidiPan;

		public static void Start()
		{
			MidiChannels = new int[8];
			MidiPan = Enumerable.Repeat(10, 8).ToArray();

			Midi.Enable();

			for (var channel = 0; channel < 8; channel++)
			{
				Midi.ControlChange(channel, 123, 0);
				Midi.ProgramChange(channel, 80);
				//Midi.ProgramChange(channel, 49);
			}
		}

		public static void Stop()
		{
			for (var channel = 0; channel < 8; channel++)
			{
				Midi.ControlChange(channel, 123, 0);
				Midi.ProgramChange(channel, 80);
			}

			Midi.Disable();
		}

		public static void Update()
		{
			for (var channel = 0; channel < 8; channel++)
			{
				if (MidiPan[channel] != SongPlayer.ChannelPan[channel])
				{
					Midi.ControlChange(channel, 10, (int)((((SongPlayer.ChannelPan[channel] - 10) / 10.0) * 63.0) + 64.0));

					MidiPan[channel] = SongPlayer.ChannelPan[channel];
				}

				if (MidiChannels[channel] != SongPlayer.ChannelNotes[channel])
				{
					if (MidiChannels[channel] != 0)
						Midi.NoteOff(channel, MidiChannels[channel], 0);

					if (SongPlayer.ChannelNotes[channel] != 0)
						Midi.NoteOn(channel, SongPlayer.ChannelNotes[channel], (int)((SongPlayer.ChannelVelocities[channel] / 15.0f) * 127.0f));

					MidiChannels[channel] = SongPlayer.ChannelNotes[channel];
				}
			}
		}
	}
}