using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class Midi
	{
		[DllImport("winmm.dll")]
		static extern uint midiOutOpen(out IntPtr lphMidiOut, uint uDeviceID, IntPtr dwCallback, IntPtr dwInstance, uint dwFlags);

		[DllImport("winmm.dll")]
		static extern uint midiOutShortMsg(IntPtr hMidiOut, uint dwMsg);

		[DllImport("winmm.dll")]
		static extern uint midiOutClose(IntPtr hMidiOut);

		private delegate void MidiCallBack(IntPtr handle, uint msg, uint instance, uint param1, uint param2);

		private static IntPtr Handle;

		public static void Enable()
		{
			if (Handle == IntPtr.Zero)
			{
				//var result = midiOutOpen(out Handle, 0xFFFFFFFF, IntPtr.Zero, IntPtr.Zero, 0);
				var result = midiOutOpen(out Handle, 0, IntPtr.Zero, IntPtr.Zero, 0);

				//System.Diagnostics.Debug.WriteLine(result);
			}
		}

		public static void NoteOn(int channel, int note, int velocity)
		{
			if (Handle != IntPtr.Zero)
			{
				var result = midiOutShortMsg(Handle, 0x90u | (uint)channel | ((uint)note << 8) | ((uint)velocity << 16));

				//System.Diagnostics.Debug.WriteLine(result);
			}
		}

		public static void NoteOff(int channel, int note, int velocity)
		{
			if (Handle != IntPtr.Zero)
			{
				var result = midiOutShortMsg(Handle, 0x80u | (uint)channel | ((uint)note << 8) | ((uint)velocity << 16));

				//System.Diagnostics.Debug.WriteLine(result);
			}
		}

		public static void ProgramChange(int channel, int patch)
		{
			if (Handle != IntPtr.Zero)
			{
				var result = midiOutShortMsg(Handle, 0xC0u | (uint)channel | ((uint)patch << 8));

				//System.Diagnostics.Debug.WriteLine(result);
			}
		}

		public static void ControlChange(int channel, int control, int value)
		{
			if (Handle != IntPtr.Zero)
			{
				var result = midiOutShortMsg(Handle, 0xB0u | (uint)channel | ((uint)control << 8) | ((uint)value << 16));

				//System.Diagnostics.Debug.WriteLine(result);
			}
		}

		public static void PitchBendChange(int channel, int value)
		{
			if (Handle != IntPtr.Zero)
			{
				var value1 = value & 0x7f;
				var value2 = (value >> 7) & 0x7f;

				var result = midiOutShortMsg(Handle, 0xE0u | (uint)channel | ((uint)value1 << 8) | ((uint)value2 << 16));

				//System.Diagnostics.Debug.WriteLine(result);
			}
		}

		public static void Disable()
		{
			if (Handle != IntPtr.Zero)
			{
				var result = midiOutClose(Handle);

				Handle = IntPtr.Zero;

				//System.Diagnostics.Debug.WriteLine(result);
			}
		}

		public static class Controls
		{
			public const int Bank = 0x00;
			public const int Modulation = 0x01;
			public const int Breath = 0x02;
			public const int Foot = 0x04;
			public const int Portamento = 0x05;
			public const int Volume = 0x07;
			public const int Balance = 0x07;
			public const int Pan = 0x0A;
			public const int Expression = 0x0B;
			public const int SustainEnable = 0x40;
			public const int PortamentoEnable = 0x041;
			public const int SostenutoEnable = 0x42;
			public const int SoftPedalEnable = 0x43;
			public const int LegatoPedalEnable = 0x044;
			public const int HoldEnable = 0x45;
			public const int PortamentoControl = 0x54;
			public const int Reverb = 0x5B;
			public const int Tremolo = 0x5B;
			public const int Chorus = 0x5B;
			public const int Detune = 0x5B;
			public const int Phaser = 0x5B;
		}
	}
}
