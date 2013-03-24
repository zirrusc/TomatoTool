using System;
using System.Collections.Generic;

namespace TomatoTool
{
	public class TomatoAdventure : ROM
	{
		private MapList mapList;
		public MapList MapList
		{
			get
			{
				return mapList;
			}

			set
			{
				mapList = value;
			}
		}

		private GimmickList gimmickList;
		public GimmickList GimmickList
		{
			get
			{
				return gimmickList;
			}

			set
			{
				gimmickList = value;
			}
		}

		private List<Gimmick> gimmick;
		public List<Gimmick> Gimmick
		{
			get
			{
				return gimmick;
			}
		}

		private List<Monster> monster;
		public List<Monster> Monster
		{
			get
			{
				return monster;
			}
		}

		private List<Clothes> clothes;
		public List<Clothes> Clothes
		{
			get
			{
				return clothes;
			}
		}

		private StatusFontList statusCharacterFontList;
		public StatusFontList StatusCharacterFontList
		{
			get
			{
				return statusCharacterFontList;
			}

			set
			{
				statusCharacterFontList = value;
			}
		}

		private WindowFontList windowCharacterFontList;
		public WindowFontList WindowCharacterFontList
		{
			get
			{
				return windowCharacterFontList;
			}

			set
			{
				windowCharacterFontList = value;
			}
		}

		private Dictionary<Type, List<ROMObject>> objectList;
		public Dictionary<Type, List<ROMObject>> ObjectList
		{
			get
			{
				return objectList;
			}

			set
			{
				objectList = value;
			}
		}

		public TomatoAdventure(byte[] rom)
			: base(rom)
		{
			objectList = new Dictionary<Type, List<ROMObject>>();
			load(rom);
		}

		public void load(byte[] rom)
		{
			mapList = new MapList(this);
			//gimmickList = new GimmickList(this);
			gimmick = createGimmick(this, ROM.addBase(0x0062C250));
			monster = createMonster(this, ROM.addBase(0x00634F50));

			//test
			statusCharacterFontList = new StatusFontList(this, ROM.addBase(0x00648748));
			windowCharacterFontList = new WindowFontList(this, ROM.addBase(0x0064274E));

			//test2

			//tomatoAdventure.test();
			/*uint i = 0;
			
			foreach (KeyValuePair<uint, ROMObject> r in ROMObject.ROMObjectDictionary)
			{
				i += r.Value.getSize();
			}
			Console.WriteLine(i);*/
		}

		private Dictionary<Type, List<ROMObject>> saveObjectList;
		public void save()
		{
			MapList.save(this);
			++saved;
		}
		public void save(ROMObject romObject)
		{
			if (romObject != null)
			{
				Type type = romObject.GetType();

				if (saveObjectList.ContainsKey(type))
				{
					if (saveObjectList[type].IndexOf(romObject) == -1)
					{
						saveObjectList[type].Add(romObject);
					}
				}
				else
				{
					saveObjectList.Add(type, new List<ROMObject>());
					saveObjectList[type].Add(romObject);
				}
			}
			else
			{
				throw new Exception();
			}
		}


		public void rebuild()
		{
			/*
			const uint rebuildAddress = 0x650000;
			uint position = 0;

			//アドレスの変更
			for (uint i = 0; i < TomatoTool.Map.topAddress.Count; ++i)
			{
				tomatoAdventure.writeAsAddress((int)TomatoTool.Map.topAddress[i], rebuildAddress + position);
			}

			//マップの分だけ移動
			position = map.Count * TomatoTool.Map.SIZE;

			for (uint i = 0; i < map.Count; ++i)
			{
				map[i].WarpScriptList.save(tomatoAdventure, rebuildAddress + position);
				position += map[i].WarpScriptList.getSize();
			}

			position = 0;
			for (uint i = 0; i < map.Count; ++i)
			{
				position += map[i].save(tomatoAdventure, rebuildAddress + position);
			}
			*/
		}

		public ROMObject add(ROMObject romObject)
		{
			if (romObject != null)
			{
				Type type = romObject.GetType();

				if (objectList.ContainsKey(type))
				{
					int index = objectList[type].IndexOf(romObject);

					if (index == -1)
					{
						objectList[type].Add(romObject);
						return romObject;
					}
					else
					{
						return objectList[type][index];
					}
				}
				else
				{
					objectList.Add(type, new List<ROMObject>());
					objectList[type].Add(romObject);
					return romObject;
				}
			}
			else
			{
				throw new Exception();
			}
		}
		public void objectListSort()
		{
			foreach (KeyValuePair<Type, List<ROMObject>> listROMObject in objectList)
			{
				listROMObject.Value.Sort();
			}
		}

