using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using SmolHatchling.Components;
using SmolHatchling.Interfaces;
using System.Reflection;

namespace SmolHatchling;

public class ModMain : ModBehaviour
{
    public static ModMain Instance { get; private set; }

    public IHikersMod HikersModAPI { get; private set; }

    public bool IsImmersionInstalled { get; private set; }

    public bool UseCustomPlayerScale { get; private set; }

    public float CustomPlayerScale { get; private set; }

    public bool UseScaleHotkeys { get; private set; }

    public bool UseScaledPlayerAttributes { get; private set; }

    public bool UseOtherCustomScales { get; private set; }

    public float CustomAnglerfishScale { get; private set; }

    public float CustomJellyfishScale { get; private set; }

    public float CustomInhabitantScale { get; private set; }

    public float PlayerWideness { get; private set; }

    public delegate void ConfigureEvent();

    public event ConfigureEvent OnConfigured;

    public void Print(string text, MessageType messageType = MessageType.Message)
    {
        ModHelper.Console.WriteLine(text, messageType);
    }

    public override object GetApi()
    {
        return new SmolHatchlingAPI();
    }

    public override void Configure(IModConfig config)
    {
        base.Configure(config);

        UseCustomPlayerScale = config.GetSettingsValue<bool>("UseCustomPlayerScale");
        CustomPlayerScale = config.GetSettingsValue<float>("CustomPlayerScale");
        UseScaleHotkeys = config.GetSettingsValue<bool>("UseScaleHotkeys");
        UseScaledPlayerAttributes = config.GetSettingsValue<bool>("UseScaledPlayerAttributes");
        UseOtherCustomScales = config.GetSettingsValue<bool>("UseOtherCustomScales");
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
}

/*      
 *      
 *  ISSUES
 *  - Footstep particles stay huge when you shrink back down (may have fixed itself??? be on lookout) (nope...nevermind. rare)
 *  - flashlight distance doesn't scale
 *  - damage movement speed damping may not scale
 *  
 *  IDEAS
 *  - maybe i should reduce wind volume when big?
 *  - make probe bigger (or smoller) when launched
 *  
 */