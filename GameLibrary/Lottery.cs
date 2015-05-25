using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class Lottery
    {
        public string Codes { get; set; }
        public string Result { get; set; }
        public string Position { get; set; }
        public bool Change { get; set; }
        public string UserCode { get; set; }
        public string CodeChange { get; set; }
        protected int ArrayItemLenghtProduct(string[] strArr, int maxIndex)
        {
            if (maxIndex == 0)
            {
                return strArr[maxIndex].Length;
            }
            return strArr[maxIndex].Length * ArrayItemLenghtProduct(strArr, maxIndex - 1);
        }
        protected int ArrayItemLenghtProduct(string[] strArr)
        {
            return ArrayItemLenghtProduct(strArr, strArr.Length - 1);
        }
        protected int Factorial(int n)
        {
            if (n < 0)
            {
                return 0;
            }
            if (n == 1 || n == 0)
            {
                return 1;
            }
            return n * Factorial(n - 1);
        }
        protected long Factorial(long n)
        {
            if (n < 0)
            {
                return 0;
            }
            if (n == 1 || n == 0)
            {
                return 1;
            }
            return n * Factorial(n - 1);
        }
        protected int Combination(int n, int m)
        {
            if (m > n || n < 0 || m < 0)
            {
                return 0;
            }
            return Factorial(n) / (Factorial(n - m) * Factorial(m));
        }
        protected int Permutation(int n, int m)
        {
            if (m > n || n < 0 || m < 0)
            {
                return 0;
            }
            return Factorial(n) / Factorial(n - m);
        }
        public virtual Dictionary<int, int> ResultStringToDictionary(string resultString)
        {
          Dictionary<int,int> resultDictioary=new Dictionary<int,int>();
          if (resultString.Contains('|'))
          {
              foreach (var item in resultString.Split('|'))
              {
                  string[] temp = item.Split(':');
                  resultDictioary.Add(int.Parse(temp[0]), int.Parse(temp[1]));
              }
          }
          else
          {
              string[] temp = resultString.Split(':');
              resultDictioary.Add(int.Parse(temp[0]),int.Parse(temp[1]));
          }
            return resultDictioary;
        }
    }
}
