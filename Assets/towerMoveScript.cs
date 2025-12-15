using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerMoveScript : MonoBehaviour
{
    
    public float moveSpeed = 5;
    private float deadZone = -10;
    public bool collisionMoveStop = true;

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionMoveStop == false)
        {
            return;
        }

        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x < deadZone)
        {
            Debug.Log("Tower Deleted");
            Destroy(gameObject);
        }
    }

  
}
