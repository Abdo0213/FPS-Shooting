using UnityEngine;
using System;
using System.IO;
public class FileDataHandler
{
    private string dataPath = "";
    private string dataFileName = "";
    public FileDataHandler(string dataPath, string dataFileName){
        this.dataPath = dataPath;
        this.dataFileName = dataFileName;
    }
    public GameData Load(){
        string fullPath = Path.Combine(dataPath, dataFileName);
        GameData loadData = null;
        if(File.Exists(fullPath)){
            try{
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open)){
                    using (StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            } catch(Exception e) {
                Debug.Log("Error occured when trying to save data to file:" + fullPath + "\n" + e);
            }
        }
        return loadData;
    }
    public void Save(GameData data){
        string fullPath = Path.Combine(dataPath, dataFileName);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)){
                using(StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }
        } catch(Exception e) {
            Debug.Log("Error occured when trying to save data to file:" + fullPath + "\n" + e);
        }
    }
}
