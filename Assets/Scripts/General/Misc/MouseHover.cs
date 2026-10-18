using UnityEngine;
using UnityEngine.EventSystems;
public class MouseHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
  
{
    public GameObject info;

    private void Start()
    {
        info.SetActive(false);
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        
 info.SetActive(true);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {   info.SetActive(false);
       
    }

   
}
