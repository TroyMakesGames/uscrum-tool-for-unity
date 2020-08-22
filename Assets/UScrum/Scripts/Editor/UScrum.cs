using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using uScrum.Tasks;
using uScrum.Sprints;

namespace uScrum
{
    public static class UScrum
    {
        /// <summary>
        /// Location of tasks within the Unity project folder.
        /// </summary>
        public const string TASK_PATH = "Assets/UScrum/Data/Tasks";

        /// <summary>
        /// Location of sprints saved within the unity proct folder.
        /// </summary>
        public const string SPRINT_PATH = "Assets/UScrum/Data/Sprints";

        private static Sprint[] sprints;
        private static Task[] tasks;

        /// <summary>
        /// Every sprint in this project.
        /// </summary>
        public static Sprint[] Sprints
        {
            get
            {
                if (sprints == null || sprints.Length == 0)
                    RefreshSprintList();
                return sprints;
            }
        }

        /// <summary>
        /// Every task current active in the board/s.
        /// </summary>
        public static Task[] Tasks
        {
            get
            {
                if (tasks == null || tasks.Length == 0)
                    RefreshTaskList();
                return tasks;
            }
        }

        /// <summary>
        /// Reloads the sprint array with sprints in the Unity folder filled with sprint assets.
        /// </summary>
        public static void RefreshSprintList()
        {
            string[] guids = AssetDatabase.FindAssets("t:Sprint", new[] { SPRINT_PATH });
            sprints = new Sprint[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                sprints[i] = (Sprint)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[i]), typeof(Sprint));
            }
        }


        /// <summary>
        /// Reloads the task array with tasks in the Unity folder filled with task assets.
        /// </summary>
        public static void RefreshTaskList()
        {
            string[] guids = AssetDatabase.FindAssets("t:Task", new[] { TASK_PATH });
            tasks = new Task[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                tasks[i] = (Task)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[i]), typeof(Task));
            }
        }

        /// <summary>
        /// Logs to the console if USCRUM_LOGGING is defined.
        /// </summary>
        public static void Log(string log)
        {
#if USCRUM_LOGGING
            Debug.Log("<color=blue>[USCRUM]</color> " + log);
#endif
        }
    }
}