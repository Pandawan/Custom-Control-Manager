using UnityEngine;
using System.Collections;
using System;

/**
    A simple Input manager for Unity

    --------------------------------

    You can check out the GitHub here https://github.com/PandawanFr/Custom-Control-Manager/

    This script is the same as Unity's input manager, but better, you can modify it in real time, and has a lot of features that the default should have. 

    This is made under the MIT license.

    This currently does not work with iOS for file saving reasons, a fix is in the work!

    Written by PandawanFr http://pandawan.xyz

    --------------------------------

    How To Use

    Put this component on any game object

    You can change the name of the file where it will save the controls using the file variable.
    Note, the file will save in the default dataPath. You can learn more about dataPath here http://docs.unity3d.com/ScriptReference/Application-dataPath.html

    For info on API use, please look here https://github.com/PandawanFr/Custom-Control-Manager/wiki
**/


namespace CustomControls
{
    //[ExecuteInEditMode]
    public class CustomControls : MonoBehaviour
    {
        SaveAndLoad sal = new SaveAndLoad();
        [Tooltip("This is the file's name where all you controls will be saved.  You do not need to add .json at the end!")]
        public string file = "CustomControls";


        void Start()
        {
            sal.fullPath = Application.dataPath + "/";

            // Check if file exists
            if (!sal.FileExists(file))
                sal.CreateFile(file);


        }

        /// <summary>
        /// A Custom Debug.Log method
        /// </summary>
        /// <param name="input">The text to output</param>
        /// <param name="type">Type of log, normal, error, or warning</param>
        public static void Log (string input, string type)
        {
            if (type == "normal" || type == null)
                Debug.Log(input);
            else if (type == "error")
                Debug.LogError(input);
            else if (type == "warning")
                Debug.LogWarning(input);

        }

        // Returns a KeyCode of the primary key of the keyName
        KeyCode GetPrimaryKey (string fileName, string keyName)
        {
            string primKeyName = sal.GetNodeAuto(fileName)["custom_controls"][keyName]["primary"];
            KeyCode thisKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), primKeyName);

