using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    // Players
    
    // Wickets
    public List<WicketController> Wickets;
    public WicketController[] SortedWicketGates;

    public List<WicketGate> WicketGateOrder;
    public WicketGate CurrentWicketGate;

    private void Awake()
    {
        //
    }

    private void Start()
    {
        // Subscribe
        //WicketController.OnWicketControllerCleared.AddListener(HandleWicketControllerCleared);
        
        // Get Wickets
        Wickets = FindObjectsOfType<WicketController>().ToList();
        SortedWicketGates = Wickets.OrderBy(wicket => wicket.TrueWicketId).ToArray();

        // Set Wicket Order
        WicketGateOrder = new List<WicketGate>() // TODO: Save as ScriptableObject?
        {
            new WicketGate(1, false, SortedWicketGates[0]),
            new WicketGate(2, false, SortedWicketGates[1]),
            new WicketGate(3, true, SortedWicketGates[2]),
            new WicketGate(4, true, SortedWicketGates[3]),
            new WicketGate(5, false, SortedWicketGates[4]),
            new WicketGate(6, false, SortedWicketGates[5]),
            new WicketGate(7, true, SortedWicketGates[1]),
            new WicketGate(8, true, SortedWicketGates[0]),
            new WicketGate(9, false, SortedWicketGates[3]),
            new WicketGate(10, false, SortedWicketGates[2]),
            new WicketGate(11, true, SortedWicketGates[5]),
            new WicketGate(12, true, SortedWicketGates[4]),
            new WicketGate(13, true, SortedWicketGates[2])
        };
        CurrentWicketGate = WicketGateOrder[0];
    }

    private void OnEnable()
    {
        WicketController.OnWicketCleared += HandleWicketControllerCleared;
    }

    private void OnDisable()
    {
        //WicketController.OnWicketControllerCleared.RemoveListener(HandleWicketControllerCleared);
        WicketController.OnWicketCleared -= HandleWicketControllerCleared;
    }
    
    // + + + + | Functions | + + + +

    private void HandleWicketControllerCleared(WicketController wc, BallController bc, bool fromFront)
    {
        // Is this the current WicketController?
        if (CurrentWicketGate.CorrespondingController.TrueWicketId == wc.TrueWicketId)
        {
            // Did the Ball approach from the proper side?
            if (CurrentWicketGate.ReceivesFromFront == fromFront)
            {
                // TODO: Score! using the proper Player / Team from the BallController
                Debug.Log($"Cleared WicketGate {CurrentWicketGate.WicketGateIndex} ({CurrentWicketGate}). Advancing to WicketGate #{CurrentWicketGate.WicketGateIndex + 1}!");
                CurrentWicketGate = WicketGateOrder[CurrentWicketGate.WicketGateIndex]; // Goes to next WicketGate unsafely TODO: CHANGE THIS
            }
        }
        
        
    }
}
