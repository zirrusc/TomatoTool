﻿using System.Collections;
using System.Drawing;

namespace TomatoTool
{
	public class CharacterImage : ROMObject
	{
		public static readonly CharacterImage NULL;

		private ActionList actionList;
		public ActionList ActionList
		{
			get
			{
				return actionList;
			}
		}

		private OAMSetList oamSetList;
		public OAMSetList OAMSetList
		{
			get
			{
				return oamSetList;
			}
		}

		private Tile4BitList tileList;
		public Tile4BitList TileList
		{
			get
			{
				return tileList;
			}

			set
			{
				tileList = value;
			}
		}

		private byte width;
		public byte Width
		{
			get
			{
				return width;
			}

			set
			{
				width = value;
			}
		}

		private byte height;
		public byte Height
		{
			get
			{
				return height;
			}

			set
			{
				height = value;
			}
		}

		public static readonly uint SIZE = 16;

		static CharacterImage()
		{
			CharacterImage.NULL = new CharacterImage();
		}

		public CharacterImage()
		{
			initialize();
		}
		public CharacterImage(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void initialize()
		{
			this.address = ROM.ADDRESS_NULL;

			width = 0;
			height = 0;
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			height = tomatoAdventure.read(address);
			width = tomatoAdventure.read(address + 1);

			tileList = new Tile4BitList(tomatoAdventure, tomatoAdventure.readAsAddress(address + 4), tomatoAdventure.read(address + 3));
			actionList = new ActionList(tomatoAdventure, tomatoAdventure.readAsAddress(address + 8));
			oamSetList = new OAMSetList(tomatoAdventure, tomatoAdventure.readAsAddress(address + 12));
		}

		public void save(TomatoAdventure tomatoAdventure)
		{
			if (!Saved)
			{
				save(tomatoAdventure, tomatoAdventure.malloc(SIZE));
				saved = true;
			}
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			tomatoAdventure.write(address, height);
			tomatoAdventure.write(address + 1, width);
			tomatoAdventure.write(address + 2, 0x00);
			tomatoAdventure.write(address + 3, (byte)tileList.Count);

			tileList.save(tomatoAdventure, tileList.Address);
			tomatoAdventure.writeAsAddress(address + 4, tileList.Address);

			actionList.save(tomatoAdventure);
			tomatoAdventure.writeAsAddress(address + 8, actionList.Address);

			oamSetList.save(tomatoAdventure);
			tomatoAdventure.writeAsAddress(address + 12, oamSetList.Address);
		}

		public override uint getSize()
		{
			return SIZE;
		}

		public Bitmap toBitmap(CharacterScriptDirection characterScriptDirection, int frame, Palette palette, bool isTransparent)
		{
			if (characterScriptDirection != CharacterScriptDirection.None)
			{
				return oamSetList.OAMSet[(ushort)(((IList)actionList[characterScriptDirection])[frame])].toBitmap(tileList, palette, isTransparent);
			}
			else
			{
				return new Bitmap(0x0200, 0x0100);
			}
		}
	}
}
