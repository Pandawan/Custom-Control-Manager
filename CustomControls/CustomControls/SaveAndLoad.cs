using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/**
    For license information, please look at the top of the "CustomControls.cs" script
**/

namespace CustomControls
{

    public class SaveAndLoad
    {
        public string fullPath ()
        {
            string _fullPath = Application.dataPath + "/";

#if UNITY_IOS
            _fullPath = Application.persistentDataPath + "/";
#elif UNITY_EDITOR
            bool containsAsset = _fullPath.Contains("Assets");
            if (!containsAsset)
            {
                _fullPath = Application.dataPath + "/Assets/";
            }
#else
            _fullPath = Application.dataPath + "/";
#endif

            return _fullPath;
        }

        // Returns true if file exists in dataPath, returns false if not
        public bool FileExists(string fileName)
        {

            if (File.Exists(fullPath() + fileName + ".json"))
            {
                return true;
            }
            else
            {
                return false;
            }

            

        }

        // Creates a file in the dataPath
        public void CreateFile(string fileName)
        {
            StreamWriter sr = new StreamWriter(fullPath() + fileName + ".json");
            sr.WriteLine("{\"custom_controls\": {}");
            sr.Close();
            CustomControls.Log("Created file " + fullPath() + fileName + ".json", "normal");
        }

        // Reads the file (input) and returns the content as a string
        public string ReadFile(string fileName)
        {
            StreamReader sr = new StreamReader(fullPath() + fileName + ".json");
            string fileContents = sr.ReadToEnd();
            sr.Close();

            return fileContents;
        }

        // Uses the SimpleJSON parser to find and output a JSONNode value
        public JSONNode GetNode(string fileContent)
        {
            JSONNode data = JSON.Parse(fileContent);

            return data;
        }

        // Uses the SimpleJSON parser to find and output a JSONNode value
        // Note: This is different from GetNode because it does the ReadFile by itself, GetNode requires a content, while GetNodeAuto requires a fileName
        public JSONNode GetNodeAuto(string fileName)
        {
            JSONNode data = JSON.Parse(ReadFile(fileName));

            return data;
        }

        // Creates a Node in the JSON file with a value
        // Note: Any nodeName that doesn't exist will be created
        public void CreateNodeKey(string fileName, string keyName, KeyCode primaryValue, string type)
        {
            JSONNode data = GetNodeAuto(fileName);

            data["custom_controls"][keyName]["primary"] = primaryValue.ToString();
            data["custom_controls"][keyName].Remove("secondary");

            // Inputting Data in variable in order to format/indent it
            string fileCnt = data.ToString();
            JsonConvert.SerializeObject(fileCnt);
            fileCnt = JValue.Parse(fileCnt).ToString(Formatting.Indented);

            // Save the text in a file
            File.WriteAllText(fullPath() + fileName + ".json", fileCnt);

            //Testing if node was created
            if (GetNodeAuto(fileName)["custom_controls"][keyName]["primary"].ToString().Replace("\"", "") == primaryValue.ToString())
            {
                CustomControls.Log("Successfully " + type + "ed " + keyName + " with Primary " + primaryValue.ToString() + "!", "normal");
            }
            else
            {
                CustomControls.Log("Could not create/change key " + keyName + ". Primary key did not register!", "error");
            } 
        }

        // Creates a Node in the JSON file with a value
        // Note: Any nodeName that doesn't exist will be created
        public void CreateNodeKey(string fileName, string keyName, KeyCode primaryValue, KeyCode secondaryValue, string type)
        {
            JSONNode data = GetNodeAuto(fileName);

            data["custom_controls"][keyName]["primary"] = primaryValue.ToString();

            data["custom_controls"][keyName]["secondary"] = secondaryValue.ToString();

            // Inputting Data in variable in order to format/indent it
            string fileCnt = data.ToString();
            JsonConvert.SerializeObject(fileCnt);
            fileCnt = JValue.Parse(fileCnt).ToString(Formatting.Indented);

            // Save the text in a file
            File.WriteAllText(fullPath() + fileName + ".json", fileCnt);

            //Testing if node was created
            if (GetNodeAuto(fileName)["custom_controls"][keyName]["primary"].ToString().Replace("\"", "") == primaryValue.ToString())
            {
                if (GetNodeAuto(fileName)["custom_controls"][keyName]["secondary"].ToString().Replace("\"", "") == secondaryValue.ToString())
                {

                    CustomControls.Log("Successfully " + type + "ed " + keyName + " with Primary " + primaryValue.ToString() + " and Secondary " + secondaryValue.ToString() + "!", "normal");
                }
                else
                {
                    CustomControls.Log("Could not create/change key " + keyName + ". Secondary key did not register!", "error");
                }
            }
            else
            {
                CustomControls.Log("Could not create/change key " + keyName + ". Primary key did not register!", "error");
            }
        }

        // Returns true if the key has a Secondary
        public bool SecondaryExists (string fileName, string keyName)
        {
            JSONNode data = GetNodeAuto(fileName);

            if (data["custom_controls"][keyName]["secondary"] != "" && data["custom_controls"][keyName]["secondary"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Checks if a key exists
        public bool KeyExists (string fileName, string keyName)
        {
            JSONNode data = GetNodeAuto(fileName);

            if (data["custom_controls"][keyName] != "" && data["custom_controls"][keyName] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Removes a key
        public void RemoveKey (string fileName, string keyName)
        {
            JSONNode data = GetNodeAuto(fileName);

            data["custom_controls"].Remove(keyName);

            // Inputting Data in variable in order to format/indent it
            string fileCnt = data.ToString();
            JsonConvert.SerializeObject(fileCnt);
            fileCnt = JValue.Parse(fileCnt).ToString(Formatting.Indented);

            // Save the text in a file
            File.WriteAllText(fullPath() + fileName + ".json", fileCnt);
        }

        public void Copy (string path, string fileName)
        {
            File.Copy(path, fullPath() + fileName + ".json", true);
        }
    }
}
