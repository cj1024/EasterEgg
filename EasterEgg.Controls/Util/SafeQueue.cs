using System.Collections.Generic;

namespace EasterEgg.Controls.Util
{

    public static class SafeQueue
    {

        public static T SafeDequeue<T>(this Queue<T> queue)
        {
            return queue.SafeDequeue(default(T));
        }

        public static T SafeDequeue<T>(this Queue<T> queue, bool createNew) where T : new()
        {
            return queue.SafeDequeue(createNew ? new T() : default(T));
        }

        public static T SafeDequeue<T>(this Queue<T> queue, T defaultValue)
        {
            if (queue.Count > 0)
            {
                return queue.Dequeue();
            }
            return defaultValue;
        }

    }

}
