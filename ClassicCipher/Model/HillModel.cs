using System;
using System.Text;

namespace ClassicCipher.Model
{
    public class HillModel
    {
        private Random _random;
        //  矩阵阶数
        private int _level;
        //  加密矩阵
        private long[][] _matrix;
        //  解密矩阵
        private long[][] _inverseMatrix = null;

        //  用于填充的无效字符
        const char INVALID_CHAR = 'A';

        /// <summary>
        /// 带阶数的构造函数
        /// </summary>
        /// <param name="level">矩阵阶数</param>
        public HillModel(int level)
        {
            _random = new Random();
            _level = level;
        }

        public void GetRandomKeyMatrix()
        {
            _inverseMatrix = null;
            while (InverseMatrix == null)
            {
                _matrix = getRandomMatrix();
                _inverseMatrix = getInverseMatrix(_matrix);
            }
        }

        public void ImportKeyMatrix(long[][] array)
        {
            _inverseMatrix = getInverseMatrix(array);
            if (_inverseMatrix == null)
            {
                throw new Exception("该数组不可逆！");
            }

            _matrix = new long[Level][];
            for (int i = 0; i < Level; i++)
            {
                _matrix[i] = new long[Level];
                for (int j = 0; j < Level; j++)
                {
                    _matrix[i][j] = array[i][j];
                }
            }
        }

        #region Properties


        public int Level
        {
            get
            {
                return _level;
            }
        }

        /// <summary>
        /// 当前矩阵
        /// </summary>
        public long[][] Matrix
        {
            get
            {
                return _matrix;
            }
        }

