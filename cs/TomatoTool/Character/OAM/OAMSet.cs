using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TomatoTool
{
	public class OAMSet : ROMObject
	{
		public OAM this[int i]
		{
			get
			{
				return oam[i];
			}

			set
			{
				oam[i] = value;
			}
		}
		public int Count
		{
			get
			{
				return oam.Count;
			}
		}

		private List<OAM> oam;
		public List<OAM> OAM
		{
			get
			{
				return oam;
			}
		}

		//座標指定できる範囲0x0000-0x01FF合計0x0200
		public static readonly uint WIDTH = 0x0200;
		//座標指定できる範囲0x00-0xFF合計0x0100
		public static readonly uint HEIGHT = 0x0100;

		public static readonly uint HEADER_SIZE = 2;

		//座標指定できる範囲0x0000-0x01FF合計0x0200の中心
		public static readonly uint CENTER_X = 0x0100;
		//座標指定できる範囲0x00-0xFF合計0x0100の中心
		public static readonly uint CENTER_Y = 0x80;

		public OAMSet(TomatoAdventure tomatoAdventure, uint address)
			
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			oam = new List<OAM>();

			for (uint i = 0; i < tomatoAdventure.readLittleEndian(tomatoAdventure.readAsAddress(address), HEADER_SIZE); ++i)
			{
				oam.Add(new OAM(tomatoAdventure, tomatoAdventure.readAsAddress(address) + HEADER_SIZE + (i * TomatoTool.OAM.SIZE)));
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

			tomatoAdventure.writeLittleEndian(address, HEADER_SIZE, (uint)oam.Count);

			for (uint i = 0; i < oam.Count; ++i)
			{
				oam[(int)i].save(tomatoAdventure, address + HEADER_SIZE + (i * TomatoTool.OAM.SIZE));
			}
		}

		public void draaw(Graphics graphics, Tile4BitList tileList, Palette palette)
		{
			for (int i = 0; i < oam.Count; ++i)
			{
				//oam[i].draw(graphics, (int)(oam[i].X + CENTER_X), (int)(oam[i].Y + CENTER_Y), tileList, palette);
			}
		}

		public Bitmap toBitmap(Tile4BitList tileList, Palette palette, bool isTransparent)
		{
			Bitmap bitmap = new Bitmap((int)WIDTH, (int)HEIGHT);

			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.TranslateTransform(CENTER_X, CENTER_Y);
				
				if (isTransparent)
				{
					using (ImageAttributes imageAttributes = new ImageAttributes())
					{
						Color color = palette[0].toColor();
						imageAttributes.SetColorKey(color, color);
							
						for (int i = 0; i < oam.Count; ++i)
						{
							using (Bitmap oamBitmap = oam[i].toBitmap(tileList, palette))
							{
								graphics.DrawImage(oamBitmap, new Rectangle(oam[i].X, oam[i].Y, (oam[i].Width * Tile.WIDTH), (oam[i].Height * Tile.HEIGHT)), 0, 0, (oam[i].Width * Tile.WIDTH), (oam[i].Height * Tile.HEIGHT), GraphicsUnit.Pixel, imageAttributes);
							}
						}
					}
				}
				else
				{
					for (int i = 0; i < oam.Count; ++i)
					{
						using (Bitmap oamBitmap = oam[i].toBitmap(tileList, palette))
						{
							graphics.DrawImage(oamBitmap, oam[i].X, oam[i].Y, (oam[i].Width * Tile.WIDTH), (oam[i].Height * Tile.HEIGHT));
						}
					}
				}
			}

			return bitmap;
		}

		public override uint getSize()
		{
			return (uint)(HEADER_SIZE + (oam.Count * TomatoTool.OAM.SIZE));
		}
	}
}
