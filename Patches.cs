using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace PortalInputLengthener;

[HarmonyPatch(typeof(TeleportWorld), nameof(TeleportWorld.Interact))]
class InteractPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> source = new(instructions);
        if (!PortalInputLengthenerPlugin.CharacterLimit.Value) return source.AsEnumerable();
        foreach (CodeInstruction t in source.Where(t => t.opcode == OpCodes.Ldc_I4_S))
        {
            t.operand = (int)sbyte.MaxValue;
        }

        return source.AsEnumerable();
    }
}