using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCipher.Model
{
    class VigenereModel
    {
        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="plainText">待加密的明文字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>(plainText+key)mod 26</returns>
        public static string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "A";
            }
            string cipherText = "";
            for (int i = 0, j = 0; i < plainText.Length; i++, j++)
            {
                if (j % (key.Length) == 0)//对密钥取模运算，从而重复与明文对应
                    j = 0;
                cipherText += Convert.ToChar(plainText[i] + key[j] - 'A');//('p[i]'-'A')+('k[i]'-'A')+'A',先加后检测
                if (cipherText[i] > 'Z')//检测是否超过了Z
                {
                    char ch = Convert.ToChar(cipherText[i] - 26);//c[i]-=26,相当于26个字母对Z26（整数空间）取模运算。
                    cipherText = cipherText.Remove(i, 1);
                    cipherText = cipherText.Insert(i, Convert.ToString(ch));
                }
            }
            return cipherText;
        }

        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="cipherText">待解密的密文字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>(plainText-key)mod 26</returns>
        public static string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "A";
            }
            string plainText = "";
            for (int i = 0, j = 0; i < cipherText.Length; i++, j++)
            {
                if (j % key.Length == 0)//对密钥取模运算，从而重复与明文对应
                    j = 0;
                if (cipherText[i] < key[j])//c[i]<k[i],则减出来的char小于0会影响结果故单独处理。
                    plainText += Convert.ToChar((cipherText[i] - key[j]) % 26 + 26 + 'A');//取模加26,从而变正数，然后加'A'
                else
                {
                    plainText += Convert.ToChar(cipherText[i] - key[j] + 'A');//c[i]和k[i]的距离加上'A'
                    if (plainText[i] > 'Z')//超过Z的处理
                    {
                        char ch = Convert.ToChar(plainText[i] - 26);
                        plainText = plainText.Remove(i, plainText[i]);//删除字符plainText[i]
                        plainText = plainText.Insert(i, Convert.ToString(ch));//原plainText[i]减26再插入到plainText[i]
                    }
                }
            }
            return plainText;
        }
    }
}
