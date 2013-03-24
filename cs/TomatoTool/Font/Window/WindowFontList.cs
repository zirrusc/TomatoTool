
namespace TomatoTool
{
	public class WindowFontList : ROMObject
	{
		private WindowFont[] windowFont;
		public WindowFont[] WindowFont
		{
			get
			{
				return windowFont;
			}
		}

		private WindowChineseCharacterFont[] windowChineseCharacterFont;
		public WindowChineseCharacterFont[] WindowChineseCharacterFont
		{
			get
			{
				return windowChineseCharacterFont;
			}
		}

		public WindowFontList(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			windowFont = new WindowFont[2048];

			for (uint i = 0; i < windowFont.GetLength(0); ++i)
			{
				windowFont[i] = new WindowFont(tomatoAdventure, (uint)(address + (i * TomatoTool.WindowFont.SIZE)));
			}
		}

		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			for (uint i = 0; i < windowFont.GetLength(0); ++i)
			{
				windowFont[i].save(tomatoAdventure, (uint)(address + (i * TomatoTool.WindowFont.SIZE)));
			}
		}
	}
}
