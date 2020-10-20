using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace StarFoxBrowser
{
	public static class SongPlayer
	{
		public static bool[] ChannelNoteStart = new bool[8];
		public static int[] ChannelNotes = new int[8];
		public static int[] ChannelVelocities = new int[8];
		public static int[] ChannelTranspose = new int[8];
		public static int[] ChannelTuning = new int[8];
		public static int[] ChannelPitch = new int[8];
		public static int[] ChannelPan = new int[8];
		public static int[] ChannelPhase = new int[8];
		public static int[] ChannelInstruments = new int[8];
		public static int[] ChannelVolume = new int[8];
		public static int[] ChannelVolumeTarget = new int[8];
		public static double[] ChannelVolumeTimer = new double[8];
		public static int Volume;
		public static int PercussionInstrumentOffset = 0;
		public static bool Playing;

		private static double[] ChannelTimers = new double[8];
		private static double[] NoteTimers = new double[8];
		private static int[] ChannelLengths = new int[8];
		private static int[] ChannelDurations = new int[8];
		private static int[] ChannelReturnPositions = new int[8];
		private static int[] ChannelRepeatPositions = new int[8];
		private static int[] ChannelRepeatCounts = new int[8];
		private static double[] ChannelPitchDelay = new double[8];
		private static double[] ChannelPitchDuration = new double[8];
		private static int[] ChannelPitchSlide = new int[8];
		private static int[] ChannelPanTarget = new int[8];
		private static double[] ChannelPanTimer = new double[8];
		private static int VolumeTarget;
		private static double VolumeTimer;
		private static double Tempo;
		private static double TempoTarget;
		private static double TempoTimer;

		private static int Repeat;

		private static IEnumerator Player;

		public static void Play()
		{
			Playing = true;
			Player = PlaySong().GetEnumerator();
		}

		public static void Stop()
		{
			Player = null;
			Playing = false;
		}

		public static void Update()
		{
			if(Player != null)
				Player.MoveNext();
		}

		private static IEnumerable PlaySong()
		{
			Playing = true;
			Repeat = -1;
			Tempo = 1000.0;
			TempoTarget = 1000.0;
			TempoTimer = 0.0;
			Volume = 0xff;
			VolumeTarget = 0xff;
			VolumeTimer = 0.0;

			PercussionInstrumentOffset = 0;
			ChannelNotes = new int[8];
			NoteTimers = new double[8];
			ChannelLengths = new int[8];
			ChannelDurations = new int[8];
			ChannelVelocities = new int[8];
			ChannelTranspose = new int[8];
			ChannelTuning = new int[8];
			ChannelPan = Enumerable.Repeat(10, 8).ToArray();
			ChannelPanTarget = Enumerable.Repeat(10, 8).ToArray();
			ChannelPanTimer = new double[8];
			ChannelVolume = Enumerable.Repeat(0xFF, 8).ToArray();
			ChannelVolumeTarget = Enumerable.Repeat(0xFF, 8).ToArray();
			ChannelVolumeTimer = new double[8];
			ChannelPitch = new int[8];
			ChannelPitchDelay = new double[8];
			ChannelPitchDuration = new double[8];
			ChannelPitchSlide = new int[8];

			while (Playing)
			{
				SongReader.Read();

				switch (SongReader.EventType)
				{
					case SongReader.EventTypes.Stop:
						Playing = false;
						break;

					case SongReader.EventTypes.Loop:
						SongReader.Position = SongReader.LoopPosition;
						break;

					case SongReader.EventTypes.Repeat:
						if (Repeat != 0)
							SongReader.Position = SongReader.LoopPosition;

						if (Repeat == -1)
							Repeat = SongReader.RepeatCount;
						else
							Repeat--;
						break;

					case SongReader.EventTypes.Track:
						var enumerator = PlayTrack().GetEnumerator();

						while (enumerator.MoveNext())
							yield return null;

						break;
				}
			}
		}

		private static IEnumerable PlayTrack()
		{
			TrackReader.Position = SongReader.TrackPosition;

			TrackReader.Read();

			ChannelNoteStart = new bool[8];
			ChannelTimers = new double[8];
			ChannelReturnPositions = new int[8];
			ChannelRepeatPositions = new int[8];
			ChannelRepeatCounts = new int[8];
			ChannelPhase = new int[8];

			var last = Environment.TickCount;

			var stopped = false;

			while (!stopped)
			{
				var time = Environment.TickCount;
				var elapsed = (time - last) * 0.0001;

				if (VolumeTimer > 0.0)
				{
					VolumeTimer -= elapsed;

					if (Volume > VolumeTarget)
						Volume--;
					else if (Volume < VolumeTarget)
						Volume++;
				}
				else
					Volume = VolumeTarget;

				if (TempoTimer > 0.0)
				{
					TempoTimer -= elapsed;

					if (Tempo > TempoTarget)
						Tempo--;
					else if (Tempo < TempoTarget)
						Tempo++;
				}
				else
					Tempo = TempoTarget;

				for (var channel = 0; channel < 8; channel++)
					ChannelNoteStart[channel] = false;

				for (var channel = 0; channel < 8 && !stopped; channel++)
				{
					if (NoteTimers[channel] > 0.0)
						NoteTimers[channel] -= elapsed * Tempo;

					if (ChannelVolumeTimer[channel] > 0.0)
					{
						ChannelVolumeTimer[channel] -= elapsed;

						if (ChannelVolume[channel] > ChannelVolumeTarget[channel])
							ChannelVolume[channel]--;
						else if (ChannelVolume[channel] < ChannelVolumeTarget[channel])
							ChannelVolume[channel]++;
					}
					else
						ChannelVolume[channel] = ChannelVolumeTarget[channel];

					if (ChannelPanTimer[channel] > 0.0)
					{
						ChannelPanTimer[channel] -= elapsed;

						if (ChannelPan[channel] > ChannelPanTarget[channel])
							ChannelPan[channel]--;
						else if (ChannelPan[channel] < ChannelPanTarget[channel])
							ChannelPan[channel]++;
					}
					else
						ChannelPan[channel] = ChannelPanTarget[channel];

					if (ChannelPitchDelay[channel] > 0.0)
						ChannelPitchDelay[channel] -= elapsed * Tempo;
					else if (ChannelPitchDuration[channel] > 0.0)
					{
						ChannelPitch[channel] = Math.Min(ChannelPitch[channel] + 64, ChannelPitchSlide[channel]);
						ChannelPitchDuration[channel] -= elapsed * Tempo;
					}
					else
					{
						ChannelPitch[0] = 0;
					}

					if (TrackReader.Channels[channel] != 0)
					{
						ChannelTimers[channel] -= elapsed * Tempo;

						while (ChannelTimers[channel] < 0 && !stopped)
						{
							ChannelReader.Position = TrackReader.Channels[channel];

							ChannelReader.Read();

							if (ChannelReader.EventType == ChannelReader.EventTypes.Note)
							{
								ChannelNotes[channel] = ChannelReader.Note;
								ChannelNoteStart[channel] = true;

								ChannelTimers[channel] += ChannelLengths[channel] / 18.0f;
								NoteTimers[channel] = (ChannelLengths[channel] / 18.0f) * ((ChannelDurations[channel] + 1) / 8.0f);

								ChannelPitch[channel] = 0;
								ChannelPitchSlide[channel] = ChannelReader.PitchSlide;
								ChannelPitchDelay[channel] = (ChannelLengths[channel] / 18.0f) * ((ChannelReader.Delay) / 16.0f);
								ChannelPitchDuration[channel] = (ChannelLengths[channel] / 18.0f) * ((ChannelReader.Duration) / 16.0f);
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Tie)
							{
								ChannelTimers[channel] += ChannelLengths[channel] / 18.0f;
								NoteTimers[channel] = (ChannelLengths[channel] / 18.0f) * ((ChannelDurations[channel] + 1) / 8.0f);
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Rest)
							{
								ChannelTimers[channel] += ChannelLengths[channel] / 18.0f;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Length)
							{
								ChannelLengths[channel] = ChannelReader.Length;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.LengthDurationVelocity)
							{
								ChannelLengths[channel] = ChannelReader.Length;
								ChannelDurations[channel] = ChannelReader.Duration;
								ChannelVelocities[channel] = ChannelReader.Velocity;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Instrument)
							{
								if ((ChannelReader.Instrument & 0x80) == 0)
									ChannelInstruments[channel] = ChannelReader.Instrument;
								else
									ChannelInstruments[channel] = ChannelReader.Instrument - 0xCA + PercussionInstrumentOffset;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Pan)
							{
								ChannelPan[channel] = ChannelReader.Pan;
								ChannelPanTarget[channel] = ChannelReader.Pan;
								ChannelPanTimer[channel] = 0.0;
								ChannelPhase[channel] = ChannelReader.Phase;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.PanFade)
							{
								ChannelPanTarget[channel] = ChannelReader.Pan;
								ChannelPanTimer[channel] = ChannelReader.Fade;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Call)
							{
								ChannelReturnPositions[channel] = ChannelReader.Position;
								ChannelRepeatPositions[channel] = ChannelReader.Call;
								ChannelRepeatCounts[channel] = ChannelReader.Repeat;
								ChannelReader.Position = ChannelReader.Call;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Tempo)
							{
								Tempo = ChannelReader.Tempo;
								TempoTarget = ChannelReader.Tempo;
								TempoTimer = 0.0;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.TempoFade)
							{
								TempoTarget = ChannelReader.Tempo;
								TempoTimer = ChannelReader.Fade;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.PercussionInstrumentOffset)
							{
								PercussionInstrumentOffset = ChannelReader.PercussionInstrumentOffset;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Volume)
							{
								ChannelVolume[channel] = ChannelReader.Volume;
								ChannelVolumeTarget[channel] = ChannelReader.Volume;
								ChannelVolumeTimer[channel] = 0.0;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.VolumeFade)
							{
								ChannelVolumeTarget[channel] = ChannelReader.Volume;
								ChannelVolumeTimer[channel] = ChannelReader.Fade;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.MasterVolume)
							{
								Volume = ChannelReader.Volume;
								VolumeTarget = Volume;
								VolumeTimer = 0.0;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.MasterVolumeFade)
							{
								VolumeTimer = ChannelReader.Fade / 18.0f;
								VolumeTarget = ChannelReader.Volume;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Transpose)
							{
								ChannelTranspose[channel] = ChannelReader.Transpose;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Tuning)
							{
								ChannelTuning[channel] = ChannelReader.Tuning;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.PitchSlide)
							{
								ChannelPitchDelay[channel] = (ChannelLengths[channel] / 18.0f) * (ChannelReader.Delay / 8.0f);
								ChannelPitchDuration[channel] = (ChannelLengths[channel] / 18.0f) * (ChannelReader.Duration / 8.0f);
								ChannelPitchSlide[channel] = ChannelReader.PitchSlide;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Other)
							{

							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Percussion)
							{
								ChannelTimers[channel] += ChannelLengths[channel] / 18.0f;
							}
							else if (ChannelReader.EventType == ChannelReader.EventTypes.Stop)
							{
								if (ChannelReturnPositions[channel] == 0)
									stopped = true;
								else
								{
									if (ChannelRepeatCounts[channel] == 1)
									{
										ChannelReader.Position = ChannelReturnPositions[channel];
										ChannelReturnPositions[channel] = 0;
									}
									else
									{
										ChannelReader.Position = ChannelRepeatPositions[channel];
										ChannelRepeatCounts[channel]--;
									}
								}
							}

							TrackReader.Channels[channel] = ChannelReader.Position;
						}
					}

					if (NoteTimers[channel] <= 0.0)
						ChannelNotes[channel] = 0;
				}

				last = time;

				yield return null;
			}
		}
	}
}