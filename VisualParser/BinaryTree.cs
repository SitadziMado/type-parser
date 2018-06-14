using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    public class BinaryTree<T> : ICollection<T>
    {
        public BinaryTree()
        {

        }

        public BinaryTree(IEnumerable<T> data)
        {
            foreach (var v in data)
            {
                Add(v);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var queue = new Queue<T>();

            Dfs(queue, mRoot);

            while (queue.Count > 0)
            {
                yield return queue.Dequeue();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (mRoot == null)
            {
                mRoot = new Node(null, item);
            }
            else
            {
                Node cur = Find(item); ;

                int cmp = Compare(item, cur.Data);

                if (cmp == -1)
                {
                    cur.Left = new Node(cur, item);
                }
                else if (cmp == +1)
                {
                    cur.Right = new Node(cur, item);
                }
                else
                {
                    return;
                }
            }

            ++Count;
        }

        public void Clear()
        {
            mRoot = null;
        }

        public bool Contains(T item)
        {
            foreach (var v in this)
            {
                if (Compare(item, v) == 0)
                    return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            //array = new T [Count];

            int index = arrayIndex;

            foreach (var v in this)
                array[index++] = v;
        }

        public bool Remove(T item)
        {
            bool result = false;

            if (Count > 0)
            {
                var node = Find(item);

                if (node.Data.Equals(item))
                {
                    var parent = node.Parent;

                    if (parent == null)
                    {
                        mRoot = null;
                    }
                    else
                    {
                        if (parent.Left != null && parent.Left == node)
                        {
                            parent.Left = null;
                        }
                        else if (parent.Right != null && parent.Right == node)
                        {
                            parent.Right = null;
                        }
                    }

                    --Count;
                    result = true;
                }
            }

            return result;
        }

        private void Dfs(Queue<T> queue, Node cur)
        {
            if (cur == null)
                return;

            Dfs(queue, cur.Left);
            queue.Enqueue(cur.Data);
            Dfs(queue, cur.Right);
        }

        private int Compare(T lhs, T rhs)
        {
            return mComparer.Compare(lhs, rhs);
        }

        private Node Find(T item)
        {
            Node cur = mRoot;

            while (true)
            {
                int cmp = Compare(item, cur.Data);

                if (cmp == -1)
                {
                    if (cur.Left != null)
                    {
                        cur = cur.Left;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (cmp == +1)
                {
                    if (cur.Right != null)
                    {
                        cur = cur.Right;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return cur;
        }

        private class Node
        {
            public Node(Node parent, T data)
            {
                Parent = parent;
                Data = data;
            }

            public override string ToString()
            {
                return Data.ToString();
            }

            public T Data { get; set; }
            public Node Parent { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        public int Count { get; set; }
        public bool IsReadOnly => false;

        private Comparer<T> mComparer = Comparer<T>.Default;
        private Node mRoot = null;
    }

    public static class BinaryTreeUtility
    {
        public static BinaryTree<T> ToBinaryTree<T>(this IEnumerable<T> enumerable)
        {
            return new BinaryTree<T>(enumerable);
        }
    }
}
