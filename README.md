# Ray Reflection (and it's problems) in Unity

The project is a simple 2D scene that consists of a square that creates a ray that reflects on the walls
and the walls that reflect the ray. The green ray represents the current ray, the red line represents the normal of the intersected surface, and the blue ray represents the reflection of the current array.

![](https://github.com/fsaltunyuva/RayReflection/blob/main/demo.gif)

> [!WARNING]
> * Use the scene view to see the ray reflections because they are drawn with [Debug.DrawRay](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Debug.DrawRay.html).
> * Use the pause button in Unity Editor before playing and use the step buttons to see the reflections step by step.
> * Walls must have a collider and "Reflectable" tag to reflect the ray.

## Problem
In a basic approach like this:

```csharp
using UnityEngine;

public class ReflectableRaycaster : MonoBehaviour
{
    Vector2 direction = new Vector2(10, -10); // Initial direction of the ray
    RaycastHit2D hit; 
    Vector2 origin; 

    void Start()
    {
        origin = transform.position; // Start from the center of the square
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
            origin = hit.point;
        }
    }
```

After the second reflection, the ray starts to behave weirdly. 
It starts to reflect in a way that it hits an invisible wall and reflects to the opposite direction again and again.

![image](https://github.com/user-attachments/assets/06269088-0868-4d66-988b-080a95ac6801)


This problem occurs because of the floating point precision and the tolerance of the Physics2D.Raycast method. 
When we hit the collider, the hit point is positioned slightly inside the collider.
Therefore, when the second ray is cast from the hit point (which is positioned inside the wall), it hits the same collider again.

## Solutions
There are two solutions to this problem.

### Solution 1
As stated in [this StackOverflow answer](https://stackoverflow.com/a/38193001/19469259), the setting 
**Edit->Project Settings->Physics 2D->Queries Start In Colliders** can be unchecked to prevent the problem.
By doing this, the ray will not hit the collider that it starts from. So, it does not hit the same collider again.
More information about this setting can be found in [this documentation page of Unity](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics2D-queriesStartInColliders.html).

### Solution 2
To prevent the ray from hitting the same collider again, we can add an offset to the origin slightly in the reflected direction.

```csharp
using UnityEngine;

public class ReflectableRaycaster : MonoBehaviour
{
    Vector2 direction = new Vector2(10, -10); // Initial direction of the ray
    RaycastHit2D hit;
    Vector2 origin;

    void Start()
    {
        origin = transform.position; // Start from the center of the square
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
            // Offset the origin slightly in the reflected direction to avoid self-collision
            origin = hit.point + direction.normalized * 0.01f;
        }
    }
}
```
