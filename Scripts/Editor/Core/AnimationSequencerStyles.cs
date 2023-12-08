using UnityEditor;
using UnityEngine;

namespace BrunoMikoski.AnimationSequencer
{
    public static class AnimationSequencerStyles
    {
        public static readonly GUIStyle InspectorTitlebar;
        public static readonly GUIStyle TitlebarFoldout;

        static AnimationSequencerStyles()
        {
            InspectorTitlebar = GetStyle("IN Title");
            TitlebarFoldout = GetStyle("Titlebar Foldout");
        }

        private static GUIStyle GetStyle(string styleName)
        {
            return GUI.skin.FindStyle(styleName) ??
                   EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
        }
    }
}