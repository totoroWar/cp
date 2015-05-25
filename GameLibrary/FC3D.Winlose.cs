using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class FC3D
    {
        public int GetResultS1_1_1p1_1_1(string codes, string result)
        {
            return GetResultZHI_XFS(codes, result);
        }
        public int GetResultS1_1p0_1_1(string codes, string result)
        {
            result = result.Substring(2);
            return GetResultZHI_XFS(codes, result);
        }
        public int GetResultS1_1p1_1_0(string codes, string result)
        {
            result = result.Substring(0, 3);
            return GetResultZHI_XFS(codes, result);
        }
        public string Result1047()
        {
            return string.Format("185:{0}", GetResultS1_1_1p1_1_1(Codes, Result));
        }
        public string Result1048()
        {
            return string.Format("185:{0}", GetResultZHI_XDS(Codes, Result));
        }
        public string Result1049()
        {
            return string.Format("185:{0}", GetResultZHI_XHZ(Codes, Result));
        }
        public string Result1051()
        {
            return string.Format("187:{0}", GetResultZU3_FS(Codes, Result));
        }
        public string Result1052()
        {
            return string.Format("187:{0}", GetResultZU3_DS(Codes, Result));
        }
        public string Result1053()
        {
            return string.Format("188:{0}", GetResultZU6_FS(Codes, Result));
        }
        public string Result1054()
        {
            return string.Format("188:{0}", GetResultZU6_DS(Codes, Result));
        }
        public string Result1055()
        {
            return string.Format("188:{0}|187:{1}", GetResultZU6_DS(Codes, Result), GetResultZU3_DS(Codes, Result));
        }
        public string Result1056()
        {
            return string.Format("188:{0}|187:{1}", GetResultZU6_XHZ(Codes, Result), GetResultZU3_XHZ(Codes, Result));
        }
        public string Result1058()
        {
            return string.Format("190:{0}", GetResultZHI_XFS(Codes, Result.Substring(0, 3)));
        }
        public string Result1059()
        {
            return string.Format("190:{0}", GetResultZHI_XDS(Codes, Result.Substring(0, 3)));
        }
        public string Result1060()
        {
            return string.Format("194:{0}", GetResultZHI_XFS(Codes, Result.Substring(2)));
        }
        public string Result1061()
        {
            return string.Format("194:{0}", GetResultZHI_XDS(Codes, Result.Substring(2)));
        }
        public string Result1063()
        {
            return string.Format("192:{0}", GetResultZU2_FS(Codes, Result.Substring(0, 3)));
        }
        public string Result1064()
        {
            return string.Format("192:{0}", GetResultZU2_DS(Codes, Result.Substring(0, 3)));
        }
        public string Result1065()
        {
            return string.Format("196:{0}", GetResultZU2_FS(Codes, Result.Substring(2)));
        }
        public string Result1066()
        {
            return string.Format("196:{0}", GetResultZU2_DS(Codes, Result.Substring(2)));
        }
        public string Result1068()
        {
            return string.Format("198:{0}", GetResultDWD(Codes, Result));
        }
        public string Result1070()
        {
            return string.Format("200:{0}", GetResultS1p1_1_1(Codes, Result));
        }
        public string Result1071()
        {
            return string.Format("202:{0}", GetResultS2p1_1_1(Codes, Result));
        }
        public string Result1073()
        {
            return string.Format("225:{0}", GetResultDXDSp1_1(Codes, Result.Substring(0, 3)));
        }
        public string Result1074()
        {
            return string.Format("225:{0}", GetResultDXDSp1_1(Codes, Result.Substring(2)));
        }
    }
}
