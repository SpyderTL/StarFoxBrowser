using StarFoxBrowser.Nodes;
using System;
using System.Diagnostics;
using System.Linq;

namespace StarFoxBrowser
{
	public static class MidiPlayer
	{
		public static int MasterVolume;

		public static int[] Notes;
		public static int[] Volume;
		public static int[] Pan;
		public static int[] Instruments;
		public static int[] Drums;
		public static int[] NoteOffsets;
		public static int[] Transpose;
		public static int[] Tuning;
		public static int[] Pitch;

		public static void Start()
		{
			MasterVolume = 0xff;
			Notes = new int[8];
			Pan = Enumerable.Repeat(10, 8).ToArray();
			Volume = Enumerable.Repeat(0xFF, 8).ToArray();
			Instruments = new int[8];
			Drums = new int[8];
			NoteOffsets = new int[8];
			Transpose = new int[8];
			Tuning = new int[8];
			Pitch = new int[8];

			Midi.Enable();

			for (var channel = 0; channel < 8; channel++)
			{
				Midi.ControlChange(channel, 123, 0);
				Midi.ProgramChange(channel, Instruments[channel]);
				Midi.ControlChange(channel, 0x5b, 127);
				Midi.ControlChange(channel, 0x5c, 127);
				Midi.ControlChange(channel, 0x5d, 127);
				Midi.ControlChange(channel, 0x5e, 127);
				Midi.ControlChange(channel, 0x5f, 127);

				//Midi.ProgramChange(channel, 49);
			}
		}

		public static void Stop()
		{
			for (var channel = 0; channel < 8; channel++)
				Midi.ControlChange(channel, 123, 0);

			Midi.Disable();
		}

		public static void Update()
		{
			UpdateVolume();

			for (var channel = 0; channel < 8; channel++)
			{
				UpdateInstruments(channel);
				UpdateVolume(channel);
				UpdatePan(channel);
				UpdateTuning(channel);
				UpdatePitch(channel);
				UpdateNotes(channel);
			}
		}

		private static void UpdatePitch(int channel)
		{
			if (Pitch[channel] != SongPlayer.ChannelPitch[channel])
			{
				int value = 0x2000 + (int)((SongPlayer.ChannelPitch[channel] / 255.0f) * 0x1000);
				Midi.PitchBendChange(channel, value);
				Pitch[channel] = SongPlayer.ChannelPitch[channel];
			}
		}

		private static void UpdateTuning(int channel)
		{
			if (Tuning[channel] != SongPlayer.ChannelTuning[channel])
			{
				int value = 0x2000 + (int)((SongPlayer.ChannelTuning[channel] / 255.0f) * 0x1000);
				Midi.PitchBendChange(channel, value);
				Tuning[channel] = SongPlayer.ChannelTuning[channel];
			}
		}

		private static void UpdateNotes(int channel)
		{
			if (SongPlayer.ChannelNotes[channel] == 0 && Notes[channel] != 0)
			{
				if (Drums[channel] == 0)
					Midi.NoteOff(channel, Notes[channel] + NoteOffsets[channel] + SongPlayer.ChannelTranspose[channel], 0);
				else
					Midi.NoteOff(9, Drums[channel], 0);

				Notes[channel] = SongPlayer.ChannelNotes[channel];
			}
			else if (SongPlayer.ChannelNoteStart[channel])
			{
				if (Drums[channel] == 0)
				{
					if (Notes[channel] != 0)
						Midi.NoteOff(channel, Notes[channel] + NoteOffsets[channel] + SongPlayer.ChannelTranspose[channel], 0);

					if (SongPlayer.ChannelNotes[channel] != 0)
						Midi.NoteOn(channel, SongPlayer.ChannelNotes[channel] + NoteOffsets[channel] + SongPlayer.ChannelTranspose[channel], (int)((SongPlayer.ChannelVelocities[channel] / 15.0f) * 127.0f));
				}
				else
				{
					if (Notes[channel] != 0)
						Midi.NoteOff(9, Drums[channel], 0);

					if (SongPlayer.ChannelNotes[channel] != 0)
						Midi.NoteOn(9, Drums[channel], (int)((SongPlayer.ChannelVelocities[channel] / 15.0f) * 127.0f));
				}

				Notes[channel] = SongPlayer.ChannelNotes[channel];
			}
		}

