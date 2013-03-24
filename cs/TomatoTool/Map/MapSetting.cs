using System;

namespace TomatoTool
{
	public class MapSetting
	{
		//マップナンバー
		private ushort number;
		public ushort Number
		{
			get
			{
				return number;
			}

			set
			{
				number = value;
			}
		}

		//セーブのうんたん
		private byte saveType;
		public byte SaveType
		{
			get
			{
				return saveType;
			}

			set
			{
				saveType = value;
			}
		}

		//橫の大きさ
		private byte width;
		public byte Width
		{
			get
			{
				return width;
			}
		}

		//縦の大きさ
		private byte height;
		public byte Height
		{
			get
			{
				return height;
			}
		}

		//入った時のBGM
		private byte bgmNumber;
		public byte BGMNumber
		{
			get
			{
				return bgmNumber;
			}

			set
			{
				bgmNumber = value;
			}
		}

		//戦闘する時の背景画像の番号
		private byte battleBackGroundNumber;
		public byte BattleBackGroundNumber
		{
			get
			{
				return battleBackGroundNumber;
			}

			set
			{
				battleBackGroundNumber = value;
			}
		}
		
		private byte animationTileNumber;
		public byte AnimationTileNumber
		{
			get
			{
				return animationTileNumber;
			}

			set
			{
				animationTileNumber = value;
			}
		}

		private byte transitionPaletteNumber;
		public byte TransitionPaletteNumber
		{
			get
			{
				return transitionPaletteNumber;
			}

			set
			{
				transitionPaletteNumber = value;
			}
		}

		//部屋の横の最小サイズ
		private static readonly uint MINIMUM_WIDTH = 15;
		//部屋の縦の最小サイズ
		private static readonly uint MINIMUM_HEIGHT = 10;


		public MapSetting(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public void load(TomatoAdventure tomatoAdventure, uint address)
		{
			number = (ushort)tomatoAdventure.readLittleEndian(address, 2);
			saveType = tomatoAdventure.read(address + 2);
			width = tomatoAdventure.read(address + 3);
			height = tomatoAdventure.read(address + 4);
			bgmNumber = tomatoAdventure.read(address + 5);
			battleBackGroundNumber = tomatoAdventure.read(address + 6);
			animationTileNumber = tomatoAdventure.read(address + 7);
			transitionPaletteNumber = tomatoAdventure.read(address + 8);
		}

		public void save(TomatoAdventure tomatoAdventure, uint address)
		{
			tomatoAdventure.writeLittleEndian(address, 2, number);
			tomatoAdventure.write(address + 2, saveType);
			tomatoAdventure.write(address + 3, width);
			tomatoAdventure.write(address + 4, height);
			tomatoAdventure.write(address + 5, bgmNumber);
			tomatoAdventure.write(address + 6, battleBackGroundNumber);
			tomatoAdventure.write(address + 7, animationTileNumber);
			tomatoAdventure.write(address + 8, transitionPaletteNumber);

			//以下不明値
			tomatoAdventure.write(address + 9, 0x00);
			tomatoAdventure.write(address + 10, 0x00);
			tomatoAdventure.write(address + 11, 0x00);
		}

		public void resize(byte width, byte height)
		{
			if ((width == MINIMUM_WIDTH) && (height >= MINIMUM_HEIGHT))
			{
				this.width = width;
				this.height = height;
			}
			else if ((width >= MINIMUM_WIDTH) && (height == MINIMUM_HEIGHT))
			{
				this.width = width;
				this.height = height;
			}
			else
			{
				throw new Exception();
			}
		}
	}
}
