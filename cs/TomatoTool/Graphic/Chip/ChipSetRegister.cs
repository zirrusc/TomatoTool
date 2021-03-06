﻿using System;


namespace TomatoTool
{
	public class ChipSetRegister : ROMObject
	{
		private ushort[,] chipSet;
		public ushort[,] ChipSet
		{
			get
			{
				return chipSet;
			}

			set
			{
				chipSet = value;
			}
		}

		//使うチップセットの最大枚数
		public ushort useChipSetNumber;

		public ChipSetRegister(TomatoAdventure tomatoAdventure, uint address, Map map)	
		{
			load(tomatoAdventure, address, map);
		}

		public void load(TomatoAdventure tomatoAdventure, uint address, Map map)
		{
			this.address = address;

			chipSet = new ushort[map.Width, map.Height];

			for (uint y = 0; y < chipSet.GetLength(1); ++y)
			{
				for (uint x = 0; x < chipSet.GetLength(0); ++x)
				{
					chipSet[x, y] = (ushort)tomatoAdventure.readLittleEndian((uint)(address + (y * chipSet.GetLength(0) * 2) + (x * 2)), 2);

					if (useChipSetNumber <= chipSet[x, y])
					{
						//チップセットの読み込み枚数
						//16枚のセットで読む
						useChipSetNumber = (ushort)(chipSet[x, y] + 1);
					}
				}
			}
		}
		public void save(TomatoAdventure tomatoAdventure)
		{
			if (!saved)
			{
				save(tomatoAdventure, tomatoAdventure.malloc(getSize()));
				saved = true;
			}
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			for (uint y = 0; y < chipSet.GetLength(1); ++y)
			{
				for (uint x = 0; x < chipSet.GetLength(0); ++x)
				{
					tomatoAdventure.writeLittleEndian((uint)(address + (y * chipSet.GetLength(0) * 2) + (x * 2)), 2, chipSet[x, y]);
				}
			}
		}

		public void resize(byte width, byte height)
		{
			ushort[,] chipSet = new ushort[width, height];

			for (int y = 0; y < this.chipSet.GetLength(1); ++y)
			{
				for (int x = 0; x < this.chipSet.GetLength(0); ++x)
				{
					chipSet[x, y] = this.chipSet[x, y];
				}
			}

			this.chipSet = chipSet;
		}

		public override uint getSize()
		{
			return (uint)(chipSet.Length * 2);
		}
	}
}
