using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using uScrum.Tasks;

namespace uScrum.Windows
{
    /// <summary>
    /// Backlog that holds tasks not yet added to the board.
    /// </summary>
    public class BacklogWindow : EditorWindow
    {
        private static BacklogWindow backlog;
        private static string BACKLOG_TITLE = "Backlog";

        private Vector2 scrollPosition;

        [MenuItem("Tools/uScrum/Backlog %b")]
        public static void ShowEditor()
        {
            backlog = (BacklogWindow)EditorWindow.GetWindow((typeof(BacklogWindow)), false, "Backlog");
            UScrum.RefreshSprintList();
        }

        public static void ShowFocus()
        {
            if (backlog) backlog.Focus();
            UScrum.RefreshSprintList();
        }

        private void OnGUI()
        {
            DrawTitle();
            DrawTasks();
            DrawCreateButton();
        }

        private void DrawTitle()
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(BACKLOG_TITLE, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawTasks()
        {
            Task[] issues = UScrum.Tasks;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < issues.Length; i++)
            {
                if (issues[i].progress == 0) issues[i].DrawSummary();
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawCreateButton()
        {
            if (GUILayout.Button(Environment.NewLine +  "+" + Environment.NewLine))
            {
                CreateTaskWindow.ShowEditor();
                Repaint();
                return;
            }
        }

        private void AddIssueToBacklog()
        {
            Task newIssue = ScriptableObject.CreateInstance<Task>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(UScrum.TASK_PATH + "/uscrum-" + typeof(Task).ToString() + ".asset");
            AssetDatabase.CreateAsset(newIssue, assetPathAndName);
            AssetDatabase.SaveAssets();
        }
    }
}