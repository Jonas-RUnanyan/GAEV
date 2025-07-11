using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    void Start()
    {
        LoadRoomConfiguration();
    }

    void LoadRoomConfiguration()
    {
        string path = Path.Combine(Application.persistentDataPath, "roomConfiguration.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            RoomObjects roomObjects = JsonConvert.DeserializeObject<RoomObjects>(json);
            CreateRoomFromData(roomObjects);
            Debug.Log("Room configuration loaded from " + path);
        }
        else
        {
            Debug.LogError("Room configuration file not found at " + path);
        }
    }

    void CreateRoomFromData(RoomObjects roomObjects)
    {
        GameObject room = new GameObject("Room");

        foreach (var data in roomObjects.gameObjectsData)
        {
            string prefabName = data.prefabName.Replace("_w", "");
            Debug.Log(prefabName);
            GameObject prefab = GetPrefabByName(prefabName);
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab, room.transform);
                obj.transform.localPosition = data.position.ToVector3();
                obj.transform.localRotation = data.rotation.ToQuaternion();
                obj.transform.localScale = data.scale.ToVector3();
                if (data.prefabName.Contains("_w")&&obj.GetComponent<MeshRenderer>()!=null)
                    obj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", data.color.ToColor());
            }
            else
            {
                Debug.LogWarning("Prefab not found for name: " + data.prefabName);
            }
        }
    }

    GameObject GetPrefabByName(string name)
    {
        string path = "Prefabs/" + name;
        GameObject prefab = Resources.Load<GameObject>(name);
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not found at path: " + path);
        }
        return prefab;
    }
}