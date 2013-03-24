
using System.Drawing;

namespace TomatoTool
{
	public class Palette : ROMObject
	{
		public Color16Bit this[int i]
		{
			get
			{
				return color[i];
			}

			set
			{
				color[i] = value;
			}
		}
		public int GetLength(int dimension)
		{
			return color.GetLength(dimension);
		}
		
		public static readonly Palette GrayScale;

		private Color16Bit[] color;
		public Color16Bit[] Color
		{
			get
			{
				return color;
			}
		}

		public static readonly uint COLOR_LENGTH_0 = 16;
		public static readonly uint SIZE = COLOR_LENGTH_0 * TomatoTool.Color16Bit.SIZE;

		static Palette()
		{
			Color16Bit[] color = new Color16Bit[COLOR_LENGTH_0];

			for (uint i = 0; i < color.GetLength(0); ++i)
			{
				color[i] = new Color16Bit((byte)(i * 2), (byte)(i * 2), (byte)(i * 2));
			}
			GrayScale = new Palette(color);
		}

		public Palette()
		{
			initialize();
		}
		public Palette(Color16Bit[] color)
		{
			address = TomatoTool.ROM.ADDRESS_NULL;

			this.color = new Color16Bit[COLOR_LENGTH_0];

			for (uint i = 0; (i < color.GetLength(0)) && (i < this.color.GetLength(0)); ++i)
			{
				if (color[i] != null)
				{
					this.color[i] = color[i].Clone();
				}
			}
		}
		public Palette(Color[] color)
		{
			set(color);
		}
		public Palette(byte[] palette)
		{
			set(palette);
		}
		public Palette(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void initialize()
		{
			color = new Color16Bit[COLOR_LENGTH_0];
		}
		public void load(TomatoAdventure tomatoAdventure)
		{
			load(tomatoAdventure, address);
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			color = new Color16Bit[COLOR_LENGTH_0];
			for (uint i = 0; i < color.GetLength(0); ++i)
			{
				color[i] = new Color16Bit(tomatoAdventure, address + (i * TomatoTool.Color16Bit.SIZE));
			}
		}
		public void save(TomatoAdventure tomatoAdventure)
		{
			if (!Saved)
			{
				save(tomatoAdventure, tomatoAdventure.malloc(SIZE));
				saved = true;
			}
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			for (uint i = 0; i < color.GetLength(0); ++i)
			{
				color[i].save(tomatoAdventure, address + (i * TomatoTool.Color16Bit.SIZE));
			}
		}

		public byte[] get()
		{
			byte[] palette = new byte[SIZE];

			for (uint i = 0; i < color.GetLength(0); ++i)
			{
				ROM.writeLittleEndian(palette, i * TomatoTool.Color16Bit.SIZE, TomatoTool.Color16Bit.SIZE, color[i].get());
			}

			return palette;
		}
		public void set(byte[] palette)
		{
			for (uint i = 0; i < color.GetLength(0); ++i)
			{
				color[i].set((ushort)ROM.readLittleEndian(palette, i * TomatoTool.Color16Bit.SIZE, TomatoTool.Color16Bit.SIZE));
			}
		}
		public void set(Color[] color)
		{
			this.color = new Color16Bit[COLOR_LENGTH_0];

			for (uint i = 0; i < color.GetLength(0); ++i)
			{
				this.color[i] = new Color16Bit(color[i]);
			}
		}

		public override uint getSize()
		{
			return SIZE;
		}

		public void draw(Graphics graphics, int width, int height)
		{
			if (color != null)
			{
				for (int i = 0; i < color.GetLength(0); ++i)
				{
					graphics.FillRectangle(new SolidBrush(color[i].toColor()), i * width, 0, width, height);
				}
			}
		}
	}
}
