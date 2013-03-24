using System;
using TomatoTool;

namespace TomatoTool
{
	public abstract class ROMObject : IEquatable<ROMObject>, IComparable<ROMObject>, ISizeGettable, Loadable, Savable
	{
		bool IEquatable<ROMObject>.Equals(ROMObject romObject)
		{
			return this.address == romObject.address;
		}
		int IComparable<ROMObject>.CompareTo(ROMObject romObject)
		{
			return (int)(this.address - romObject.address);
		}

		public static bool operator ==(ROMObject left, ROMObject right)
		{
			if (Object.ReferenceEquals(left, right))
			{
				return true;
			}

			if (((object)left == null) || ((object)right == null))
			{
				return false;
			}

			return left.address == right.address;
		}
		public static bool operator !=(ROMObject left, ROMObject right)
		{
			if (Object.ReferenceEquals(left, right))
			{
				return false;
			}

			if (((object)left == null) || ((object)right == null))
			{
				return true;
			}

			return left.address != right.address;
		}

		private static uint virtualAddress = 0xFF000000;
		public static uint VirtualAddress
		{
			get
			{
				return virtualAddress++;
			}
		}

		protected bool saved;
		public virtual bool Saved
		{
			get
			{
				return saved;
			}

			set
			{
				saved = value;
			}
		}

		protected uint address;
		public uint Address
		{
			get
			{
				return address;
			}

			set
			{
				address = value;
			}
		}

		public ROMObject()
		{
			saved = true;
		}
		public ROMObject(uint address)
		{
			this.address = address;
		}

		public virtual void initialize()
		{
		}
		
		void Loadable.load(TomatoAdventure tomatoAdventure, uint address)
		{
			load(tomatoAdventure, address);
		}
		public virtual void load(TomatoAdventure tomatoAdventure, uint address)
		{
		}
		
		void Savable.save(TomatoAdventure tomatoAdventure, uint address)
		{
			save(tomatoAdventure, address);
		}
		public virtual void save(TomatoAdventure tomatoAdventure, uint address)
		{
		}
	
		uint ISizeGettable.getSize()
		{
			return getSize();
		}
		public virtual uint getSize()
		{
			return 0;
		}
	}
}
