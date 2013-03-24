
using System;

namespace TomatoTool
{
	public class MapScript : ROMObject
	{
		private MapRangeObject mapRangeObject;

		public byte BeginX
		{
			get
			{
				return mapRangeObject.BeginX;
			}

			set
			{
				mapRangeObject.BeginX = value;
				saved = false;
			}
		}

		public byte BeginY
		{
			get
			{
				return mapRangeObject.BeginY;
			}

			set
			{
				mapRangeObject.BeginY = value;
				saved = false;
			}
		}

		public byte EndX
		{
			get
			{
				return mapRangeObject.EndX;
			}

			set
			{
				mapRangeObject.EndX = value;
				saved = false;
			}
		}

		public byte EndY
		{
			get
			{
				return mapRangeObject.EndY;
			}

			set
			{
				mapRangeObject.EndY = value;
				saved = false;
			}
		}

		private bool hasTrigger;
		public bool HasTrigger
		{
			get
			{
				return hasTrigger;
			}

			set
			{
				hasTrigger = value;
				saved = false;
			}
		}

		private Script script;
		public Script Script
		{
			get
			{
				return script;
			}

			set
			{
				script = value;
				saved = false;
			}
		}

		public static readonly uint SIZE = 4 + TomatoTool.ROM.ADDRESS_SIZE;

		public MapScript()
			
		{
			initialize();
		}
		public MapScript(TomatoAdventure tomatoAdventure, uint address)
			
		{
			load(tomatoAdventure, address);
		}

		public override void initialize()
		{
			mapRangeObject = new MapRangeObject();
			hasTrigger = false;
			script = Script.NULL;
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			mapRangeObject = new MapRangeObject(tomatoAdventure.read(address), tomatoAdventure.read(address + 1), (byte)(tomatoAdventure.read(address + 2) & (~0x80)), tomatoAdventure.read(address + 3));
			hasTrigger = (tomatoAdventure.read(address + 2) & 0x80) != 0;
			script = (Script)tomatoAdventure.add(new Script(tomatoAdventure, tomatoAdventure.readAsAddress(address + 4)));
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			tomatoAdventure.write(address, mapRangeObject.BeginX);
			tomatoAdventure.write(address + 1, mapRangeObject.BeginY);
			tomatoAdventure.write(address + 2, (byte)(mapRangeObject.EndX + (Convert.ToByte(hasTrigger) << 7)));
			tomatoAdventure.write(address + 3, mapRangeObject.EndY);

			script.save(tomatoAdventure);
			tomatoAdventure.writeAsAddress(address, script.Address);

			saved = true;
		}
		public override uint getSize()
		{
			return SIZE;
		}

		public void move(byte startX, byte startY)
		{
			mapRangeObject.move(startX, startY);
		}
	}
}
