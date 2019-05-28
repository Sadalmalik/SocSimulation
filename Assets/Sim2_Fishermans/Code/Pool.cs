using System;
using System.Collections.Generic;

namespace Simulation2
{
    internal class Pool<T>
    {
        public Func<T> constructor_;
        private Stack<T> stack_;

        public Pool(Func<T> ctr)
        {
            constructor_ = ctr;
            stack_ = new Stack<T>();
        }

        public T Get()
        {
            if (stack_.Count == 0)
                return constructor_();
            return stack_.Pop();
        }

        public void Free(T t)
        {
            stack_.Push(t);
        }
    }
}
