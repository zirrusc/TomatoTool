using System;
using System.Collections;
using System.Collections.Generic;


namespace TomatoTool
{
	public class Action : ROMObject, IList
	{
		private List<ushort> action;

		public readonly ushort FOOTER = 0xFFFF;
		public readonly uint FOOTER_SIZE = 2;

		public Action(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}

		public override void initialize()
		{
			action = new List<ushort>();
		}
		public override void load(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			action = new List<ushort>();
			{
				uint i = 0;
				while (tomatoAdventure.readLittleEndian(address + (i * 2), 2) != FOOTER)
				{
					action.Add((ushort)tomatoAdventure.readLittleEndian(address + (i * 2), 2));
					++i;
				}
			}
		}
		public void save(TomatoAdventure tomatoAdventure)
		{
			if (!saved)
			{
				save(tomatoAdventure, tomatoAdventure.malloc(getSize(), 2));
				saved = true;
			}
		}
		public override void save(TomatoAdventure tomatoAdventure, uint address)
		{
			this.address = address;

			for (uint i = 0; i < action.Count; ++i)
			{
				tomatoAdventure.writeLittleEndian(address + (i * 2), 2, action[(int)i]);
			}

			tomatoAdventure.writeLittleEndian((uint)(address + (action.Count * 2)), 2, FOOTER);
		}
		public override uint getSize()
		{
			return (uint)((action.Count * 2) + FOOTER_SIZE);
		}

		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)action).IsFixedSize;
			}
		}
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)action).IsReadOnly;
			}
		}
		object IList.this[int index]
		{
			get
			{
				return ((IList)action)[index];
			}

			set
			{
				((IList)action)[index] = value;
				saved = false;
			}
		}
		int IList.Add(object value)
		{
			saved = false;
			return ((IList)action).Add(value);
		}
		void IList.Clear()
		{
			((IList)action).Clear();
			saved = false;
		}
		bool IList.Contains(object value)
		{
			return ((IList)action).Contains(value);
		}
		int IList.IndexOf(object value)
		{
			return ((IList)action).IndexOf(value);
		}
		void IList.Insert(int index, object value)
		{
			((IList)action).Insert(index, value);
			saved = false;
		}
		void IList.Remove(object value)
		{
			((IList)action).Remove(value);
			saved = false;
		}
		void IList.RemoveAt(int index)
		{
			((IList)action).RemoveAt(index);
			saved = false;
		}
		int ICollection.Count
		{
			get
			{
				return ((ICollection)action).Count;
			}
		}
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((ICollection)action).IsSynchronized;
			}
		}
		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)action).SyncRoot;
			}
		}
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)action).CopyTo(array, index);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)action).GetEnumerator();
		}
	}
}
