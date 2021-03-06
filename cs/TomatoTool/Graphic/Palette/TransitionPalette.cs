﻿using System;
using System.Collections.Generic;

namespace TomatoTool
{
	public class TransitionPalette : ROMObject
	{
		private byte overwritePaletteNumber;
		public byte OverwritePaletteNumber
		{
			get
			{
				return overwritePaletteNumber;
			}

			set
			{
				if (value < 0x20)
				{
					overwritePaletteNumber = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private byte updateInterval;
		public byte UpdateInterval
		{
			get
			{
				return updateInterval;
			}

			set
			{
				updateInterval = value;
			}
		}

		private List<Palette> palette;
		private List<Palette> Palette
		{
			get
			{
				return palette;
			}

			set
			{
				palette = value;
			}
		}

		//予想だと全部0x01
		private byte unknownValue1;

		public static readonly uint MAIN_SIZE = 8;


		public TransitionPalette(TomatoAdventure tomatoAdventure, uint address)
			
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			unknownValue1 = tomatoAdventure.read(address);

			overwritePaletteNumber = tomatoAdventure.read(address + 1);

			updateInterval = tomatoAdventure.read(address + 2);

			palette = new List<Palette>();

			for (uint i = 0; i < tomatoAdventure.read(address + 3); ++i)
			{
				palette.Add(new Palette(tomatoAdventure, tomatoAdventure.readAsAddress(address + 4) + (i * TomatoTool.Palette.SIZE)));
			}
		}

		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			tomatoAdventure.write(address, unknownValue1);

			tomatoAdventure.write(address + 1, overwritePaletteNumber);

			tomatoAdventure.write(address + 2, updateInterval);

			tomatoAdventure.write(address + 3, (byte)palette.Count);

			for (uint i = 0; i < palette.Count; ++i)
			{
				palette[(int)i].save(tomatoAdventure, address + MAIN_SIZE + (i * TomatoTool.Palette.SIZE));
			}

			tomatoAdventure.writeAsAddress(address + 4, palette[0].Address);
		}

		public Palette getPalette(ushort time)
		{
			return palette[(time / updateInterval) % palette.Count];
		}
		public Palette getPalette()
		{
			return getPalette(Map.Time);
		}

		public override uint getSize()
		{
			return MAIN_SIZE + ((uint)(palette.Count) * TomatoTool.Palette.SIZE);
		}
	}
}
