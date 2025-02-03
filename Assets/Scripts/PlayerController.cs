using NUnit.Framework;
using UnityEngine;

public class PlayerController : MonoBehaviour
{  
    private bool IsSpinning;   
    void Start(){
        IsSpinning = true;
    }
    void Update(){
        if (IsSpinning){
            transform.Rotate(0, 0, 90);
        }
    }

}