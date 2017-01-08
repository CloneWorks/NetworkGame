using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shake : MonoBehaviour {

    //public Vector3 pointB;
    private Vector3 pointA;
    private Vector3 pointB;

    void Awake()
    {
        pointA = transform.FindChild("startPoint").position;

        pointB = transform.FindChild("patrollPoint").position;
    }

    void OnDrawGizmos()
    {
        //if (pointA != null && pointB != null)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawSphere(pointA, 1);
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(pointB, 1);
        //}
    }

    IEnumerator Start()
    {
        while (true)
        {
            yield return StartCoroutine(MoveObject(pointA, pointB, 3.0f)); //move to pos
            yield return StartCoroutine(MoveObject(pointB, pointA, 3.0f)); //move back
        }
    }

    IEnumerator MoveObject(Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localPosition = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }
}
