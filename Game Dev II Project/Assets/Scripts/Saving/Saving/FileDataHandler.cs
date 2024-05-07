using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
        private string dataDirPath = "";
        private string dataFileName = "";

        private bool useEncryption = false;
	    private readonly string encryptionCodeWord = "NoMAN";


        public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
            this. useEncryption = useEncryption;
        }

        public GameData Load()
        {
           //use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadedData = null;
            if(File.Exists(fullPath))
            {
                try
                {

                    //Load the serialized data from the file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                     if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch(Exception e)
                {
                    Debug.LogError("Error occured when trying to load data to file: " + fullPath + "\n" + e);
                }
            } 
            return loadedData;
        }

        public void Save(GameData data)
        {
            //use Path.Combine to account for different OS's having different path separators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            try
            {
                //create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                //write the serialized datato the file
                string dataToStore = JsonUtility.ToJson(data, true);
                if (useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }
                //write the serialized data too the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = ""; // Initialize an empty string

            for (int i = 0; i < data.Length; i++)
            {
                // XOR operation to encrypt/decrypt the character
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }

            return modifiedData;
        }

}
