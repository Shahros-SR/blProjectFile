using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TweenTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Use this for initialization
    //void Start () {
    //    iTween.ScaleTo(gameObject, iTween.Hash("x",1,"y",1,"easeType", "easeOutElastic","delay",1,"NameValuedColor", "_ReflectColor"));
    //}

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (this.gameObject.GetComponent<Button>().interactable == true)
            iTween.ScaleTo(gameObject, iTween.Hash("x", 2, "y", 2, "easeType", "easeOutElastic"));
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "easeType", "easeOutElastic"));
    }

}
