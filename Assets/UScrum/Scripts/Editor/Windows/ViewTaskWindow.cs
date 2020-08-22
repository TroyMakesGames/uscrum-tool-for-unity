using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System;
using uScrum.Tasks;

namespace uScrum.Windows
{
    public class ViewTaskWindow : EditorWindow
    {
        private const float xPos = 700;
        private const float yPos = 150;
        private const float width = 600;
        private const float height = 600;

        private static Task issue;

        public static void ShowEditor(Task issue)
        {
            ViewTaskWindow.issue = issue;
            ViewTaskWindow editor = (ViewTaskWindow)EditorWindow.GetWindow((typeof(ViewTaskWindow)), false, issue.summary);
            editor.position = new Rect(new Vector2(xPos, yPos), new Vector2(width, height));
        }

        private void OnGUI()
        {
            issue.Draw();
            DrawCloseButton();
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