using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace DVRewards
{
    [HarmonyPatch(typeof(UnityModManager.UI), "OnGUI")]
    internal class ModUI
    {
        private static void Postfix(UnityModManager.UI __instance)
        {
            if (!Main.showUI) return;

            DrawModUI();
        }

        private static void DrawModUI()
        {
            GUI.skin = DVGUI.skin;
            GUI.skin.label.fontSize = 9;
            GUI.skin.button.fontSize = 10;
            GUI.skin.box.normal.textColor = Color.yellow;

            GUI.Box(new Rect((Screen.currentResolution.width - 450) / 2, (Screen.currentResolution.height - 100), 450, 30), $"Diamond collected. Added $<color=green>{Main.playerCash}</color> to player's wallet.");
        }
    }
}
