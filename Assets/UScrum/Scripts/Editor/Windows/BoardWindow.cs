using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using uScrum.Tasks;
using uScrum.Sprints;

namespace uScrum.Windows
{
    /// <summary>
    /// Editor Ui for a scrum board.
    /// </summary>
    public class BoardWindow : EditorWindow
    {
        private static BoardWindow board;

        private Sprint sprint = null;

        [MenuItem("Tools/uScrum/Scrumboard %n")]
        public static void ShowEditor()
        {
            board = (BoardWindow)EditorWindow.GetWindow((typeof(BoardWindow)), false, "Scrumboard");
        }

        public static void ShowFocus(Sprint sprint = null)
        {
            if (board)board.Focus();
            if (sprint != null) ShowSprint(sprint);
        }

        public static void ShowSprint(Sprint sprint)
        {
            if (board) board.sprint = sprint;
        }

        private void OnGUI()
        {
            if (sprint == null && UScrum.Sprints.Length != 0)
            {
                ShowSprint(UScrum.Sprints[0]);
            }

            DrawHeader();
            if (sprint == null)
            {
                DrawNoActiveSprint();
            }
            else
            {
                DrawCollumns();
            }

            //DrawTasks();
            //DrawCreateButton();
        }

        private void DrawHeader()
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format("{0}{1}{0}", Environment.NewLine, sprint != null ? sprint.title : "No Sprint Selected!"), EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawCollumns()
        {
            GUILayout.BeginHorizontal();

            List<Task> tasks = new List<Task>();
            Task[] allTasks = UScrum.Tasks;
            for (int i = 0; i < allTasks.Length; i++)
            {
                if (allTasks[i].sprint == sprint.id)
                {
                    tasks.Add(allTasks[i]);
                }
            }

            DrawCollumn("Todo", 1, tasks);
            GUILayout.FlexibleSpace();
            DrawCollumn("In Progress", 2, tasks);
            GUILayout.FlexibleSpace();
            DrawCollumn("Testing", 3, tasks);
            GUILayout.FlexibleSpace();
            DrawCollumn("Completed", 4, tasks);

            GUILayout.EndHorizontal();
        }

        private void DrawCollumn(string name, int progress, List<Task> tasks)
        {
            GUILayout.BeginVertical("box", GUILayout.Width(Screen.width/4));
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(name, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].progress == progress)
                {
                    tasks[i].DrawSummary();
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        private void DrawNoActiveSprint()
        {
            GUILayout.BeginVertical();


            GUILayout.FlexibleSpace();
            /*
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("You haven't create any sprints!", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            */

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Create a sprint");
            if (GUILayout.Button("+"))
            {
                CreateSprintWindow.ShowEditor();
                Repaint();
                return;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.EndVertical();
        }
    }
}