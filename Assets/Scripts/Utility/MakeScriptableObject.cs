using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// custom editor thing
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class MakeScriptableObject : MonoBehaviour
{
    [MenuItem("Assets/Create/ScriptableObject/NumberFonts")]

    public static void CreateMyAsset()
    {
        NumberFont asset = ScriptableObject.CreateInstance<NumberFont>();

        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/NumberFonts/NewNumberFont.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
#endif