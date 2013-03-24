
namespace TomatoTool
{
	public class Clothes : ROMObject
	{
		//8文字まで
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

		private ushort price;
		public ushort Price
		{
			get
			{
				return price;
			}

			set
			{
				price = value;
			}
		}

		private byte wear;
		public byte Wear
		{
			get
			{
				return wear;
			}

			set
			{
				wear = value;
			}
		}

		public byte unknownValue0;
		public byte unknownValue1;
		public byte unknownValue2;

		public static readonly uint NAME_LENGTH_0 = 8;

		public static readonly uint SIZE = 16;

		public Clothes(TomatoAdventure tomatoAdventure, uint address)
			
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			name = new StatusString(tomatoAdventure, address, NAME_LENGTH_0);

			diffence = (ushort)tomatoAdventure.readLittleEndian(address + 8, 2);

			price = (ushort)tomatoAdventure.readLittleEndian(address + 10, 2);

			wear = tomatoAdventure.read(address + 12);

			unknownValue0 = tomatoAdventure.read(address + 13);
			unknownValue1 = tomatoAdventure.read(address + 14);
			unknownValue2 = tomatoAdventure.read(address + 15);
		}

		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{

		}

		public override uint getSize()
		{
			return SIZE;
		}
	}
}
