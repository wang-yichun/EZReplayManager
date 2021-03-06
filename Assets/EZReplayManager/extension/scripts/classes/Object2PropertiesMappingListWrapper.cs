using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using ProtoBuf;

//[Serializable()]
[ProtoContract]
public class Object2PropertiesMappingListWrapper// : ISerializable {
{
	[ProtoMember (1)]
	public List<Object2PropertiesMapping> object2PropertiesMappings = new List<Object2PropertiesMapping> ();
	[ProtoMember (2)]
	public string EZR_VERSION = EZReplayManager.EZR_VERSION;
	[ProtoMember (3)]
	public float recordingInterval;

	//serialization constructor
	//	protected Object2PropertiesMappingListWrapper (SerializationInfo info, StreamingContext context)
	//	{
	//		object2PropertiesMappings = (List<Object2PropertiesMapping>)info.GetValue ("object2PropertiesMappings", typeof(List<Object2PropertiesMapping>));
	//		EZR_VERSION = info.GetString ("EZR_VERSION");
	//		recordingInterval = (float)info.GetValue ("recordingInterval", typeof(float));
	//	}

	public Object2PropertiesMappingListWrapper (List<Object2PropertiesMapping> mappings)
	{
		object2PropertiesMappings = mappings;
	}

	public Object2PropertiesMappingListWrapper ()
	{

	}

	public void addMapping (Object2PropertiesMapping mapping)
	{
		object2PropertiesMappings.Add (mapping);
	}

	//	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	//	{
	//		info.AddValue ("object2PropertiesMappings", this.object2PropertiesMappings);
	//		info.AddValue ("EZR_VERSION", EZR_VERSION);
	//		info.AddValue ("recordingInterval", this.recordingInterval);
	//		//base.GetObjectData(info, context);
	//	}

	public void CalcParentMapingIdx ()
	{
		for (int i = 0; i < object2PropertiesMappings.Count; i++) {
			Object2PropertiesMapping mapping = object2PropertiesMappings [i];
			mapping.currentMappingIdx = i;
		}
		for (int i = 0; i < object2PropertiesMappings.Count; i++) {
			Object2PropertiesMapping mapping = object2PropertiesMappings [i];
			if (mapping.parentMapping != null) {
				mapping.parentMappingIdx = mapping.parentMapping.currentMappingIdx;
			} else {
				mapping.parentMappingIdx = -1;
			}
		}
	}

	public void CalcParentMapingRef ()
	{
		for (int i = 0; i < object2PropertiesMappings.Count; i++) {
			Object2PropertiesMapping mapping = object2PropertiesMappings [i];
			mapping.currentMappingIdx = i;
			if (mapping.parentMappingIdx != -1) {
				mapping.parentMapping = object2PropertiesMappings [mapping.parentMappingIdx];
			} else {
				mapping.parentMapping = null;
			}
		}
	}
}
