﻿using UnityEngine;

namespace SmolHatchling.Components;

public class ScaleController : MonoBehaviour
{
    public static ScaleController Instance { get; private set; }
    public Vector3 TargetScale { get; private set; }
    public Vector3 CurrentScale { get; private set; }

    private PlayerBody _playerBody;
    private PlayerCharacterController _characterController;
    private PlayerCameraController _cameraController;
    private PlayerScreamingController _npcPlayer;
    private PlayerCloneController _cloneController;
    private ShipCockpitController _cockpitController;
    private ShipLogController _logController;
    private OWAudioSource[] _audioSources;
    private CapsuleCollider _playerCollider;
    private CapsuleCollider _detectorCollider;
    private CapsuleShape _detectorShape;
    private GameObject _marshmallowStick;
    private GameObject _playerThruster;
    private Vector3 _scaleVelocity;
    private Vector3 _colliderScale;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerBody = GetComponent<PlayerBody>();
        _characterController = GetComponent<PlayerCharacterController>();
        _cameraController = Locator.GetPlayerCameraController();
        _npcPlayer = FindObjectOfType<PlayerScreamingController>();
        _cloneController = FindObjectOfType<PlayerCloneController>();
        _cockpitController = FindObjectOfType<ShipCockpitController>();
        _logController = FindObjectOfType<ShipLogController>();
        _playerCollider = GetComponent<CapsuleCollider>();
        _detectorCollider = transform.Find("PlayerDetector").GetComponent<CapsuleCollider>();
        _detectorShape = GetComponentInChildren<CapsuleShape>();
        _marshmallowStick = transform.Find("RoastingSystem").transform.Find("Stick_Root").gameObject;
        _playerThruster = transform.Find("PlayerVFX").gameObject;

        PlayerAudioController audioController = GetComponentInChildren<PlayerAudioController>();
        PlayerBreathingAudio breathingAudio = GetComponentInChildren<PlayerBreathingAudio>();
        _audioSources = new OWAudioSource[]
        {
            audioController._oneShotSleepingAtCampfireSource,
            audioController._oneShotSource,
            breathingAudio._asphyxiationSource,
            breathingAudio._breathingLowOxygenSource,
            breathingAudio._breathingSource,
            breathingAudio._drowningSource
        };

        Config.OnConfigure += UpdateTargetScale;
        UpdateTargetScale();

        CurrentScale = TargetScale;
        UpdatePlayerScale();
    }

    private void OnDestroy()
    {
        Config.OnConfigure -= UpdateTargetScale;
    }

    private void Update()
    {
        if (CurrentScale != TargetScale)
        {
            CurrentScale = Vector3.SmoothDamp(CurrentScale, TargetScale, ref _scaleVelocity, 0.25f);
            UpdatePlayerScale();
        }
    }

    private void UpdateTargetScale()
    {
        TargetScale = new Vector3(Config.PlayerScale * Config.PlayerWidthFactor, Config.PlayerScale, Config.PlayerScale * Config.PlayerWidthFactor);
        _cameraController._playerCamera.nearClipPlane = 0.1f * Mathf.Min(TargetScale.x, TargetScale.y, 1f);
        if (ModMain.Instance.HikersModAPI != null)
        {
            ModMain.Instance.HikersModAPI.UpdateConfig();
        }
        else
        {
            if (Config.UseScaledPlayerAttributes)
            {
                _characterController._runSpeed = 6f * TargetScale.x;
                _characterController._strafeSpeed = 4f * TargetScale.x;
                _characterController._walkSpeed = 3f * TargetScale.x;
                _characterController._airSpeed = 3f * TargetScale.x;
                _characterController._acceleration = 0.5f * TargetScale.x;
                _characterController._airAcceleration = 0.09f * TargetScale.x;
                _characterController._minJumpSpeed = 3f * Mathf.Sqrt(TargetScale.y);
                _characterController._maxJumpSpeed = 7f * Mathf.Sqrt(TargetScale.y);
            }
            else
            {
                _characterController._runSpeed = 6f;
                _characterController._strafeSpeed = 4f;
                _characterController._walkSpeed = 3f;
                _characterController._airSpeed = 3f;
                _characterController._acceleration = 0.5f;
                _characterController._airAcceleration = 0.09f;
                _characterController._minJumpSpeed = 3f;
                _characterController._maxJumpSpeed = 7f;
            }
        }
    }

    private void UpdatePlayerScale()
    {
        // Move camera and marshmallow stick root down to match new player height
        _cameraController._origLocalPosition = new Vector3(0f, -1 + 1.8496f * CurrentScale.y, 0.15f * CurrentScale.x);
        _cameraController.transform.localPosition = _cameraController._origLocalPosition;
        _marshmallowStick.transform.localPosition = new Vector3(0.25f, -1.8496f + 1.8496f * CurrentScale.y, 0.08f - 0.15f + 0.15f * CurrentScale.x);

        // move jetpack flames
        _playerThruster.transform.localScale = CurrentScale;
        _playerThruster.transform.localPosition = new Vector3(0, -1 + CurrentScale.y, 0);

        // Change collider height and radius, and move colliders down so they touch the ground
        if (Config.ColliderMode == "Height & Radius")
        {
            _colliderScale = TargetScale;
        }
        else if (Config.ColliderMode == "Height Only")
        {
            _colliderScale = new Vector3(1f, TargetScale.y, 1f);
        }
        else
        {
            _colliderScale = Vector3.one;
        }
        float height = 2 * _colliderScale.y;
        float radius = Mathf.Min(0.5f * _colliderScale.x, 0.5f * height);
        Vector3 center = new Vector3(0f, _colliderScale.y - 1f, 0f);
        _playerCollider.height = _detectorCollider.height = _detectorShape.height = height;
        _playerCollider.radius = _detectorCollider.radius = _detectorShape.radius = radius;
        _playerCollider.center = _detectorCollider.center = _detectorShape.center = _playerBody._centerOfMass = _playerCollider.center = _detectorCollider.center = _playerBody._activeRigidbody.centerOfMass = center;

        // Change pitch if enabled
        float pitch = Config.IsPitchChangeEnabled ? 0.5f * Mathf.Pow(CurrentScale.y, -1) + 0.5f : 1f;
        foreach (OWAudioSource audio in _audioSources)
        {
            audio.pitch = pitch;
        }

        if (_npcPlayer != null)
        {
            _npcPlayer.GetComponentInChildren<Animator>().transform.localScale = CurrentScale / 10;
            _npcPlayer.transform.Find("NPC_Player_FocalPoint").localPosition = new Vector3(-0.093f, 0.991f * TargetScale.y, 0.102f);
        }
        if (_cloneController != null)
        {
            _cloneController._signal._owAudioSource.pitch = _audioSources[0].pitch;
        }
        if (_cockpitController != null)
        {
            _cockpitController._origAttachPointLocalPos = new Vector3(0, 2.1849f - 1.8496f * CurrentScale.y, 4.2307f + 0.15f - 0.15f * CurrentScale.x);
        }
        if (_logController != null)
        {
            _logController._attachPoint._attachOffset = new Vector3(0f, 1.8496f - 1.8496f * CurrentScale.y, 0.15f - 0.15f * CurrentScale.x);
        }
    }
}