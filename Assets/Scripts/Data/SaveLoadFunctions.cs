using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using SaveData;
using UnityEngine;

namespace Data {
    public static class SaveLoadFunctions {
        public static void SaveFile(string filename, object o) {
            var bf = new BinaryFormatter();
            var path = $"{Application.persistentDataPath}/{filename}";
            var file = File.Create(path);
            bf.Serialize(file, o);
            file.Close();
        }

        [CanBeNull]
        public static void Load(out List<Profile> profiles) {
            profiles = LoadData(SaveLoadConstants.ProfileSaveFilename, out List<Profile> data)
                ? data
                : new List<Profile>{ new Profile() };
        }

        private static bool LoadData<T>(string filename, out T result) {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (DebugFlags.DebugDontLoadGame) {
                result = default;
                return false;
            }

            var path = $"{Application.persistentDataPath}/{filename}";
            if (File.Exists(path)) {
                var bf = new BinaryFormatter();
                var file = File.Open(path, FileMode.Open);
                try {
                    result = (T) bf.Deserialize(file);
                }
                catch (SerializationException) {
                    file.Close();
                    File.Delete(path);
                    Debug.LogError($"Failed to deserialize `{path}`.");
                    result = default;
                    return false;
                }

                file.Close();
                return true;
            }

            result = default;
            return false;
        }
    }
}