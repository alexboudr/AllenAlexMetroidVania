using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private Door Door;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (!Door.IsOpen)
            {
                // Get the contact point of the collision
                ContactPoint contact = other.contacts[0];

                // Get the normal of the contact surface
                Vector3 normal = contact.normal;

                // Get the direction from the bullet to the door (to determine the side)
                Vector3 directionToDoor = other.transform.position - transform.position;

                Debug.Log("Normal: " + normal);
                Debug.Log("Direction to Door: " + directionToDoor);

                // If the normal vector is facing away from the door (based on the bullet's direction)
                if (Vector3.Dot(normal, directionToDoor) > 0)
                {

                    // NOTE-TO-SELF: FIX THIS STUPID ASS BUG

                    Debug.Log("Bullet hit the front of the door");
                }
                else
                {
                    Debug.Log("Bullet hit the back of the door");

                    Door.Open();
                }
            }
        }
    }



    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent<CharacterController>(out CharacterController controller))
    //    {
    //        if (!Door.IsOpen)
    //        {
    //            Door.Open(other.transform.position);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent<CharacterController>(out CharacterController controller))
    //    {
    //        if (Door.IsOpen)
    //        {
    //            Door.Close();
    //        }
    //    }
    //}
}