using System.Drawing;

namespace TomatoTool
{
	public abstract class Tile : ROMObject
	{
		protected byte[,] tile;

		public static readonly int WIDTH = 8;
		public static readonly int HEIGHT = 8;

		public virtual Bitmap toBitmap(Palette palette)
		{
			return null;
		}
		public virtual Bitmap toBitmap(Palette palette, bool flipX, bool flipY)
		{
			return null;
		}
	}
}