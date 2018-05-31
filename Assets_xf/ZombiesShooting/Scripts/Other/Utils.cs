using UnityEngine;

public class Utils
{
	public static float Angle(Vector3 origin, Vector3 target)
	{
		float x = target.x - origin.x;
		float y = target.y - origin.y;
		return (Mathf.Atan2(y, x) / Mathf.PI) * 180;
	}

	public static Quaternion Rotate(Vector3 origin, Vector3 target)
	{
		return Quaternion.AngleAxis(Utils.Angle(origin, target) + 90, Vector3.forward);
	}

	public static Quaternion EffectRotation()
	{
		return Quaternion.AngleAxis(90, Vector3.left);
	}
}
