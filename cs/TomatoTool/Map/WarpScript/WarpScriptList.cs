using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TomatoTool
{
	public class WarpScriptList : ROMObject, IList
	{
		private List<WarpScript> warpScript;

		public override bool Saved
		{
			get
			{
				for (int i = 0; i < warpScript.Count; ++i)
				{
					if (!warpScript[i].Saved)
					{
						return false;
					}
				}
				return saved;
			}
		}

		public static readonly byte[] FOOTER = new byte[]
		#region
		{
			0xFF,
			0x00,
			0x00,
			0x00
		};
		#endregion
		public static readonly uint FOOTER_SIZE = 4;

		public static Color Color = Color.FromArgb(0, 0, 255);

		public WarpScriptList()
		{
			initialize();
		}
		public WarpScriptList(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void initialize()
		{
			address = TomatoTool.ROM.ADDRESS_NULL;
			warpScript = new List<WarpScript>();
		}
		public void load(TomatoAdventure tomatoAdventure)
		{
			load(tomatoAdventure, address);
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			warpScript = new List<WarpScript>();

			{
				uint i = 0;
				while ((tomatoAdventure.read((uint)(address + (i * WarpScript.SIZE))) != 0xFF))
				{
					warpScript.Add(new WarpScript(tomatoAdventure, address + (i * WarpScript.SIZE)));
					++i;
				}
			}
		}
		public void save(TomatoAdventure tomatoAdventure)
		{
			if (!Saved)
			{
				save(tomatoAdventure, tomatoAdventure.malloc(getSize()));
			}
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			if (warpScript != null)
			{
				for (uint i = 0; i < warpScript.Count; ++i)
				{
					warpScript[(int)i].save(tomatoAdventure, address + (i * TomatoTool.WarpScript.SIZE));
				}

				tomatoAdventure.writeArray((uint)(address + (warpScript.Count * TomatoTool.WarpScript.SIZE)), (uint)FOOTER.GetLength(0), FOOTER);
			}

			saved = true;
		}
		public override uint getSize()
		{
			return (uint)((WarpScript.SIZE * warpScript.Count) + FOOTER_SIZE);
		}

		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)warpScript).IsFixedSize;
			}
		}
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)warpScript).IsReadOnly;
			}
		}
		object IList.this[int index]
		{
			get
			{
				return ((IList)warpScript)[index];
			}

			set
			{
				((IList)warpScript)[index] = value;
				saved = false;
			}
		}
		int IList.Add(object value)
		{
			saved = false;
			return ((IList)warpScript).Add(value);
		}
		void IList.Clear()
		{
			((IList)warpScript).Clear();
			saved = false;
		}
		bool IList.Contains(object value)
		{
			return ((IList)warpScript).Contains(value);
		}
		int IList.IndexOf(object value)
		{
			return ((IList)warpScript).IndexOf(value);
		}
		void IList.Insert(int index, object value)
		{
			((IList)warpScript).Insert(index, value);
			saved = false;
		}
		void IList.Remove(object value)
		{
			((IList)warpScript).Remove(value);
			saved = false;
		}
		void IList.RemoveAt(int index)
		{
			((IList)warpScript).RemoveAt(index);
			saved = false;
		}
		int ICollection.Count
		{
			get
			{
				return ((ICollection)warpScript).Count;
			}
		}
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((ICollection)warpScript).IsSynchronized;
			}
		}
		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)warpScript).SyncRoot;
			}
		}
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)warpScript).CopyTo(array, index);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)warpScript).GetEnumerator();
		}

		public void draw(Graphics graphics)
		{
			draw(graphics, -1);
		}
		public void draw(Graphics graphics, int select)
		{
			if (warpScript != null)
			{
				for (int i = 0; i < warpScript.Count; ++i)
				{
					using (Pen pen = new Pen((select == i) ? Map.SelectColor : Color, 2))
					{
						graphics.DrawRectangle(pen, (warpScript[i].BeginX * (int)Map.BLOCK_WIDTH) + 1, (warpScript[i].BeginY * (int)Map.BLOCK_HEIGHT) + 1, ((warpScript[i].EndX - warpScript[i].BeginX) * Map.BLOCK_WIDTH) + (Map.BLOCK_WIDTH - pen.Width), ((warpScript[i].EndY - warpScript[i].BeginY) * Map.BLOCK_HEIGHT) + (Map.BLOCK_HEIGHT - pen.Width));
					}
				}
			}
		}
	}
}