		private List<Gimmick> createGimmick(TomatoAdventure tomatoAdventure, uint address)
		{
			List<Gimmick> gimmick = new List<Gimmick>();

			{
				uint i = 0;
				while
				(
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 60 + (i * TomatoTool.Gimmick.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 64 + (i * TomatoTool.Gimmick.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 68 + (i * TomatoTool.Gimmick.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 72 + (i * TomatoTool.Gimmick.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 76 + (i * TomatoTool.Gimmick.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 80 + (i * TomatoTool.Gimmick.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 84 + (i * TomatoTool.Gimmick.SIZE)))
				)
				{
					gimmick.Add((Gimmick)tomatoAdventure.add(new Gimmick(tomatoAdventure, address + (i * TomatoTool.Gimmick.SIZE))));
					gimmick[(int)i].Icon = (LZ77)tomatoAdventure.add(new LZ77(tomatoAdventure, tomatoAdventure.readAsAddress(ROM.addBase(0x0064A4AC) + (i * ROM.ADDRESS_SIZE))));
					gimmick[(int)i].Palette = (Palette)tomatoAdventure.add(new Palette(tomatoAdventure, tomatoAdventure.readAsAddress(ROM.addBase(0x0064A574) + (i * ROM.ADDRESS_SIZE))));
					++i;
				}
			}

			return gimmick;
		}

		private List<Monster> createMonster(TomatoAdventure tomatoAdventure, uint address)
		{
			List<Monster> monster = new List<Monster>();

			{
				uint i = 0;

				while
				(
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 20 + (i * TomatoTool.Monster.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 24 + (i * TomatoTool.Monster.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 28 + (i * TomatoTool.Monster.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 32 + (i * TomatoTool.Monster.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 36 + (i * TomatoTool.Monster.SIZE))) &&
					(tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 52 + (i * TomatoTool.Monster.SIZE))) || (ROM.ADDRESS_NULL == tomatoAdventure.readAsAddress(address + 52 + (i * TomatoTool.Monster.SIZE)))) &&
					(tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 56 + (i * TomatoTool.Monster.SIZE))) || (ROM.ADDRESS_NULL == tomatoAdventure.readAsAddress(address + 56 + (i * TomatoTool.Monster.SIZE)))) &&
					(tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 60 + (i * TomatoTool.Monster.SIZE))) || (ROM.ADDRESS_NULL == tomatoAdventure.readAsAddress(address + 60 + (i * TomatoTool.Monster.SIZE)))) &&
					(tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 64 + (i * TomatoTool.Monster.SIZE))) || (ROM.ADDRESS_NULL == tomatoAdventure.readAsAddress(address + 64 + (i * TomatoTool.Monster.SIZE)))) &&
					(tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 68 + (i * TomatoTool.Monster.SIZE))) || (ROM.ADDRESS_NULL == tomatoAdventure.readAsAddress(address + 68 + (i * TomatoTool.Monster.SIZE)))) &&
					(tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 72 + (i * TomatoTool.Monster.SIZE))) || (ROM.ADDRESS_NULL == tomatoAdventure.readAsAddress(address + 72 + (i * TomatoTool.Monster.SIZE))))
				)
				{
					monster.Add((Monster)tomatoAdventure.add(new Monster(tomatoAdventure, address + (i * TomatoTool.Monster.SIZE))));
					++i;
				}
			}

			return monster;
		}

		private List<Clothes> createClothes(TomatoAdventure tomatoAdventure, uint address)
		{
			List<Clothes> clothes = new List<Clothes>();

			{
				uint i = 0;

				while
				(
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 20 + (i * TomatoTool.Monster.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 24 + (i * TomatoTool.Monster.SIZE))) &&
					tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(address + 28 + (i * TomatoTool.Monster.SIZE)))
				)
				{
					clothes.Add(new Clothes(tomatoAdventure, address + (i * TomatoTool.Monster.SIZE)));
					++i;
				}
			}

			return clothes;
		}
	}
}
