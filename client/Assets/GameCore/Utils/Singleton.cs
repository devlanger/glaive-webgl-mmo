using UnityEngine;

namespace GameCoreEngine
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object objLock = new object();
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                lock (objLock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = (T)FindObjectOfType(typeof(T));
                    }

                    return m_Instance;
                }
            }
        }
    }
}