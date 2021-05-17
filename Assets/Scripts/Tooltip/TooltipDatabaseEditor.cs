#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace trollschmiede.Generic.Tooltip
{
    [CustomEditor(typeof(TooltipDatabase), true)]
    public class TooltipDatabaseEditor : Editor
    {
        GUIContent addButton = new GUIContent("Add New Tooltip", "Add a new Tooltip at first Element of the List");
        GUIContent delButton = new GUIContent("X", "Delete Tooltip");
        GUIContent insertButton = new GUIContent("+", "Add new Tooltip here");
        GUILayoutOption miniButton = GUILayout.Width(20f);

        SerializedProperty database;

        private void OnEnable()
        {
            database = serializedObject.FindProperty("database");
        }

        public override void OnInspectorGUI()
        {
            TooltipDatabase myTarget = (TooltipDatabase)target;

            myTarget.searchString = EditorGUILayout.TextField(new GUIContent("Search: "), myTarget.searchString);

            if (GUILayout.Button(addButton))
            {
                database.InsertArrayElementAtIndex(0);
            }

            serializedObject.ApplyModifiedProperties();

            DrawList(database, myTarget);

            serializedObject.ApplyModifiedProperties();
        }

        void DrawList(SerializedProperty list, TooltipDatabase tooltipDatabase)
        {
            list.isExpanded = EditorGUILayout.ToggleLeft(list.name, list.isExpanded);
            EditorGUI.indentLevel += 2;
            if (list.isExpanded)
            {
                for (int i = 0; i < list.arraySize; i++)
                {
                    if (!tooltipDatabase.database[i].tooltipName.ToLower().Contains(tooltipDatabase.searchString.ToLower()))
                    {
                        continue;
                    }
                    foreach (string triggerword in tooltipDatabase.database[i].triggerWords)
                    {
                        if (!triggerword.ToLower().Contains(tooltipDatabase.searchString.ToLower()))
                        {
                            continue;
                        }
                    }

                    EditorGUILayout.BeginHorizontal();
                    
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));

                    if (GUILayout.Button(insertButton, miniButton))
                    {
                        list.InsertArrayElementAtIndex(i);
                        serializedObject.ApplyModifiedProperties();
                    }

                    if (GUILayout.Button(delButton, miniButton))
                    {
                        list.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                    
                }
            }
            EditorGUI.indentLevel -= 2;
        }
    }
}
#endif