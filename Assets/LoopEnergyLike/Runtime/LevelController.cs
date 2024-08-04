using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CircuitController[] circuitControllers;
    private int circuitsClosed;
    
    void Start()
    {
        for (int i = 0; i < circuitControllers.Length; i++)
        {
            circuitControllers[i].OnClose += Circuits_OnClose;
            circuitControllers[i].OnOpen += controller => circuitsClosed--;
        }        
    }

    private void Circuits_OnClose(CircuitController obj)
    {
        circuitsClosed++;

        if (circuitsClosed == circuitControllers.Length)
        {
            print("Level finished");
        }
    }
}
