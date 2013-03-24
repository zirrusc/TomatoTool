using System.Drawing;

namespace TomatoTool
{
	public class Monster : ROMObject
	{
		private StatusString name;
		public StatusString Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}

		private ushort hp;
		public ushort HP
		{
			get
			{
				return hp;
			}

			set
			{
				hp = value;
			}
		}

		private ushort speed;
		public ushort Speed
		{
			get
			{
				return speed;
			}

			set
			{
				speed = value;
			}
		}

		private ushort diffence;
		public ushort Diffence
		{
			get
			{
				return diffence;
			}

			set
			{
				diffence = value;
			}
		}

		private ushort experience;
		public ushort Experience
		{
			get
			{
				return experience;
			}

			set
			{
				experience = value;
			}
		}

		private ushort money;
		public ushort Money
		{
			get
			{
				return money;
			}

			set
			{
				money = value;
			}
		}

		private byte size;
		public byte Size
		{
			get
			{
				return size;
			}

			set
			{
				size = value;
			}
		}

		public Palette palette;
		public Palette Palette
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

		public Tile4BitList[] tileList;

		public static readonly uint NAME_LENGTH_0 = 8;
		public static readonly uint TILE_LIST_LENGTH_0 = 2;

		public static readonly uint SIZE = 76;


		public Monster(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			name = new StatusString(tomatoAdventure, address, NAME_LENGTH_0);

			hp = (ushort)tomatoAdventure.readLittleEndian(address + 10, 2);

			speed = (ushort)tomatoAdventure.readLittleEndian(address + 12, 2);

			diffence = (ushort)tomatoAdventure.readLittleEndian(address + 14, 2);

			experience = (ushort)tomatoAdventure.readLittleEndian(address + 40, 2);

			money = (ushort)tomatoAdventure.readLittleEndian(address + 42, 2);

			size = tomatoAdventure.read(address + 47);

			if (tomatoAdventure.readAsAddress(address + 52) != ROM.ADDRESS_NULL)
			{
				palette = new Palette(tomatoAdventure, tomatoAdventure.readAsAddress(address + 52));
			}

			tileList = new Tile4BitList[TILE_LIST_LENGTH_0];

			for (uint i = 0; i < tileList.GetLength(0); ++i)
			{
				if (tomatoAdventure.readAsAddress(address + 56 + (i * ROM.ADDRESS_SIZE)) != ROM.ADDRESS_NULL)
				{
					if (tomatoAdventure.read(tomatoAdventure.readAsAddress(address + 56) + (i * ROM.ADDRESS_SIZE)) == LZ77.HEADER)
					{
						tileList[i] = new LZ77(tomatoAdventure, tomatoAdventure.readAsAddress(address + 56) + (i * ROM.ADDRESS_SIZE));
					}
					else
					{
					}
				}
			}
		}

		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{

		}

		public void draw(Graphics graphics)
		{
		}

		public override uint getSize()
		{
			return SIZE;
		}
	}
}
