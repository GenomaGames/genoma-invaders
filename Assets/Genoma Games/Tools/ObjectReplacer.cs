#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ReplaceWithPrefab : EditorWindow
{
    [SerializeField]
    private GameObject prefab;

    [MenuItem("Tools/Replace With Prefab")]
    public static void CreateReplaceWithPrefab()
    {
        GetWindow<ReplaceWithPrefab>();
    }

    [MenuItem("Tools/Replace With Prefab", true)]
    public static bool ValidateCreateReplaceWithPrefab()
    {
        return Selection.transforms.Length > 0 && !EditorUtility.IsPersistent(Selection.activeGameObject);
    }

    private void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace"))
        {
            Undo.IncrementCurrentGroup();

            Undo.RecordObjects(Selection.transforms, "Object to replace");

            foreach (Transform transform in Selection.transforms)
            {
                PrefabAssetType prefabAssetType = PrefabUtility.GetPrefabAssetType(prefab);
                GameObject newObject;

                if (prefabAssetType != PrefabAssetType.NotAPrefab)
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    newObject = Instantiate(prefab);
                }

                if (newObject == null)
                {
                    throw new UnityException("Error instantiating prefab");
                }

                Undo.RegisterCreatedObjectUndo(newObject, "Create new object");

                int siblingIndex = transform.GetSiblingIndex();

                Undo.SetTransformParent(newObject.transform, transform.parent, "Modify parent");

                Undo.RegisterChildrenOrderUndo(transform.parent, "Update object");
                Undo.RegisterCompleteObjectUndo(newObject, "Update object");

                newObject.transform.localPosition = transform.localPosition;
                newObject.transform.localRotation = transform.localRotation;
                newObject.transform.localScale = transform.localScale;
                newObject.transform.SetSiblingIndex(siblingIndex);
                newObject.name = prefab.name;

                if (newObject.transform.parent.childCount > 1)
                {
                    newObject.name += $".{siblingIndex.ToString("D3")}";
                }

                PrefabUtility.RecordPrefabInstancePropertyModifications(newObject);

                Undo.DestroyObjectImmediate(transform.gameObject);

                Undo.SetCurrentGroupName("Replace With Prefabs");
            }
        }

        GUI.enabled = false;
        EditorGUILayout.LabelField($"Selection count: {Selection.objects.Length}");
    }
}
#endif