using UnityEditor;
using UnityEngine;

namespace Project_AR.Editor
{
    [InitializeOnLoad]
    public static class HierarchyActivationToggle
    {
        static HierarchyActivationToggle()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null) return;

            // --- 右端にアクティブ/非アクティブトグルボタン ---
            float toggleSize = selectionRect.height - 2;
            Rect toggleRect = new Rect(selectionRect.xMax, selectionRect.y + 1, toggleSize, toggleSize);
            bool currentActive = obj.activeSelf;
            EditorGUI.BeginChangeCheck();
            bool toggled = GUI.Toggle(toggleRect, currentActive, "");
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(obj, "Toggle Active State");
                obj.SetActive(toggled);
            }
        }
    }
}