﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class SongPlayerForm
	{
		public static PlayerForm Form;
		public static int Song;
		public static Timer Timer = new Timer(Timer_Elapsed, null, Timeout.Infinite, 10);

		public static void Show()
		{
			if (Form == null)
			{
				Form = new PlayerForm();

				Form.PlayButton.Click += PlayButton_Click;
				Form.StopButton.Click += StopButton_Click;
				Form.Timer.Tick += Timer_Tick;
				Form.FormClosing += Form_FormClosing;
			}

			Form.Show();
		}

		public static void Close()
		{
			if (Form != null)
			{
				Form.Close();

				Form.PlayButton.Click -= PlayButton_Click;
				Form.StopButton.Click -= StopButton_Click;
				Form.Timer.Tick -= Timer_Tick;
				Form.FormClosing -= Form_FormClosing;

				Form.Dispose();

				Form = null;
			}
		}

		private static void Timer_Elapsed(object state)
		{
			SongPlayer.Update();
			MidiPlayer.Update();
		}


		private static void Timer_Tick(object sender, EventArgs e)
		{
			Form.SongLabel.Text = SongReader.Position.ToString("X4");
			Form.TrackLabel.Text = TrackReader.Position.ToString("X4");

			Form.Channel0Label.Visible = SongPlayer.ChannelNotes[0] != 0;
			Form.Channel0Label.Text = SongPlayer.ChannelNotes[0].ToString("X2");
			Form.Channel0Label.Left = (SongPlayer.ChannelNotes[0] * 8) + 64;

			Form.Channel1Label.Visible = SongPlayer.ChannelNotes[1] != 0;
			Form.Channel1Label.Text = SongPlayer.ChannelNotes[1].ToString("X2");
			Form.Channel1Label.Left = (SongPlayer.ChannelNotes[1] * 8) + 64;

			Form.Channel2Label.Visible = SongPlayer.ChannelNotes[2] != 0;
			Form.Channel2Label.Text = SongPlayer.ChannelNotes[2].ToString("X2");
			Form.Channel2Label.Left = (SongPlayer.ChannelNotes[2] * 8) + 64;

			Form.Channel3Label.Visible = SongPlayer.ChannelNotes[3] != 0;
			Form.Channel3Label.Text = SongPlayer.ChannelNotes[3].ToString("X2");
			Form.Channel3Label.Left = (SongPlayer.ChannelNotes[3] * 8) + 64;

			Form.Channel4Label.Visible = SongPlayer.ChannelNotes[4] != 0;
			Form.Channel4Label.Text = SongPlayer.ChannelNotes[4].ToString("X2");
			Form.Channel4Label.Left = (SongPlayer.ChannelNotes[4] * 8) + 64;

			Form.Channel5Label.Visible = SongPlayer.ChannelNotes[5] != 0;
			Form.Channel5Label.Text = SongPlayer.ChannelNotes[5].ToString("X2");
			Form.Channel5Label.Left = (SongPlayer.ChannelNotes[5] * 8) + 64;

			Form.Channel6Label.Visible = SongPlayer.ChannelNotes[6] != 0;
			Form.Channel6Label.Text = SongPlayer.ChannelNotes[6].ToString("X2");
			Form.Channel6Label.Left = (SongPlayer.ChannelNotes[6] * 8) + 64;

			Form.Channel7Label.Visible = SongPlayer.ChannelNotes[7] != 0;
			Form.Channel7Label.Text = SongPlayer.ChannelNotes[7].ToString("X2");
			Form.Channel7Label.Left = (SongPlayer.ChannelNotes[7] * 8) + 64;
		}

		private static void PlayButton_Click(object sender, EventArgs e)
		{
			if (!SongPlayer.Playing)
			{
				Song = SongReader.Position;

				MidiPlayer.Start();

				SongPlayer.Play();

				Form.Timer.Start();

				Timer.Change(0, 10);
			}
		}

		private static void StopButton_Click(object sender, EventArgs e)
		{
			Timer.Change(0, Timeout.Infinite);

			SongReader.Position = Song;

			Form.Timer.Stop();

			MidiPlayer.Stop();

			SongPlayer.Stop();
		}

		private static void Form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			if (SongPlayer.Playing)
			{
				Timer.Change(0, Timeout.Infinite);

				Form.Timer.Stop();

				MidiPlayer.Stop();

				SongPlayer.Stop();
			}
		}
	}
}
