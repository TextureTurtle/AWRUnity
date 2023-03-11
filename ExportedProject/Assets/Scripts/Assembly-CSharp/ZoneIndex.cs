using System;

public struct ZoneIndex
{
	public static readonly ZoneIndex Zero = new ZoneIndex(0, 0);

	public int X;

	public int Z;

	public ZoneIndex(int x, int z)
	{
		X = x;
		Z = z;
	}

	public bool Equals(ZoneIndex other)
	{
		return X == other.X && Z == other.Z;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		return obj is ZoneIndex && Equals((ZoneIndex)obj);
	}

	public override int GetHashCode()
	{
		return (X * 397) ^ Z;
	}

	public static ZoneIndex FromPosition(DVector3 worldPosition, double zoneSize)
	{
		return new ZoneIndex((int)Math.Floor(worldPosition.X / zoneSize), (int)Math.Floor(worldPosition.Z / zoneSize));
	}

	public static DVector3 ToPosition(ZoneIndex index, double zoneSize)
	{
		return new DVector3((double)index.X * zoneSize, 0.0, (double)index.Z * zoneSize);
	}

	public static DVector3 GetCenter(ZoneIndex index, double zoneSize)
	{
		double num = zoneSize * 0.5;
		return ToPosition(index, zoneSize) + new DVector3(num, 0.0, num);
	}

	public override string ToString()
	{
		return string.Format("{0},{1}", X, Z);
	}

	public static bool operator ==(ZoneIndex left, ZoneIndex right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ZoneIndex left, ZoneIndex right)
	{
		return !left.Equals(right);
	}
}
