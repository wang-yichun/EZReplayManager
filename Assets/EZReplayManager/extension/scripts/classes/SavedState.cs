using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;
using ProtoBuf;

/*
 * SoftRare - www.softrare.eu
 * This class represents a state of a single object in one single frame. 
 * You may only use and change this code if you purchased it in a legal way.
 * Please read readme-file, included in this package, to see further possibilities on how to use/execute this code. 
 */

//[Serializable()]
[ProtoContract]
public class SavedState// : ISerializable
{
	//so far 3 state attributes are saved: position, rotation, and if the game object was emitting particles when being in this state
	//	public SerVector3 position;
	//	public SerVector3 localPosition;
	//	public SerQuaternion rotation;
	//	public SerQuaternion localRotation;

	[ProtoMember (1)]
	public PBVector3 position;
	[ProtoMember (2)]
	public PBVector3 localPosition;
	[ProtoMember (3)]
	public PBQuaternion3 rotation;
	[ProtoMember (4)]
	public PBQuaternion3 localRotation;

	[ProtoMember (5)]
	public bool emittingParticles = false;
	[ProtoMember (6)]
	public bool isActive = false;

	public SavedState ()
	{
	}

	//serialization constructor
	//	protected SavedState (SerializationInfo info, StreamingContext context)
	//	{
	//		try {
	//			this.position = (SerVector3)info.GetValue ("position", typeof(SerVector3));
	//			this.rotation = (SerQuaternion)info.GetValue ("rotation", typeof(SerQuaternion));
	//		} catch {
	//			//not available if used an older version to save the replay, ignore
	//		}
	//		this.localPosition = (SerVector3)info.GetValue ("localPosition", typeof(SerVector3));
	//		this.localRotation = (SerQuaternion)info.GetValue ("localRotation", typeof(SerQuaternion));
	//
	//		emittingParticles = info.GetBoolean ("emittingParticles");
	//		isActive = info.GetBoolean ("isActive");
	//	}
	
	//as this is not derived from MonoBehaviour, we have a constructor
	public SavedState (GameObject go)
	{
		
		if (go != null) {
			if (go.GetComponent<ParticleEmitter> ())
				emittingParticles = go.GetComponent<ParticleEmitter> ().emit;
			
			this.position = go.transform.position;
			this.rotation = go.transform.rotation;
			this.localPosition = go.transform.localPosition;
			this.localRotation = go.transform.localRotation;
			this.isActive = go.activeInHierarchy;
		} else {
			this.position = Vector3.zero;
			this.rotation = Quaternion.identity;
			this.localPosition = Vector3.zero;
			this.localRotation = Quaternion.identity;			
			this.isActive = false;	
		}
		
	}

	//	public Vector3 serVec3ToVec3 (SerVector3 serVec3)
	//	{
	//		return new Vector3 (serVec3.x, serVec3.y, serVec3.z);
	//	}
	//
	//	public Quaternion serQuatToQuat (SerQuaternion serQuat)
	//	{
	//		return new Quaternion (serQuat.x, serQuat.y, serQuat.z, serQuat.w);
	//	}

	public bool isDifferentTo (SavedState otherState, Object2PropertiesMapping o2m)
	{
		bool changed = false;
		
		if (!changed && isActive != otherState.isActive)
			changed = true;			
		
		if (o2m.isParentObj && o2m.getGameObject ().transform.parent != null) {
			if (!changed && position.isDifferentTo (otherState.position))
				changed = true;
			
			if (!changed && rotation.isDifferentTo (otherState.rotation))
				changed = true;
		}
		
		if (!changed && localPosition.isDifferentTo (otherState.localPosition))
			changed = true;
		
		if (!changed && localRotation.isDifferentTo (otherState.localRotation))
			changed = true;
		
		if (!changed && emittingParticles != otherState.emittingParticles)
			changed = true;	
		
		return changed;
	}
	
	//called to synchronize gameObjectClone of Object2PropertiesMapping back to this saved state
	public void synchronizeProperties (GameObject go, Object2PropertiesMapping o2m)
	{
		
		//HINT: lerping is still highly experimental
		//EZReplayManager.singleton.StartCoroutine_Auto(EZReplayManager.singleton.MoveTo (go.transform,serVec3ToVec3(this.localPosition),0.08f));	
		
		if (o2m.isParentObj && o2m.getGameObject () != null && o2m.getGameObject ().transform.parent != null) {
			
			go.transform.position = this.position;
			go.transform.rotation = this.rotation;	
			
			
		} else {
			go.transform.localPosition = this.localPosition;
			go.transform.localRotation = this.localRotation;		
			
		}
		
		go.SetActive (this.isActive);
		
		if (emittingParticles)
			go.GetComponent<ParticleEmitter> ().emit = true;
		else if (go.GetComponent<ParticleEmitter> ())
			go.GetComponent<ParticleEmitter> ().emit = false;
	}
	
	/*[SecurityPermissionAttribute(
	            SecurityAction.Demand,
	            SerializationFormatter = true)]		*/
	//	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	//	{
	//
	//		info.AddValue ("position", position);
	//		info.AddValue ("localPosition", localPosition);
	//		info.AddValue ("rotation", rotation);
	//		info.AddValue ("localRotation", localRotation);
	//
	//		info.AddValue ("emittingParticles", this.emittingParticles);
	//		info.AddValue ("isActive", this.isActive);
	//		//base.GetObjectData(info, context);
	//	}
	
}