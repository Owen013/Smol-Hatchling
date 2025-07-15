using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using SmolHatchling.Components;
using SmolHatchling.Interfaces;
using System.Reflection;
using UnityEngine.InputSystem;
using UnityEngine;

namespace SmolHatchling;

public class ModMain : ModBehaviour
{
    public static ModMain Instance { get; private set; }

    public IHikersMod HikersModAPI { get; private set; }

    public bool IsImmersionInstalled { get; private set; }

    public bool UsingCustomPlayerScale { get; private set; }

    public float CustomPlayerScale { get; private set; }

    public bool UsingScaleHotkeys { get; private set; }

    public bool UsingScaledPlayerAttributes { get; private set; }

    public bool UsingOtherCustomScales { get; private set; }

    public float CustomAnglerfishScale { get; private set; }

    public float CustomJellyfishScale { get; private set; }

    public float CustomInhabitantScale { get; private set; }

    public float PlayerWideness { get; private set; }

    private float _resetButtonHeldTime;

    public delegate void ConfigureEvent();

    public event ConfigureEvent OnConfigured;

    public override object GetApi()
    {
        return new SmolHatchlingAPI();
    }

    public override void Configure(IModConfig config)
    {
        base.Configure(config);

        UsingCustomPlayerScale = config.GetSettingsValue<bool>("UsingCustomPlayerScale");
        CustomPlayerScale = config.GetSettingsValue<float>("CustomPlayerScale");
        UsingScaleHotkeys = config.GetSettingsValue<bool>("UsingScaleHotkeys");
        UsingScaledPlayerAttributes = config.GetSettingsValue<bool>("UsingScaledPlayerAttributes");
        UsingOtherCustomScales = config.GetSettingsValue<bool>("UsingOtherCustomScales");
        CustomAnglerfishScale = config.GetSettingsValue<float>("CustomAnglerfishScale");
        CustomJellyfishScale = config.GetSettingsValue<float>("CustomJellyfishScale");
        CustomInhabitantScale = config.GetSettingsValue<float>("CustomInhabitantScale");
        PlayerWideness = config.GetSettingsValue<float>("PlayerWideness");

        if (CustomPlayerScale <= 0f)
        {
            Print("Player Scale cannot be 0 or less.", MessageType.Error);
            SetConfigSetting("CustomPlayerScale", 1f);
        }

        if (CustomAnglerfishScale <= 0f)
        {
            Print("Anglerfish Scale cannot be 0 or less.", MessageType.Error);
            SetConfigSetting("CustomAnglerfishScale", 1f);
        }

        if (CustomJellyfishScale <= 0f)
        {
            Print("Jellyfish Scale cannot be 0 or less.", MessageType.Error);
            SetConfigSetting("CustomJellyfishScale", 1f);
        }

        if (CustomInhabitantScale <= 0f)
        {
            Print("Inhabitant Scale cannot be 0 or less.", MessageType.Error);
            SetConfigSetting("CustomInhabitantScale", 1f);
        }

        OnConfigured?.Invoke();
        HikersModAPI?.UpdateConfig();
    }

    public void Print(string text, MessageType messageType = MessageType.Message)
    {
        ModHelper.Console.WriteLine(text, messageType);
    }

    public void Configure()
    {
        Configure(ModHelper.Config);
    }

    public void SetConfigSetting(string settingName, object value)
    {
        ModHelper.Config.SetSettingsValue(settingName, value);
        Configure();
    }

    private void Awake()
    {
        Instance = this;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    private void Start()
    {
        HikersModAPI = ModHelper.Interaction.TryGetModApi<IHikersMod>("Owen013.MovementMod");
        IsImmersionInstalled = ModHelper.Interaction.ModExists("Owen_013.FirstPersonPresence");

        if (HikersModAPI != null)
        {
            ModHelper.HarmonyHelper.AddPrefix<DreamLanternItem>(nameof(DreamLanternItem.OverrideMaxRunSpeed), typeof(PlayerScaleController), nameof(PlayerScaleController.DreamLanternItem_OverrideMaxRunSpeed));
        }

        Print($"Smol Hatchling is ready to go!", MessageType.Success);
    }

    private void Update()
    {
        if ((UsingCustomPlayerScale || UsingOtherCustomScales) && Keyboard.current[Key.Slash].isPressed)
        {
            if (_resetButtonHeldTime >= 5f)
            {
                SetConfigSetting("UsingCustomPlayerScale", false);
                SetConfigSetting("UsingOtherCustomScales", false);
                _resetButtonHeldTime = 0f;
                Print("Custom Scales disabled");
            }
            else
            {
                _resetButtonHeldTime += Time.unscaledDeltaTime;
            }
        }
        else
        {
            _resetButtonHeldTime = 0f;
        }
    }
}

/*      
 *      
 *  ISSUES
 *  - Footstep particles stay huge when you shrink back down (may have fixed itself??? be on lookout) (nope...nevermind. rare)
 *  - flashlight distance doesn't scale
 *  
 *  IDEAS
 *  - maybe i should reduce wind volume when big?
 *  - make probe bigger (or smoller) when launched
 *  
 */