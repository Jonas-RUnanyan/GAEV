using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//This script controls the aspects of the cursor, such as its position, rotation, and which item is currently selected
//and thus shown as the cursor

public class Cursor : MonoBehaviour
{
    public List<GameObject> sampleBook;
    private GameObject cursor;
    private Vector3 cursorPos;
    private Vector3 rot;
    private ItemPlacer itm;
    public Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the script ItemPlacer
        itm = GameObject.Find("ItemPlacer").GetComponent<ItemPlacer>();

        //Loads each of the GameObjects from ItemPlacer as a placeholder cursor
        Debug.Log(itm.catalogue.Count);
        foreach (string file in itm.catalogue)
        {
            sampleBook.Add(Instantiate(Resources.Load<GameObject>(file)));
            sampleBook[^1].SetActive(false);
            //MeshCollider meshCollider = sampleBook[^1].GetComponent<MeshCollider>();
            //Destroy(meshCollider);
            ignoreRaycast(sampleBook[^1]);
        }

        //Creates a generic cursor for when there is not a valid object selected and adds it to the list of cursors
        GameObject genericCursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        genericCursor.name = "GenericCursor";
        SphereCollider collider = genericCursor.GetComponent<SphereCollider>();
        Destroy(collider);
        sampleBook.Add(genericCursor);

        cursor = sampleBook[^1];
    }

    // Update is called once per frame
    void Update()
    {
        SelectElement();
        cursor.transform.localPosition = cursorPos;
        cursor.transform.eulerAngles = rot;

        PointLooked();

        if (Input.GetMouseButtonDown(0))
            itm.Place(cursorPos, rot);
    }

    //Selects what element acts as cursor based on the String inputted on ItemPlacer
    private void SelectElement()
    {
        cursor.SetActive(false);
        cursor = sampleBook[itm.index];
        cursor.SetActive(true);
    }

    //Updates position and rotation of cursor based on input from the mouse
    private void PointLooked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
        mousePos = (hit.transform == null ? new Vector3(0, -10, 0) : hit.point);
        //Rotation of cursor
        if (Input.GetMouseButton(1))
            rot = new Vector3(rot.x, -Input.mousePosition.x, rot.z);

        if (!Input.GetMouseButton(1)) cursorPos = mousePos;
    }

    //Moves the GameObject and all of its children to the Ignore Raycast layer
    private void ignoreRaycast(GameObject item)
    {
        item.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        foreach (Transform child in item.transform)
        {
            ignoreRaycast(child.gameObject);
        }
    }
}
