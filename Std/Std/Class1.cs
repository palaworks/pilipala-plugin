using System;

namespace Std
{
    public class StdStats
    {
        public static double max(double[] a)
        {
            validateNotNull(a);

            double max = double.NegativeInfinity;
            for (int i = 0; i < a.Length; i++)
            {
                if (double.IsNaN(a[i])) return Double.NaN;
                if (a[i] > max) max = a[i];
            }
            return max;
        }
        public static double max(double[] a, int lo, int hi)
        {
            validateNotNull(a);
            validateSubarrayIndices(lo, hi, a.Length);

            double max = double.NegativeInfinity;
            for (int i = lo; i < hi; i++)
            {
                if (double.IsNaN(a[i])) return Double.NaN;
                if (a[i] > max) max = a[i];
            }
            return max;
        }
        public static int max(int[] a)
        {
            validateNotNull(a);

            int max = int.MinValue;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > max) max = a[i];
            }
            return max;
        }

        public static double min(double[] a)
        {
            validateNotNull(a);

            double min = double.PositiveInfinity;
            for (int i = 0; i < a.Length; i++)
            {
                if (double.IsNaN(a[i])) return Double.NaN;
                if (a[i] < min) min = a[i];
            }
            return min;
        }
        public static double min(double[] a, int lo, int hi)
        {
            validateNotNull(a);
            validateSubarrayIndices(lo, hi, a.Length);

            double min = double.PositiveInfinity;
            for (int i = lo; i < hi; i++)
            {
                if (double.IsNaN(a[i])) return Double.NaN;
                if (a[i] < min) min = a[i];
            }
            return min;
        }
        public static int min(int[] a)
        {
            validateNotNull(a);

            int min = int.MaxValue;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] < min) min = a[i];
            }
            return min;
        }

        private static void validateNotNull(object x)
        {
            if (x == null)
                throw new NotImplementedException("argument is null");//??
        }

        private static void validateSubarrayIndices(int lo, int hi, int length)
        {
            if (lo < 0 || hi > length || lo > hi)
                throw new NotImplementedException("subarray indices out of bounds: [" + lo + ", " + hi + ")");
        }

        /// <summary>
        /// 交换值的方法，引用类型
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void Exch<T>(ref T i, ref T j)
        {
            T temp = i;
            i = j;
            j = temp;
        }
        /// <summary>
        /// 判断小于的方法
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>若i小于j返回true</returns>
        public static bool Less<T>(T i, T j) where T : IComparable
        {
            return i.CompareTo(j) < 0;
        }
        /// <summary>
        /// 判断奇数的方法
        /// </summary>
        /// <param name="num">待判断的数值</param>
        /// <returns>num为奇数返回true，num为偶数返回false</returns>
        public static bool IsOdd(int num)
        {
            return (num % 2) == 1;
        }
    }
}
