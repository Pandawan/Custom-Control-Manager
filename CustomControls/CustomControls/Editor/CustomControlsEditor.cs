using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CustomControls {

    [CustomEditor(typeof(CustomControls))]
    public class CustomControlsEditor : Editor
    {

        string currentKeyName;
        string primaryKey;
        string secondaryKey;

        bool error;
        string errorCode;

        bool errorFile;
        string errorCodeFile;
        string filePath;
        string fileNameConfig;

        SaveAndLoad sal = new SaveAndLoad();

        public override void OnInspectorGUI()
        {
            CustomControls myTarget = (CustomControls)target;

            GUILayout.Box("Custom Controls", GUILayout.ExpandWidth(true));

            GUILayout.Label("Settings", EditorStyles.boldLabel);
            myTarget.file = EditorGUILayout.TextField("File Name", myTarget.file);
            //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

            GUILayout.Label("Add New", EditorStyles.boldLabel);
            currentKeyName = EditorGUILayout.TextField("Key Name", currentKeyName);
            primaryKey = EditorGUILayout.TextField("Primary Key", primaryKey);
            secondaryKey = EditorGUILayout.TextField("Secondary Key", secondaryKey);

            if (error)
                EditorGUILayout.HelpBox(errorCode, MessageType.Error);

            if (GUILayout.Button("Add Key")){
                if (!sal.FileExists(myTarget.file))
                    sal.CreateFile(myTarget.file);

                // Check if there is a Key Name
                if (currentKeyName != "" & currentKeyName != " " && currentKeyName != null) {

                    KeyCode primKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), primaryKey);

                    // Check if Primary Key is valid
                    if (myTarget.isKey(primKeyCode))
                    {
                        // Check if there's a Secondary Key
                        if (secondaryKey != null && secondaryKey != "" && secondaryKey != " ")
                        {
                            KeyCode secKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), secondaryKey);

                            // Check if the Secondary Key is valid
                            if (myTarget.isKey(secKeyCode))
                            {
                                myTarget.AddKey(currentKeyName, primKeyCode, secKeyCode);

                                error = false;
                            }
                            else
                            {
                                errorCode = "Secondary Key is invalid!";
                                error = true;
                            }
                        }
                        // No Secondary Key, add with only one
                        else
                        {
                            myTarget.AddKey(currentKeyName, primKeyCode);
                            error = false;
                        }
                    }
                    else
                    {
                        errorCode = "Primary Key is invalid!";
                        error = true;
                    }
                }
                else
                {
                    errorCode = "Key Name is invalid!";
                    error = true;
                }
            }

            GUILayout.Label("Import", EditorStyles.boldLabel);
            filePath = EditorGUILayout.TextField("Path of file to copy", filePath);
            fileNameConfig = EditorGUILayout.TextField("Name of file after import", fileNameConfig);

            if (errorFile)
                EditorGUILayout.HelpBox(errorCodeFile, MessageType.Error);

            if (GUILayout.Button("Import Config"))
            {
                if (!sal.FileExists(myTarget.file))
                    sal.CreateFile(myTarget.file);

                if (filePath != "" && filePath != null)
                {
                    if (fileNameConfig != "" && fileNameConfig != null)
                    {
                        myTarget.LoadConfig(filePath, fileNameConfig);
                        error = false;
                    }
                    else
                    {
                        errorCodeFile = "Name of file is invalid!";
                        errorFile = true;
                    }
                }
                else
                {
                    errorCodeFile = "Path is invalid!";
                    errorFile = true;
                }
            }
        }
    }
}
