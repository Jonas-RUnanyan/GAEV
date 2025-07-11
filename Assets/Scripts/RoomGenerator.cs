using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using Newtonsoft.Json;
using System.IO;
using Unity.VisualScripting;

public class RoomGenerator : MonoBehaviour
{
    //Prefab panel and object pool
    public GameObject panel;
    public GameObject door;
    private Pool pool;

    [FormerlySerializedAs("roomData")] public static RoomObjects roomObjects;



    //reference for the room object so ItemPlacer can place the objects as children of room (this is important for exporting the room)
    public RoomParameters roomPar;

    //Current and desired number of doors
    private int N_doors = 0;
    private int S_doors = 0;
    private int E_doors = 0;
    private int W_doors = 0;

    //GameObjects for all the components of the room
    private GameObject room;
    private GameObject floor;
    private GameObject roof;
    private GameObject walls;
    private GameObject N_wall;
    private GameObject S_wall;
    private GameObject E_wall;
    private GameObject W_wall;

    public static GameObject roomRef;

    //0.03 6 2.5
    private Pool doorPool;

    //Wall renderer and color
    private Renderer rend;
    //[Range(0.0f, 1.0f)]
    //public float r = 1;
    //[Range(0.0f, 1.0f)]
    //public float g=1;
    //[Range(0.0f, 1.0f)]
    //public float b=1;

    void Start()
    {
        //creation (and naming) of the GameObjects corresponding to the components of the room
        room = new GameObject("Room");
        floor = new GameObject("Floor");
        roof = new GameObject("Roof");
        walls = new GameObject("Walls");
        N_wall = new GameObject("North Wall");
        S_wall = new GameObject("South Wall");
        E_wall = new GameObject("East Wall");
        W_wall = new GameObject("West Wall");

        //creation of the pool of tiles
        pool = new Pool(panel, 150);
        doorPool = new Pool(door, 100);

        //creation of the hierachy of components
        floor.transform.parent = room.transform;
        walls.transform.parent = room.transform;
        roof.transform.parent = room.transform;
        N_wall.transform.parent = walls.transform;
        S_wall.transform.parent = walls.transform;
        E_wall.transform.parent = walls.transform;
        W_wall.transform.parent = walls.transform;

        //construction of basic room without doors
        Instantiate(panel, new Vector3(0, 0, 0), Quaternion.identity, floor.transform).SetActive(true);
        Instantiate(panel, new Vector3(0, 0, 0), Quaternion.identity, roof.transform).SetActive(true);
        Instantiate(panel, new Vector3(0, 0, 0), Quaternion.identity, N_wall.transform).SetActive(true);
        Instantiate(panel, new Vector3(0, 0, 0), Quaternion.identity, S_wall.transform).SetActive(true);
        Instantiate(panel, new Vector3(0, 0, 0), Quaternion.identity, E_wall.transform).SetActive(true);
        Instantiate(panel, new Vector3(0, 0, 0), Quaternion.identity, W_wall.transform).SetActive(true);

        roof.transform.localRotation = UnityEngine.Quaternion.Euler(180, 0, 0);
        N_wall.transform.localRotation = UnityEngine.Quaternion.Euler(-90, 0, 0);
        S_wall.transform.localRotation = UnityEngine.Quaternion.Euler(-90, 180, 0);
        E_wall.transform.localRotation = UnityEngine.Quaternion.Euler(-90, 90, 0);
        W_wall.transform.localRotation = UnityEngine.Quaternion.Euler(-90, -90, 0);

        roomPar.baseColor = new Color();

        roomRef = room;

        //creation of a renderer for each wall and the color they will share
        foreach (Transform child in walls.transform)
        {
            child.gameObject.AddComponent<MeshRenderer>();

            //rend.material = Resources.Load("Color", typeof(Material)) as Material;
            rend = child.GetComponent<MeshRenderer>();
            rend.material.SetColor("_BaseColor", roomPar.baseColor);
        }
    }

