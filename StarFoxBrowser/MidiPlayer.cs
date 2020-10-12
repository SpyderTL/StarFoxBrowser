using StarFoxBrowser.Nodes;
using System;
using System.Diagnostics;
using System.Linq;

namespace StarFoxBrowser
{
	public static class MidiPlayer
	{
		public static int[] MidiNotes;
		public static int[] MidiVolume;
		public static int[] MidiPan;
		public static int[] MidiInstruments;
		public static int[] MidiDrums;
		public static int[] MidiNoteOffsets;

		public static void Start()
		{
			MidiNotes = new int[8];
			MidiPan = Enumerable.Repeat(10, 8).ToArray();
			MidiVolume = Enumerable.Repeat(0xFF, 8).ToArray();
			MidiInstruments = new int[8];
			MidiDrums = new int[8];
			MidiNoteOffsets = new int[8];

			Midi.Enable();

			for (var channel = 0; channel < 8; channel++)
			{
				Midi.ControlChange(channel, 123, 0);
				Midi.ProgramChange(channel, MidiInstruments[channel]);
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
			for (var channel = 0; channel < 8; channel++)
			{
				//if (SongPlayer.ChannelInstruments[channel] == 0)
				//	continue;

				if (MidiInstruments[channel] != SongPlayer.ChannelInstruments[channel])
				{
					if (MidiNotes[channel] != 0)
					{
						if (MidiDrums[channel] != 0)
							Midi.NoteOff(9, MidiDrums[channel], 0);
						else
							Midi.NoteOff(channel, MidiNotes[channel] + MidiNoteOffsets[channel], 0);
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
									MidiDrums[channel] = 0;

									switch (name)
									{
										case "Orchestra Hit":
											MidiNoteOffsets[channel] = 60;
											break;

										case "Orchestra Hit 2":
											MidiNoteOffsets[channel] = 36;
											break;

										case "Orchestra Hit 3":
											MidiNoteOffsets[channel] = 24;
											break;

										case "Haunting Music":
											MidiNoteOffsets[channel] = 36;
											break;

										case "French Horns":
											MidiNoteOffsets[channel] = 36;
											break;

										default:
											MidiNoteOffsets[channel] = 24;
											break;
									}
								}
								else if (SpcInstruments.Drums.TryGetValue(name, out midiInstrument))
								{
									MidiDrums[channel] = midiInstrument;
								}
								else
								{
									Midi.ProgramChange(channel, 0);
									MidiDrums[channel] = 0;
									Debug.WriteLine("Channel " + channel + " Instrument Not Found: " + offset2.ToString("X6") + " " + name);
								}
							}
							else
							{
								Midi.ProgramChange(channel, 0);
								MidiDrums[channel] = 0;
								Debug.WriteLine("Channel " + channel + " Instrument Not Found: " + offset2.ToString("X6"));
							}
						}
						else
						{
							Midi.ProgramChange(channel, 0);
							MidiDrums[channel] = 0;
							Debug.WriteLine("Channel " + channel + " Block Not Found: " + address.ToString("X6"));
						}
					}
					else
					{
						MidiDrums[channel] = 40;
					}

					MidiInstruments[channel] = SongPlayer.ChannelInstruments[channel];
				}

				if (MidiVolume[channel] != SongPlayer.ChannelVolume[channel])
				{
					var value = SongPlayer.ChannelVolume[channel] >> 1;

					Midi.ControlChange(channel, 7, value);
					MidiVolume[channel] = SongPlayer.ChannelVolume[channel];
				}

				if (MidiPan[channel] != SongPlayer.ChannelPan[channel])
				{
					var value = (int)((((SongPlayer.ChannelPan[channel] - 10) / 10.0f) * 63.5f) + 63.5f);

					Midi.ControlChange(channel, 10, value);
					MidiPan[channel] = SongPlayer.ChannelPan[channel];
				}

				//if (MidiChannels[channel] != SongPlayer.ChannelNotes[channel] ||
				//	SongPlayer.ChannelNoteOn[channel] == true)
				if (SongPlayer.ChannelNotes[channel] == 0 && MidiNotes[channel] != 0)
				{
					if (MidiDrums[channel] == 0)
						Midi.NoteOff(channel, MidiNotes[channel] + MidiNoteOffsets[channel], 0);
					else
						Midi.NoteOff(9, MidiDrums[channel], 0);

					MidiNotes[channel] = SongPlayer.ChannelNotes[channel];
				}
				else if (SongPlayer.ChannelNoteOn[channel])
				{
					if (MidiDrums[channel] == 0)
					{
						if (MidiNotes[channel] != 0)
							Midi.NoteOff(channel, MidiNotes[channel] + MidiNoteOffsets[channel], 0);

						if (SongPlayer.ChannelNotes[channel] != 0)
							Midi.NoteOn(channel, SongPlayer.ChannelNotes[channel] + MidiNoteOffsets[channel], (int)((SongPlayer.ChannelVelocities[channel] / 15.0f) * 127.0f));
					}
					else
					{
						if (MidiNotes[channel] != 0)
							Midi.NoteOff(9, MidiDrums[channel], 0);

						if (SongPlayer.ChannelNotes[channel] != 0)
							Midi.NoteOn(9, MidiDrums[channel], (int)((SongPlayer.ChannelVelocities[channel] / 15.0f) * 127.0f));
					}

					MidiNotes[channel] = SongPlayer.ChannelNotes[channel];
				}
			}
		}
	}
}