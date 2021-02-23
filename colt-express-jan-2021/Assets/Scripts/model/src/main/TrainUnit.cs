using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Protocol.Serialization;


//The following code is executed right after creating the SmartFox object:
// using System.Reflection;
//        DefaultSFSDataSerializer.RunningAssembly = Assembly.GetExecutingAssembly();
public class TrainUnit : SerializableSFSType
{

    public CarType carType;
    public TrainUnit otherfloor;
    public Marshal marshalHere;
    public ArrayList banditPositions = new ArrayList();
    public ArrayList lootInCabin = new ArrayList();
    public ArrayList adjacent = new ArrayList();
    public ArrayList horses = new ArrayList();

    public TrainUnit() { }

}
