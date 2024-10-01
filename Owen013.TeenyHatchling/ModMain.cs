﻿using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using SmolHatchling.Components;
using SmolHatchling.Interfaces;
using System.Reflection;

namespace SmolHatchling;

public class ModMain : ModBehaviour
{
    public static ModMain Instance { get; private set; }

    public static IHikersMod HikersModAPI { get; private set; }

    public static bool IsImmersionInstalled;

    public static void Print(string text, MessageType messageType = MessageType.Message)
    {
        if (Instance == null || Instance.ModHelper == null) return;
        Instance.ModHelper.Console.WriteLine(text, messageType);
    }

    public override object GetApi()
    {
        return new SmolHatchlingAPI();
    }

    public override void Configure(IModConfig config)
    {
        base.Configure(config);
        Config.Configure();
    }

    private void Awake()
    {
        Instance = this;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    private void Start()
    {
        HikersModAPI = ModHelper.Interaction.TryGetModApi<IHikersMod>("Owen013.MovementMod");
        if (HikersModAPI != null)
        {
            ModHelper.HarmonyHelper.AddPrefix<DreamLanternItem>(nameof(DreamLanternItem.OverrideMaxRunSpeed), typeof(PlayerScaleController), nameof(PlayerScaleController.DreamLanternItem_OverrideMaxRunSpeed));
        }

        IsImmersionInstalled = ModHelper.Interaction.ModExists("Owen_013.FirstPersonPresence");

        Print($"Smol Hatchling is ready to go!", MessageType.Success);
    }
}