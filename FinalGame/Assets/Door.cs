using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;
    //[SerializeField]
    //private bool IsRotatingDoor = true;
    [SerializeField]
    private float Speed = 1f;
    //[Header("Rotation Configs")]
    //[SerializeField]
    //private float RotationAmount = 90f;
    //[SerializeField]
    //private float ForwardDirection = 0;
    [Header("Sliding Configs")]
    [SerializeField]
    private Vector3 SlideDirection = Vector3.back;
    [SerializeField]
    private float SlideAmount = 1.9f;

    //private Vector3 StartRotation;
    private Vector3 StartPosition;
    private Vector3 Forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        //StartRotation = transform.rotation.eulerAngles;
        // Since "Forward" actually is pointing into the door frame, choose a direction to think about as "forward" 
        Forward = transform.right;
        StartPosition = transform.position;
    }

    //public void Open(Vector3 UserPosition)
    public void Open()
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            //if (IsRotatingDoor)
            //{
            //    float dot = Vector3.Dot(Forward, (UserPosition - transform.position).normalized);
            //    Debug.Log($"Dot: {dot.ToString("N3")}");
            //    AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            //}
            //else
            //{
                AnimationCoroutine = StartCoroutine(DoSlidingOpen());
            //}
        }
    }


    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = StartPosition + SlideAmount * SlideDirection;
        Vector3 startPosition = transform.position;

        float time = 0;
        IsOpen = true;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    //public void Close()
    //{
    //    if (IsOpen)
    //    {
    //        if (AnimationCoroutine != null)
    //        {
    //            StopCoroutine(AnimationCoroutine);
    //        }
            
    //        AnimationCoroutine = StartCoroutine(DoSlidingClose());
    //    }
    //}

    //private IEnumerator DoSlidingClose()
    //{
    //    Vector3 endPosition = StartPosition;
    //    Vector3 startPosition = transform.position;
    //    float time = 0;

    //    IsOpen = false;

    //    while (time < 1)
    //    {
    //        transform.position = Vector3.Lerp(startPosition, endPosition, time);
    //        yield return null;
    //        time += Time.deltaTime * Speed;
    //    }
    //}
}