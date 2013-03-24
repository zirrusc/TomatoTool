using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TomatoTool
{
	public class AnimationTileList : ROMObject
	{
		private List<AnimationTile> animationTile;
		public List<AnimationTile> AnimationTile
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

		public AnimationTileList()
		{
			initialize();
		}
		public AnimationTileList(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void initialize()
		{
			animationTile = new List<AnimationTile>();
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			animationTile = new List<AnimationTile>();
			while (address + getSize() != 0xFF)
			{
				animationTile.Add(new AnimationTile(tomatoAdventure, address + getSize()));
			}
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
		}
		public override uint getSize()
		{
			uint size = 0;
			for (int i = 0; i < animationTile.Count; ++i)
			{
				size += animationTile[i].getSize();
			}

			return size + 4;
		}
	}
}
