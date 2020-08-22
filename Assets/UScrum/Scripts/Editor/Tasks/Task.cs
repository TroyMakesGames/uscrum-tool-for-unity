using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace uScrum.Tasks
{
    public class Task : ScriptableObject
    {
        [SerializeField, HideInInspector]
        public string summary;

        [SerializeField, HideInInspector]
        public string task;

        [SerializeField, HideInInspector]
        public string timeEstimate;

        [SerializeField, HideInInspector]
        public TaskPriority priority = 0;

        [SerializeField, HideInInspector]
        public string sprint;

        [SerializeField, HideInInspector]
        public ushort progress = 0;


        private bool editMode = false;

        /// <summary>
        /// Draws a micro version of this task with a button callback, used for sorting.
        /// </summary>
        public void DrawButton(Action onPress)
        {
            if (GUILayout.Button(summary))
            {
                if (onPress != null)
                {
                    onPress.Invoke();
                }
            }
        }

        /// <summary>
        /// Draws a short version of the task, for use in the board.
        /// </summary>
        public void DrawSummary()
        {
            GUILayout.BeginVertical("box", GUILayout.Height(60));
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label(string.Format("{0}", summary, priority), EditorStyles.miniBoldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format("{0} ({1})", task, priority), EditorStyles.miniLabel);
            GUILayout.EndVertical();

            if (GUILayout.Button("...", EditorStyles.miniButton))
            {
                editMode = false;
                Windows.ViewTaskWindow.ShowEditor(this);
            }

            GUILayout.FlexibleSpace();
            /*
            if (GUILayout.Button("->", EditorStyles.miniButton))
            {
                progress = TaskProgress.Todo;
            }
            */

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a large full version of the task, will be in edit mode if edit mode is selected.
        /// </summary>
        public void Draw()
        {
            if (!editMode)
            {
                DrawMain();
            }
            else
            {
                DrawEditable();
            }
        }

        /// <summary>
        /// Draws a large non-editable version of the task.
        /// </summary>
        public void DrawMain()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Edit"))
            {
                editMode = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(summary, EditorStyles.boldLabel);
            GUILayout.Space(20);
            GUILayout.Label(task);
            GUILayout.Space(20);
            GUILayout.Label("Priority: " + priority);
            GUILayout.Space(20);
            GUILayout.Label("Time Estimate: " + timeEstimate);
            GUILayout.Space(20);
            
            
            if (progress != 4)
            {
                string moveButtonString = "Continue";
                if (GUILayout.Button(Environment.NewLine + moveButtonString + Environment.NewLine))
                {
                    progress += 1;
                    return;
                }
            }
        }

        /// <summary>
        /// Draws the editable version.
        /// </summary>
        public void DrawEditable()
        {
            EditorGUILayout.LabelField("Summary");
            summary = EditorGUILayout.TextField(summary);
            EditorGUILayout.LabelField("Task");
            task = EditorGUILayout.TextArea(task, GUILayout.Height(40));
            EditorGUILayout.LabelField("Time Estimate");
            timeEstimate = EditorGUILayout.TextField(timeEstimate);
            //ValidateTimeEstimate();
            EditorGUILayout.LabelField("Priority");
            priority = (TaskPriority)GUILayout.Toolbar(((int)priority), new string[] { "Low", "Medium", "High" });
        }

        public void AssignToSprint(string sprintId)
        {
            sprint = sprintId;
            if (progress == 0) progress = 1;
        }

        public void RemoveFromSprint()
        {
            sprint = string.Empty;
            progress = 0;
        }

        // This doesnt work....
        private void ValidateTimeEstimate()
        {
            if (string.IsNullOrEmpty(timeEstimate))
                return;

            timeEstimate = Regex.Replace(timeEstimate, @"[^smh0-9 ]", "");
            char lastChar = timeEstimate[timeEstimate.Length - 1];
            if (lastChar != 'm' && lastChar != 'h' && lastChar != 's')
            {
                timeEstimate = timeEstimate + "m";
            }
        }
    }
}