            return thisKeyCode;
        }

        // Returns a KeyCode of the secondary key of the keyName
        KeyCode GetSecondary(string fileName, string keyName)
        {
            string primKeyName = sal.GetNodeAuto(fileName)["custom_controls"][keyName]["secondary"];
            KeyCode thisKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), primKeyName);

            return thisKeyCode;
        }

        // Returns true if the key is a correct KeyCode
        public bool isKey (KeyCode key)
        {
            //KeyCode thisKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), key);

            if (Enum.IsDefined(typeof(KeyCode), key))
            {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Returns true when either primary or secondary key is pressed
        /// </summary>
        /// <param name="fileName"> Name of JSON File</param>
        /// <param name="keyName"> Name of Key</param>
        /// <returns>Returns true whenever the key is pressed</returns>
        public bool OnKey (string keyName)
        {
            // Checks if pressing primary
            if (Input.GetKey(GetPrimaryKey(file, keyName)))
            {
                return true;
            }
            // Checks if there is a secondary
            else if (sal.SecondaryExists(file, keyName))
            {
                // Checks if pressing secondary
                if (Input.GetKey(GetSecondary(file, keyName)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true when either primary or secondary key is pressed, only once
        /// </summary>
        /// <param name="fileName"> Name of JSON File</param>
        /// <param name="keyName"> Name of Key</param>
        /// <returns>Returns true when key is pressed</returns>
        public bool OnKeyDown(string keyName)
        {
            // Checks if pressing primary
            if (Input.GetKeyDown(GetPrimaryKey(file, keyName)))
            {
                return true;
            }
            // Checks if there is a secondary
            else if (sal.SecondaryExists(file, keyName))
            {
                // Checks if pressing secondary
                if (Input.GetKeyDown(GetSecondary(file, keyName)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true when either primary or secondary key is released, only once
        /// </summary>
        /// <param name="fileName"> Name of JSON File</param>
        /// <param name="keyName"> Name of Key</param>
        /// <returns>Returns true when key is released</returns>
        public bool OnKeyUp(string keyName)
        {
            // Checks if pressing primary
            if (Input.GetKeyUp(GetPrimaryKey(file, keyName)))
            {
                return true;
            }
            // Checks if there is a secondary
            else if (sal.SecondaryExists(file, keyName))
            {
                // Checks if pressing secondary
                if (Input.GetKeyUp(GetSecondary(file, keyName)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a new key to the JSON file.
        /// </summary>
        /// <param name="fileName"> Name of JSON File</param>
        /// <param name="keyName"> Name of the Key (e.i. Fire)</param>
        /// <param name="primaryKey"> Primary Key to use</param>
        public void AddKey (string keyName, KeyCode primaryKey)
        {
            if (!sal.KeyExists(file, keyName))
            {
                if (isKey(primaryKey))
                {
                    // Checks if the primary key is valid
                    sal.CreateNodeKey(file, keyName, primaryKey, "add");
                }else
                {
                    Log("Primary Key is not valid!", "error");
                }
            }
            else
            {
                Log("Key already exists! Please use ChangeKey instead!", "error");
            }
        }

        /// <summary>
        /// Adds a new key to the JSON file.
        /// </summary>
        /// <param name="fileName"> Name of JSON File</param>
        /// <param name="keyName"> Name of the Key (e.i. Fire)</param>
        /// <param name="primaryKey"> Primary Key to use</param>
        /// <param name="secondaryKey"> (Optional) Secondary key</param>
        public void AddKey(string keyName, KeyCode primaryKey, KeyCode secondaryKey)
        {
            if (!sal.KeyExists(file, keyName))
            {
                // Checks if the primary key is valid
                if (isKey(primaryKey))
                {
                    // Checks if the secondary key is valid
                    if (isKey(secondaryKey))
                    {
                        sal.CreateNodeKey(file, keyName, primaryKey, secondaryKey, "add");
                    }
                    else
                    {
                        Log("Secondary Key is not valid!", "error");
                    }
                }
                else
                {
                    Log("Primary Key is not valid!", "error");
                }
            }
            else
            {
                Log("Key already exists! Please use ChangeKey instead!", "error");
            }
        }

        /// <summary>
        /// Changes the values of a key to the JSON file.
        /// </summary>
        /// <param name="fileName"> Name of JSON File</param>
        /// <param name="keyName"> Name of the Key (e.i. Fire)</param>
        /// <param name="primaryKey"> Primary Key to use</param>
        public void ChangeKey (string keyName, KeyCode primaryKey)
        {
            if (sal.KeyExists(file, keyName))
            {
                // Checks if the primary key is valid
                if (isKey(primaryKey))
                {
                    sal.CreateNodeKey(file, keyName, primaryKey, "change");
                }
                else
                {
                    Log("Primary Key is not valid!", "error");
                }
            }
            else
            {
                Log("Key doesn't exists! Please use AddKey instead!", "error");
            }
        }

        /// <summary>
        /// Changes the values of a key to the JSON file.
        /// </summary>
        /// <param name="fileName"> Name of JSON File</param>
        /// <param name="keyName"> Name of the Key (e.i. Fire)</param>
        /// <param name="primaryKey"> Primary Key to use</param>
        /// <param name="secondaryKey"> (Optional) Secondary key</param>
        public void ChangeKey(string keyName, KeyCode primaryKey, KeyCode secondaryKey)
        {
            if (sal.KeyExists(file, keyName))
            {
                // Checks if the primary key is valid
                if (isKey(primaryKey))
                {
                    // Checks if the secondary key is valid
                    if (isKey(secondaryKey))
                    {
                        sal.CreateNodeKey(file, keyName, primaryKey, secondaryKey, "change");
                    }
                    else
                    {
                        Log("Secondary Key is not valid!", "error");
                    }
                }
                else
                {
                    Log("Primary Key is not valid!", "error");
                }
            }
            else
            {
                Log("Key doesn't exists! Please use AddKey instead!", "error");
            }
        }

        /// <summary>
        /// Removes a key from the JSON
        /// </summary>
        /// <param name="keyName">The name of the Key to remove</param>
        public void RemoveKey (string keyName)
        {
            if (sal.KeyExists(file,keyName))
            {
                sal.RemoveKey(file, keyName);
            }
            else
            {
                Log("Key doesn't exists!", "error");
            }
        }

        /// <summary>
        /// Give it a file, it will copy it at the right spot for use by the system.
        /// Note: It doesn't remove any file unless the fileName is the same
        /// </summary>
        /// <param name="path"> Path to the file</param>
        /// <param name="fileName"> Name of the file</param>
        public void LoadConfig (string path, string fileName)
        {
            sal.Copy(path, fileName);
            file = fileName;
            Log("Successfully copied control config!", "normal");
        }
    }
}
