using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using TomatoTool;

namespace TomatoTool
{
	public class ROM
	{
		protected byte[] rom;

		protected byte saved;
		public byte Saved
		{
			get
			{
				return saved;
			}

			set
			{
				saved = value;
			}
		}


		public static readonly uint ADDRESS_SIZE = 4;

		public static readonly uint ADDRESS_NULL = 0x00000000;
		public static readonly uint ADDRESS_BASE = 0x08000000;

		Bitmap bitmap;
		BitmapData bitmapData;
		IntPtr intPtr;
		byte[] data;

		public ROM(byte[] rom)
		{
			set(rom);
			/*
			bitmap = new Bitmap(1024, rom.GetLength(0) / 1024);

			bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			intPtr = bitmapData.Scan0;
			data = new byte[Math.Abs(bitmapData.Stride) * bitmap.Height * 4];
			
			for (uint i = 0x64B4EC; i < rom.GetLength(0); ++i)
			{
				//read(i + ADDRESS_BASE);
			}*/
		}

		//ROMに入れる
		public void set(byte[] rom)
		{
			this.rom = rom;
		}

		//ROMを返す
		public byte[] get()
		{
			return rom;
		}

		//1byte取得
		public byte read(uint address)
		{
			/*
			data[(((address - ADDRESS_BASE) / 1024) * 1024 * 4) + ((address - ADDRESS_BASE) % 1024)*4 + 0] = 0xFF;
			data[(((address - ADDRESS_BASE) / 1024) * 1024 * 4) + ((address - ADDRESS_BASE) % 1024)*4 + 1] = 0xFF;
			data[(((address - ADDRESS_BASE) / 1024) * 1024 * 4) + ((address - ADDRESS_BASE) % 1024)*4 + 2] = 0xFF;
			data[(((address - ADDRESS_BASE) / 1024) * 1024 * 4) + ((address - ADDRESS_BASE) % 1024)*4 + 3] = 0xFF;
			*/
			return rom[address - ADDRESS_BASE];
		}

		//ビッグエンディアンのデータをビッグエンディアンで取得
		public uint readBigEndian(uint address, uint length)
		{
			/*uint data = 0;

			for (uint i = 0; i < length; ++i)
			{
				data <<= 8;
				data += read(address + i);
			}

			return data;*/

			switch (length)
			{
				case 1:
					return read(address);

				case 2:
					return (uint)((read(address) << 8) + read(address + 1));

				case 3:
					return (uint)((read(address) << 16) + (read(address + 1) << 8) + read(address + 2));

				case 4:
					return (uint)((read(address) << 24) + (read(address + 1) << 16) + (read(address + 2) << 8) + read(address));

				default:
					throw new Exception();
			}
		}

		//リトルエンディアンのデータをビッグエンディアンに変換して取得
		public uint readLittleEndian(uint address, uint length)
		{
			/*uint data = 0;

			for (uint i = 1; i <= length; ++i)
			{
				data <<= 8;
				data += read(address + (length - i));
			}

			return data;*/

			switch (length)
			{
				case 1:
					return read(address);

				case 2:
					return (uint)((read(address + 1) << 8) + read(address));

				case 3:
					return (uint)((read(address + 2) << 16) + (read(address + 1) << 8) + read(address));

				case 4:
					return (uint)((read(address + 3) << 24) + (read(address + 2) << 16) + (read(address + 1) << 8) + read(address));

				default:
					throw new Exception();
			}
		}
		public static uint readLittleEndian(byte[] rom, uint address, uint length)
		{
			switch (length)
			{
				case 1:
					return rom[address];

				case 2:
					return (uint)((rom[address + 1] << 8) + rom[address]);

				case 3:
					return (uint)((rom[address + 2] << 16) + (rom[address + 1] << 8) + rom[address]);

				case 4:
					return (uint)((rom[address + 3] << 24) + (rom[address + 2] << 16) + (rom[address + 1] << 8) + rom[address]);

				default:
					throw new Exception();
			}
		}
		public byte[] readArray(uint address, uint length)
		{
			byte[] data = new byte[length];

			Buffer.BlockCopy(rom, (int)(address - ROM.ADDRESS_BASE), data, 0, (int)length);

			for (uint i = address; i < address + length; ++i)
			{
				read(i + ADDRESS_BASE);
			}

			return data;
		}

		//上位4ビットを返す
		public byte readTopBit(uint address)
		{
			return (byte)(read(address) >> 4);
		}

		//下位4ビットを返す
		public byte readBottomBit(uint address)
		{
			return (byte)(read(address) & 0x0F);
		}

		//アドレスとして読み込む
		public uint readAsAddress(uint address)
		{
			return readLittleEndian(address, ADDRESS_SIZE);
		}


		//1byte書き込む
		public void write(uint address, byte data)
		{
			rom[address - ADDRESS_BASE] = data;
		}

