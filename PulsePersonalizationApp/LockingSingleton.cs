namespace PulsePersonalizationApp
{
    public class LockingSingleton
    {
        // Used for locking critical code
        private LockingSingleton() { }

        private static LockingSingleton _instance;
        public object lockObj = new object();
        public bool isLocked = false;

        public static LockingSingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LockingSingleton();
            }
            return _instance;
        }

    }
}