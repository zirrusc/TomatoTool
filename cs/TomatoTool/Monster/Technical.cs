
namespace TomatoTool
{
	public class Technical : ROMObject
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

		//これの(値 * 2) - 防御力で食らうダメージ
		//0でも無敵化していなければ1食らう
		private byte attack;
		public byte Attack
		{
			get
			{
				return attack;
			}

			set
			{
				attack = value;
			}
		}

		public byte unknownValue0;
		public byte unknownValue1;
		public byte unknownValue2;

		public uint unknownAddress;

		public static readonly uint NAME_LENGTH_0 = 8;

		public static readonly uint SIZE = 16;

		public Technical(TomatoAdventure tomatoAdventure, uint address) 
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			name = new StatusString(tomatoAdventure, address, NAME_LENGTH_0);

			attack = tomatoAdventure.read(address + 8);
			
			unknownValue0 = tomatoAdventure.read(address + 9);
			unknownValue1 = tomatoAdventure.read(address + 10);
			unknownValue2 = tomatoAdventure.read(address + 11);

			unknownAddress = tomatoAdventure.readAsAddress(address + 12);
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
