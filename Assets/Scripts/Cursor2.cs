using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Cursor2 : MonoBehaviour
{
    public List<GameObject> sampleBook;
    public GameObject cursor;
    private Vector3 cursorPos;
    private Vector3 rot;
    private ItemPlacer itm;
    public Vector3 mousePos;
    public Camera hmdCamera;  // Asigna la cámara del HMD en el Inspector
    public float rayLength = 10f;

    void Start()
    {
        itm = GameObject.Find("ItemPlacer").GetComponent<ItemPlacer>();

        foreach (string file in itm.catalogue)
        {
            sampleBook.Add(Instantiate(Resources.Load<GameObject>(file)));
            sampleBook[^1].SetActive(false);
            ignoreRaycast(sampleBook[^1]);
        }

        GameObject genericCursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        genericCursor.name = "GenericCursor";
        SphereCollider collider = genericCursor.GetComponent<SphereCollider>();
        Destroy(collider);
        sampleBook.Add(genericCursor);

        cursor = sampleBook[^1];
    }

    void Update()
    {
        SelectElement();
        cursor.transform.localPosition = cursorPos;
        cursor.transform.eulerAngles = rot;

        PointLooked();

        if (Input.GetMouseButtonDown(0))
            itm.Place(cursorPos, rot);
    }

    private void SelectElement()
    {
        cursor.SetActive(false);
        cursor = sampleBook[itm.index];
        cursor.SetActive(true);
    }

    private void PointLooked()
    {
        Ray ray = new Ray(hmdCamera.transform.position, hmdCamera.transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, rayLength);
        Debug.Log(hit.transform.gameObject.name);
        mousePos = (hit.transform == null ? new Vector3(0, -10, 0) : hit.point);

        if (Input.GetMouseButton(1))
            rot = new Vector3(rot.x, -Input.mousePosition.x, rot.z);

        if (!Input.GetMouseButton(1)) cursorPos = mousePos;
    }

    private void ignoreRaycast(GameObject item)
    {
        item.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        foreach (Transform child in item.transform)
        {
            ignoreRaycast(child.gameObject);
        }
    }
}