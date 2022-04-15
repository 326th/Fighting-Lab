using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorDetection : MonoBehaviour
{
    public Transform currentCharacter;
    private void Update()
    {
        IsPointerOverUIObject();
    }
    public void IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0)
        {
            SetCurrentCharacter(results[0].gameObject.transform.parent.transform);
            print(results[0].gameObject.transform.parent.name);
        }
        //return results.Count > 0;
    }
    public void SetCurrentCharacter(Transform t)
    {
        currentCharacter = t;
    }
}

