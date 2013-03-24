using System;
using System.Drawing;
using System.Windows.Forms;
using TomatoTool.Map;

namespace TomatoTool
{
	public partial class FormMapEditor : Form
	{
		private ushort selectChipSetRegister;

		#region コピーリスト

		private void pictureBoxChipSetRegisterCopyList_Paint(object sender, PaintEventArgs e)
		{
			int bg = radioButtonChipSetRegisterBG();

			MapGraphic mapGraphic = map.MapGraphic;
			ChipSetList chipSetList = map.MapGraphic.ChipSetList[bg];

			if (chipSetList != null)
			{
				//リサイズ
				((PictureBox)sender).Width = ChipSet.WIDTH * 16;
				((PictureBox)sender).Height = ChipSet.HEIGHT * ((chipSetList.Count / 16) + (((chipSetList.Count % 16) == 0) ? 0 : 1));

				e.Graphics.FillRectangle(new SolidBrush(mapGraphic.Palette[0][0].toColor()), 0, 0, ((PictureBox)sender).Width, ((PictureBox)sender).Height - ChipSet.HEIGHT);
				e.Graphics.FillRectangle(new SolidBrush(mapGraphic.Palette[0][0].toColor()), 0, ((PictureBox)sender).Height - ChipSet.HEIGHT, (chipSetList.Count % 16 == 0 ? 16 : chipSetList.Count % 16) * ChipSet.WIDTH, ((PictureBox)sender).Height);

				for (int i = 0; i < chipSetList.Count; ++i)
				{
					using (Bitmap bitmap = mapGraphic.MapTile.MainTile.toBitmap(chipSetList[i], mapGraphic.Palette, true))
					{
						e.Graphics.DrawImage(bitmap, (i % 16) * ChipSet.WIDTH, (i / 16) * ChipSet.HEIGHT, ChipSet.WIDTH, ChipSet.HEIGHT);
					}
				}

				//グリッド描写
				if (toolStripMenuItemViewMapGrid.Checked)
				{
					gridDraw(sender, e.Graphics);
				}

				//枠描写
				using (Pen pen = new Pen(TomatoTool.Map.Map.SelectColor, 2))
				{
					e.Graphics.DrawRectangle(pen, ((selectChipSetRegister % 16) * ChipSet.WIDTH) + 1, ((selectChipSetRegister / 16) * ChipSet.HEIGHT) + 1, ChipSet.WIDTH - pen.Width, ChipSet.HEIGHT - pen.Width);
				}
			}
		}
		private void pictureBoxChipSetRegisterCopyList_MouseMove(object sender, MouseEventArgs e)
		{
			int bg = radioButtonChipSetRegisterBG();

			MapGraphic mapGraphic = map.MapGraphic;
			ChipSetList chipSetList = mapGraphic.ChipSetList[bg];
			ChipSetRegister chipSetRegister = mapGraphic.ChipSetRegister[bg];

			if ((e.Button == MouseButtons.Left) &&
			((0 <= e.X) && (e.X < ((PictureBox)sender).Width)) &&
			((0 <= e.Y) && (e.Y < ((PictureBox)sender).Height)) &&
			(chipSetList.Count >= (e.X / 16) + ((e.Y / 16) * 16)))
			{
				selectChipSetRegister = (ushort)((e.X / 16) + ((e.Y / 16) * 16));
			}

			pictureBoxChipSetRegisterCopyList.Refresh();
		}

		#endregion

		private void radioButtonChipSetRegisterBG_CheckedChanged(object sender, EventArgs e)
		{
			selectChipSetRegister = 0;
			pictureBoxChipSetRegister.Refresh();
			pictureBoxChipSetRegisterCopyList.Refresh();
		}
		public int radioButtonChipSetRegisterBG()
		{
			if (radioButtonChipSetRegisterBG1.Checked)
			{
				return 0;
			}

			if (radioButtonChipSetRegisterBG2.Checked)
			{
				return 1;
			}

			if (radioButtonChipSetRegisterBG3.Checked)
			{
				return 2;
			}

			return -1;
		}

