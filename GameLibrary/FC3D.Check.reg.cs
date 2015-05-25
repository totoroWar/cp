using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class FC3D
    {
        static Regex reg1047 = new Regex(@"^(([0-9]\&){0,9}[0-9]\|){2}([0-9]\&){0,9}[0-9]$");
        static Regex reg1048 = new Regex(@"^([0-9]{3}\&)*[0-9]{3}$");
        static Regex reg1049 = new Regex(@"^(((1[0-9])|(2[0-7])|[0-9])\&){0,27}((1[0-9])|(2[0-7])|[0-9])$");
        static Regex reg1051 = new Regex(@"^([0-9]\&){1,9}[0-9]$");
        static Regex reg1052 = new Regex(@"^([0-9]{3}\&)*[0-9]{3}$");
        static Regex reg1053 = new Regex(@"^([0-9]\&){2,9}[0-9]$");
        static Regex reg1054 = new Regex(@"^([0-9]{3}\&)*[0-9]{3}$");
        static Regex reg1055 = new Regex(@"^([0-9]{3}\&)*[0-9]{3}$");
        static Regex reg1056 = new Regex(@"^(((1[0-9])|(2[0-6])|[1-9])\&){0,25}((1[0-9])|(2[0-6])|[1-9])$");
        static Regex reg1058 = new Regex(@"^(([0-9]\&){0,9}[0-9]\|)([0-9]\&){0,9}[0-9]$");
        static Regex reg1059 = new Regex(@"^([0-9]{2}\&)*[0-9]{2}$");
        static Regex reg1060 = reg1058;
        static Regex reg1061 = reg1059;
        static Regex reg1063 = reg1051;
        static Regex reg1064 = reg1059; 
        static Regex reg1065 = reg1051;
        static Regex reg1066 = reg1059;
        static Regex reg1068 = new Regex(@"^((([0-9]\&){0,9}[0-9])?\|){2}(([0-9]\&){0,9}[0-9])?$");
        static Regex reg1070 = new Regex(@"^([0-9]\&){0,9}[0-9]$");
        static Regex reg1071 = reg1051;
        static Regex reg1073 = new Regex(@"^((大|小|单|双)\&){0,3}(大|小|单|双)\|((大|小|单|双)\&){0,3}(大|小|单|双)$");
        static Regex reg1074 = reg1073;
    }
}
