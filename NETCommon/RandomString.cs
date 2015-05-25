using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETCommon
{
    public class RandomString
    {
        public static string Get(string allChar, int codeCount)
        {
            if (allChar.Length == 0)
            {
                allChar = "1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,~,!,@,#,$,%,^,&,*,(,),_,+,-,=";
            }
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }
    }
}
