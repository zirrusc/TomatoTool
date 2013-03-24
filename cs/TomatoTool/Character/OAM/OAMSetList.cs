using System.Collections.Generic;

namespace TomatoTool
{
	public class OAMSetList : ROMObject
	{
		public OAMSet this[int i]
		{
			get
			{
				return oamSet[i];
			}

			set
			{
				oamSet[i] = value;
			}
		}
		public int Count
		{
			get
			{
				return oamSet.Count;
			}
		}

		private List<OAMSet> oamSet;
		public List<OAMSet> OAMSet
		{
			get
			{
				return oamSet;
			}
		}

		public OAMSetList(TomatoAdventure tomatoAdventure, uint address)
			
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			{
				oamSet = new List<OAMSet>();
				uint i = 0;
				while (tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + (i * ROM.ADDRESS_SIZE))))
				{
					oamSet.Add(new OAMSet(tomatoAdventure, (address) + (i * ROM.ADDRESS_SIZE)));
					++i;
				}
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

			for (uint i = 0; i < oamSet.Count; ++i)
			{
				oamSet[(int)i].save(tomatoAdventure);
				tomatoAdventure.writeAsAddress(address + (i * ROM.ADDRESS_SIZE), oamSet[(int)i].Address);
			}
		}

		public override uint getSize()
		{
			return (uint)(oamSet.Count * ROM.ADDRESS_SIZE);
		}
	}
}
