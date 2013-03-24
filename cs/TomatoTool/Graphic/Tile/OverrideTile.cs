
namespace TomatoTool
{
	public class OverrideTile : ROMObject
	{
		private byte x;
		public byte X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		private byte y;
		public byte Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		private byte width;
		public byte Width
		{
			get
			{
				return width;
			}

			set
			{
				width = value;

			}
		}

		private byte height;
		public byte Height
		{
			get
			{
				return height;
			}

			set
			{
				height = value;
			}
		}

		private byte bg;
		public byte BG
		{
			get
			{
				return bg;
			}

			set
			{
				bg = value;
			}
		}

		private Chip[, ,] chip;
		public Chip[, ,] Chip
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

		public byte unknownValue0;
		public byte unknownValue1;
		public byte unknownValue2;

		public OverrideTile(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			unknownValue0 = tomatoAdventure.read(address);
			unknownValue1 = tomatoAdventure.read(address + 1);
			unknownValue2 = tomatoAdventure.read(address + 7);

			this.x = tomatoAdventure.read(address + 2);
			this.y = tomatoAdventure.read(address + 3);

			width = tomatoAdventure.read(address + 4);
			height = tomatoAdventure.read(address + 5);

			chip = new Chip[CHIP_LENGTH_0, this.x, this.y];

			for (uint i = 0; i < chip.GetLength(0); ++i)
			{
				for (uint y = 0; y < chip.GetLength(2); ++y)
				{
					for (uint x = 0; x < chip.GetLength(1); ++x)
					{
						try
						{
							chip[i, x, y] = new Chip(tomatoAdventure, (uint)(address + (((i * (this.x * this.y)) + (y * this.x) + x) * 2)));
						}
						catch
						{
						}
					}
				}
			}
		}

		public void save(TomatoAdventure tomatoAdventure)
		{
			if (!saved)
			{
				save(tomatoAdventure, tomatoAdventure.malloc(getSize(), 2));
				saved = true;
			}
		}

		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			tomatoAdventure.write(address, unknownValue0);
			tomatoAdventure.write(address + 1, unknownValue1);
			tomatoAdventure.write(address + 7, unknownValue2);

			tomatoAdventure.write(address + 2, this.x);
			tomatoAdventure.write(address + 3, this.y);

			tomatoAdventure.write(address + 4, width);
			tomatoAdventure.write(address + 5, height);

			for (uint i = 0; i < chip.GetLength(0); ++i)
			{
				for (uint y = 0; y < chip.GetLength(2); ++y)
				{
					for (uint x = 0; x < chip.GetLength(1); ++x)
					{
						chip[i, x, y].save(tomatoAdventure, (uint)(address + (((i * (this.x * this.y)) + (y * this.x) + x) * 2)));
					}
				}
			}
		}

		public override uint getSize()
		{
			return (uint)(8 + (((x * y) * 2) * CHIP_LENGTH_0));
		}
	}
}
