using System.Collections.Generic;
using System.Drawing;

namespace TomatoTool
{
	public class BattleBackGround : ROMObject
	{
		public static List<uint> PaletteTopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x0004A078),
			ROM.addBase(0x0004A0BC),
		};
		public static List<uint> LZ77TopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x0004A030),
		};
		public static List<uint> ChipTopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x0004A03C),
		};

		private LZ77 lz77;
		public LZ77 LZ77
		{
			get
			{
				return lz77;
			}

			set
			{
				lz77 = value;
			}
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

		private Palette[] palette;
		public Palette[] Palette
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

		public static readonly uint CHIP_LENGTH_0 = 32;
		public static readonly uint CHIP_LENGTH_1 = 16;

		public static readonly uint PALETTE_LENGTH_0 = 4;

		public static readonly uint WIDTH = 0;
		public static readonly uint HEIGHT = 0;

		public BattleBackGround()
		{
			initialize();
		}
		public BattleBackGround(TomatoAdventure tomatoAdventure, Map map)
		{
			load(tomatoAdventure, map);
		}

		public override void initialize()
		{
			
		}
		public void load(TomatoAdventure tomatoAdventure, Map map)
		{
			palette = new Palette[PALETTE_LENGTH_0];
			for (uint i = 0; i < palette.GetLength(0); ++i)
			{
				palette[i] = new Palette(tomatoAdventure, tomatoAdventure.readAsAddress(tomatoAdventure.readAsAddress(PaletteTopAddressPointer[0]) + (map.BattleBackGroundNumber * ROM.ADDRESS_SIZE)) + (i * TomatoTool.Palette.SIZE));
			}

			lz77 = new LZ77(tomatoAdventure, tomatoAdventure.readAsAddress(tomatoAdventure.readAsAddress(LZ77TopAddressPointer[0]) + (map.BattleBackGroundNumber * ROM.ADDRESS_SIZE)));

			chip = new Chip[CHIP_LENGTH_0, CHIP_LENGTH_1];
			for (uint y = 0; y < chip.GetLength(1); ++y)
			{
				for (uint x = 0; x < chip.GetLength(0); ++x)
				{
					chip[x, y] = new Chip(tomatoAdventure, tomatoAdventure.readAsAddress(tomatoAdventure.readAsAddress(ChipTopAddressPointer[0]) + (map.BattleBackGroundNumber * ROM.ADDRESS_SIZE)) + (x * TomatoTool.Chip.SIZE) + (y * CHIP_LENGTH_0 * TomatoTool.Chip.SIZE));
					chip[x, y].TileNumber -= 0x200;
				}
			}
		}
		public void save(TomatoAdventure tomatoAdventure, Map map)
		{

		}
		public override uint getSize()
		{
			throw new System.NotImplementedException();
		}

		public Bitmap toBitmap()
		{
			Bitmap bitmap = new Bitmap((int)(CHIP_LENGTH_0 * Tile4Bit.WIDTH), (int)(CHIP_LENGTH_1 * Tile4Bit.WIDTH));
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				Palette[] palette = new Palette[16]
				{
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
					this.palette[0],
					this.palette[1],
					this.palette[2],
					this.palette[3],
					TomatoTool.Palette.GrayScale,
					TomatoTool.Palette.GrayScale,
				};

				for (int y = 0; y < CHIP_LENGTH_1; ++y)
				{
					for (int x = 0; x < CHIP_LENGTH_0; ++x)
					{
						Bitmap b = lz77.toBitmap(chip[x, y], palette);
						graphics.DrawImage(b, x * Tile4Bit.WIDTH, y * Tile4Bit.HEIGHT);
						b.Dispose();
					}
				}
			}

			return bitmap;
		}
	}
}
