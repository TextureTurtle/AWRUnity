using System;

[Flags]
public enum MountType
{
	None = 0,
	Generic = 1,
	Fuel = 2,
	Burner = 4,
	Small = 8
}
