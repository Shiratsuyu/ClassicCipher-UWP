using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCipher.Model
{
    public class PlayfairModel
    {
        // const var
        const int ALPHABET_LENGTH = 25;
        const int A = 65;
        const int J = 74;
        const int Z = 90;
        char[] playfairLine;

        /// <summary>
        /// 创建一个新的Playfair实例
        /// </summary>
        /// <param name="keyword">生成密钥矩阵所需的密钥</param>
        public PlayfairModel(string keyword)
        {
            this.playfairLine = CreatePlayfairLine(keyword);
        }

        /// <summary>
        /// 重新设定更改当前实例的密钥
        /// </summary>
        /// <param name="keyword">生成密钥矩阵所需的密钥</param>
        public void SetPkayfairKey(string keyword)
        {
            this.playfairLine = CreatePlayfairLine(keyword);
        }

        /// <summary>
        /// 使用当前的密钥矩阵对明文进行加密
        /// </summary>
        /// <param name="plaintext">待加密的明文</param>
        /// <returns>返回使用当前密钥矩阵加密后的结果</returns>
        public string Encrypt(string plaintext)
        {
            return Encrypt(plaintext, this.playfairLine);
        }

        /// <summary>
        /// 使用当前的密钥矩阵对密文进行解密
        /// </summary>
        /// <param name="cipherText">待解密的密文</param>
        /// <returns>返回使用当前密钥矩阵解密后的结果</returns>
        public string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, this.playfairLine);
        }

        /// <summary>
        /// 使用给定的密钥矩阵对明文进行加密
        /// </summary>
        /// <param name="plaintext">待加密的明文</param>
        /// <param name="playfairLine">用于加密明文的密钥矩阵</param>
        /// <returns>返回使用给定密钥矩阵加密后的结果</returns>
        private static string Encrypt(string plaintext, char[] playfairLine)
        {
            char[] text = CleanString(plaintext, true);
            StringBuilder result = new StringBuilder(text.Length);
            int[] bufferPos = new int[2];
            int[] pos = new int[2];
            for (int i = 0; i < text.Length; i++)
            {

                if ((i & 1) == 0)
                {
                    bufferPos = SearchInArray(text[i], playfairLine);
                    continue;
                }
                pos = SearchInArray(text[i], playfairLine);

                if (bufferPos[0] == pos[0])
                {

                    bufferPos[1]++;
                    bufferPos[1] -= ((bufferPos[1] * 7) >> 5) * 5;
                    pos[1]++;
                    pos[1] -= ((pos[1] * 7) >> 5) * 5;

                    if (bufferPos[1] == pos[1])
                    {

                        bufferPos[0]++;
                        bufferPos[0] -= ((bufferPos[0] * 7) >> 5) * 5;
                        pos[0]++;
                        pos[0] -= ((pos[0] * 7) >> 5) * 5;
                        result.Append(playfairLine[(bufferPos[0] * 5) + bufferPos[1]]);
                        result.Append(playfairLine[(pos[0] * 5) + pos[1]]);
                        continue;
                    }
                }
                else if (bufferPos[1] == pos[1])
                {

                    bufferPos[0]++;
                    bufferPos[0] -= ((bufferPos[0] * 7) >> 5) * 5;
                    pos[0]++;
                    pos[0] -= ((pos[0] * 7) >> 5) * 5;
                }
                else
                {

                    int buffer = bufferPos[1];
                    bufferPos[1] = pos[1];
                    pos[1] = buffer;
                }
                result.Append(playfairLine[(bufferPos[0] * 5) + bufferPos[1]]);
                result.Append(playfairLine[(pos[0] * 5) + pos[1]]);
            }
            return result.ToString();
        }

        /// <summary>
        /// 使用给定的密钥矩阵对密文进行解密
        /// </summary>
        /// <param name="cipherText">待解密的密文</param>
        /// <param name="playfairLine">用于解密密文的密钥矩阵</param>
        /// <returns>返回使用给定密钥矩阵解密后的结果</returns>
        private static string Decrypt(string cipherText, char[] playfairLine)
        {
            char[] text = CleanString(cipherText);
            StringBuilder result = new StringBuilder(text.Length);

            int[] bufferPos = new int[2];
            int[] pos = new int[2];
            for (int i = 0; i < text.Length; i++)
            {
                if ((i & 1) == 0)
                {
                    bufferPos = SearchInArray(text[i], playfairLine);
                    continue;
                }
                pos = SearchInArray(cipherText[i], playfairLine);
                if (bufferPos[0] == pos[0])
                {
                    bufferPos[1] += 9;
                    bufferPos[1] -= ((bufferPos[1] * 7) >> 5) * 5;
                    pos[1] += 9;
                    pos[1] -= ((pos[1] * 7) >> 5) * 5;

                    if (bufferPos[1] == pos[1])
                    {
                        bufferPos[0] += 9;
                        bufferPos[0] -= ((bufferPos[0] * 7) >> 5) * 5;
                        pos[0] += 9;
                        pos[0] -= ((pos[0] * 7) >> 5) * 5;
                        result.Append(playfairLine[(bufferPos[0] * 5) + bufferPos[1]]);
                        result.Append(playfairLine[(pos[0] * 5) + pos[1]]);
                        continue;
                    }
                }
                else if (bufferPos[1] == pos[1])
                {
                    bufferPos[0] += 9;
                    bufferPos[0] -= ((bufferPos[0] * 7) >> 5) * 5;
                    pos[0] += 9;
                    pos[0] -= ((pos[0] * 7) >> 5) * 5;
                }
                else
                {
                    int buffer = bufferPos[1];
                    bufferPos[1] = pos[1];
                    pos[1] = buffer;
                }
                result.Append(playfairLine[(bufferPos[0] * 5) + bufferPos[1]]);
                result.Append(playfairLine[(pos[0] * 5) + pos[1]]);
            }
            return result.ToString();
        }

        /// <summary>
        /// 使用给定的密钥对密文进行解密
        /// </summary>
        /// <param name="plaintext">待加密的明文</param>
        /// <param name="keyword">用于加密明文的密钥</param>
        /// <returns>返回使用给定密钥加密后的结果</returns>
        public static string Encrypt(string plaintext, string keyword)
        {
            return Encrypt(plaintext, CreatePlayfairLine(keyword));
        }

        /// <summary>
        /// 使用给定的密钥对密文进行解密
        /// </summary>
        /// <param name="cipherText">待解密的密文</param>
        /// <param name="keyword">用于解密密文的密钥</param>
        /// <returns>返回使用给定密钥解密后的结果</returns>
        public static string Decrypt(string cipherText, string keyword)
        {
            return Decrypt(cipherText, CreatePlayfairLine(keyword));
        }

        /// <summary>
        /// 使用给定的密钥创建一个密钥矩阵
        /// </summary>
        /// <param name="keyword">生成密钥矩阵所需的密钥</param>
        /// <returns>返回</returns>
        public static char[] CreatePlayfairLine(string keyword)
        {
            char[] input = CleanString(keyword);
            char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] result = new char[25];
            bool[] alreadyGiven = new bool[25];

            int pointer = 0;
            for (int i = 0; i < input.Length; i++)
            {
                int charValue = (int)input[i];

                if (charValue >= J)
                {
                    charValue--;
                }
                if (!alreadyGiven[charValue - A])
                {
                    alreadyGiven[charValue - A] = true;
                    result[pointer++] = input[i];
                }
            }

            if (pointer < ALPHABET_LENGTH)
            {
                for (int i = 0; i < ALPHABET_LENGTH; i++)
                {
                    if (!alreadyGiven[i])
                    {
                        result[pointer++] = alphabet[i];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 清理将要加密的字符串
        /// </summary>
        /// <param name="text">需要清理、将要被加密的字符串</param>
        /// <param name="forCipher">是否作为密文</param>
        /// <returns>清理后的字符串</returns>
        private static char[] CleanString(string text, bool forCipher = false)
        {
            char[] input = text.ToUpper().ToCharArray();
            List<char> result = new List<char>(input.Length);

            int pointer = -1;
            for (int i = 0; i < input.Length; i++)
            {
                pointer++;

                switch (input[i])
                {
                    // Ä or Æ -> AE
                    case 'Ä':
                    case 'Æ':
                        if (forCipher && pointer > 0 && result[pointer - 1] == 'A')
                        {
                            result.Add('X');
                            pointer++;
                        }
                        result.Add('A');
                        result.Add('E');
                        pointer++;
                        continue;
                    // Ö or Ø -> OE
                    case 'Ö':
                    case 'Ø':
                        if (forCipher && pointer > 0 && result[pointer - 1] == 'O')
                        {
                            result.Add('X');
                            pointer++;
                        }
                        result.Add('O');
                        result.Add('E');
                        pointer++;
                        continue;
                    // Ü -> UE
                    case 'Ü':
                        if (forCipher && pointer > 0 && result[pointer - 1] == 'U')
                        {
                            result.Add('X');
                            pointer++;
                        }
                        result.Add('U');
                        result.Add('E');
                        pointer++;
                        continue;
                    // Å -> AA
                    case 'Å':
                        if (forCipher && pointer > 0 && result[pointer - 1] == 'A')
                        {
                            result.Add('X');
                            pointer++;
                        }
                        result.Add('A');
                        if (forCipher)
                        {
                            result.Add('X');
                            pointer++;
                        }
                        result.Add('A');
                        pointer++;
                        continue;
                    // ß -> SS
                    case 'ß':
                        if (forCipher && pointer > 0 && result[pointer - 1] == 'S')
                        {
                            result.Add('X');
                            pointer++;
                        }
                        result.Add('S');
                        if (forCipher)
                        {
                            result.Add('X');
                            pointer++;
                        }
                        result.Add('S');
                        pointer++;
                        continue;
                }

                if ((int)input[i] < A || (int)input[i] > Z)
                {
                    pointer--;
                    continue;
                }
                if (forCipher && pointer > 0 && result[pointer - 1] == input[i])
                {
                    result.Add('X');
                }
                result.Add(input[i]);
            }
            if (forCipher && (result.Count & 1) == 1)
            {
                result.Add('X');
            }
            return result.ToArray();
        }

        /// <summary>
        /// 字符查找，对字符在数组中的查找的封装
        /// </summary>
        /// <param name="c">需要查找的字符</param>
        /// <param name="array">需要进行字符查找的数组</param>
        /// <returns>返回两个元素的位置</returns>
        private static int[] SearchInArray(char c, char[] array)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (array[(i * 5) + j] == c)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return null;
        }
    }

}
