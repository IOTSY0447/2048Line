/* Brief: IDConfig.json reader, support Editor Mode and Game Mode
 * Author: Komal
 * Date: "2019-07-10"
 */

#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

namespace komal {
    public class StreamAssetsConfig
    {
        private static Dictionary <string, object> m_ConfigDataDictionary =  new Dictionary<string, object>();
        private static T GetConfig<T>(string path, string key){
            object _data;
            if (!m_ConfigDataDictionary.TryGetValue(key, out _data)) {
                // the key isn't in the dictionary.
                _data = KomalUtil.Instance.ReadFromStreamAssets<T>(path);
                m_ConfigDataDictionary.Add(key, _data);
            }
            return (T)_data;
        }
        public static IDConfigData IDConfig {
            get {
                return GetConfig<IDConfigData>("Komal/IDConfig.json", "IDConfig");;
            }
        }

        [Serializable]
        public class IDConfigData
        {
            public string IronSourceAppKey;
            public string AdMobAppId;
            public string RemoveAds;
            public string GameTime;
            public string HighScore;
            public string AppUrl;
            public string UmengKey;
        }
    }
}