    void Update()
    {
        //N_target = (int)Mathf.Clamp(N_target, 0, 5*N_wall.transform.localScale.x / doorWidth);

        //scaling of roof and floor
        floor.transform.localScale = new Vector3(roomPar.width, 1, roomPar.length);
        roof.transform.localScale = new Vector3(roomPar.width, 1, roomPar.length);
        roof.transform.localPosition = new Vector3(0, roomPar.height * 10, 0);
        roof.SetActive(roomPar.hasRoof);

        //scaling and position of each wall
        //north wall
        N_wall.transform.localPosition = new Vector3(0, roomPar.height * 5, roomPar.length * 5);
        N_wall.transform.localScale = new Vector3(roomPar.width, 1, roomPar.height);
        UpdateWall(0);

        //south wall
        S_wall.transform.localPosition = new Vector3(0, roomPar.height * 5, roomPar.length * -5);
        S_wall.transform.localScale = new Vector3(roomPar.width, 0.01f, roomPar.height);
        UpdateWall(1);

        //west wall
        W_wall.transform.localPosition = new Vector3(roomPar.width * -5, roomPar.height * 5, 0);
        W_wall.transform.localScale = new Vector3(roomPar.length, 1, roomPar.height);
        UpdateWall(2);

        //east wall
        E_wall.transform.localPosition = new Vector3(roomPar.width * 5, roomPar.height * 5, 0);
        E_wall.transform.localScale = new Vector3(roomPar.length, 1, roomPar.height);
        UpdateWall(3);

        //color each wall
        foreach (Transform wall in walls.transform)
        {
            foreach (Transform tile in wall)
            {
                if (!tile.gameObject.name.Contains("Door"))
                {
                    Renderer rend = tile.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        rend.material.SetColor("_BaseColor", roomPar.baseColor);
                    }
                }
            }
        }


