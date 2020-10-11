using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace StarFoxBrowser
{
	public static class SongPlayer
	{
		public static int[] ChannelNotes = new int[8];
		public static int[] ChannelVelocities = new int[8];
		public static int[] ChannelPan = new int[8];
		public static int[] ChannelPhase = new int[8];
		public static double Tempo;
		public static bool Stopped;

		private static double[] ChannelTimers = new double[8];
		private static double[] NoteTimers = new double[8];
		private static int[] ChannelLengths = new int[8];
		private static int[] ChannelDurations = new int[8];
		private static int[] ChannelReturnPositions = new int[8];
		private static int[] ChannelRepeatPositions = new int[8];
		private static int[] ChannelRepeatCounts = new int[8];

		private static int Repeat;

		private static IEnumerator Player;

		public static void Play()
		{
			Stopped = false;
			Player = PlaySong().GetEnumerator();
		}

		public static void Stop()
		{
		}

		public static void Update()
		{
			Player.MoveNext();
		}

		private static IEnumerable PlaySong()
		{
			Stopped = false;
			Repeat = -1;
			Tempo = 1000.0f;

			while (!Stopped)
			{
				SongReader.Read();

				switch (SongReader.EventType)
				{
					case SongReader.EventTypes.Stop:
						Stopped = true;
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

			Midi.Disable();
		}

		private static IEnumerable PlayTrack()
		{
			TrackReader.Position = SongReader.TrackPosition;

			TrackReader.Read();

			ChannelNotes = new int[8];
			ChannelTimers = new double[8];
			NoteTimers = new double[8];
			ChannelLengths = new int[8];
			ChannelDurations = new int[8];
			ChannelVelocities = new int[8];
			ChannelReturnPositions = new int[8];
			ChannelRepeatPositions = new int[8];
			ChannelRepeatCounts = new int[8];
			ChannelPan = Enumerable.Repeat(10, 8).ToArray();
			ChannelPhase = new int[8];

			var last = Environment.TickCount;

			var stopped = false;

			while (!stopped)
			{
				var time = Environment.TickCount;
				var elapsed = (time - last) * 0.0001;

				for (var channel = 0; channel < 8 && !stopped; channel++)
				{
					if (TrackReader.Channels[channel] == 0)
						continue;

					if (NoteTimers[channel] > 0.0)
					{
						NoteTimers[channel] -= elapsed * Tempo;

						if (NoteTimers[channel] <= 0.0)
							ChannelNotes[channel] = 0;
					}

					ChannelTimers[channel] -= elapsed * Tempo;

					while (ChannelTimers[channel] < 0 && !stopped)
					{
						ChannelReader.Position = TrackReader.Channels[channel];

						ChannelReader.Read();

						if (ChannelReader.EventType == ChannelReader.EventTypes.Note)
						{
							ChannelNotes[channel] = ChannelReader.Note;

							ChannelTimers[channel] += ChannelLengths[channel] / 18.0f;
							NoteTimers[channel] = ChannelDurations[channel] / 1.0f;
						}
						else if (ChannelReader.EventType == ChannelReader.EventTypes.Tie)
						{
							ChannelTimers[channel] += ChannelLengths[channel] / 18.0f;
							NoteTimers[channel] = ChannelDurations[channel] / 1.0f;
						}
						else if (ChannelReader.EventType == ChannelReader.EventTypes.Rest)
						{
							ChannelNotes[channel] = 0;

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
						else if (ChannelReader.EventType == ChannelReader.EventTypes.Pan)
						{
							ChannelPan[channel] = ChannelReader.Pan;
							ChannelPhase[channel] = ChannelReader.Phase;
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

				last = time;

				if (!stopped)
					yield return null;
			}
		}
	}
}