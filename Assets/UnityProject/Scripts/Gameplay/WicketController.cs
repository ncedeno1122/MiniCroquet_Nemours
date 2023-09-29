using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class WicketController : MonoBehaviour
{
    [SerializeField] private bool m_IsActive = true;
    public bool IsActive => m_IsActive;
    
    public int TrueWicketId; // Allows the MatchManager to sort the instances of WicketControllers

    [SerializeField]
    private BoxCollider m_Collider;

    [SerializeField] private List<BallController> m_BallsFromSouth = new();
    [SerializeField] private List<BallController> m_BallsFromNorth = new();

    //public static UnityEvent<WicketController, BallController, bool> OnWicketControllerCleared;
    public static event Action<WicketController, BallController, bool> OnWicketCleared = delegate {  };

    private void Awake()
    {
        m_Collider = GetComponentInChildren<BoxCollider>();
    }

    private void OnValidate()
    {
        // Set Name according to TrueWicketId
        if (!gameObject.name.Equals($"Wicket_{TrueWicketId.ToString()}"))
        {
            gameObject.name = $"Wicket_{TrueWicketId.ToString()}";
        }
    }

    // + + + + | Functions | + + + + 

    public bool IsPointInFrontOfCollider(Vector3 point)
    {
        return m_Collider.ClosestPointOnBounds(point).z >= m_Collider.transform.position.z;
    }
    
    // + + + + | Collision Handling | + + + +

    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsActive) return;
        
        // If a ball enters,
        if (other.gameObject.CompareTag(TagManager.BallTag))
        {
            // Get BallController
            BallController bc = other.gameObject.GetComponent<BallController>();
            
            Debug.Log($"Closest Point is {m_Collider.ClosestPointOnBounds(other.transform.position)}");
            
            if (IsPointInFrontOfCollider(bc.transform.position))
            {
                // From North
                Debug.Log($"Added Ball ({bc.name}) to FromNorth list!");
                m_BallsFromNorth.Add(bc);
            }
            else if (!IsPointInFrontOfCollider(bc.transform.position))
            {
                // From South
                Debug.Log($"Added Ball ({bc.name}) to FromSouth list!");
                m_BallsFromSouth.Add(bc);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_IsActive) return;

        // If a ball exits,
        if (other.gameObject.CompareTag(TagManager.BallTag))
        {
            // Get BallController
            BallController bc = other.gameObject.GetComponent<BallController>();
            
            if (IsPointInFrontOfCollider(bc.transform.position)) // Exits towards Front
            {
                if (m_BallsFromSouth.Contains(bc))
                {
                    m_BallsFromSouth.Remove(bc);
                    //OnWicketControllerCleared?.Invoke(this, bc, false);
                    //OnWicketCleared.Invoke(this, bc, false);
                    Debug.Log($"Trying to send {this}, {bc}, and {false}!");
                    OnWicketCleared(this, bc, false);
                    Debug.Log($"Wicket True#{TrueWicketId} was cleared from Back to Front!");
                }

                m_BallsFromNorth.Remove(bc);
                m_BallsFromSouth.Remove(bc);
            }
            else if (!IsPointInFrontOfCollider(bc.transform.position)) // Exits towards Back
            {
                if (m_BallsFromNorth.Contains(bc))
                {
                    m_BallsFromNorth.Remove(bc);
                    //OnWicketControllerCleared?.Invoke(this, bc, true);
                    //OnWicketCleared.Invoke(this, bc, true);
                    Debug.Log($"Trying to send {this}, {bc}, and {true}!");
                    OnWicketCleared(this, bc, true);
                    Debug.Log($"Wicket True#{TrueWicketId} was cleared from Front to Back!");
                }
                
                m_BallsFromNorth.Remove(bc);
                m_BallsFromSouth.Remove(bc);
            }
        }
    }

    // + + + + | Gizmos | + + + + 

    public void OnDrawGizmos()
    {
        if (m_IsActive)
        {
            BoxCollider collider = (m_Collider) ? m_Collider : GetComponentInChildren<BoxCollider>();
            
            // North
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(collider.transform.position + collider.center, collider.transform.position + Vector3.forward);
            
            if (m_BallsFromNorth.Count > 0)
            {
                foreach (BallController bc in m_BallsFromNorth)
                {
                    Gizmos.DrawLine(collider.transform.position, bc.transform.position);
                }
            }
            
            // South
            Gizmos.color = Color.red;
            Gizmos.DrawLine(collider.transform.position + collider.center, collider.transform.position + Vector3.back);
            
            if (m_BallsFromSouth.Count > 0)
            {
                foreach (BallController bc in m_BallsFromSouth)
                {
                    Gizmos.DrawLine(collider.transform.position, bc.transform.position);
                }
            }
        }
        
    }
}

public class WicketGate
{
    public int WicketGateIndex;
    public bool ReceivesFromFront;
    public WicketController CorrespondingController;

    public WicketGate(int gateIndex, bool receivesFromFront, WicketController correspondingController)
    {
        WicketGateIndex = gateIndex;
        ReceivesFromFront = receivesFromFront;
        CorrespondingController = correspondingController;
    }

    public override string ToString()
    {
        return $"WicketGate #{WicketGateIndex} (True Wicket #{CorrespondingController.TrueWicketId}), approaches from the {(ReceivesFromFront ? "North" : "South")}";
    }
}