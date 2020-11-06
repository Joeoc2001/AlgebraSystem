using System;
using System.Collections.Generic;
using System.Text;

namespace Algebra.Compilation
{
    namespace Default.Stack
    {
        internal struct FastStack<T>
        {
            private int top;
            private readonly T[] datas;

            public FastStack(int size)
            {
                top = -1;
                datas = new T[size];
            }

            public T Pop()
            {
                T item = datas[top];
                top--;
                return item;
            }

            public void Push(T item)
            {
                top++;
                datas[top] = item;
            }
        }
    }
}
