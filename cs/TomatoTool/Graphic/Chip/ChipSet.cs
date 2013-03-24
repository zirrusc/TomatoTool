using System;
using System.Drawing;

namespace TomatoTool
{
	public class ChipSet
	{
		public Chip this[int i, int j]
		{
			get
			{
				return chip[i, j];
			}

			set
			{
				chip[i, j] = value;
			}
		}
		public int GetLength(int dimension)
		{
			return chip.GetLength(dimension);
		}

		private Chip[,] chip;
		public Chip[,] Chip
		{
			get
			{
				return chip;
			}

			set
			{
				chip = value;
			}
		}

		public static readonly uint CHIP_LENGTH_0 = 2;
		public static readonly uint CHIP_LENGTH_1 = 2;

		public static readonly int WIDTH = (int)(CHIP_LENGTH_0 * TomatoTool.Chip.WIDTH);
		public static readonly int HEIGHT = (int)(CHIP_LENGTH_1 * TomatoTool.Chip.HEIGHT);

		public static readonly uint SIZE = (CHIP_LENGTH_0 * CHIP_LENGTH_1) * TomatoTool.Chip.SIZE;

		public ChipSet()
		{
			initialize();
		}
		public ChipSet(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public void initialize()
		{
			chip = new Chip[CHIP_LENGTH_0, CHIP_LENGTH_1];
			for (uint y = 0; y < chip.GetLength(1); ++y)
			{
				for (uint x = 0; x < chip.GetLength(0); ++x)
				{
					chip[x, y] = new Chip();
				}
			}
		}

		public void load(TomatoAdventure tomatoAdventure, uint address)
		{
			chip = new Chip[CHIP_LENGTH_0, CHIP_LENGTH_1];
			for (uint y = 0; y < chip.GetLength(1); ++y)
			{
				for (uint x = 0; x < chip.GetLength(0); ++x)
				{
					chip[x, y] = new Chip(tomatoAdventure, address + (y * (CHIP_LENGTH_0 * TomatoTool.Chip.SIZE)) + (x * TomatoTool.Chip.SIZE));
				}
			}
		}

		public void save(TomatoAdventure tomatoAdventure, uint address)
		{
			for (uint y = 0; y < chip.GetLength(1); ++y)
			{
				for (uint x = 0; x < chip.GetLength(0); ++x)
				{
					chip[x, y].save(tomatoAdventure, address + (y * (CHIP_LENGTH_0 * TomatoTool.Chip.SIZE)) + (x * TomatoTool.Chip.SIZE));
				}
			}
		}
	}
}
