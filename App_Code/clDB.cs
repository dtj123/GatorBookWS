using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for clDB
/// </summary>
public class clDB
{
    private string strConnString = "";
    private SqlConnection cn = new SqlConnection();
    private SqlTransaction trans;
        
	public clDB()
	{
		//
		// TODO: Add constructor logic here
		//
        trans = null;
	}
    public string ConnectionString
    {
        get
        {
            string strConn = "Data Source=whsql-v23.prod.mesa1.secureserver.net;Initial Catalog=ASOD;";
            strConn += "User ID=ASOD;Password='Oko1we*316';";
            return strConn;
        }
    }

    public static SqlParameterCollection GetParamCollection()
    {
        SqlCommand cmd = new SqlCommand();
         
        return cmd.Parameters;
    }

    public void OpenConn()
    {
        try
        {
            trans = null;
            if (cn.State == ConnectionState.Open)
            {
                return;
            }

            cn.ConnectionString = ConnectionString;
            cn.Open();
        }
        catch (Exception ex)
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            throw ex;
        }
    }

    public void OpenConnWithTrans()
    {
        try
        {
            if (cn.State == ConnectionState.Open)
            {
                return;
            }

            cn.ConnectionString = ConnectionString;
            cn.Open();
            trans = cn.BeginTransaction();
        }
        catch (Exception ex)
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            throw ex;
        }
    }

    public void CloseConn()
    {
        if (cn.State != ConnectionState.Closed)
        {
            cn.Close();
        }
    }

    public void CommitTrans()
    {
        if (trans != null)
        {
            trans.Commit();
            trans = null;
        }
    }

    public void CommitWithNewTrans()
    {
        if (trans != null)
        {
            trans.Commit();
        }
        trans = cn.BeginTransaction();
    }

    public void RollBackTrans()
    {
        if (trans != null)
        {
            trans.Rollback();
            trans = null;
        }
    }

    public void RollBackAndStartNewTrans()
    {
        if (trans != null)
        {
            trans.Rollback();
        }
        trans = cn.BeginTransaction();
    }

    public DataTable GetSchema(string strQuery)
    {
        SqlCommand cmd = new SqlCommand(strQuery, cn);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);

        da.FillSchema(dt, SchemaType.Source);

        return dt;
    }

    public DataTable ExecSelectQuery(string strQuery)
    {
        SqlCommand cmd = new SqlCommand(strQuery, cn);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);

        if (trans != null)
        {
            cmd.Transaction = trans;
        }

        try
        {
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw new Exception("SQL: " + strQuery + "\r\r" + ex.ToString(), ex);
        }

        return dt;
    }

    public DataTable ExecSelectQuery(string strQuery, SqlParameterCollection pParams)
    {
        SqlCommand cmd = new SqlCommand(strQuery, cn);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);

        foreach (SqlParameter pparam in pParams)
        {
            cmd.Parameters.Add(pparam);
        }

        if (trans != null)
        {
            cmd.Transaction = trans;
        }

        try
        {
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw new Exception("SQL: " + strQuery + "\r\r" + ex.ToString(), ex);
        }

        return dt;
    }

    public DataTable GetDropDownDT(string strTable, string strValueField, string strDisplayField, Boolean AddBlank, string strWhere, string strOrderField)
    {
        string strSQL;

        strSQL = "SELECT " + strValueField + " AS ValueField, " + strDisplayField + " AS TextField "
                + "FROM " + strTable;

        if (strWhere != "")
        {
            strSQL += " WHERE " + strWhere;
        }

        if (strOrderField != "")
        {
            strSQL += " ORDER BY " + strDisplayField;
        }
        else
        {
            strSQL += " ORDER BY " + strOrderField;
        }

        SqlCommand cmd = new SqlCommand(strSQL, cn);
        DataTable dt = new DataTable();
        SqlDataAdapter daCombo = new SqlDataAdapter(cmd);

        try
        {
            daCombo.Fill(dt);

            if (AddBlank == true)
            {
                //Add a blank row to the datatable
                DataRow newRow = dt.NewRow();
                newRow["TextField"] = "-Please Select-";
                dt.Rows.InsertAt(newRow, 0);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SQL: " + strSQL + "\r\r" + ex.ToString(), ex);
        }
        
        return dt;
    }

    public Int32 ExecNonQuery(string strQuery)
    {
        SqlCommand cmd = new SqlCommand(strQuery, cn);

        if (trans != null)
        {
            cmd.Transaction = trans;
        }

        try
        {
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("SQL: " + strQuery + "\r\r" + ex.ToString(), ex);
        }
    }

    public Int32 ExecNonQuery(string strQuery, SqlParameterCollection pParams)
    {
        SqlCommand cmd = new SqlCommand(strQuery, cn);

        foreach (SqlParameter pparam in pParams)
        {
            cmd.Parameters.Add(pparam);
        }

        if (trans != null)
        {
            cmd.Transaction = trans;
        }

        try
        {
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("SQL: " + strQuery + "\r\r" + ex.ToString(), ex);
        }
    }

    public Object ExecScalar(string strQuery)
    {
        SqlCommand cmd = new SqlCommand(strQuery, cn);

        if (trans != null)
        {
            cmd.Transaction = trans;
        }

        try
        {
            return cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw new Exception("SQL: " + strQuery + "\r\r" + ex.ToString(), ex);
        }
    }
    public Object ExecScalar(string strQuery, SqlParameterCollection pParams)
    {
        SqlCommand cmd = new SqlCommand(strQuery, cn);

        foreach (SqlParameter pparam in pParams)
        {
            cmd.Parameters.Add(pparam);
        }

        if (trans != null)
        {
            cmd.Transaction = trans;
        }

        try
        {
            return cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw new Exception("SQL: " + strQuery + "\r\r" + ex.ToString(), ex);
        }
    }
}
