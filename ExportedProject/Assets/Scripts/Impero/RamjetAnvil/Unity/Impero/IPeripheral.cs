namespace RamjetAnvil.Unity.Impero
{
	public interface IPeripheral
	{
		string Identifier { get; }

		int PlayerId { get; set; }

		int NumButtons { get; }

		int NumAxes { get; }

		float Deadzone { get; }

		float MappingThreshold { get; }

		float GetAxis(int axis);

		bool GetButton(int button);

		bool GetButtonUp(int button);

		bool GetButtonDown(int button);

		string GetHumanReadableAxisName(int axis);

		string GetHumanReadableButtonName(int button);

		void Update();
	}
}
