using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class that represents a pool of GameObjects (prefabs), it is comprised of a Stack, which represents the pool itself, and an int,
/// which represents the size of the pool
/// </summary>
public class Pool
{
    private Stack<GameObject> pool;
    private readonly int n;
    public int remainingElements;

    /// <summary>
    /// basic constructor, takes as parameters tho GameObject the pool will contain and the size of the pool
    /// </summary>
    /// <param name="thing">Prefab to be stored in the pool</param>
    /// <param name="n">Size of the pool</param>
    public Pool(GameObject thing, int n)
    {
        remainingElements = n;
        pool = new Stack<GameObject>();
        for (int i = 0; i < n; i++)
        {
            GameObject poolElement = Object.Instantiate(thing, new Vector3(0, -1, 0), Quaternion.identity);
            poolElement.SetActive(false);
            pool.Push(poolElement);
        }

        this.n = n;
    }

    /// <summary>
    /// takes out an instance of the pool's object and makes it a child of the GameObject parent
    /// </summary>
    /// <param name="parent">The object to be the parent of the pool object</param>
    public GameObject Emerge(GameObject parent)
    {
        GameObject thing = pool.Pop();
        thing.SetActive(true);
        if(parent != null)  thing.transform.parent = parent.transform;
        thing.transform.localRotation = Quaternion.Euler(0, 0, 0);
        remainingElements--;
        return thing;
    }

    /// <summary>
    /// activates an object from the pool
    /// </summary>
    /// <returns>the object from the pool</returns>
    public GameObject Emerge()
    {
        GameObject thing = pool.Pop();
        thing.SetActive(true);
        thing.transform.localRotation = Quaternion.Euler(0, 0, 0);
        remainingElements--;
        return thing;
    }

    /// <summary>
    /// returns the GameObject child to the pool
    /// </summary>
    /// <param name="child">The object to be returned to the pool</param>
    public void Submerge(GameObject child)
    {
        remainingElements++;
        child.transform.parent = null;
        child.SetActive(false);
        pool.Push(child);
    }
}
