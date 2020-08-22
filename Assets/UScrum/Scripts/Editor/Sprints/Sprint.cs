using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using uScrum.Tasks;
using uScrum.Windows;

namespace uScrum.Sprints
{
    public class Sprint : ScriptableObject
    {
        private static float TASK_BOX_HEIGHT = 300;

        [SerializeField, HideInInspector]
        public string title;

        [SerializeField, HideInInspector]
        public string goal;

        [SerializeField, HideInInspector]
        public SprintProgress progress = SprintProgress.NotStarted;

        [SerializeField, HideInInspector]
        public string id;

        private Vector2 backlogScrollPosition;
        private Vector2 sprintTasksscrollPosition;

        /// <summary>
        /// Draws the sprint to be created / edited.
        /// </summary>
        public void DrawEditable()
        {
            Task[] tasks = UScrum.Tasks;

            CheckId();
            EditorGUILayout.LabelField("Title");
            title = EditorGUILayout.TextField(title);
            EditorGUILayout.LabelField("Sprint Goal");
            goal = EditorGUILayout.TextArea(goal, GUILayout.Height(40));

            EditorGUILayout.LabelField("Tasks");
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical("box", GUILayout.Height(TASK_BOX_HEIGHT), GUILayout.Width(Screen.width / 2));
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Backlog", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            backlogScrollPosition = GUILayout.BeginScrollView(backlogScrollPosition);
            for (int i = 0; i < tasks.Length; i++)
            {
                Task task = tasks[i];
                if (string.IsNullOrEmpty(task.sprint)) task.DrawButton(() =>
                {
                    task.AssignToSprint(id);
                });
            }
            EditorGUILayout.EndScrollView();

            GUILayout.EndVertical();

            GUILayout.BeginVertical("box", GUILayout.Height(TASK_BOX_HEIGHT), GUILayout.Width(Screen.width / 2));
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("In Sprint", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            sprintTasksscrollPosition = GUILayout.BeginScrollView(sprintTasksscrollPosition);
            for (int i = 0; i < tasks.Length; i++)
            {
                Task task = tasks[i];
                if (task.sprint == id) task.DrawButton(() =>
                {
                    task.RemoveFromSprint();
                });
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();



            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Create an id and saves his sprint to data.
        /// </summary>
        public void Create()
        {
            CheckId();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", UScrum.SPRINT_PATH, id));
            AssetDatabase.CreateAsset(this, assetPathAndName);
            AssetDatabase.SaveAssets();
            UScrum.Log(string.Format("Created Sprint '{0}' with id of '{1}'.", title, id));
        }

        private void CheckId()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = GenerateNewId();
            }
        }

        private string GenerateNewId()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        }
    }
}