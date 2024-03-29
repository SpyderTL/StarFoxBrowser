﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarFoxBrowser
{
	public static class Compression
	{
		public static byte[] CompressedData;
		public static byte[] Data;

		public static void Decompress()
		{
			src = new byte[0x10000]; // source buffer
			dest = new byte[0x10000]; // destination buffer
			s = 0;
			d = 0;
			d_length = 0;
			buffer = 0;
			b = 0;

			Array.Copy(CompressedData, src, CompressedData.Length);

			s = CompressedData.Length;

			decrunch();

			Data = new byte[d_length];

			Array.Copy(dest, Data, d_length);
		}

		// 64k should be enough for anything
		static byte[] src = new byte[0x10000]; // source buffer
		static byte[] dest = new byte[0x10000]; // destination buffer
		static int s = 0; // source pointer
		static int d; // destination pointer
		static int d_length = 0; // length of decrunched data

		static uint buffer; // 32-bit buffer
		static int b = 0; // buffer bit offset

		static void put_byte(int value)
		{
			if (d == 0) return;
			dest[--d] = (byte)value;
		}

		static void refill_buffer()
		{
			buffer = 0;
			b = 0;

			if (s == 0) return;
			buffer = src[--s];
			b += 8;

			if (s == 0) return;
			buffer |= ((uint)src[--s] << 8);
			b += 8;

			if (s == 0) return;
			buffer |= ((uint)src[--s] << 16);
			b += 8;

			if (s == 0) return;
			buffer |= ((uint)src[--s] << 24);
			b += 8;
		}

		static void init_buffer()
		{
			refill_buffer();
			uint mask = 1;
			b = 1;
			while ((buffer & ~mask) != 0)
			{
				mask <<= 1;
				mask |= 1;
				b++;	
			}

			// mask off the last bit
			buffer &= (mask >> 1);
			b--;
		}

		static int get_bits(int n)
		{
			int bits = 0;
			if (n > b)
			{
				n -= b;
				bits = get_bits(b);
				refill_buffer();
			}
			for (int i = 0; i < n; i++)
			{
				bits <<= 1;
				bits |= (int)(buffer & 1u);
				buffer >>= 1;
			}
			b -= n;
			return bits;
		}

		static void put_raw(int run)
		{
			// write uncompressed bytes
			while (run-- != 0) put_byte(get_bits(8));
		}

		static void put_lzw(int run, int offset)
		{
			if (d == 0)
				return;

			// write bytes from destination buffer (lzw)
			while (run-- != 0) put_byte(dest[d + offset - 1]);
		}

		static void decrunch_lzw()
		{
			int control = get_bits(2);
			int run = 0;
			int offset = -1;

			if (control == 0)
			{
				// comx10 (run is 2 bytes)
				run = 2;
				offset = get_bits(8);

			}
			else if (control == 1)
			{
				// com1xx (run is 3 bytes)
				run = 3;
				if (get_bits(1) != 0)
				{
					offset = get_bits(8);
				}
				else
				{
					offset = get_bits(14);
				}
			}
			else if (control == 2)
			{
				// fill (run is 4 bytes)
				run = 4;
			}
			else if ((control = get_bits(2)) < 2)
			{
				// run is 5 or 6 bytes
				run = control + 5;
			}
			else if (control == 2)
			{
				// com11x (run is 7 to 10 bytes)
				run = get_bits(2) + 7;
			}
			else if (control == 3)
			{
				// com111 (run is 11 to 255 bytes)
				run = get_bits(8);
			}

			if (run > 3)
			{
				// fill
				if (get_bits(1) == 0)
				{
					offset = get_bits(16);
				}
				else if (get_bits(1) != 0)
				{
					offset = get_bits(8);
				}
				else
				{
					offset = get_bits(12);
				}
			}

			put_lzw(run, offset);
		}

		static void decrunch()
		{
			// get the decrunched length
			d_length |= src[--s];
			d_length |= src[--s] << 8;
			s -= 2; // skip two bytes
			d = d_length;

			// initialize the bit buffer
			init_buffer();

			while (d != 0)
			{
				int run = get_bits(3);
				if (run == 0)
				{
					// compressed, fall through to below
				}
				else if (run < 7)
				{
					// copy 1 to 6 bytes
					put_raw(run);
				}
				else if (get_bits(1) == 0)
				{
					// copy 7 to 22 bytes
					put_raw(get_bits(4) + 7);
				}
				else if ((run = get_bits(10)) != 0)
				{
					// copy up to 0x3FF bytes
					put_raw(run);
				}
				else
				{
					// copy up to 0x3FFFF bytes
					put_raw(get_bits(18));
				}
				decrunch_lzw();
			}
		}
	}
}