using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System;
using uScrum.Tasks;
using uScrum.Sprints;

namespace uScrum.Windows
{
    /// <summary>
    /// Used to create a new sprint, and assign tasks to it.
    /// </summary>
    public class CreateSprintWindow : EditorWindow
    {
        private const float X_POS = 700;
        private const float y_POS = 150;
        private const float WIDTH = 600;
        private const float HEIGHT = 600;

        private Sprint newSprint = null;

        [MenuItem("Tools/uScrum/Sprints/Create Sprint")]
        public static void ShowEditor()
        {
            CreateSprintWindow editor = (CreateSprintWindow)EditorWindow.GetWindow((typeof(CreateSprintWindow)), false, "Create New Sprint");
            editor.position = new Rect(new Vector2(X_POS, y_POS), new Vector2(WIDTH, HEIGHT));
        }

        private void OnEnable()
        {
            newSprint = ScriptableObject.CreateInstance<Sprint>();
        }

        private void OnGUI()
        {
            DrawTitle();
            newSprint.DrawEditable();
            DrawCreateButton();
            DrawCloseButton();
        }

        private void DrawTitle()
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Create New Sprint", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawCreateButton()
        {
            if (GUILayout.Button(Environment.NewLine + "Create" + Environment.NewLine))
            {
                newSprint.Create();
                UScrum.RefreshSprintList();
                BoardWindow.ShowFocus(newSprint);
                Close();
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
    }
}