		private void pictureBoxChipSetRegister_Paint(object sender, PaintEventArgs e)
		{
			int bg = radioButtonChipSetRegisterBG();

			MapGraphic mapGraphic = map.MapGraphic;

			if (mapGraphic.ChipSetRegister[bg] != null)
			{
				ChipSetList chipSetList = mapGraphic.ChipSetList[bg];
				ChipSetRegister chipSetRegister = mapGraphic.ChipSetRegister[bg];

				((PictureBox)sender).Width = chipSetRegister.ChipSet.GetLength(0) * 16;
				((PictureBox)sender).Height = chipSetRegister.ChipSet.GetLength(1) * 16;

				//塗りつぶし透過させないため
				e.Graphics.FillRectangle(new SolidBrush(mapGraphic.Palette[0][0].toColor()), 0, 0, ((PictureBox)sender).Width, ((PictureBox)sender).Height);

				for (int y = 0; y < chipSetRegister.ChipSet.GetLength(1); ++y)
				{
					for (int x = 0; x < chipSetRegister.ChipSet.GetLength(0); ++x)
					{
						try
						{
							e.Graphics.DrawImage(mapGraphic.MapTile.MainTile.toBitmap(chipSetList[chipSetRegister.ChipSet[x, y]], mapGraphic.Palette, true), x * 16, y * 16);
							//mapGraphic.MapTile.MainTile.draw(e.Graphics, x * TomatoTool.ChipSet.WIDTH, y * TomatoTool.ChipSet.HEIGHT, TomatoTool.ChipSet.WIDTH, TomatoTool.ChipSet.HEIGHT, chipSetRegisterList[chipSetList.ChipSet[x, y]], mapGraphic.Palette, true);
						}
						catch
						{
						}
					}
				}

				//マップエリアの描写
				if (toolStripMenuItemViewMapArea.Checked)
				{
					map.MapArea.draw(e.Graphics, true);
				}

				//グリッド描写
				if (toolStripMenuItemViewMapGrid.Checked)
				{
					gridDraw(sender, e.Graphics);
				}

				//ワープスクリプト描写
				if (toolStripMenuItemViewWarpScript.Checked)
				{
					map.WarpScriptList.draw(e.Graphics);
				}

				//マップスクリプト描写
				if (toolStripMenuItemViewMapScript.Checked)
				{
					map.MapScriptList.draw(e.Graphics);
				}

				//キャラクタースクリプト描写
				if (toolStripMenuItemViewCharacterScript.Checked)
				{
					map.CharacterScriptList.draw(e.Graphics);
				}
			}
		}
		private void pictureBoxChipSetRegister_MouseMove(object sender, MouseEventArgs e)
		{
			int bg = radioButtonChipSetRegisterBG();
			MapGraphic mapGraphic = map.MapGraphic;
			ChipSetList chipSetList = mapGraphic.ChipSetList[bg];
			ChipSetRegister chipSetRegister = mapGraphic.ChipSetRegister[bg];

			if (((0 <= e.X) && (e.X < ((PictureBox)sender).Width)) &&
				((0 <= e.Y) && (e.Y < ((PictureBox)sender).Height)) &&
				(e.X / 16) <= chipSetRegister.ChipSet.GetLength(0) &&
				(e.Y / 16) <= chipSetRegister.ChipSet.GetLength(1))
			{
				switch (e.Button)
				{
					case MouseButtons.Left:

						chipSetRegister.ChipSet[e.X / 16, e.Y / 16] = selectChipSetRegister;
						map.MapGraphic.updata();

						pictureBoxChipSetRegister.Refresh();
						break;

					case MouseButtons.Right:

						selectChipSetRegister = (ushort)chipSetRegister.ChipSet[e.X / 16, e.Y / 16];
						pictureBoxChipSetRegisterCopyList.Refresh();

						break;

					default:
						break;
				}
			}
		}
	}
}
