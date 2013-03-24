
using System;
using System.Collections.Generic;
using TomatoTool;

namespace TomatoTool
{
	public class MapList : ROMObject, ISizeGettable
	{
		public Map this[int i]
		{
			get
			{
				return map[i];
			}

			set
			{
				map[i] = value;
			}
		}

		public int Count
		{
			get
			{
				return map.Count;
			}
		}

		public List<uint> TopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x0002C2A8),
			ROM.addBase(0x00033CFC),
			ROM.addBase(0x0003F8F4),
			ROM.addBase(0x0004A028),
			ROM.addBase(0x0004A070),
			ROM.addBase(0x0004A0B0),
			ROM.addBase(0x0006C4F4)
		};

		public int SelectTopAddressPointer = 0;

		private List<Map> map;
		public List<Map> Map
		{
			get
			{
				return map;
			}
		}

		public MapList(TomatoAdventure tomatoAdventure)
		{
			load(tomatoAdventure);
		}

		public void load(TomatoAdventure tomatoAdventure)
		{
			address = tomatoAdventure.readAsAddress(TopAddressPointer[SelectTopAddressPointer]);

			map = new List<Map>();
			{
				uint i = 0;
				while
				(
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 12 + (i * TomatoTool.Map.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 16 + (i * TomatoTool.Map.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 20 + (i * TomatoTool.Map.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 24 + (i * TomatoTool.Map.SIZE)))
				)
				{
					map.Add((Map)tomatoAdventure.add(new Map(tomatoAdventure, address + (i * TomatoTool.Map.SIZE))));
					++i;
				}
			}
		}

		public void save(TomatoAdventure tomatoAdventure)
		{
			address = tomatoAdventure.malloc((uint)(map.Count * TomatoTool.Map.SIZE));

			for (int i = 0; i < TopAddressPointer.Count; ++i)
			{
				tomatoAdventure.writeAsAddress(TopAddressPointer[i], address);
			}

			FormSaveProgress formSaveProgress = new FormSaveProgress();
			formSaveProgress.Show();
			formSaveProgress.Maximum = map.Count;
			for (uint i = 0; i < map.Count; ++i)
			{
				System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
				stopwatch.Start();
				formSaveProgress.setText("Map" + System.String.Format("{0:X4}", i));
				formSaveProgress.Refresh();
				map[(int)i].save(tomatoAdventure, address + (i * TomatoTool.Map.SIZE));
				formSaveProgress.performStep();
				stopwatch.Stop();
				System.Console.WriteLine(System.String.Format("{0:X4}", i) + ":" + stopwatch.Elapsed);
			}
			formSaveProgress.Close();
		}

		public override uint getSize()
		{
			return (uint)(map.Count * TomatoTool.Map.SIZE);
		}
	}
}
