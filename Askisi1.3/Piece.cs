using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private bool follow = false;
    private Transform dummy;

    public Transform target;
    private float speed;

    public bool isWhite;
    public int value;

    public string codeName;

    public Transform blackCapturedPieces;
    public Transform whiteCapturedPieces;

    void Awake()
    {
     //   GetComponent<Rigidbody>().isKinematic = true;
    }

    void Start()
    {
        speed = 15f;
        blackCapturedPieces = GameObject.FindGameObjectWithTag("BlackCapturedPieces").transform;
        whiteCapturedPieces = GameObject.FindGameObjectWithTag("WhiteCapturedPieces").transform;
    }

    public void Follow(Transform dummy)
    {
        this.dummy = dummy;
        follow = true;
    }

    public void IsKinematic(bool isIt)
    {
        GetComponent<Rigidbody>().isKinematic = isIt;
    }

    public void StopFollow()
    {
        follow = false;
    }

    void Update()
    {
        if (follow)
        {
            transform.position = dummy.position;
        }
    }

    public void Capture()
    {
        Vector3 randPos = Vector3.zero;
        if (isWhite)
        {
            randPos = whiteCapturedPieces.position + new Vector3(Random.Range(-2.5f, 3f), 0f, Random.Range(-1f, 1f));
        }
        else
        {
            randPos = blackCapturedPieces.position + new Vector3(Random.Range(-2.5f, 3f), 0f, Random.Range(-1f, 1f));
        }
        //this.transform.position = randPos;
        StartCoroutine(MoveObject(this.transform, randPos, 1f));
    }

    IEnumerator MoveObject(Transform obj, Vector3 target, float overTime)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        foreach (Collider c in obj.GetComponents<Collider>()) //Disabling colliders so pawns dont collide when AI is moving them
        {
            c.enabled = false;
        }
        //target = new Vector3(target.x, target.y - 3f, target.z);
        float startTime = Time.time;
        while (Time.time < startTime + overTime/3f)
        {
            obj.position = Vector3.Lerp(obj.position, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        obj.position = target;
        foreach (Collider c in obj.GetComponents<Collider>())
        {
            c.enabled = true;
        }
        obj.GetComponent<Rigidbody>().isKinematic = false;
    }
}