        public long[][] InverseMatrix
        {
            get
            {
                return _inverseMatrix;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个新的整数矩阵
        /// </summary>
        /// <returns>矩阵</returns>
        public long[][] getNewMatrix()
        {
            long[][] res = new long[_level][];
            for (int i = 0; i < _level; ++i) res[i] = new long[_level];
            for (int i = 0; i < _level; ++i)
                for (int j = 0; j < _level; ++j) res[i][j] = 0;
            return res;
        }

        /// <summary>
        /// 得到一个n阶整数矩阵
        /// </summary>
        /// <param name="level">阶数</param>
        /// <returns>矩阵</returns>
        public static long[][] getNewMatrix(int level)
        {
            long[][] res = new long[level][];
            for (int i = 0; i < level; ++i) res[i] = new long[level];
            for (int i = 0; i < level; ++i)
                for (int j = 0; j < level; ++j) res[i][j] = 0;
            return res;
        }

        /// <summary>
        /// 求关于MOD26的逆矩阵
        /// </summary>
        /// <param name="o">原矩阵</param>
        /// <returns>逆矩阵</returns>
        public long[][] getInverseMatrix(long[][] o)
        {
            long[][] res = getNewMatrix();
            long[][] original = getNewMatrix();

            for (int i = 0; i < _level; ++i)
            {
                for (int j = 0; j < _level; ++j)
                {
                    if (i == j) res[i][j] = 1;
                    else res[i][j] = 0;
                    original[i][j] = o[i][j];
                }
            }
            for (int k = 0; k < _level; ++k)
            {
                bool isGCD = false;
                for (int i = k; i < _level; ++i)
                {
                    if (GCD(original[i][k], 26) == 1)
                    {
                        isGCD = true;
                        if (i != k)
                        {
                            long[] temp1 = original[i], temp2 = res[i];
                            original[i] = original[k]; res[i] = res[k];
                            original[k] = temp1; res[k] = temp2;
                        }
                        break;
                    }
                }
                //  若矩阵一列中没有与26互素的元素，则认为该矩阵不可逆
                if (!isGCD) return null;
                long ie = getInverseElement(original[k][k], 26);
                Console.WriteLine(original[k][k] + "的逆元是：" + ie);
                if (-1 == ie) return null;
                for (int j = 0; j < _level; ++j)
                {
                    original[k][j] = (original[k][j] * ie) % 26;
                    res[k][j] = (res[k][j] * ie) % 26;
                }
                for (int i = k + 1; i < _level; ++i)
                {
                    long l = original[i][k] / original[k][k];
                    for (int j = 0; j < _level; ++j)
                    {
                        //  对增广矩阵的运算
                        res[i][j] = getMOD((res[i][j] - l * res[k][j]), 26);
                        //  对原矩阵的运算
                        original[i][j] = getMOD((original[i][j] - l * original[k][j]), 26);
                    }
                }
            }
            for (int k = _level - 1; k > 0; --k)
            {
                if (original[k][k] == 0) return null;
                for (int i = k - 1; i >= 0; --i)
                {
                    long l = original[i][k] / original[k][k];

                    //  对增广矩阵的运算
                    for (int j = 0; j < _level; ++j)
                    {
                        if (res[k][j] == 0) continue;
                        res[i][j] = getMOD((res[i][j] - l * res[k][j]), 26);
                    }
                    //  对原矩阵的运算
                    original[i][k] = getMOD((original[i][k] - l * original[k][k]), 26);
                }
            }
            return res;
        }

        private long getMOD(long x, long m)
        {
            while (x < m)
            {
                x += m;
            }
            return x % m;
        }

        /// <summary>
        /// 求a关于m的乘法逆元
        /// </summary>
        /// <param name="a"></param>
        /// <param name="m"></param>
        /// <returns>逆元</returns>
        public static long getInverseElement(long a, long m)
        {
            long x = 0, y = 0;
            long gcd = E_GCD(a, m, ref x, ref y);
            if (1 % gcd != 0) return -1;
            x *= 1 / gcd;
            m = Math.Abs(m);
            long res = x % m;
            if (res <= 0) res += m;
            return res;
        }

        /// <summary>
        /// 拓展欧几里德算法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>GCD（a， b）</returns>
        public static long E_GCD(long a, long b, ref long x, ref long y)
        {
            if (0 == b)
            {
                x = 1;
                y = 0;
                return a;
            }
            long res = E_GCD(b, a % b, ref x, ref y);
            long temp = x;
            x = y;
            y = temp - a / b * y;
            return res;
        }

        /// <summary>
        /// 求最大公约数
        /// </summary>
        /// <param name="x">第一个参数</param>
        /// <param name="y">第二个参数</param>
        /// <returns>最大公约数</returns>
        static public long GCD(long x, long y)
        {
            if (y == 0) return x;
            return GCD(y, x % y);
        }

        int GetRandomSeed()
        {
            return _random.Next();
        }

        private long[][] getRandomMatrix()
        {
            long[][] res = getNewMatrix();

            for (int i = 0; i < _level; ++i)
            {
                for (int j = 0; j < _level; ++j)
                {
                    int t;
                    Random rd = new Random(GetRandomSeed());
                    t = rd.Next(0, 26);
                    res[i][j] = t;
                }
            }
            return res;
        }

        private string getOneGroup(string input, long[][] matrix)
        {
            StringBuilder sb = new StringBuilder();
            int[] p = new int[_level];
            for (int i = 0; i < _level; ++i)
            {
                if (i < input.Length)
                    p[i] = input[i] - 'A';
                else p[i] = INVALID_CHAR;
            }
            for (int i = 0; i < _level; ++i)
            {
                long o = 0;
                for (int j = 0; j < _level; ++j)
                {
                    o += matrix[i][j] * p[j];
                }
                Console.Write(o.ToString() + " ");
                sb.Append((char)(o % 26 + 'A'));
            }
            Console.WriteLine();
            return sb.ToString();
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input">请确保输入的字符串只有字母</param>
        /// <returns></returns>
        public string Encrypt(string input)
        {
            StringBuilder sb = new StringBuilder();
            input = input.ToUpper();
            for (int i = 0; i < input.Length; i += _level)
            {
                int end = _level < (input.Length - i) ? _level : (input.Length - i);
                sb.Append(getOneGroup(input.Substring(i, end), _matrix));
            }
            return sb.ToString();
        }

        public string Decrypt(string input)
        {
            StringBuilder sb = new StringBuilder();
            input = input.ToUpper();
            for (int i = 0; i < input.Length; i += _level)
            {
                int end = _level < (input.Length - i) ? _level : (input.Length - i);
                sb.Append(getOneGroup(input.Substring(i, end), _inverseMatrix));
            }
            return sb.ToString();
        }
    }
}
