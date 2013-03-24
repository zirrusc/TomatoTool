using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;

namespace TomatoTool
{
	public class MapAreaList : ROMObject
	{
		public MapArea this[MapSetting mapSetting]
		{
			get
			{
				try
				{
					return mapArea[mapSetting.Number];
				}
				catch
				{
					mapArea.AddRange(new MapArea[mapSetting.Number - mapArea.Count]);
					return mapArea[mapSetting.Number];
				}
			}

			set
			{
				try
				{
					mapArea[mapSetting.Number] = value;
				}
				catch
				{
					mapArea.AddRange(new MapArea[mapSetting.Number - mapArea.Count]);
					mapArea[mapSetting.Number] = value;
				}
			}
		}

		public static List<uint> Pointer = new List<uint>()
		{
			TomatoTool.ROM.addBase(0x00033EAC)
		};
		public static uint SelectPointer = 0;


		private List<MapArea> mapArea = new List<MapArea>();
		

		public MapAreaList(TomatoAdventure tomatoAdventure)
		{
			load(tomatoAdventure);
		}

		public void load(TomatoAdventure tomatoAdventure)
		{
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
		}

		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
		}
	}
}
