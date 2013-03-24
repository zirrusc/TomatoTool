
namespace TomatoTool
{
	public interface ISizeGettable
	{
		//そのclassをROMに書き込む時のサイズ
		//そのclassの全体のサイズを返すものではない
		uint getSize();
	}
}