		//ビッグエンディアンのデータをビッグエンディアンで書き込む
		//Lengthはバイトの長さ
		public void writeBigEndian(uint address, uint length, uint data)
		{
			/*for (uint i = length - 1; i >= 0; i--)
			{
				write(address + i, (byte)((data >> ((int)i * 8)) & 0xFF));
			}*/

			switch (length)
			{
				case 1:
					write(address, (byte)(data & 0xFF));
					break;

				case 2:
					write(address, (byte)((data & 0xFF00) >> 8));
					write(address + 1, (byte)(data & 0xFF));
					break;

				case 3:
					write(address, (byte)((data & 0xFF0000) >> 16));
					write(address + 1, (byte)((data & 0xFF00) >> 8));
					write(address + 2, (byte)(data & 0xFF));
					break;

				case 4:
					write(address, (byte)((data & 0xFF000000) >> 24));
					write(address + 1, (byte)((data & 0xFF0000) >> 16));
					write(address + 2, (byte)((data & 0xFF00) >> 8));
					write(address + 3, (byte)(data & 0xFF));
					break;

				default:
					throw new Exception();
			}
		}

		//ビッグエンディアンのデータをリトルエンディアンに変換して書き込む
		//Lengthはバイトの長さ
		public void writeLittleEndian(uint address, uint length, uint data)
		{
			/*for (uint i = 0; i < length; ++i)
			{
				write(address + i, (byte)((data >> ((int)i * 8)) & 0xFF));
			}*/

			switch (length)
			{
				case 1:
					write(address, (byte)(data & 0xFF));
					break;

				case 2:
					write(address, (byte)(data & 0xFF));
					write(address + 1, (byte)((data & 0xFF00) >> 8));
					break;

				case 3:
					write(address, (byte)(data & 0xFF));
					write(address + 1, (byte)((data & 0xFF00) >> 8));
					write(address + 2, (byte)((data & 0xFF0000) >> 16));
					break;

				case 4:
					write(address, (byte)(data & 0xFF));
					write(address + 1, (byte)((data & 0xFF00) >> 8));
					write(address + 2, (byte)((data & 0xFF0000) >> 16));
					write(address + 3, (byte)((data & 0xFF000000) >> 24));
					break;

				default:
					throw new Exception();
			}
		}
		public static void writeLittleEndian(byte[] rom, uint address, uint length, uint data)
		{
			switch (length)
			{
				case 1:
					rom[address] = (byte)(data & 0xFF);
					break;

				case 2:
					rom[address] = (byte)(data & 0xFF);
					rom[address + 1] = (byte)((data & 0xFF00) >> 8);
					break;

				case 3:
					rom[address] = (byte)(data & 0xFF);
					rom[address + 1] = (byte)((data & 0xFF00) >> 8);
					rom[address + 2] = (byte)((data & 0xFF0000) >> 16);
					break;

				case 4:
					rom[address] = (byte)(data & 0xFF);
					rom[address + 1] = (byte)((data & 0xFF00) >> 8);
					rom[address + 2] = (byte)((data & 0xFF0000) >> 16);
					rom[address + 3] = (byte)((data & 0xFF000000) >> 24);
					break;

				default:
					throw new Exception();
			}
		}

		public void writeArray(uint address, uint length, byte[] data)
		{
			Buffer.BlockCopy(data, 0, rom, (int)(address - ROM.ADDRESS_BASE), (int)length);
		}

		//上位4ビットに書き込む
		public void writeTopBit(uint address, byte data)
		{
			write(address, (byte)(((data & 0x0F) << 4) + (read(address) & 0x0F)));
		}

		//下位4ビットに書き込む
		public void writeBottomBit(uint address, byte data)
		{
			write(address, (byte)((read(address) & 0xF0) + (data & 0x0F)));
		}


		//アドレスとして書き込み
		public void writeAsAddress(uint address, uint data)
		{
			writeLittleEndian(address, ADDRESS_SIZE, data);
		}

		//アドレスかどうか判定
		public bool isAddress(uint address)
		{
			return (ADDRESS_BASE <= address) && (address < (ADDRESS_BASE + rom.GetLength(0)));
		}

		public static uint addBase(uint address)
		{
			return address + ROM.ADDRESS_BASE;
		}

		public void test()
		{
			/*
			Marshal.Copy(data, 0, intPtr, data.GetLength(0));
			bitmap.UnlockBits(bitmapData);
			bitmap.Save(@"tomato.png", System.Drawing.Imaging.ImageFormat.Png);
			bitmap.Dispose();
			*/
		}

		public void resize(int size)
		{
			if (rom.Length < size)
			{
				int i = rom.Length;
				Array.Resize(ref rom, size);
				for (; i < size; ++i)
				{
					rom[i] = 0xFF;
				}
			}
			else
			{
				Array.Resize(ref rom, size);
			}
		}

		protected uint virtualAddress = 0x08650000;
		public uint malloc(uint size)
		{
			return malloc(size, 4);
		}
		public uint malloc(uint size, byte multiple)
		{
			if (virtualAddress % multiple == 0)
			{
				uint address = virtualAddress;
				virtualAddress += size;
				return address;
			}
			else
			{
				virtualAddress += multiple - (virtualAddress % multiple);
				uint address = virtualAddress;
				virtualAddress += size;
				return address;
			}
		}
		public uint malloc(ISizeGettable iSizeGettable)
		{
			uint address = virtualAddress;
			virtualAddress += iSizeGettable.getSize();
			return address;
		}
	}
}
