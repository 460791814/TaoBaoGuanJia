using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoGuanJia.Util
{
    public class PermutationAndCombination<T>
    {
        public static void Swap(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }
        private static void GetCombination(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
        {
            for (int i = n; i >= m; i--)
            {
                b[m - 1] = i - 1;
                if (m > 1)
                {
                    PermutationAndCombination<T>.GetCombination(ref list, t, i - 1, m - 1, b, M);
                }
                else
                {
                    if (list == null)
                    {
                        list = new List<T[]>();
                    }
                    T[] array = new T[M];
                    for (int j = 0; j < b.Length; j++)
                    {
                        array[j] = t[b[j]];
                    }
                    list.Add(array);
                }
            }
        }
        private static void GetPermutation(ref List<T[]> list, T[] t, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                if (list == null)
                {
                    list = new List<T[]>();
                }
                T[] array = new T[t.Length];
                t.CopyTo(array, 0);
                list.Add(array);
                return;
            }
            for (int i = startIndex; i <= endIndex; i++)
            {
                PermutationAndCombination<T>.Swap(ref t[startIndex], ref t[i]);
                PermutationAndCombination<T>.GetPermutation(ref list, t, startIndex + 1, endIndex);
                PermutationAndCombination<T>.Swap(ref t[startIndex], ref t[i]);
            }
        }
        public static List<T[]> GetPermutation(T[] t, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex > t.Length - 1)
            {
                return null;
            }
            List<T[]> result = new List<T[]>();
            PermutationAndCombination<T>.GetPermutation(ref result, t, startIndex, endIndex);
            return result;
        }
        public static List<T[]> GetPermutation(T[] t)
        {
            return PermutationAndCombination<T>.GetPermutation(t, 0, t.Length - 1);
        }
        public static List<T[]> GetPermutation(T[] t, int n)
        {
            if (n > t.Length)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            List<T[]> combination = PermutationAndCombination<T>.GetCombination(t, n);
            for (int i = 0; i < combination.Count; i++)
            {
                List<T[]> collection = new List<T[]>();
                PermutationAndCombination<T>.GetPermutation(ref collection, combination[i], 0, n - 1);
                list.AddRange(collection);
            }
            return list;
        }
        public static List<T[]> GetCombination(T[] t, int n)
        {
            if (t.Length < n)
            {
                return null;
            }
            int[] b = new int[n];
            List<T[]> result = new List<T[]>();
            PermutationAndCombination<T>.GetCombination(ref result, t, t.Length, n, b, n);
            return result;
        }
    }
}
