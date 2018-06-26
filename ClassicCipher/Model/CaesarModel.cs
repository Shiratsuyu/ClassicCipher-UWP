using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCipher.Model
{
    class CaesarModel
    {
        public string CaesarEncrypt(string PlainText, int Offset)
        {
            char[] Encyclopedia = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char c in PlainText.ToUpper())
            {
                int idx = c - 'A';
                if (idx >= 0 && idx < Encyclopedia.Length)
                {
                    sb.Append(Encyclopedia[((idx + Offset) % 26)]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public string CaesarDecrypt(string PlainText, int Offset)
        {
            return CaesarEncrypt(PlainText, 26-Offset);
        }
    }
}
