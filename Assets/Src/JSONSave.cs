using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

/// <summary>
/// Script que se usa para serializar y deserializar el proceso
/// </summary>

namespace MazesAndMore
{
    [Serializable]
    public class JSONSave
    {
        public string hash;
        public int c, i, h;

        /// <summary>
        /// carga el progreso guardado
        /// si no hay guardado, lo genera y lo devuelve
        /// </summary>
        /// <returns>progreso del juego</returns>
        public static JSONSave FromJson()
        {
            string save = PlayerPrefs.GetString("Save", "null");
            if (save == "null")
                return CreateNewGameFile();

            else
            {
                string toHash = "{";
                bool hashEnded = false;
                for (int i = 0; i < save.Length; i++)
                {
                    if (!hashEnded)
                    {
                        if (save[i] == ',')
                            hashEnded = true;
                    }
                    else
                        toHash += save[i];
                }
                JSONSave mySave = JsonUtility.FromJson<JSONSave>(save);

                if (mySave.hash == CreateMD5(toHash))
                    return mySave;
                else
                    return CreateNewGameFile();
            }

        }

        /// <summary>
        /// genera un progreso desde la nada (todo nuevo)
        /// </summary>
        /// <returns></returns>
        private static JSONSave CreateNewGameFile()
        {
            string newGame = "{\"hash\": \"";
            string hash = CreateMD5("{\"c\":0,\"i\":0,\"h\":0}");
            newGame += hash;
            newGame += "\",\"c\":0,\"i\":0,\"h\":0}";

            PlayerPrefs.SetString("Save", newGame);
            PlayerPrefs.Save();
            return JsonUtility.FromJson<JSONSave>(newGame);
        }

        /// <summary>
        /// guarda el progreso
        /// </summary>
        public void ToJson()
        {
            string newHash = CreateMD5("{\"c\":" + c + ",\"i\":" + i + ",\"h\":" + h + "}");
            hash = newHash;
            PlayerPrefs.SetString("Save", JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }

        /// <summary>
        /// crea el hash del progreso
        /// </summary>
        /// <param name="input">progreso sin hash</param>
        /// <returns>hash</returns>
        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}