using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQLib
{
    public class PriorityQueue<T>
     where T : IComparable<T>
    {
        public List<T> Data { get; private set; }
        public int Count => Data.Count;
        public PriorityQueue(int capacity)
        {
            //left = index * 2 + 1
            //right = index * 2 + 2
            //parent = (index - 1) / 2
            Data = new List<T>(capacity);
        }

        public void Insert(T value)
        {
            Data.Add(value);
            HeapifyUp(Count - 1);
        }

        private void HeapifyUp(int index)
        {
            int parent = (index - 1) / 2;
            if (index == 0) return;

            if (Data[index].CompareTo(Data[parent]) < 0)
            {
                var temp = Data[index];
                Data[index] = Data[parent];
                Data[parent] = temp;
            }
            HeapifyUp(parent);
        }

        public T Pop()
        {
            var root = Data[0];

            Data[0] = Data[Count - 1];
            Data.RemoveAt(Count - 1);

            HeapifyDown(0);

            return root;
        }

        private void HeapifyDown(int parent)
        {
            int left = parent * 2 + 1;
            int right = parent * 2 + 2;
            if (left >= Count)
            {
                return;
            }

            int swapWith;
            if (right >= Count)
            {
                swapWith = left;
            }
            else
            {
                swapWith = Data[left].CompareTo(Data[right]) < 0 ? left : right;
            }

            if (Data[parent].CompareTo(Data[swapWith]) > 0)
            {
                var temp = Data[swapWith];
                Data[swapWith] = Data[parent];
                Data[parent] = temp;
            }
            HeapifyDown(swapWith);
        }
    }
}
