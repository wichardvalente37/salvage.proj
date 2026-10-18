using UnityEngine;

public class follow : MonoBehaviour
{
    private Transform ObjtoFollow;
    public string Objtag;
    void Start()
    {
        ObjtoFollow = GameObject.FindWithTag(Objtag).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(ObjtoFollow != null)
        {
  transform.position = ObjtoFollow.position;
        }
      
    }
}