		private static void UpdatePan(int channel)
		{
			if (Pan[channel] != SongPlayer.ChannelPan[channel])
			{
				var value = (int)((((SongPlayer.ChannelPan[channel] - 10) / 10.0f) * 63.5f) + 63.5f);

				Midi.ControlChange(channel, 10, value);
				Pan[channel] = SongPlayer.ChannelPan[channel];
			}
		}

		private static void UpdateVolume()
		{
			if (MasterVolume != SongPlayer.Volume)
			{
				for (var channel = 0; channel < 8; channel++)
				{
					var value = (int)((Volume[channel] / (double)0xff) * (MasterVolume / (double)0xff) * 0x7f);

					Midi.ControlChange(channel, 7, value);
				}

				MasterVolume = SongPlayer.Volume;
			}
		}

		private static void UpdateVolume(int channel)
		{
			if (Volume[channel] != SongPlayer.ChannelVolume[channel])
			{
				var value = (int)((Volume[channel] / (double)0xff) * (MasterVolume / (double)0xff) * 0x7f);

				Midi.ControlChange(channel, 7, value);
				Volume[channel] = SongPlayer.ChannelVolume[channel];
			}
		}

		private static void UpdateInstruments(int channel)
		{
			if (Instruments[channel] != SongPlayer.ChannelInstruments[channel])
			{
				if (Notes[channel] != 0)
				{
					if (Drums[channel] != 0)
						Midi.NoteOff(9, Drums[channel], 0);
					else
						Midi.NoteOff(channel, Notes[channel] + NoteOffsets[channel], 0);
				}

				var instrument = SongPlayer.ChannelInstruments[channel];

				var instrument2 = Spc.Ram[0x3d00 + (instrument * 6)];

				if ((instrument2 & 0x80) == 0)
				{
					var address = BitConverter.ToUInt16(Spc.Ram, 0x3c00 + (instrument2 * 4));

					var block = Enumerable.Range(0, SpcInstruments.BlockAddresses.Count)
						.LastOrDefault(x => SpcInstruments.BlockAddresses[x] <= address &&
							SpcInstruments.BlockAddresses[x] + SpcInstruments.BlockLengths[x] > address);

					if (block != 0)
					{
						var offset2 = address - SpcInstruments.BlockAddresses[block] + SpcInstruments.BlockOffsets[block];

						if (Usa10.AudioClipNames.TryGetValue(offset2, out string name))
						{
							if (SpcInstruments.Instruments.TryGetValue(name, out int midiInstrument))
							{
								Midi.ProgramChange(channel, midiInstrument);
								Drums[channel] = 0;

								switch (name)
								{
									case "Orchestra Hit":
										NoteOffsets[channel] = 60;
										break;

									case "Orchestra Hit 2":
										NoteOffsets[channel] = 36;
										break;

									case "Orchestra Hit 3":
										NoteOffsets[channel] = 24;
										break;

									case "Haunting Music":
									case "French Horns":
									case "Calliope":
									case "Flute":
									case "Flute 2":
									case "Strings Hit":
										NoteOffsets[channel] = 36;
										break;

									default:
										NoteOffsets[channel] = 24;
										break;
								}
							}
							else if (SpcInstruments.Drums.TryGetValue(name, out midiInstrument))
							{
								Drums[channel] = midiInstrument;
							}
							else
							{
								Midi.ProgramChange(channel, 0);
								Drums[channel] = 0;
								Debug.WriteLine("Channel " + channel + " Instrument Not Found: " + offset2.ToString("X6") + " " + name);
							}
						}
						else
						{
							Midi.ProgramChange(channel, 0);
							Drums[channel] = 0;
							Debug.WriteLine("Channel " + channel + " Instrument Not Found: " + offset2.ToString("X6"));
						}
					}
					else
					{
						Midi.ProgramChange(channel, 0);
						Drums[channel] = 0;
						Debug.WriteLine("Channel " + channel + " Block Not Found: " + address.ToString("X6"));
					}
				}
				else
				{
					Drums[channel] = 40;
				}

				Instruments[channel] = SongPlayer.ChannelInstruments[channel];
			}
		}
	}
}