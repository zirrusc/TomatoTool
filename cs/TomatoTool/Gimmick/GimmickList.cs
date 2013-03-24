using System.Collections.Generic;


namespace TomatoTool
{
	public class GimmickList : ROMObject
	{
		public Gimmick this[int i]
		{
			get
			{
				return gimmick[i];
			}

			set
			{
				gimmick[i] = value;
			}
		}

		private List<Gimmick> gimmick;
		public List<Gimmick> Gimmick
		{
			get
			{
				return gimmick;
			}

			set
			{
				gimmick = value;
			}
		}


		public GimmickList(TomatoAdventure tomatoAdventure)
		{
			load(tomatoAdventure);
		}

		public override void initialize()
		{
		}
		public void load(TomatoAdventure tomatoAdventure)
		{
			uint i = 0;
			while
			(
				tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 64 + (i * TomatoTool.Gimmick.SIZE))) &&
				tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 68 + (i * TomatoTool.Gimmick.SIZE))) &&
				tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 72 + (i * TomatoTool.Gimmick.SIZE))) &&
				tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 76 + (i * TomatoTool.Gimmick.SIZE))) &&
				tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 80 + (i * TomatoTool.Gimmick.SIZE))) &&
				tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 84 + (i * TomatoTool.Gimmick.SIZE)))
			)
			{
				gimmick.Add((Gimmick)tomatoAdventure.add(new Gimmick(tomatoAdventure, address + (i * TomatoTool.Gimmick.SIZE))));
				gimmick[(int)i].Icon = new LZ77(tomatoAdventure, tomatoAdventure.readAsAddress(ROM.addBase(0x0064A4AC) + (i * ROM.ADDRESS_SIZE)));
				gimmick[(int)i].Palette = new Palette(tomatoAdventure, tomatoAdventure.readAsAddress(ROM.addBase(0x0064A574) + (i * ROM.ADDRESS_SIZE)));
				++i;
			}
		}
		
	}
}
