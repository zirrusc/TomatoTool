using System;
using System.Drawing;
using System.Windows.Forms;


namespace TomatoTool
{
	public partial class FormMapEditor : Form
	{
		private ushort selectChipSetRegister;

		#region コピーリスト

		private void pictureBoxChipSetRegisterCopyList_Paint(object sender, PaintEventArgs e)
		{
			int bg = radioButtonChipSetRegisterBG();

			ChipSetList chipSetList = map.ChipSetList[bg];

			if (chipSetList != null)
			{
				//リサイズ
				((PictureBox)sender).Width = ChipSet.WIDTH * 16;
				((PictureBox)sender).Height = ChipSet.HEIGHT * ((chipSetList.Count / 16) + (((chipSetList.Count % 16) == 0) ? 0 : 1));

				e.Graphics.FillRectangle(new SolidBrush(map.Palette[0][0].toColor()), 0, 0, ((PictureBox)sender).Width, ((PictureBox)sender).Height - ChipSet.HEIGHT);
				e.Graphics.FillRectangle(new SolidBrush(map.Palette[0][0].toColor()), 0, ((PictureBox)sender).Height - ChipSet.HEIGHT, (chipSetList.Count % 16 == 0 ? 16 : chipSetList.Count % 16) * ChipSet.WIDTH, ((PictureBox)sender).Height);

				for (int i = 0; i < chipSetList.Count; ++i)
				{
					using (Bitmap bitmap = map.MapTile.MainTile.toBitmap(chipSetList[i], map.Palette, true))
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
				using (Pen pen = new Pen(TomatoTool.Map.SelectColor, 2))
				{
					e.Graphics.DrawRectangle(pen, ((selectChipSetRegister % 16) * ChipSet.WIDTH) + 1, ((selectChipSetRegister / 16) * ChipSet.HEIGHT) + 1, ChipSet.WIDTH - pen.Width, ChipSet.HEIGHT - pen.Width);
				}
			}
		}
		private void pictureBoxChipSetRegisterCopyList_MouseMove(object sender, MouseEventArgs e)
		{
			int bg = radioButtonChipSetRegisterBG();

			ChipSetList chipSetList = map.ChipSetList[bg];
			ChipSetRegister chipSetRegister = map.ChipSetRegister[bg];

			if ((e.Button == MouseButtons.Left) &&
			((0 <= e.X) && (e.X < ((PictureBox)sender).Width)) &&
			((0 <= e.Y) && (e.Y < ((PictureBox)sender).Height)) &&
			((e.X / TomatoTool.ChipSet.WIDTH) + ((e.Y / TomatoTool.ChipSet.HEIGHT) * 16)) < chipSetList.Count)
			{
				selectChipSetRegister = (ushort)((e.X / TomatoTool.ChipSet.WIDTH) + ((e.Y / TomatoTool.ChipSet.HEIGHT) * 16));
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

			

			if (map.ChipSetRegister[bg] != null)
			{
				ChipSetList chipSetList = map.ChipSetList[bg];
				ChipSetRegister chipSetRegister = map.ChipSetRegister[bg];

				((PictureBox)sender).Width = chipSetRegister.ChipSet.GetLength(0) * TomatoTool.ChipSet.WIDTH;
				((PictureBox)sender).Height = chipSetRegister.ChipSet.GetLength(1) * TomatoTool.ChipSet.HEIGHT;

				//塗りつぶし透過させないため
				e.Graphics.FillRectangle(new SolidBrush(map.Palette[0][0].toColor()), 0, 0, ((PictureBox)sender).Width, ((PictureBox)sender).Height);

				for (int y = 0; y < chipSetRegister.ChipSet.GetLength(1); ++y)
				{
					for (int x = 0; x < chipSetRegister.ChipSet.GetLength(0); ++x)
					{
						try
						{
							e.Graphics.DrawImage(map.MapTile.MainTile.toBitmap(chipSetList[chipSetRegister.ChipSet[x, y]], map.Palette, true), x * TomatoTool.ChipSet.WIDTH, y * TomatoTool.ChipSet.HEIGHT);
						}
						catch
						{
							chipSetRegister.ChipSet[x, y] = (ushort)0x0000;
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

			ChipSetList chipSetList = map.ChipSetList[bg];
			ChipSetRegister chipSetRegister = map.ChipSetRegister[bg];

			if (((0 <= e.X) && (e.X < ((PictureBox)sender).Width)) &&
				((0 <= e.Y) && (e.Y < ((PictureBox)sender).Height)) &&
				(e.X / TomatoTool.ChipSet.WIDTH) < chipSetRegister.ChipSet.GetLength(0) &&
				(e.Y / TomatoTool.ChipSet.HEIGHT) < chipSetRegister.ChipSet.GetLength(1))
			{
				switch (e.Button)
				{
					case MouseButtons.Left:

						chipSetRegister.ChipSet[e.X / TomatoTool.ChipSet.WIDTH, e.Y / TomatoTool.ChipSet.HEIGHT] = selectChipSetRegister;
						chipSetRegister.Saved = false;
						map.updata();

						pictureBoxChipSetRegister.Refresh();
						break;

					case MouseButtons.Right:

						selectChipSetRegister = (ushort)chipSetRegister.ChipSet[e.X / TomatoTool.ChipSet.WIDTH, e.Y / TomatoTool.ChipSet.HEIGHT];
						pictureBoxChipSetRegisterCopyList.Refresh();

						break;

					default:
						break;
				}
			}
		}
	}
}
