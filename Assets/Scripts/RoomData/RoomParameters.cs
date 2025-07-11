using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu()]
public class RoomParameters : ScriptableObject
{
    public float width = 5f;
    public float length = 5f;
    public float height = 2f;
    
    public bool hasRoof = false;
    //public float doorHeight = 1.5f;
    //public float doorWidth = 1;
    
    public Vector2 doorMeasures = Vector2.one;

    //Current and desired number of doors
    public int N_target = 0;
    public int S_target = 0;
    public int E_target = 0;
    public int W_target = 0;
    
    public Color baseColor = Color.white;
    
    protected void OnValidate()
    {
        if(width < 0) width = 0.001f;
        if(length < 0) length = 0.001f;
        if(height < 0) height = 0.001f;
        
        if(doorMeasures.x < 0) doorMeasures.x = 0.001f;
        if(doorMeasures.y < 0) doorMeasures.y = 0.001f;
        
        if(N_target < 0) N_target = 0;
        if(S_target < 0) S_target = 0;
        if(E_target < 0) E_target = 0;
        if(W_target < 0) W_target = 0;
    }
}
