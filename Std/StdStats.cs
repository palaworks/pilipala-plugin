using System;

namespace Std
{
    public class StdStats
    {
        private StdStats()
        {
        }
        
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

        public static double mean(double[] a)
        {
            validateNotNull(a);

            if (a.Length == 0) return Double.NaN;
            double sum = StdStats.sum(a);
            return sum / a.Length;
        }
        public static double mean(double[] a, int lo, int hi)
        {
            validateNotNull(a);
            validateSubarrayIndices(lo, hi, a.Length);

            int length = hi - lo;
            if (length == 0) return Double.NaN;

            double sum = StdStats.sum(a, lo, hi);
            return sum / length;
        }
        public static double mean(int[] a)
        {
            validateNotNull(a);

            if (a.Length == 0) return Double.NaN;
            int sum = StdStats.sum(a);
            return 1.0 * sum / a.Length;
        }

        public static double var(double[] a)
        {
            validateNotNull(a);

            if (a.Length == 0) return Double.NaN;
            double avg = mean(a);
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / (a.Length - 1);
        }
        public static double var(double[] a, int lo, int hi)
        {
            validateNotNull(a);
            validateSubarrayIndices(lo, hi, a.Length);

            int length = hi - lo;
            if (length == 0) return Double.NaN;

            double avg = mean(a, lo, hi);
            double sum = 0.0;
            for (int i = lo; i < hi; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / (length - 1);
        }
        public static double var(int[] a)
        {
            validateNotNull(a);
            if (a.Length == 0) return Double.NaN;
            double avg = mean(a);
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / (a.Length - 1);
        }

        public static double varp(double[] a)
        {
            validateNotNull(a);
            if (a.Length == 0) return Double.NaN;
            double avg = mean(a);
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / a.Length;
        }
        public static double varp(double[] a, int lo, int hi)
        {
            validateNotNull(a);
            validateSubarrayIndices(lo, hi, a.Length);

            int length = hi - lo;
            if (length == 0) return Double.NaN;

            double avg = mean(a, lo, hi);
            double sum = 0.0;
            for (int i = lo; i < hi; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / length;
        }

        public static double stddev(double[] a)
        {
            validateNotNull(a);
            return Math.Sqrt(var(a));
        }
        public static double stddev(int[] a)
        {
            validateNotNull(a);
            return Math.Sqrt(var(a));
        }
        public static double stddev(double[] a, int lo, int hi)
        {
            validateNotNull(a);
            validateSubarrayIndices(lo, hi, a.Length);

            return Math.Sqrt(var(a, lo, hi));
        }

        public static double stddevp(double[] a)
        {
            validateNotNull(a);
            return Math.Sqrt(varp(a));
        }
        public static double stddevp(double[] a, int lo, int hi)
        {
            validateNotNull(a);
            validateSubarrayIndices(lo, hi, a.Length);

            return Math.Sqrt(varp(a, lo, hi));
        }

        private static double sum(double[] a)
        {
            validateNotNull(a);
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i];
            }
            return sum;
        }
        private static double sum(double[] a, int lo, int hi)
        {
            validateNotNull(a);
            validateSubarrayIndices(lo, hi, a.Length);

            double sum = 0.0;
            for (int i = lo; i < hi; i++)
            {
                sum += a[i];
            }
            return sum;
        }
        private static int sum(int[] a)
        {
            validateNotNull(a);
            int sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i];
            }
            return sum;
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

        public static void main(string[] args)
        {
            /*
            double[] a = StdArrayIO.readDouble1D();
            StdOut.printf("       min %10.3f\n", min(a));
            StdOut.printf("      mean %10.3f\n", mean(a));
            StdOut.printf("       max %10.3f\n", max(a));
            StdOut.printf("    stddev %10.3f\n", stddev(a));
            StdOut.printf("       var %10.3f\n", var(a));
            StdOut.printf("   stddevp %10.3f\n", stddevp(a));
            StdOut.printf("      varp %10.3f\n", varp(a));
            */
        }
    }
}
