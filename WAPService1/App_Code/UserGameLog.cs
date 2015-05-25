using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// UserGameLog 的摘要说明
/// </summary>
[WebService(Namespace = "WAPService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class UserGameLog : System.Web.Services.WebService {

    public UserGameLog () {
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    
}
