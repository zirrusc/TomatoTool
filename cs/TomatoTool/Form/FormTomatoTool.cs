using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TomatoTool
{
	public partial class FormTomatoTool : Form
	{
		private TextBox[] textBoxGimmickLv;
		private ToolStripMenuItem[] toolStripMenuItemMapChipBG;

		public static readonly string applicationName = "TomatoTool" + " " + Application.ProductVersion;
		private string filePath;

		//ファイル変更の有無
		private bool modified = false;

		private TomatoAdventure tomatoAdventure;

		#region Form

		public FormTomatoTool()
		{
			InitializeComponent();
		}

		private void FormTomatoTool_Shown(object sender, EventArgs e)
		{
			Text = applicationName;

			#region スプラッシュスクリーン

			// スプラッシュスクリーン表示
			SplashScreen splashScreen = new SplashScreen();
			splashScreen.Show();
			splashScreen.Refresh();

			Bitmap errorBitmap = new Bitmap(16, 16);

			using (Graphics graphics = Graphics.FromImage(errorBitmap))
			{
				graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 255)), 0, 0, 16, 16);
			}

			Bitmap[] bitmap = new Bitmap[256];
			for (uint i = 0; i < bitmap.GetLength(0); ++i)
			{
				bitmap[i] = (Bitmap)TomatoTool.Properties.Resources.ResourceManager.GetObject("MapArea" + String.Format("{0:X2}", i), TomatoTool.Properties.Resources.Culture);
				if (bitmap[i] == null)
				{
					bitmap[i] = errorBitmap;
				}
			}

			MapArea.image = Array.AsReadOnly<Bitmap>(bitmap);

			// スプラッシュスクリーン表示終了
			splashScreen.Close();
			
			// メインフォームをアクティブに
			this.Activate();

			textBoxGimmickLv = new TextBox[9];
			textBoxGimmickLv[0] = textBoxGimmickLv1;
			textBoxGimmickLv[1] = textBoxGimmickLv2;
			textBoxGimmickLv[2] = textBoxGimmickLv3;
			textBoxGimmickLv[3] = textBoxGimmickLv4;
			textBoxGimmickLv[4] = textBoxGimmickLv5;
			textBoxGimmickLv[5] = textBoxGimmickLv6;
			textBoxGimmickLv[6] = textBoxGimmickLv7;
			textBoxGimmickLv[7] = textBoxGimmickLv8;
			textBoxGimmickLv[8] = textBoxGimmickLv9;

			toolStripMenuItemMapChipBG = new ToolStripMenuItem[3];
			toolStripMenuItemMapChipBG[0] = toolStripMenuItemMapChipBG1;
			toolStripMenuItemMapChipBG[1] = toolStripMenuItemMapChipBG2;
			toolStripMenuItemMapChipBG[2] = toolStripMenuItemMapChipBG3;

			#endregion

			openFileDialog.ShowDialog();

			//ファイルを開かなかった場合は起動させないようにする
			if (tomatoAdventure == null)
			{
				Close();
			}
		}

		private void FormTomatoTool_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (modified)
			{
				switch (MessageBox.Show(Path.GetFileName(filePath) + "への変更内容を保存しますか?", applicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
				{
					case DialogResult.Yes:
						toolStripMenuItemSave_Click(sender, e);

						break;

					case DialogResult.No:

						break;

					case DialogResult.Cancel:
						e.Cancel = true;

						break;
				}
			}

		}

		private void FormTomatoTool_FormClosed(object sender, FormClosedEventArgs e)
		{
			pictureBoxMapDispose();

			for (uint i = 0; i < MapArea.image.Count; ++i)
			{
				//MapArea.image[(int)i].Dispose();
			}
		}

		/*private void setText(string state)
		{
			Text = Path.GetFileName(filePath) + " - " + applicationName + state;
		}*/

		#endregion

		#region File

		private void openFileDialog_FileOk(object sender, CancelEventArgs e)
		{
			try
			{
				filePath = openFileDialog.FileName;
				//setText("");

				tomatoAdventure = null;
				tomatoAdventure = new TomatoAdventure(File.ReadAllBytes(filePath));
				tomatoAdventure.test();
				tomatoAdventure.objectListSort();
				loadEtc();

				listBoxMap.DataSource = tomatoAdventure.MapList.Map;
				listBoxGimmick.DataSource = tomatoAdventure.Gimmick;
				listBoxMonster.DataSource = tomatoAdventure.Monster;

				foreach (KeyValuePair<Type, List<ROMObject>> listROMObject in tomatoAdventure.ObjectList)
				{
					TreeNode treeNode = treeViewObjectList.Nodes.Add(((Type)listROMObject.Key).Name);

					for (int i = 0; i < listROMObject.Value.Count; ++i)
					{
						treeNode.Nodes.Add(String.Format("{0:X8}", listROMObject.Value[i].Address));
					}
				}

				pictureBoxMapRefresh();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), applicationName + " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
		{
			try
			{
				//マップの保存
				saveMap();

				tomatoAdventure.save();


				filePath = saveFileDialog.FileName;
				File.WriteAllBytes(filePath, tomatoAdventure.get());
				//setText("");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), applicationName + " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
		{
			if (modified)
			{
				switch (MessageBox.Show(Path.GetFileName(filePath) + "への変更内容を保存しますか?", applicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
				{
					case DialogResult.Yes:

						toolStripMenuItemSave_Click(sender, e);
						openFileDialog.ShowDialog();

						break;

					case DialogResult.No:

						openFileDialog.ShowDialog();

						break;

					case DialogResult.Cancel:

						break;
				}
			}
			else
			{
				openFileDialog.ShowDialog();
			}
		}
		private void toolStripMenuItemSave_Click(object sender, EventArgs e)
		{
			try
			{
				saveMap();

				tomatoAdventure.save();


				File.WriteAllBytes(filePath, tomatoAdventure.get());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				toolStripMenuItemSaveAs_Click(sender, e);
			}
		}
		private void toolStripMenuItemSaveAs_Click(object sender, EventArgs e)
		{
			saveFileDialog.ShowDialog();
		}
		private void toolStripMenuItemExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion

		#region ギミック

		private void listBoxGimmick_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxGimmick.SelectedItem != null)
			{
				Gimmick gimmick = listBoxGimmick.SelectedItem as Gimmick;
				pictureBoxGimmick.Refresh();
				controlGimmickLoad(gimmick);
			}
		}
		private void listBoxGimmick_Format(object sender, ListControlConvertEventArgs e)
		{
			e.Value = String.Format("{0:X4}", (e.ListItem as Gimmick).Name.Text);
		}

		private void pictureBoxGimmick_Paint(object sender, PaintEventArgs e)
		{
			if (listBoxGimmick.SelectedItem != null)
			{
				Gimmick gimmick = listBoxGimmick.SelectedItem as Gimmick;

				(sender as PictureBox).Width = 208;
				(sender as PictureBox).Height = 48;

				for (int y = 0; y < 4; ++y)
				{
					for (int x = 0; x < 4; ++x)
					{
						Bitmap b = gimmick.Icon[(y * 4) + x].toBitmap(gimmick.Palette);
						e.Graphics.DrawImage(b, x * TomatoTool.Tile4Bit.WIDTH, y * TomatoTool.Tile4Bit.HEIGHT);
						//b.Dispose();
					}
				}

				e.Graphics.DrawImage(tomatoAdventure.StatusCharacterFontList.toBitmap(gimmick.Name), 0, 32);
				e.Graphics.DrawImage(tomatoAdventure.StatusCharacterFontList.toBitmap(gimmick.DescriptionBattle), 0, 40);
			}
		}

		private void controlGimmickLoad(Gimmick gimmick)
		{
			if (gimmick != null)
			{
				textBoxGimmickName.Text = gimmick.Name.Text;
				textBoxGimmickUses.Text = gimmick.Uses.ToString();
				textBoxGimmickAttack.Text = gimmick.Attack.ToString();
				textBoxGimmickBattery.Text = gimmick.Battery.ToString();
				textBoxGimmickDescriptionBattle.Text = gimmick.DescriptionBattle.Text;

				for (uint i = 0; i < textBoxGimmickLv.GetLength(0); ++i)
				{
					textBoxGimmickLv[i].Text = gimmick.Level[i].ToString();
				}

				//setText(" - ギミック - " + gimmick.Name.Text);
			}
		}

		#endregion

		#region マップ

		private void listBoxMap_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxMap.SelectedItem != null)
			{
				controlMapLoad(listBoxMap.SelectedItem as Map);
			}
		}
		private void listBoxMap_Format(object sender, ListControlConvertEventArgs e)
		{
			e.Value = String.Format("{0:X4}", (e.ListItem as Map).Number);
		}
		private void listBoxMap_Add(object sender, EventArgs e)
		{
		}
		private void listBoxMap_Delete(object sender, EventArgs e)
		{
			if (listBoxMap.SelectedItem != null)
			{
				switch (MessageBox.Show("削除しますか", applicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
				{
					case DialogResult.Yes:
						try
						{
							int SelectedIndex = listBoxMap.SelectedIndex;
							listBoxMap.DataSource = null;
							tomatoAdventure.MapList.Map.RemoveAt(SelectedIndex);
							listBoxMap.DataSource = tomatoAdventure.MapList.Map;
							listBoxMap.SelectedItem = null;
							pictureBoxMapRefresh();
						}
						catch
						{
						}
						break;
				}
			}
		}

		private void controlMapLoad(Map map)
		{
			if (map != null)
			{
				textBoxMapSaveType.Text = String.Format("{0:X2}", map.SaveType);
				textBoxMapNumber.Text = String.Format("{0:X4}", map.Number);

				textBoxMapWarpScriptAddress.Text = String.Format("{0:X8}", map.WarpScriptList.Address);
				textBoxMapMainScriptAddress.Text = String.Format("{0:X8}", map.MapScriptList.MainScript.Address);
				textBoxMapObjectScriptAddress.Text = String.Format("{0:X8}", map.CharacterScriptList.Address);
				textBoxMapAreaAddress.Text = String.Format("{0:X8}", map.MapArea.Address);

				textBoxMapX.Text = map.Width.ToString();
				textBoxMapY.Text = map.Height.ToString();

				comboBoxMapBGM.SelectedIndex = map.BGMNumber;

				pictureBoxMapRefresh();

				//setText(" - マップ - " + String.Format("{0:X4}", mapSetting.Number));
			}
		}

		private void saveMap()
		{
			if (listBoxMap.SelectedItem != null)
			{
				Map map = listBoxMap.SelectedItem as Map;

				//設定
				//mapSetting.SaveType = (byte)numericUpDownSaveType.Value;
				//mapSetting.Number = (ushort)numericUpDownMapNumber.Value;

				//"//"がついてるコードはアクセス権問題
				//tomatoAdventure.Map[Index].warpScript.address = Convert.ToInt32(TextBoxMapWarpScriptAddress.Text, 16);
				map.MapScriptList.MainScript.Address = Convert.ToUInt32(textBoxMapMainScriptAddress.Text, 16);
				//tomatoAdventure.Map[Index].characterScript.address = Convert.ToInt32(TextBoxMapObjectScriptAddress.Text, 16);
				map.MapArea.Address = Convert.ToUInt32(textBoxMapAreaAddress.Text, 16);
			}
		}

		#region マップビュー

		private void PictureBoxMapView_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				if (listBoxMap.SelectedItem != null)
				{
					Map map = listBoxMap.SelectedItem as Map;

					pictureBoxResize(sender, map);

					//マップチップナンバーリスト
					map.draw(e.Graphics, toolStripMenuItemMapChipBG[0].Checked, toolStripMenuItemMapChipBG[1].Checked, toolStripMenuItemMapChipBG[2].Checked);

					//マップエリアの描写
					if (toolStripMenuItemMapArea.Checked)
					{
						map.MapArea.draw(e.Graphics, toolStripMenuItemMapChipBG[0].Checked || toolStripMenuItemMapChipBG[1].Checked || toolStripMenuItemMapChipBG[2].Checked);
					}

					//グリッド描写
					if (toolStripMenuItemMapGrid.Checked)
					{
						gridDraw(sender, e.Graphics);
					}

					//ワープスクリプト描写
					if (toolStripMenuItemMapWarpScript.Checked)
					{
						map.WarpScriptList.draw(e.Graphics);
					}

					//マップスクリプト
					if (toolStripMenuItemMapScript.Checked)
					{
						map.MapScriptList.draw(e.Graphics);
					}

					//キャラクタースクリプト描写
					if (toolStripMenuItemMapCharacterScript.Checked)
					{
						map.CharacterScriptList.draw(e.Graphics);
					}
				}
			}
			catch
			{
			}
		}

		#endregion

		//グリッド
		private void gridDraw(object sender, Graphics graphics)
		{
			PictureBox pictureBox = sender as PictureBox;

			//色指定
			Pen pen = new Pen(Color.FromArgb(255, 0, 0), 1);

			//縦線
			for (int x = 0; x < (int)(pictureBox.Width / 16); ++x)
			{
				graphics.DrawLine(pen, x * 16, 0, x * 16, pictureBox.Height);
			}

			//横線
			for (int y = 0; y < (int)(pictureBox.Height / 16); ++y)
			{
				graphics.DrawLine(pen, 0, y * 16, pictureBox.Width, y * 16);
			}
		}

		private void pictureBoxResize(object sender, Map map)
		{
			(sender as PictureBox).Width = map.Width * 16;
			(sender as PictureBox).Height = map.Height * 16;
		}

		//全てのMapのPictureBoxをRefresh
		private void pictureBoxMapRefresh()
		{
			pictureBoxMapView.Refresh();
		}

		private void pictureBoxMapDispose()
		{
			pictureBoxMapView.Dispose();
		}

		private void ToolStripMenuItemMap_CheckedChanged(object sender, EventArgs e)
		{
			pictureBoxMapRefresh();
		}

		//マップスクリプト編集
		private void ButtonMapScriptEdit_Click(object sender, EventArgs e)
		{
			if (listBoxMap.SelectedItem != null)
			{
				ScriptEditor(tomatoAdventure, tomatoAdventure.MapList[listBoxMap.SelectedIndex].MapScriptList.MainScript.Address);
			}
		}


		private void textBoxMapSaveType_TextChanged(object sender, EventArgs e)
		{
			if (listBoxMap.SelectedItem != null)
			{
				try
				{
					((Map)listBoxMap.SelectedItem).SaveType = Convert.ToByte(textBoxMapSaveType.Text, 16);
				}
				catch
				{
				}
			}
		}

		private void textBoxMapNumber_TextChanged(object sender, EventArgs e)
		{
			if (listBoxMap.SelectedItem != null)
			{
				try
				{
					((Map)listBoxMap.SelectedItem).Number = Convert.ToByte(textBoxMapNumber.Text, 16);
				}
				catch
				{
				}
			}
		}

		#endregion

		#region モンスター

		private void listBoxMonster_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxMonster.Items != null)
			{
				Monster monster = listBoxMonster.SelectedItem as Monster;
				controlMonsterLoad(monster);
			}
			else
			{
				controlMonsterClear();
			}
		}
		private void listBoxMonster_Format(object sender, ListControlConvertEventArgs e)
		{
			e.Value = String.Format("{0:X4}", (e.ListItem as Monster).Name.Text);
		}

		private void controlMonsterLoad(Monster monster)
		{
			if (monster != null)
			{
				textBoxMonsterName.Text = monster.Name.Text;
				textBoxMonsterHP.Text = monster.HP.ToString();
				textBoxMonsterDiffence.Text = monster.Diffence.ToString();
				textBoxMonsterExperience.Text = monster.Experience.ToString();
				textBoxMonsterMoney.Text = monster.Money.ToString();
				textBoxMonsterSpeed.Text = monster.Speed.ToString();
				textBoxMonsterSize.Text = String.Format("{0:X2}", monster.Size);
				pictureBox2.Refresh();
				pictureBox3.Refresh();
			}
		}

		private void controlMonsterClear()
		{
			textBoxMonsterName.Text = null;
			textBoxMonsterHP.Text = null;
			textBoxMonsterDiffence.Text = null;
			textBoxMonsterExperience.Text = null;
			textBoxMonsterMoney.Text = null;
			textBoxMonsterSpeed.Text = null;
		}

		#endregion

		//スクリプトエディター呼び出し
		public void ScriptEditor(TomatoAdventure tomatoAdventure, uint address)
		{
			ScriptEditor scriptEditor = new ScriptEditor(tomatoAdventure, address);
			scriptEditor.Show();
			scriptEditor.Refresh();
			scriptEditor.Activate();
		}


		private void button2_Click(object sender, EventArgs e)
		{
			FormMapEditor mapEditor = new FormMapEditor(tomatoAdventure, listBoxMap.SelectedItem as Map);
			mapEditor.ShowDialog();

		}

		private void button3_Click(object sender, EventArgs e)
		{
			WindowStringEditor windowStringEditor = new WindowStringEditor(tomatoAdventure, null);
			windowStringEditor.Show();
			windowStringEditor.Refresh();
			windowStringEditor.Activate();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			Map map = tomatoAdventure.MapList[3];

			BattleBackGround BBG = new BattleBackGround(tomatoAdventure, map);
			Bitmap bitmap = BBG.toBitmap();
			e.Graphics.DrawImage(bitmap, 0, 0);
			bitmap.Dispose();
		}

		private void pictureBox2_Paint(object sender, PaintEventArgs e)
		{
			Monster monster = listBoxMonster.SelectedItem as Monster;

			if (monster.tileList != null && monster.palette != null)
			{
				(sender as PictureBox).Width = monster.tileList[0].Tile.Count * 8;
				(sender as PictureBox).Height = 8;

				for (int i = 0; i < monster.tileList[0].Count; ++i)
				{
					Bitmap b = monster.tileList[0][i].toBitmap(monster.palette);
					e.Graphics.DrawImage(b, i * 8, 0);
					b.Dispose();
				}
			}
			else
			{
				e.Graphics.DrawImage(MapArea.image[0], 0, 0);
			}
		}

		private void pictureBox3_Paint(object sender, PaintEventArgs e)
		{
			Monster monster = listBoxMonster.SelectedItem as Monster;

			if (monster.tileList != null && monster.tileList[1] != null && monster.palette != null)
			{
				(sender as PictureBox).Width = monster.tileList[1].Tile.Count * 8;
				(sender as PictureBox).Height = 8;

				for (int i = 0; i < monster.tileList[0].Count; ++i)
				{
					Bitmap b = monster.tileList[1][i].toBitmap(monster.palette);
					e.Graphics.DrawImage(b, i * 8, 0);
					b.Dispose();
				}
			}
			else
			{
				e.Graphics.DrawImage(MapArea.image[0], 0, 0);
			}
		}

		private void loadEtc()
		{
			textBoxROMSize.Text = String.Format("{0:X8}", tomatoAdventure.get().Length);
		}

		private void buttonROMResize_Click(object sender, EventArgs e)
		{
			try
			{
				tomatoAdventure.resize(Convert.ToInt32(textBoxROMSize.Text, 16));
				MessageBox.Show("リサイズ成功", applicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), applicationName + " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			SplitContainer[] splitContainerList = new SplitContainer[]
			{
				splitContainer1,
				splitContainer2,
				splitContainer3,
				splitContainer5,
			};

			int splitterDistance = ((SplitContainer)sender).SplitterDistance;

			for (int i = 0; i < splitContainerList.Length; ++i)
			{
				splitContainerList[i].SplitterMoved -= splitContainer_SplitterMoved;
				splitContainerList[i].SplitterDistance = splitterDistance;
				splitContainerList[i].SplitterMoved += splitContainer_SplitterMoved;
			}

		}
	}
}