        if (Input.GetKeyUp(KeyCode.G))
        {
            SaveRoomConfiguration();
        }
    }

    /// <summary>
    /// this funciton adds a door to the selected wall by adding to tile from the pool to it
    /// </summary>
    /// <param name="wallSelector">selects the wall to be changed (0=north, 1=south, 2=east, 3=west), any other value throws an error</param>
    void AddDoor(int wallSelector)
    {
        GameObject wall;
        switch (wallSelector)
        {
            case 0:
                wall = N_wall;
                N_doors++;
                break;
            case 1:
                wall = S_wall;
                S_doors++;
                break;
            case 2:
                wall = E_wall;
                E_doors++;
                break;
            case 3:
                wall = W_wall;
                W_doors++;
                break;
            default:
                throw new Exception("Invalid wallSelector " + wallSelector + " must be between 0 and 3");
        }

        pool.Emerge(wall);
        pool.Emerge(wall);
        doorPool.Emerge(wall);
    }

    /// <summary>
    /// this funciton removes a door of the selected wall by pooling two of its tiles
    /// </summary>
    /// <param name="wallSelector">selects the wall to be changed (0=north, 1=south, 2=east, 3=west), any other value throws an error</param>
    void RemoveDoor(int wallSelector)
    {
        GameObject wall;
        switch (wallSelector)
        {
            case 0:
                wall = N_wall;
                N_doors--;
                break;
            case 1:
                wall = S_wall;
                S_doors--;
                break;
            case 2:
                wall = E_wall;
                E_doors--;
                break;
            case 3:
                wall = W_wall;
                W_doors--;
                break;
            default:
                throw new Exception("Invalid wallSelector " + wallSelector + " must be between 0 and 3");
        }

        pool.Submerge(wall.transform.GetChild(0).gameObject);
        pool.Submerge(wall.transform.GetChild(0).gameObject);

        foreach (Transform child in wall.transform)
        {
            if (prefabChecker("Door", child.gameObject))
            {
                doorPool.Submerge(child.gameObject);

            }

        }
    }

    /// <summary>
    /// places all of the tiles of a wall so that it has the correct number of doors and they are evenly spaced
    /// </summary>
    /// <param name="wallSelector">selects the wall to be changed (0=north, 1=south, 2=east, 3=west), any other value throws an error</param>
    void UpdateWall(int wallSelector)
    {
        //wall selector
        GameObject wall;
        int doors;
        int targetDoors;
        switch (wallSelector)
        {
            case 0:
                wall = N_wall;
                doors = N_doors;
                targetDoors = roomPar.N_target;
                break;
            case 1:
                wall = S_wall;
                doors = S_doors;
                targetDoors = roomPar.S_target;
                break;
            case 2:
                wall = E_wall;
                doors = E_doors;
                targetDoors = roomPar.E_target;
                break;
            case 3:
                wall = W_wall;
                doors = W_doors;
                targetDoors = roomPar.W_target;
                break;
            default:
                throw new Exception("Invalid wallSelector " + wallSelector + " must be between 0 and 3");
        }

        //adds or romoves the necesary number of doors so that the wall has as many doors as the user chooses
        while (targetDoors != doors)
        {
            if (doors < targetDoors)
            {
                AddDoor(wallSelector);
                doors++;
            }
            else if (doors > targetDoors)
            {
                RemoveDoor(wallSelector);
                doors--;
            }
            else break;
        }

        float wallWidth = wall.transform.localScale.x;
        float wallHeight = wall.transform.localScale.z;

        float doorSeparation = (wallWidth - (doors) * roomPar.doorMeasures.x) / (doors + 1);
        float overDoor = (wallHeight - roomPar.doorMeasures.y) / wallHeight;
        float doorWidth = roomPar.doorMeasures.x / wallWidth;
        float doorHeight = roomPar.doorMeasures.y / wallHeight;

        bool isOverDoor = false;
        float pos = -5 + doorSeparation * 5 / wallWidth;
        float doorPos = 0f;

        //as the boolean isOverDoor changes, the tiles of the wall are either the space between two doors or the wall above one
        foreach (Transform thing in wall.transform)
        {
            if (prefabChecker("Door", thing.gameObject))
            {
                thing.transform.localScale = new Vector3(doorWidth, 1, doorHeight);

                thing.transform.localRotation = Quaternion.Euler(0, 0, 0);

                thing.transform.localPosition = new Vector3(doorPos, 0, -((roomPar.height - roomPar.doorMeasures.y) / 2) / (roomPar.height / 10)); ;
            }
            else
            {
                if (isOverDoor)
                {
                    thing.localPosition = new Vector3(pos, 0f, doorHeight * 5);
                    thing.localScale = new Vector3(doorWidth, 1, overDoor);
                    doorPos = pos;
                    pos += (roomPar.doorMeasures.x + doorSeparation) * 5 / wallWidth;
                }
                else
                {
                    thing.localPosition = new Vector3(pos, 0f, 0f);
                    thing.localScale = new Vector3(doorSeparation / wallWidth, 1, 1);
                    pos += (doorSeparation + roomPar.doorMeasures.x) * 5 / wallWidth;
                }

                isOverDoor = !isOverDoor;
            }
        }
    }
    bool prefabChecker(string prefabName, GameObject obj)
    {
        // Aquí puedes ajustar cómo se verifica si un objeto es de un tipo de prefab específico.
        // Por simplicidad, compararé los nombres.
        return obj.name.Contains(prefabName);
    }


    void SaveRoomConfiguration()
    {
        RoomObjects roomObjects = new RoomObjects();

        // Guardar paneles en el piso, techo y paredes
        SaveChildTransforms(floor.transform, roomObjects);
        SaveChildTransforms(roof.transform, roomObjects);
        SaveWallTransforms(walls.transform, roomObjects);

        // Guardar otros prefabs directamente en la habitación (sillas, camas, mesas, etc.)
        foreach (Transform child in room.transform)
        {
            if (!IsParentObject(child.name)) // Verificar si no es un objeto padre
            {
                AddGameObjectData(child, roomObjects,false);
            }
        }

        string json = JsonConvert.SerializeObject(roomObjects, Formatting.Indented);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "roomConfiguration.json"), json);
        Debug.Log("Room configuration saved to " + Path.Combine(Application.persistentDataPath, "roomConfiguration.json"));
    }

    void SaveChildTransforms(Transform parent, RoomObjects roomObjects)
    {
        foreach (Transform child in parent)
        {
            AddGameObjectData(child, roomObjects,false);
        }
    }

    void SaveWallTransforms(Transform wallsParent, RoomObjects roomObjects)
    {
        foreach (Transform wall in wallsParent)
        {
            foreach (Transform child in wall)
            {
                AddGameObjectData(child, roomObjects, true);
            }
        }
    }




    void AddGameObjectData(Transform obj, RoomObjects roomObjects, bool isWall)
    {
        GameObjectData data = new GameObjectData
        {
            prefabName = obj.name.Replace("(Clone)", (isWall?"_w":"")), // Personaliza cómo obtener el nombre del prefab
            position = new SerializableVector3(obj.position),
            rotation = new SerializableQuaternion(obj.rotation),
            scale = new SerializableVector3(obj.lossyScale),
            color = new SerializableVector3(roomPar.baseColor)
        };
            

        roomObjects.gameObjectsData.Add(data);
    }


    bool IsParentObject(string name)
    {
        return name == "Walls" || name == "Roof" || name == "Floor" ||
               name == "North Wall" || name == "South Wall" ||
               name == "East Wall" || name == "West Wall" || name == "Doors";
    }



}

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(UnityEngine.Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public SerializableVector3(UnityEngine.Color color)
    {
        x = color.r;
        y = color.g;
        z = color.b;
    }

    public UnityEngine.Vector3 ToVector3()
    {
        return new UnityEngine.Vector3(x, y, z);
    }
    public Color ToColor()
    {
        return new Color(x, y, z);
    }
}

[System.Serializable]
public struct SerializableQuaternion
{
    public float x, y, z, w;

    public SerializableQuaternion(UnityEngine.Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    public UnityEngine.Quaternion ToQuaternion()
    {
        return new UnityEngine.Quaternion(x, y, z, w);
    }
}