using System.Collections.Generic;

namespace TomatoTool
{
	public class BattleBackGroundList
	{
		public BattleBackGround this[int i]
		{
			get
			{
				return battleBackGround[i];
			}

			set
			{
				battleBackGround[i] = value;
			}
		}
		public int Count
		{
			get
			{
				return battleBackGround.Count;
			}
		}

		private List<BattleBackGround> battleBackGround;
		public List<BattleBackGround> BattleBackGround
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

		public BattleBackGroundList(TomatoAdventure tomatoAdventure)
		{
			load(tomatoAdventure);
		}

		public void load(TomatoAdventure tomatoAdventure)
		{

		}
	}
}
