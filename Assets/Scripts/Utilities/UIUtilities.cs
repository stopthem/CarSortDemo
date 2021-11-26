using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIUtilities
{
    public static bool IsPointerOverUIObject(Transform rootTransform = null)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (rootTransform)
        {
            var childList = rootTransform.GetComponentsInChildren<RectTransform>().ToList();
            results.RemoveAll(x => !childList.Contains(x.gameObject.transform));
        }
        return results.Count > 0;
    }
}
