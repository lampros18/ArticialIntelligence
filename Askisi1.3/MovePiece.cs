using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : MonoBehaviour
{
    private Transform piece;
    private Vector3 lastPosition;
    private Vector3 newPosition;
    private float distanceX;
    private float distanceY;

    public Transform dummy;

    public LayerMask layerMask;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.CompareTag("ChessPiece"))
                {
                    piece = hit.transform;
                    lastPosition = Input.mousePosition;
                    foreach (Collider c in piece.GetComponents<Collider>())
                    {
                        c.enabled = false;
                    }
                    piece.SendMessage("IsKinematic", true);
                    piece.GetComponent<Rigidbody>().isKinematic = true;
                    piece.SendMessage("Follow", dummy);
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (piece != null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
                {
                    dummy.transform.position = new Vector3(hit.point.x, hit.point.y + 0.4f, hit.point.z);
                    if (piece.GetComponent<Piece>().isWhite)
                    {
                        piece.transform.eulerAngles = new Vector3(90, 180, 270);
                    }
                    else
                    {
                        piece.transform.eulerAngles = new Vector3(90, 0, 270);
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (piece != null)
            {
                foreach (Collider c in piece.GetComponents<Collider>())
                {
                    c.enabled = true;
                }
                piece.GetComponent<Rigidbody>().isKinematic = false;
                piece.SendMessage("StopFollow");
                piece = null;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.CompareTag("ChessPiece"))
                {
                    hit.transform.SendMessage("Capture");
                }
            }
        }
    }
}
