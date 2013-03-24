using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TomatoTool
{
	public class Tile8Bit : Tile
	{
		public byte this[int i, int j]
		{
			get
			{
				return tile[i, j];
			}

			set
			{
				if (value < 16)
				{
					tile[i, j] = value;
				}
				else
				{
					throw new Exception();
				}
			}
		}

		//空のタイル
		public static readonly Tile4Bit NotExistTile;

		private Bitmap bitmap;

		public Bitmap Bitmap
		{
			get
			{
				return bitmap;
			}

			set
			{
				if ((value.Width == WIDTH) && (value.Height == HEIGHT) && (value.PixelFormat == PixelFormat.Format8bppIndexed))
				{
				}
				else
				{
					throw new Exception();
				}
			}
		}

		public static readonly uint TILE_LENGTH_0 = 8;
		public static readonly uint TILE_LENGTH_1 = 8;

		public static readonly uint SIZE = TILE_LENGTH_0 * TILE_LENGTH_1;

		static Tile8Bit()
		{
			NotExistTile = new Tile4Bit(new byte[TILE_LENGTH_0, TILE_LENGTH_1]);
		}

		public Tile8Bit(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			tile = new byte[TILE_LENGTH_0, TILE_LENGTH_1];

			for (uint y = 0; y < tile.GetLength(1); ++y)
			{
				for (uint x = 0; x < tile.GetLength(0); ++x)
				{
					tile[x, y] = tomatoAdventure.read(address + (y * TILE_LENGTH_0) + x);
				}
			}
		}

		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			for (uint y = 0; y < tile.GetLength(1); ++y)
			{
				for (uint x = 0; x < tile.GetLength(0); ++x)
				{
					tomatoAdventure.write(address + (y * TILE_LENGTH_0) + x, tile[x, y]);
				}
			}
		}

	}
}
