using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AUIPDEFINE
{
    static AUIPDEFINE()
    {
        BuildTargetGroup btg = EditorUserBuildSettings.selectedBuildTargetGroup;
        string defines_field = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
        List<string> defines = new List<string>(defines_field.Split(';'));
        if (!defines.Contains("WS_AUIP"))
        {
            defines.Add("WS_AUIP");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, string.Join(";", defines.ToArray()));
        }

        
    }
}
