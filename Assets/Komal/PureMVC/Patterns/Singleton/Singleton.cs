/* Brief: ISingleton
 * Author: Komal
 * Date: "2019-07-10"
 */
namespace komal.puremvc {

    interface ISingleton {
        void OnSingletonInit();
    }

    public class Singleton<T>: ISingleton where T : new()  {
        private static T _instance;
        public static T getInstance(){
            if(_instance == null){
                _instance = new T();
            }
            return _instance;
        } 

        public static T Instance
        {
            get {
                return getInstance();
            }
        }

        public virtual void OnSingletonInit(){

        }
    }
}
