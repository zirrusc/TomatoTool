
namespace TomatoTool
{
	public class StatusStringSet : ROMObject
	{
		private StatusString statusString;
		public StatusString StatusString
		{
			get
			{
				return statusString;
			}

			set
			{
				statusString = value;
			}
		}

		private byte row;
		public byte Row
		{
			get
			{
				return row;
			}

			set
			{
				row = value;
			}
		}

		private byte column;
		public byte Column
		{
			get
			{
				return column;
			}

			set
			{
				column = value;
			}
		}

		public ushort Length
		{
			get
			{
				return (ushort)(row * column);
			}
		}

		public byte unknownValue0;
		public byte unknownValue1;

		public static readonly uint SIZE = 8;

		public StatusStringSet(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			column = tomatoAdventure.read(address + 4);

			row = tomatoAdventure.read(address + 5);

			unknownValue0 = tomatoAdventure.read(address + 6);

			unknownValue1 = tomatoAdventure.read(address + 7);

			statusString = new StatusString(tomatoAdventure, tomatoAdventure.readAsAddress(address), Length);
		}

		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			tomatoAdventure.writeAsAddress(address, statusString.Address);
			statusString.save(tomatoAdventure, address, Length);

			tomatoAdventure.write(address + 4, column);

			tomatoAdventure.write(address + 5, row);

			tomatoAdventure.write(address + 6, unknownValue0);

			tomatoAdventure.write(address + 7, unknownValue1);
		}

		public override uint getSize()
		{
			return SIZE;
		}
	}
}
