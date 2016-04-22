using UnityEngine;
using ProtoBuf;

[ProtoContract]
public class PBQuaternion3
{
	[ProtoIgnore]
	public Quaternion quaternion = new Quaternion ();

	public PBQuaternion3 ()
	{
	}

	public PBQuaternion3 (Quaternion source)
	{
		quaternion = source;
	}

	[ProtoMember (1, Name = "qx")]
	public float x {
		get { return quaternion.x; }
		set { quaternion.x = value; }
	}

	[ProtoMember (2, Name = "qy")]
	public float y {
		get { return quaternion.y; }
		set { quaternion.y = value; }
	}

	[ProtoMember (3, Name = "qz")]
	public float z {
		get { return quaternion.z; }
		set { quaternion.z = value; }
	}

	[ProtoMember (4, Name = "qw")]
	public float w {
		get { return quaternion.w; }
		set { quaternion.w = value; }
	}

	public static implicit operator Quaternion (PBQuaternion3 i)
	{
		return i.quaternion;
	}

	public static implicit operator PBQuaternion3 (Quaternion i)
	{
		return new PBQuaternion3 (i);
	}

	public override string ToString ()
	{
		return quaternion.ToString ();
	}

	public bool isDifferentTo (PBQuaternion3 i)
	{
		return x != i.x || y != i.y || z != i.z || w != i.w;
	}
}