using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    class Stack<T>
    {
        private Object[] array;
        private int pointer = -1;

        public Stack(int size)
        {
            this.array = new Object[size];
        }

        public void push(T symbol)
        {
            if (pointer + 1 != array.Length) array[++pointer] = symbol;
            else
            {
                Console.WriteLine("Out of stack memory!");
            }
        }

        public T pop()
        {
            if (pointer >= 0) return (T)array[pointer--];
            else
            {
                Console.WriteLine("Empty stack space!");
                return (T)new Object();
            }
        }

        public T peek()
        {
            if (pointer >= 0) return (T)array[pointer];
            else
            {
                Console.WriteLine("Empty stack space!");
                return (T)new Object();
            }
        }

        public bool isEmpty()
        {
            if (pointer == -1) return true;
            return false;
        }

        public bool isFree()
        {
            if (pointer < array.Length - 1) return true;
            return false;
        }
    }
}
