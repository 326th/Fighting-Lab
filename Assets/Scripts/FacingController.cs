using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingController : MonoBehaviour
{
    public GameObject character1;
    public GameObject character2;

    public bool IsFacingRight(GameObject character)
    {
        var faceRight = character1.transform.position.x <= character2.transform.position.x;
        if( character == character1)
        {
            return faceRight;
        }
        return !faceRight;
    }
}
