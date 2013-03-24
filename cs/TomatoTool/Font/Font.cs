
namespace TomatoTool
{
	public abstract class Font : ROMObject
	{
		protected byte[] whiteLayer;
		protected byte[] paleBlueLayer;

		public static readonly int BLUE = 0;
		public static readonly int WHITE = 1;
		public static readonly int PALEBLUE = 2;
	}
}
