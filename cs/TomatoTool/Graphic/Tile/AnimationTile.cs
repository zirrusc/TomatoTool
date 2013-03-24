using System.Collections.Generic;

namespace TomatoTool
{
	public class AnimationTile : ROMObject
	{
		public static readonly AnimationTile NULL;

		private List<Tile4BitList> tileList;
		public List<Tile4BitList> TileList
		{
			get
			{
				return tileList;
			}
		}

		private byte updateInterval;
		public byte UpdateInterval
		{
			get
			{
				return updateInterval;
			}

			set
			{
				updateInterval = value;
			}
		}

		private byte addAddress;
		public byte AddAddress
		{
			get
			{
				return addAddress;
			}

			set
			{
				addAddress = value;
			}
		}

		//タイルの大きさ
		//0x00 = 1
		//0x01 = 2
		//0xFF = 0x100
		private byte tileSize;
		public byte TileSize
		{
			get
			{
				return tileSize;
			}
		}

		//予想だと全部0x00
		private byte unknownValue1;

		//予想だと全部0x07
		private byte unknownValue2;

		static AnimationTile()
		{
			NULL = new AnimationTile();
		}
		public AnimationTile()
		{
			initialize();
		}
		public AnimationTile(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void initialize()
		{
			address = ROM.ADDRESS_NULL;

			tileList = new List<Tile4BitList>();

			updateInterval = 0;
			addAddress = 0;
			tileSize = 0;

			unknownValue1 = 0x00;
			unknownValue2 = 0x07;
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			updateInterval = tomatoAdventure.read(address);
			addAddress = tomatoAdventure.read(address + 2);
			byte tileListCount = tomatoAdventure.read(address + 4);
			tileSize = tomatoAdventure.read(address + 5);

			unknownValue1 = tomatoAdventure.read(address + 1);
			unknownValue2 = tomatoAdventure.read(address + 3);

			tileList = new List<Tile4BitList>();

			for (uint i = 0; i <= tileListCount; ++i)
			{
				tileList.Add((Tile4BitList)tomatoAdventure.add(new Tile4BitList(tomatoAdventure, tomatoAdventure.readAsAddress(address + 6 + (i * ROM.ADDRESS_SIZE)), tileSize + 1)));
			}
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			tomatoAdventure.write(address, updateInterval);
			tomatoAdventure.write(address + 2, addAddress);
			tomatoAdventure.write(address + 4, (byte)(tileList.Count - 1));
			tomatoAdventure.write(address + 5, tileSize);

			for (uint i = 0; i < tileList.Count; ++i)
			{
				tomatoAdventure.writeAsAddress(address + 6 + (i * ROM.ADDRESS_SIZE), tileList[(int)i].Address);
				tileList[(int)i].save(tomatoAdventure, tileList[(int)i].Address);
			}
		}
		public override uint getSize()
		{
			return (uint)(6 + (tileList.Count * ROM.ADDRESS_SIZE));
		}

		public Tile4BitList getTileList(ushort time)
		{
			return tileList[(time / updateInterval) % tileList.Count];
		}
		public Tile4BitList getTileList()
		{
			return getTileList(TomatoTool.Map.Time);
		}
	}
}
