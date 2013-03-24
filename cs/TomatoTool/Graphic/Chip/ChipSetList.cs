using System.Collections.Generic;

namespace TomatoTool
{
	public class ChipSetList : ROMObject
	{
		public ChipSet this[int i]
		{
			get
			{
				return chipSet[i];
			}

			set
			{
				chipSet[i] = value;
			}
		}
		public int Count
		{
			get
			{
				return chipSet.Count;
			}
		}

		private List<ChipSet> chipSet;
		public List<ChipSet> ChipSet
		{
			get
			{
				return chipSet;
			}

			set
			{
				chipSet = value;
			}
		}

		public ChipSetList(TomatoAdventure tomatoAdventure, uint address, ChipSetRegister chipSetNumberList)
			
		{
			load(tomatoAdventure, address, chipSetNumberList);
		}

		public void load(TomatoAdventure tomatoAdventure, uint address, ChipSetRegister chipSetNumberList)
		{
			this.address = address;

			chipSet = new List<ChipSet>();
			
			for (uint i = 0; i < chipSetNumberList.useChipSetNumber; ++i)
			{
				chipSet.Add(new ChipSet(tomatoAdventure, address + (i * TomatoTool.ChipSet.SIZE)));
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

			for (uint i = 0; i < chipSet.Count; ++i)
			{
				chipSet[(int)i].save(tomatoAdventure, address + (i * TomatoTool.ChipSet.SIZE));
			}
		}

		public override uint getSize()
		{
			return (uint)(chipSet.Count * TomatoTool.ChipSet.SIZE);
		}
	}
}
