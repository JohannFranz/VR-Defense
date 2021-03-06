﻿using UnityEngine;

public static class Constants
{
    public const int DeckSize = 10;
    public const int HandSize = 3;

    public const int RotationYOffset = -90;

    public const string MercenaryTag = "Merc";
    public const string MinionTag = "Minion";
    public const string PlacementTag = "Placement";
    public const string PlacementAreaTag = "PlacementArea";
    public const string FactoryTag = "AgentFactory";
    public const string CameraTag = "MainCamera";
    public const string CardTag = "Card";
    public const string HandTag = "Hand";
    public const string DeckTag = "Deck";
    public const string DeckManagerTag = "DeckManager";
    public const string PlaceholderTag = "Placeholder";
    public const string HealthControllerTag = "HealthController";
    public const string SensorTag = "Sensor";
    public const string WeaponTag = "Weapon";
    public const string FloorTag = "Floor";
    public const string TeamTag = "Team";
    public const string StateMachineTag = "StateMachine";
    public const string RendererTag = "Renderer";
    public const string FormationTag = "Formation";
    public const string AITag = "AI";
    public const string SteeringTag = "Steering";
    public const string LifeTextTag = "LifeText";
    public const string DamageTextTag = "DamageText";
    public const string RangeTextTag = "RangeText";
    public const string NameTextTag = "NameText";
    public const string CurrentLivesTextTag = "CurrentLivesText";
    public const string WaveLevelTextTag = "WaveLevelText";
    public const string StartButtonTag = "StartButton";
    public const string StartButtonTextTag = "StartButtonText";
    public const string CanvasTag = "Canvas";
    public const string VR_Tag = "VR";
    public const string VR_PointerTag = "VR_Pointer";

    public const string HealthController = "HealthController";
    public const string DeckManager = "DeckManager";

    public static readonly LayerMask GroundLayer = 1 << 8;
    public static readonly LayerMask UILayer = 1 << 9;


    public const string TriggerStart = "Start";
    public const string Main_Menu = "MainMenu";

    public const string World_Western = "World_Western";
    public const string World_Vineyard = "World_Vineyard";
    public const string World_Medieval = "World_Medieval";
    public const string World_Ice = "World_Ice_2";
    public const string World_Skull_Island = "World_Skull_Island";

    public static readonly Vector3 RotationAroundYAxis = new Vector3(0, 1, 0);
    public static readonly Vector3 Origin = Vector3.zero;
    public static readonly Vector3 NullVector3 = new Vector3(-9999, -9999, -9999);
    public static readonly Vector3 VR_CanvasScale = new Vector3(0.003f, 0.003f, 0.003f);
    public static readonly Vector3 VR_CanvasMainMenuPosition = new Vector3(0, 2, 3);
    public static readonly Vector3 VR_CanvasGamePosition = new Vector3(-0.055f, 0.2f, 1.8f);
    public static readonly Vector3 VR_CanvasGameRotation = new Vector3(66.29f, 0, 0);
    public static readonly int VR_CanvasWidth = 960;
    public static readonly int VR_CanvasHeight = 250;
    public static readonly float VR_PointerLength = 100.0f;

    public static readonly Color VR_PlacementUnselectedColor = new Color32(45, 66, 166, 255);
    public static readonly Color VR_PlacementSelectedColor = new Color32(255, 0, 0, 255);

    public const float TargetChooseDotValue = 0.8f;
    public const float AttackRangeOffset = 0.9f;

    public const int scrollButtonHeight = 400;
    public const int countWorlds = 5;
}
