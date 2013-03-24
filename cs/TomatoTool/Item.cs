
namespace TomatoTool
{
	public class Item : ROMObject 
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

		public byte unknownValue0;
		public byte unknownValue1;

		public uint unknownAddress;

		public static readonly uint NAME_LENGTH_0 = 8;

		public static readonly uint SIZE = 16;

	
		public Item(TomatoAdventure tomatoAdventure, uint address)
			
        {
            load(tomatoAdventure, address);
        }

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;
		}

		public override uint getSize()
		{
			return SIZE;
		}
	}
}
