using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameObjectData
{
    public string prefabName;  // Nombre o identificador del prefab
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public SerializableVector3 scale;
    public SerializableVector3 color;
}

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData", order = 1)]
public class RoomObjects : ScriptableObject
{
    public List<GameObjectData> gameObjectsData = new List<GameObjectData>();
}

//He cambiado todo lo de arriba 
[System.Serializable]
public class RoomObjectsHolder
{
    public List<GameObjectData> gameObjectData;
}