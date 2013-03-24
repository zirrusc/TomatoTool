
namespace TomatoTool
{
	public class Gimmick : ROMObject
	{
		//名前
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

		//レベルごとの倍率
		private byte[] level;
		public byte[] Level
		{
			get
			{
				return level;
			}
		}

		//攻撃
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

		//でんち
		private byte battery;
		public byte Battery
		{
			get
			{
				return battery;
			}

			set
			{
				battery = value;
			}
		}

		//回数
		private byte uses;
		public byte Uses
		{
			get
			{
				return uses;
			}

			set
			{
				uses = value;
			}
		}

		//説明文(戦闘中)
		//26文字まで
		private StatusString descriptionBattle;
		public StatusString DescriptionBattle
		{
			get
			{
				return descriptionBattle;
			}

			set
			{
				descriptionBattle = value;
			}
		}

		//説明文(ステータス)
		//26文字まで
		private StatusString descriptionStatus;
		public StatusString DescriptionStatus
		{
			get
			{
				return descriptionStatus;
			}

			set
			{
				descriptionStatus = value;
			}
		}

		private Palette palette;
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

		private LZ77 icon;
		public LZ77 Icon
		{
			get
			{
				return icon;
			}

			set
			{
				icon = value;
			}
		}

		public static readonly uint NAME_LENGTH_0 = 8;
		public static readonly uint DESCRIPTION_BATTLE_LENGTH_0 = 26;

		public static readonly uint LEVEL_LENGTH_0 = 9;

		public static readonly uint SIZE = 88;

		public Gimmick(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void initialize()
		{
			name = new StatusString("");

			descriptionBattle = new StatusString("");

			level = new byte[LEVEL_LENGTH_0];

			attack = 0;

			battery = 0;

			uses = 0;
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			name = new StatusString(tomatoAdventure, address, NAME_LENGTH_0);

			descriptionBattle = new StatusString(tomatoAdventure, address + 8, DESCRIPTION_BATTLE_LENGTH_0);

			level = new byte[LEVEL_LENGTH_0];
			for (uint i = 0; i < level.GetLength(0); ++i)
			{
				level[i] = tomatoAdventure.read(address + 40 + (i * 2));
			}

			attack = tomatoAdventure.read(address + 34);

			battery = tomatoAdventure.read(address + 35);

			uses = tomatoAdventure.read(address + 37);
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			name.save(tomatoAdventure, address, NAME_LENGTH_0);

			descriptionBattle.save(tomatoAdventure, address + 8, DESCRIPTION_BATTLE_LENGTH_0);

			for (uint i = 0; i < level.GetLength(0); ++i)
			{
				tomatoAdventure.write(address + 40 + (i * 2), level[i]);
			}

			tomatoAdventure.write(address + 34, attack);

			tomatoAdventure.write(address + 35, battery);

			tomatoAdventure.write(address + 37, uses);
		}
		public override uint getSize()
		{
			return SIZE;
		}
	}
}
