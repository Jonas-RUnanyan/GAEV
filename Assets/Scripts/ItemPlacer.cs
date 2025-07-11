using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemPlacer : MonoBehaviour
{
    private List<Pool> inventory;
    public List<string> catalogue;
    public string itemName;
    public Vector3 pos;
    [Range(0, 360)]
    public float rot;
    public bool placeItem = false;
    public int index;

    // Start is called before the first frame update
    void Awake()
    {
        index = 0;
        inventory = new List<Pool>();
        catalogue = new List<string>();

        var info = new DirectoryInfo(Application.dataPath + "/Prefabs/Resources");
        var fileInfo = info.GetFiles("*.prefab");
        foreach (var file in fileInfo)
        {
            string fileName = file.Name.Replace(".prefab", "");
            catalogue.Add(fileName);
            inventory.Add(new Pool(Resources.Load<GameObject>(fileName), 10));
        }
    }


    // Update is called once per frame
    void Update()
    {
        UpdateEl();
        if (placeItem)
        {
            Place(pos, new Vector3(0,rot,0));
            placeItem = false;
        }
    }

    private void UpdateEl()
    {
        index = (catalogue.Contains(itemName) && inventory.Count > catalogue.IndexOf(itemName) ? catalogue.IndexOf(itemName) : catalogue.Count);
    }


    //Places an object from its respectie Pool at coordinates "position" with rotation "rotation"
    public void Place(Vector3 position, Vector3 rotation) {
        UpdateEl();
        if(catalogue.Contains(itemName)) {
            GameObject item = inventory[index].Emerge(RoomGenerator.roomRef);
            item.transform.localPosition = position;
            item.transform.localRotation = Quaternion.Euler(rotation);
        } else Debug.LogError("Invalid item name");
    }
}