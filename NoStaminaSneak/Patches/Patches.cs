using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace NoStaminaSneak.Patches
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Player), nameof(Player.OnSneaking))]
        public static IEnumerable<CodeInstruction> OnSneakingTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            int codepoint = -1;

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < instructions.Count(); i++)
            {
                CodeInstruction currentCode = codes[i];
                if (codepoint == -1)
                {
                    if (currentCode.opcode == OpCodes.Ldloc_1)
                    {
                        codepoint = i;
                        codes[i] = new CodeInstruction(OpCodes.Ldc_R4, 0f);
                        break;
                    }
                }
            }

            if (codepoint == -1)
                throw new System.Exception("No Stamina Sneak Transpiler injection point NOT found!!  Game has most likely updated and broken this mod!");

            return codes.AsEnumerable();
        }
    }
}
