using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private Vector3 Offset;

    private void Update()
    {
        if (Target != null)
            transform.position = Target.position + Offset;

        //transform.LookAt(Camera.main.transform, Vector3.up);
    }
}