using UnityEditor;
using UnityEngine;

namespace Project_AR.Editor
{
    [InitializeOnLoad]
    public static class HierarchyComponentIcon
    {
        static HierarchyComponentIcon()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null) return;

            // --- ここからカスタムアイコン ---
            // 描画位置を調整（名前の左側）
            Rect iconRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height);

            // 代表アイコンを決める（最初に見つかったコンポーネント）
            Texture2D icon = null;

            // 優先的に MonoBehaviour のアイコンを取る
            foreach (var comp in obj.GetComponents<Component>())
            {
                if (comp == null) continue; // MissingScript対応
                if (comp is Transform) continue; // Transformはスキップ

                icon = AssetPreview.GetMiniThumbnail(comp);
                if (icon != null) break;
            }

            // 見つからなければ Transform のアイコンに fallback
            if (icon == null)
                icon = EditorGUIUtility.ObjectContent(obj.transform, typeof(Transform)).image as Texture2D;

            // アイコン描画
            if (icon != null)
            {
                GUI.DrawTexture(iconRect,
                    AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Project-AR/Editor/Icons/icon_background.png"));
                GUI.DrawTexture(iconRect, icon);
            }
        }
    }
}