using System;
using System.Drawing;
using System.Windows.Forms;

namespace TomatoTool
{
	public partial class FormMapEditor : Form
	{

		private void pictureBoxBattleBackGround_Paint(object sender, PaintEventArgs e)
		{
			using(Bitmap bitmap = map.BattleBackGround.toBitmap())
			{
				e.Graphics.DrawImage(bitmap, 0, 0);
			}
		}

		private void listBoxBattleBackGround_SelectedIndexChanged(object sender, EventArgs e)
		{
			pictureBoxBattleBackGround.Refresh();
		}
	}
}
