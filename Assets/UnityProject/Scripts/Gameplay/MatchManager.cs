using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    // Players
    public int Team1Score;
    public int Team2Score;

    // BallControllers
    private int m_CurrBallIndex = 0; // Start on Blue
    public BallController CurrBallController { get => OrderedBallControllers[m_CurrBallIndex]; }
    public BallController[] OrderedBallControllers;

    // Wickets
    public List<WicketController> Wickets;
    public WicketController[] SortedWicketControllers;

    public List<WicketGate> WicketGateOrder;
    public WicketGate CurrentWicketGate;

    // Camera States
    public PlayerCameraController PCC;

    // UI
    public WicketGoalPanelController WGPC;
    public WicketIndicatorController WicketIndicator;
    public ScoreAnnouncementPanelController ScoreAnnouncement;

    private void Awake()
    {
        //
        OrderedBallControllers = new BallController[4]; // TODO: Make modular with var?

        // TEST
        Debug.Log($"Next Color from {BallColor.BLUE} is {GetNextColor(BallColor.BLUE)}");
        Debug.Log($"Next Color from {BallColor.RED} is {GetNextColor(BallColor.RED)}");
        Debug.Log($"Next Color from {BallColor.BLACK} is {GetNextColor(BallColor.BLACK)}");
        Debug.Log($"Next Color from {BallColor.YELLOW} is {GetNextColor(BallColor.YELLOW)}");
    }

    private void Start()
    {
        // Get BallControllers
        BallController[] ballControllerGOs = FindObjectsByType<BallController>(FindObjectsSortMode.None);
        foreach (BallController bc in ballControllerGOs)
        {
            OrderedBallControllers[(int)bc.BallColor] = bc;
        }
        
        // Get Wickets
        Wickets = FindObjectsOfType<WicketController>().ToList();
        SortedWicketControllers = Wickets.OrderBy(wicket => wicket.TrueWicketId).ToArray();

        // Set Wicket Order
        WicketGateOrder = new List<WicketGate>() // TODO: Save as ScriptableObject?
        {
            new WicketGate(1, false, SortedWicketControllers[0]),
            new WicketGate(2, false, SortedWicketControllers[1]),
            new WicketGate(3, true, SortedWicketControllers[2]),
            new WicketGate(4, true, SortedWicketControllers[3]),
            new WicketGate(5, false, SortedWicketControllers[4]),
            new WicketGate(6, false, SortedWicketControllers[5]),
            new WicketGate(7, true, SortedWicketControllers[1]),
            new WicketGate(8, true, SortedWicketControllers[0]),
            new WicketGate(9, false, SortedWicketControllers[3]),
            new WicketGate(10, false, SortedWicketControllers[2]),
            new WicketGate(11, true, SortedWicketControllers[5]),
            new WicketGate(12, true, SortedWicketControllers[4]),
            new WicketGate(13, true, SortedWicketControllers[2])
        };

        CurrentWicketGate = WicketGateOrder[0];

        // Initialize Wicket UI
        SetWicket(0);

        // Initialize Camera
        PCC.MatchManager = this; // TODO: Better ways for this DI...
        PCC.CamState = new ShotLineupState(PCC, OrderedBallControllers[0].transform);
    }

    private void OnEnable()
    {
        WicketController.OnWicketCleared += OnWicketControllerCleared;
    }

    private void OnDisable()
    {
        //WicketController.OnWicketControllerCleared.RemoveListener(HandleWicketControllerCleared);
        WicketController.OnWicketCleared -= OnWicketControllerCleared;
    }
    
    // + + + + | Functions | + + + +

    public void AdvanceBallController()
    {
        m_CurrBallIndex = (m_CurrBallIndex + 1) % 4;
    }

    private void OnWicketControllerCleared(WicketController wc, BallController bc, bool fromFront)
    {
        // Is this the current WicketController?
        if (CurrentWicketGate.CorrespondingController.TrueWicketId == wc.TrueWicketId)
        {
            // Did the Ball approach from the proper side?
            if (CurrentWicketGate.ReceivesFromFront == fromFront)
            {
                // TODO: Score! using the proper Player / Team from the BallController
                
                // Score
                if (((int) bc.BallColor) % 2 == 0)
                {
                    Team1Score++;
                }
                else
                {
                    Team2Score++;
                }

                // UI
                ScoreAnnouncement.TriggerAnimation();

                Debug.Log($"Ball {bc.name} cleared WicketGate {CurrentWicketGate.WicketGateIndex} ({CurrentWicketGate}). Advancing to WicketGate #{CurrentWicketGate.WicketGateIndex + 1}!");
                AdvanceWicket();
            }
        }   
    }

    private void AdvanceWicket()
    {
        SetWicket(CurrentWicketGate.WicketGateIndex); // TODO: Fix this unsafe FOOLISHNESS
    }

    private void SetWicket(int index)
    {
        CurrentWicketGate = WicketGateOrder[index];

        // UI
        //WGPC.Text.text = $"Wickets Cleared: ({CurrentWicketGate.WicketGateIndex}/{WicketGateOrder.Count})\n" +
        //    $"Team 1: {Team1Score} | Team 2: {Team2Score}";
        WGPC.UpdateGoalPanelUI(Team1Score, Team2Score, CurrentWicketGate.WicketGateIndex, WicketGateOrder.Count);
        WicketIndicator.SetPosition(CurrentWicketGate);
    }

    public static BallColor GetNextColor(BallColor color)
    {
        // TODO: COULD have longer enum but use Modulus to cap it at some point
        //Debug.Log($"Color {color}, index {(int)color}");
        return (BallColor)(((int)color + 1) % 4);
    }

    public BallController GetBallControllerFromColor(BallColor color)
    {
        return OrderedBallControllers[(int)color];
    }

    public BallController GetNextBallControllerFromColor(BallColor color)
    {
        return GetBallControllerFromColor(GetNextColor(color));
    }

    public BallController GetNextBallController()
    {
        return GetNextBallControllerFromColor(CurrBallController.BallColor);
    }
}
