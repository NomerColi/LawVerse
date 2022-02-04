using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    //public static IEnumerator TweenRoutine(

    public static Coroutine Move(this Camera target, Vector3 pos, float time)
    {
        var mb = target.GetComponent<MonoBehaviour>();

        if (mb == null)
            mb = target.gameObject.AddComponent<MonoBehaviour>();

        return mb.StartCoroutine(MoveRoutine(mb, pos, time));
    }

    public static Coroutine Move(this MonoBehaviour target, Vector3 pos, float time)
    {
        return target.StartCoroutine(MoveRoutine(target, pos, time));
    }

    public static IEnumerator MoveRoutine(this MonoBehaviour target, Vector3 pos, float time)
    {
        Debug.Log(target.gameObject.name + " Move to pos : " + pos + " time : " + time);

        var startPos = target.transform.position;

        float f = 0;
        while (f < 1f)
        {
            f += Time.deltaTime / time;

            target.transform.position = Vector3.Lerp(startPos, pos, f);

            yield return null;
        }
    }

    public static Coroutine Rotate(this Camera target, Vector3 rot, float time)
    {
        var mb = target.GetComponent<MonoBehaviour>();

        if (mb == null)
            mb = target.gameObject.AddComponent<MonoBehaviour>();

        return mb.StartCoroutine(RotateRoutine(mb, rot, time));
    }

    public static Coroutine Rotate(this MonoBehaviour target, Vector3 rot, float time)
    {
        return target.StartCoroutine(MoveRoutine(target, rot, time));
    }

    public static IEnumerator RotateRoutine(this MonoBehaviour target, Vector3 rot, float time)
    {
        var startRot = target.transform.localEulerAngles;

        float f = 0;
        while (f < 1f)
        {
            f += Time.deltaTime / time;

            target.transform.localEulerAngles = Vector3.Lerp(startRot, rot, f);

            yield return null;
        }
    }
}
