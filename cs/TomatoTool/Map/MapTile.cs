using System.Drawing;
using System.Drawing.Imaging;

namespace TomatoTool
{
	public class MapTile
	{
		private LZ77 mainTile;
		public LZ77 MainTile
		{
			get
			{
				return mainTile;
			}

			set
			{
				mainTile = value;
			}
		}

		private AnimationTile animationTile;
		public AnimationTile AnimationTile
		{
			get
			{
				return animationTile;
			}

			set
			{
				animationTile = value;
			}
		}

		public static uint MainTileTopAddress;
		public static uint AnimationTileTopAddress;

		public MapTile(TomatoAdventure tomatoAdventure, Map map)
		{
			MainTileTopAddress = tomatoAdventure.readAsAddress(ROM.addBase(0x034D70));
			AnimationTileTopAddress = tomatoAdventure.readAsAddress(ROM.addBase(0x034080));

			load(tomatoAdventure, map);
		}

		public void load(TomatoAdventure tomatoAdventure, Map map)
		{
			mainTile = (LZ77)tomatoAdventure.add(new LZ77(tomatoAdventure, tomatoAdventure.readAsAddress(MainTileTopAddress + (map.Number * ROM.ADDRESS_SIZE))));


			if (map.AnimationTileNumber != 0x00)
			{
				animationTile = (AnimationTile)tomatoAdventure.add(new AnimationTile(tomatoAdventure, tomatoAdventure.readAsAddress((uint)(AnimationTileTopAddress + ((map.AnimationTileNumber - 1) * ROM.ADDRESS_SIZE)))));
			}
			else
			{
				animationTile = (AnimationTile)tomatoAdventure.add(AnimationTile.NULL);
			}
		}

		public void save(TomatoAdventure tomatoAdventure, Map map)
		{
			mainTile.save(tomatoAdventure);
			tomatoAdventure.writeAsAddress(MainTileTopAddress + (map.Number * ROM.ADDRESS_SIZE), mainTile.Address);
		}

		public Bitmap toBitmap(Chip chip, Palette[] palette, TransitionPalette transitionPalette)
		{
			//transitionPaletteが実装されたらこれ消せ
			if (chip.TileNumber < (0x0300 + ((animationTile != null) ? animationTile.AddAddress : 0)))
			{
				if (chip.TileNumber < mainTile.Count)
				{
					return mainTile[chip.TileNumber].toBitmap(palette[chip.PaletteNumber], chip.FlipX, chip.FlipY);
				}
				else
				{
					return Tile4Bit.NotExistTile.toBitmap(palette[chip.PaletteNumber]);
				}
			}
			else
			{
				if ((animationTile != null) && ((chip.TileNumber - (0x0300 + animationTile.AddAddress)) <= animationTile.TileSize))
				{
					return animationTile.getTileList()[chip.TileNumber - (0x0300 + animationTile.AddAddress)].toBitmap(palette[chip.PaletteNumber], chip.FlipX, chip.FlipY);
				}
				else
				{
					return Tile4Bit.NotExistTile.toBitmap(palette[chip.PaletteNumber]);
				}
			}
			//transitionPaletteが実装されたらこれだけ残せ

			try
			{
				if (chip.TileNumber < 0x0300)
				{
					return mainTile[chip.TileNumber].toBitmap(((chip.PaletteNumber == transitionPalette.OverwritePaletteNumber) ? transitionPalette.getPalette() : palette[chip.PaletteNumber]), chip.FlipX, chip.FlipY);
				}
				else
				{
					return animationTile.getTileList()[chip.TileNumber - 0x0300].toBitmap(((chip.PaletteNumber == transitionPalette.OverwritePaletteNumber) ? transitionPalette.getPalette() : palette[chip.PaletteNumber]), chip.FlipX, chip.FlipY);
				}
			}
			catch
			{
				return Tile4Bit.NotExistTile.toBitmap(palette[chip.PaletteNumber]);
			}

		}
		public Bitmap toBitmap(ChipSet chipSet, Palette[] palette, TransitionPalette transitionPalette, bool transparent)
		{
			Bitmap bitmap = new Bitmap(ChipSet.WIDTH, ChipSet.HEIGHT);

			if (transparent)
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				using (ImageAttributes imageAttributes = new ImageAttributes())
				{
					for (int y = 0; y < chipSet.GetLength(1); ++y)
					{
						for (int x = 0; x < chipSet.GetLength(0); ++x)
						{
							Color color = palette[chipSet[x, y].PaletteNumber][0].toColor();
							imageAttributes.SetColorKey(color, color);

							using (Bitmap b = toBitmap(chipSet[x, y], palette, transitionPalette))
							{
								graphics.DrawImage(b, new Rectangle(x * Chip.WIDTH, y * Chip.HEIGHT, Chip.WIDTH, Chip.HEIGHT), 0, 0, Chip.WIDTH, Chip.HEIGHT, GraphicsUnit.Pixel, imageAttributes);
							}
						}
					}
				}
			}
			else
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					for (int y = 0; y < chipSet.GetLength(1); ++y)
					{
						for (int x = 0; x < chipSet.GetLength(0); ++x)
						{
							using (Bitmap b = toBitmap(chipSet[x, y], palette, transitionPalette))
							{
								graphics.DrawImage(b, x * Chip.WIDTH, y * Chip.HEIGHT);
							}
						}
					}
				}
			}
			
			return bitmap;
		}
	}
}
