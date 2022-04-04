using System;

namespace Fishing
{
    public static class Singletone<T>
    {
        public static readonly T Instance;

        static Singletone()
        {
            Instance = Activator.CreateInstance<T>();
        }
    }
}