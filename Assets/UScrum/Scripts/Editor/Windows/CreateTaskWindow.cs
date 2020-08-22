using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System;
using uScrum.Tasks;

namespace uScrum.Windows
{
    public class CreateTaskWindow : EditorWindow
    {
        private const float xPos = 700;
        private const float yPos = 150;
        private const float width = 600;
        private const float height = 600;

        private bool createAnother = false;
        private Task newTask = null;
        private Texture2D alertIcon;

        [MenuItem("Tools/uScrum/Create Task %i")]
        public static void ShowEditor()
        {
            CreateTaskWindow editor = (CreateTaskWindow)EditorWindow.GetWindow((typeof(CreateTaskWindow)), false, "New Task");
            editor.position = new Rect(new Vector2(xPos, yPos), new Vector2(width, height));
        }

        private void OnEnable()
        {
            newTask = ScriptableObject.CreateInstance<Task>();
            alertIcon = EditorGUIUtility.Load("icons/console.erroricon.png") as Texture2D;
        }

        private void OnGUI()
        {
            DrawTitle();
            //DrawFields();
            newTask.DrawEditable();
            DrawCreateButton();
            DrawCloseButton();
            //DrawWarnings();
        }

        private void DrawTitle()
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Create New Task", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawWarnings()
        {
            if (string.IsNullOrEmpty(newTask.summary)) DrawWarning("Title is empty.");
        }

        private void DrawWarning(string warning)
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(alertIcon, GUILayout.Width(20), GUILayout.Height(20));
            GUILayout.Label(warning, EditorStyles.miniLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawCreateButton()
        {
            createAnother = EditorGUILayout.Toggle("Create Another", createAnother);

            if (GUILayout.Button(Environment.NewLine + "Add To Backlog" + Environment.NewLine))
            {
                AddIssueToBacklog();

                if (createAnother)
                {
                    newTask = ScriptableObject.CreateInstance<Task>();
                }
                else
                {
                    BacklogWindow.ShowFocus();
                    Close();
                }

                return;
            }
        }

        private void DrawCloseButton()
        {
            if (GUILayout.Button(Environment.NewLine + "Close" + Environment.NewLine))
            {
                Close();
                return;
            }
        }

        private void AddIssueToBacklog()
        {
            // Do this in a Task.Create();
            Debug.Assert(newTask);
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(UScrum.TASK_PATH + "/Task.asset");
            AssetDatabase.CreateAsset(newTask, assetPathAndName);
            AssetDatabase.SaveAssets();
            UScrum.RefreshTaskList();
        }
    }
}