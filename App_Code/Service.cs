using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using System.Data;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class Service : System.Web.Services.WebService
{
    public Service () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    [ScriptMethod (ResponseFormat = ResponseFormat.Json)]
    public DataTable GetBookList()
    {
        clDB db = new clDB();
        DataTable dt = new DataTable();

        string strSQL;

        strSQL = "SELECT * " +
                "FROM BookListing";

        try
        {
            db.OpenConn();

            dt = db.ExecSelectQuery(strSQL);
            dt.TableName = "TestTable";
            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //string jsonresult = serializer.Serialize(dt);

            

            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception("ERROR:", ex);
        }


        
    }
    
}
