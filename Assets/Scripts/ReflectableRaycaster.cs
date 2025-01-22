using UnityEngine;

public class ReflectableRaycaster : MonoBehaviour
{
    Vector2 direction = new Vector2(10, -10); // Initial direction
    RaycastHit2D hit;
    Vector2 origin;

    void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        hit = Physics2D.Raycast(origin, direction);

        Debug.DrawRay(origin, direction, Color.green); 

        if (hit && hit.collider.gameObject.CompareTag("Reflectable")) 
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red); // Draw the normal of the hit point
            
            direction = Vector2.Reflect(direction, hit.normal);

            Debug.DrawRay(hit.point, direction, Color.blue); 

            // Offset the origin slightly in the reflected direction to avoid self-collision (more info in the README.md)
            origin = hit.point + direction.normalized * 0.01f;
        }
    }
}

// Old Code which causes the ray to stuck after second reflection (works when the setting Edit->Project Settings->Physics 2D->Queries Start In Colliders is NOT checked.)
// Solution's source: https://stackoverflow.com/questions/38191659/unity-physics2d-raycast-hits-itself
// This problem occurs because of the floating point precision and the tolerance of the Physics2D.Raycast method.
// When we hit the collider, the hit point is positioned slightly inside the collider.
// Therefore, when the second ray is cast from the hit point (which is positioned inside the wall), it hits the same collider again.
// Additional information: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics2D-queriesStartInColliders.html
// More explanation in README.md

// using UnityEngine;
//
// public class ReflectableRaycaster : MonoBehaviour
// {
//     Vector2 direction = new Vector2(10, -10); // Initial direction
//     RaycastHit2D hit; 
//     Vector2 origin; 
//
//     void Start()
//     {
//         origin = transform.position;
//     }
//
//     void Update()
//     {
//         hit = Physics2D.Raycast(origin, direction); 
//         Debug.DrawRay(origin, direction, Color.green);
//
//         if (hit && hit.collider.gameObject.CompareTag("Reflectable")) 
//         {
//             Debug.DrawRay(hit.point, hit.normal, Color.red); // Draw the normal of the hit point
//             direction = Vector2.Reflect(direction, hit.normal);
//             Debug.DrawRay(hit.point, direction, Color.blue);
//             origin = hit.point;
//         }
//     }
// }