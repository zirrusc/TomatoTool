using System.Collections.Generic;
using System.Drawing;

namespace TomatoTool
{
	public class Map : ROMObject
	{
		public static List<uint> TopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x0002C2A8),
			ROM.addBase(0x00033CFC),
			ROM.addBase(0x0003F8F4),
			ROM.addBase(0x0004A028),
			ROM.addBase(0x0004A070),
			ROM.addBase(0x0004A0B0),
			ROM.addBase(0x0006C4F4)
		};
		public static List<uint> MapTileTopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x00034D70),
		};
		public static List<uint> PaletteTopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x00031EF0),
			ROM.addBase(0x00033D80),
			ROM.addBase(0x0003F80C),
			ROM.addBase(0x0003F90C),
		};
		public static List<uint> ChipSetListTopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x00033EA4),
		};
		public static List<uint> ChipSetRegisterTopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x00033E9C),
		};
		public static List<uint> AnimationTileTopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x00034080),
			ROM.addBase(0x00034E1C),
		};
		public static List<uint> TileTopAddressPointer = new List<uint>()
		{
			ROM.addBase(0x00034D70),
		};

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
				saved = false;
			}
		}

		//セーブタイプ
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
				saved = false;
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

			set
			{
				width = value;
				saved = false;
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

			set
			{
				height = value;
				saved = false;
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
				saved = false;
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
				saved = false;
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
				saved = false;
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
				saved = false;
			}
		}

		private MapArea mapArea;
		public MapArea MapArea
		{
			get
			{
				return mapArea;
			}

			set
			{
				mapArea = value;
			}
		}

		private WarpScriptList warpScriptList;
		public WarpScriptList WarpScriptList
		{
			get
			{
				return warpScriptList;
			}

			set
			{
				warpScriptList = value;
			}
		}

		private MapScriptList mapScriptList;
		public MapScriptList MapScriptList
		{
			get
			{
				return mapScriptList;
			}

			set
			{
				mapScriptList = value;
			}
		}

		private CharacterScriptList characterScriptList;
		public CharacterScriptList CharacterScriptList
		{
			get
			{
				return characterScriptList;
			}

			set
			{
				characterScriptList = value;
			}
		}

		private MapTile mapTile;
		public MapTile MapTile
		{
			get
			{
				return mapTile;
			}

			set
			{
				mapTile = value;
			}
		}

		private ChipSetRegister[] chipSetRegister;
		public ChipSetRegister[] ChipSetRegister
		{
			get
			{
				return chipSetRegister;
			}

			set
			{
				chipSetRegister = value;
			}
		}

		private ChipSetList[] chipSetList;
		public ChipSetList[] ChipSetList
		{
			get
			{
				return chipSetList;
			}

			set
			{
				chipSetList = value;
			}
		}

		private Palette[] palette;
		public Palette[] Palette
		{
			get
			{
				return palette;
			}

			set
			{
				palette = value;
			}
		}

		private TransitionPalette transitionPalette;
		public TransitionPalette TransitionPalette
		{
			get
			{
				return transitionPalette;
			}

			set
			{
				transitionPalette = value;
			}
		}

		private OverrideTile overrideTile;
		public OverrideTile OverrideTile
		{
			get
			{
				return overrideTile;
			}

			set
			{
				overrideTile = value;
			}
		}

		private BattleBackGround battleBackGround;
		public BattleBackGround BattleBackGround
		{
			get
			{
				return battleBackGround;
			}

			set
			{
				battleBackGround = value;
			}
		}

		private LZ77 tile;
		public LZ77 Tile
		{
			get
			{
				return tile;
			}

			set
			{
				tile = value;
			}
		}

		private AnimationTile animationTile;
		public AnimationTile AnimationTile
		{
			get
			{
				return animationTile;
			}

			set
			{
				animationTile = value;
			}
		}

		public static readonly uint PALETTE_LENGTH_0 = 15;
		public static readonly uint CHIP_SET_REGISTER_LENGTH_0 = 3;
		public static readonly uint CHIP_SET_LIST_LENGTH_0 = 3;

		//部屋の横の最小サイズ
		private static readonly uint MINIMUM_WIDTH = 15;
		//部屋の縦の最小サイズ
		private static readonly uint MINIMUM_HEIGHT = 10;

		public static readonly uint BLOCK_WIDTH = 16;
		public static readonly uint BLOCK_HEIGHT = 16;

		public static readonly uint SIZE = 28;

		//1/60秒
		public static ushort Time = 0;
		public static Color SelectColor = Color.FromArgb(255, 0, 0);

		//不明アドレス
		public uint unownAddress;

		public Map(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override bool Saved
		{
			get
			{
				return
					saved &&
					mapArea.Saved && 
					warpScriptList.Saved && 
					mapScriptList.Saved && 
					characterScriptList.Saved && 
					transitionPalette.Saved && 
					battleBackGround.Saved;
			}
		}

		public override void initialize()
		{
			throw new System.NotImplementedException();
		}
		public void load(TomatoAdventure tomatoAdventure)
		{
			load(tomatoAdventure, address);
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			number = (ushort)tomatoAdventure.readLittleEndian(address, 2);
			saveType = tomatoAdventure.read(address + 2);
			width = tomatoAdventure.read(address + 3);
			height = tomatoAdventure.read(address + 4);
			bgmNumber = tomatoAdventure.read(address + 5);
			battleBackGroundNumber = tomatoAdventure.read(address + 6);
			animationTileNumber = tomatoAdventure.read(address + 7);
			transitionPaletteNumber = tomatoAdventure.read(address + 8);

			mapArea = new MapArea(tomatoAdventure, this);

			warpScriptList = (WarpScriptList)tomatoAdventure.add(new WarpScriptList(tomatoAdventure, tomatoAdventure.readAsAddress(address + 12)));
			
			mapScriptList = (MapScriptList)tomatoAdventure.add(new MapScriptList(tomatoAdventure, tomatoAdventure.readAsAddress(address + 16)));

			characterScriptList = (CharacterScriptList)tomatoAdventure.add(new CharacterScriptList(tomatoAdventure, tomatoAdventure.readAsAddress(address + 20)));

			overrideTile = new OverrideTile(tomatoAdventure, tomatoAdventure.readAsAddress(address + 24));

			palette = new Palette[PALETTE_LENGTH_0];
			for (uint i = 0; i < palette.GetLength(0); ++i)
			{
				palette[i] = (Palette)tomatoAdventure.add(new Palette(tomatoAdventure, tomatoAdventure.readAsAddress(tomatoAdventure.readAsAddress(PaletteTopAddressPointer[0]) + (number * ROM.ADDRESS_SIZE)) + (i * TomatoTool.Palette.SIZE)));
			}

			chipSetRegister = new ChipSetRegister[CHIP_SET_REGISTER_LENGTH_0];
			for (uint i = 0; i < chipSetRegister.GetLength(0); ++i)
			{
				if (tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(tomatoAdventure.readAsAddress(ChipSetRegisterTopAddressPointer[0]) + (number * (ROM.ADDRESS_SIZE * CHIP_SET_REGISTER_LENGTH_0)) + (i * ROM.ADDRESS_SIZE))))
				{
					chipSetRegister[i] = (ChipSetRegister)tomatoAdventure.add(new ChipSetRegister(tomatoAdventure, tomatoAdventure.readAsAddress(tomatoAdventure.readAsAddress(ChipSetRegisterTopAddressPointer[0]) + (number * (ROM.ADDRESS_SIZE * CHIP_SET_REGISTER_LENGTH_0)) + (i * ROM.ADDRESS_SIZE)), this));
				}
			}

			chipSetList = new ChipSetList[CHIP_SET_LIST_LENGTH_0];
			for (uint i = 0; i < chipSetList.GetLength(0); ++i)
			{
				if (tomatoAdventure.isAddress(tomatoAdventure.readAsAddress(tomatoAdventure.readAsAddress(ChipSetListTopAddressPointer[0]) + (number * (ROM.ADDRESS_SIZE * CHIP_SET_LIST_LENGTH_0)) + (i * ROM.ADDRESS_SIZE))))
				{
					chipSetList[i] = (ChipSetList)tomatoAdventure.add(new ChipSetList(tomatoAdventure, tomatoAdventure.readAsAddress(tomatoAdventure.readAsAddress(ChipSetListTopAddressPointer[0]) + (number * (ROM.ADDRESS_SIZE * CHIP_SET_LIST_LENGTH_0)) + (i * ROM.ADDRESS_SIZE)), chipSetRegister[i]));
				}
			}

			mapTile = new MapTile(tomatoAdventure, this);
		}

		public void save(TomatoAdventure tomatoAdventure)
		{
			if (!Saved)
			{
				save(tomatoAdventure, tomatoAdventure.malloc(SIZE));
			}
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

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

			mapArea.save(tomatoAdventure, this);

			warpScriptList.save(tomatoAdventure);
			tomatoAdventure.writeAsAddress(address + 12, warpScriptList.Address);

			mapScriptList.save(tomatoAdventure);
			tomatoAdventure.writeAsAddress(address + 16, mapScriptList.Address);

			characterScriptList.save(tomatoAdventure);
			tomatoAdventure.writeAsAddress(address + 20, characterScriptList.Address);

			overrideTile.save(tomatoAdventure);
			tomatoAdventure.writeAsAddress(address + 24, overrideTile.Address);

			for (uint i = 0; i < palette.GetLength(0); ++i)
			{
				palette[i].save(tomatoAdventure);
			}
			tomatoAdventure.writeAsAddress(tomatoAdventure.readAsAddress(PaletteTopAddressPointer[0]) + (number * ROM.ADDRESS_SIZE), palette[0].Address);

			for (uint i = 0; i < chipSetRegister.GetLength(0); ++i)
			{
				if (chipSetRegister[i] != null)
				{
					chipSetRegister[i].save(tomatoAdventure);
					tomatoAdventure.writeAsAddress(tomatoAdventure.readAsAddress(ChipSetRegisterTopAddressPointer[0]) + (number * (ROM.ADDRESS_SIZE * CHIP_SET_REGISTER_LENGTH_0)) + (i * ROM.ADDRESS_SIZE), chipSetRegister[i].Address);
				}
				else
				{
					tomatoAdventure.writeAsAddress(tomatoAdventure.readAsAddress(ChipSetRegisterTopAddressPointer[0]) + (number * (ROM.ADDRESS_SIZE * CHIP_SET_REGISTER_LENGTH_0)) + (i * ROM.ADDRESS_SIZE), ROM.ADDRESS_NULL);
				}
			}

			for (uint i = 0; i < chipSetList.GetLength(0); ++i)
			{
				if (chipSetList[i] != null)
				{
					chipSetList[i].save(tomatoAdventure);
					tomatoAdventure.writeAsAddress(tomatoAdventure.readAsAddress(ChipSetListTopAddressPointer[0]) + (number * (ROM.ADDRESS_SIZE * CHIP_SET_LIST_LENGTH_0)) + (i * ROM.ADDRESS_SIZE), chipSetList[i].Address);
				}
				else
				{
					tomatoAdventure.writeAsAddress(tomatoAdventure.readAsAddress(ChipSetListTopAddressPointer[0]) + (number * (ROM.ADDRESS_SIZE * CHIP_SET_LIST_LENGTH_0)) + (i * ROM.ADDRESS_SIZE), ROM.ADDRESS_NULL);
				}
			}

			mapTile.save(tomatoAdventure, this);
			
			saved = true;
		}

		public override uint getSize()
		{
			return SIZE;
		}

		public void resize(byte width, byte height)
		{
			mapArea.resize(width, height);

			for (int i = 0; i < chipSetRegister.GetLength(0); ++i)
			{
				chipSetRegister[i].resize(width, height);
			}
		}

		public void draw(Graphics graphics, bool bg1, bool bg2, bool bg3)
		{
			bool[] bg = new bool[]
			{
				bg1,
				bg2,
				bg3
			};

			//BG3だけ透過させないため
			graphics.FillRectangle(new SolidBrush(palette[0][0].toColor()), 0, 0, width * ChipSet.WIDTH, height * ChipSet.HEIGHT);

			if (updataFlag)
			{
				updataFlag = false;
				if (this.bg == null)
				{
					this.bg = new Bitmap[3];
				}

				for (int i = this.chipSetRegister.GetLength(0) - 1; i >= 0; i--)
				{

					if (bg[i] && (this.chipSetRegister[i] != null))
					{
						try
						{
							this.bg[i] = new Bitmap(this.chipSetRegister[i].ChipSet.GetLength(0) * ChipSet.WIDTH, this.chipSetRegister[i].ChipSet.GetLength(1) * ChipSet.HEIGHT);
						}
						catch (System.Exception ex)
						{
							System.Console.WriteLine(ex.ToString());
						}

						using (Graphics g = Graphics.FromImage(this.bg[i]))
						{
							ChipSetList chipSetList = this.chipSetList[i];
							ChipSetRegister chipSetRegister = this.chipSetRegister[i];

							for (int y = 0; y < chipSetRegister.ChipSet.GetLength(1); ++y)
							{
								for (int x = 0; x < chipSetRegister.ChipSet.GetLength(0); ++x)
								{
									if (chipSetRegister.ChipSet[x, y] < chipSetList.Count)
									{
										using (Bitmap b = mapTile.toBitmap(chipSetList[chipSetRegister.ChipSet[x, y]], palette, transitionPalette, true))
										{
											g.DrawImage(b, x * ChipSet.WIDTH, y * ChipSet.HEIGHT);
										}
									}
								}
							}
						}
					}
				}
			}

			for (int i = bg.GetLength(0) - 1; i >= 0; i--)
			{
				if (this.bg[i] != null)
				{
					graphics.DrawImage(this.bg[i], 0, 0);
				}
			}

		}

		private Bitmap[] bg;
		private bool updataFlag = true;
		public void updata()
		{
			updataFlag = true;
		}
	}
}
