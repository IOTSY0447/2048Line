/* Brief: Config Util
 * Author: Komal
 * Date: "2019-07-10"
 */
namespace komal {
    public partial class KomalUtil 
    {
#region Common
        public string GetPathsInfo(){
            return string.Format(@"streamingAssetsPath: {0} persistentDataPath: {1} dataPath: {2} ", streamingAssetsPath, persistentDataPath, UnityEngine.Application.dataPath);
        }
#endregion

#region StreamingAssets
        private string streamingAssetsPath {
            get {
                return UnityEngine.Application.streamingAssetsPath;
            }
        }

        private string GetFilePathOfStreamingAssetsPath(string filePath){
            return streamingAssetsPath + "/" + filePath;
        }

        public T ReadFromStreamAssets<T>(string jsonFilePath)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader( GetFilePathOfStreamingAssetsPath(jsonFilePath) );
            if (reader != null)
            {
                return UnityEngine.JsonUtility.FromJson<T>(reader.ReadToEnd());
            }else{
                throw new System.ArgumentException();
            }
        }
#endregion


#region PersistentData
        private string persistentDataPath {
            get {
                return UnityEngine.Application.persistentDataPath;
            }
        }

        public string GetFilePathOfPersistentDataPath(string filePath){
            return persistentDataPath + "/" + filePath;
        }

        public bool IsFileExistInPersistentDataPath(string filePath){
            return System.IO.File.Exists( GetFilePathOfPersistentDataPath(filePath) );
        }

        public void RemoveFile(string jsonFilePath){
            if(IsFileExistInPersistentDataPath(jsonFilePath)){
                System.IO.FileInfo fileInfo = new System.IO.FileInfo( GetFilePathOfPersistentDataPath(jsonFilePath) );
                if(fileInfo != null){
                    fileInfo.Delete();
                }
            }else{
                UnityEngine.Debug.Log(string.Format("File {0} is not Exist in {1}!", jsonFilePath, persistentDataPath));
            }
            // Editor Mode TODO
            // UnityEditor.FileUtil.DeleteFileOrDirectory("yourPath/YourFileOrFolder");
        }

        public T ReadFromPersistentData<T>(string jsonFilePath){
            System.IO.StreamReader reader = new System.IO.StreamReader( GetFilePathOfPersistentDataPath(jsonFilePath) );
            if (reader != null)
            {
                string jsonString = reader.ReadToEnd();
                return UnityEngine.JsonUtility.FromJson<T>(jsonString);
            }else{
                throw new System.ArgumentException();
            }
        }

        public void WriteToPersistentData<T>(string jsonFilePath, T data, bool append = false){
            System.IO.StreamWriter writer = new System.IO.StreamWriter( GetFilePathOfPersistentDataPath(jsonFilePath) );
            if (writer != null)
            {
                string jsonString = UnityEngine.JsonUtility.ToJson(data);
                writer.Write(jsonString);
                writer.Close();
            }else{
                throw new System.ArgumentException();
            }
        }
#endregion
    }
}
