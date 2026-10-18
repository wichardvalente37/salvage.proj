using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtMouse : MonoBehaviour
{
    Vector3 mousepos;
   public Camera main;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
       
      
        Ray ray= main.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(Vector3.forward, new Vector3(0f,0f,transform.position.z));

        if(plane.Raycast(ray,out float distance))
        {
            Vector3 mouseworld = ray.GetPoint(distance);
            Vector2 direction = mouseworld - transform.position;
            mouseworld.z = transform.position.z;
            mouseworld.Normalize();
 float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-90f);
Debug.DrawLine(transform.position, mouseworld, Color.red);
        }

        
       
       
    }
}
