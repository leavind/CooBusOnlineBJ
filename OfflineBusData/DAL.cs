using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

public class DAL
{

    public static DataSet GetDataSet(string Querystr, SQLiteConnection connStr)
    {
        DataSet ds = new DataSet("SqlDataset");
        SQLiteDataAdapter da = new SQLiteDataAdapter(Querystr, connStr);
        try
        {
            da.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            MessageBox.Show("DAL.SQL.GetDataSet:" + ex.Message, "错误");
            return null;
        }
    }

    public static DataTable GetDataTable(string Querystr, SQLiteConnection connStr)
    {
        DataTable dt = new DataTable();
        SQLiteDataAdapter da = new SQLiteDataAdapter(Querystr, connStr);
        try
        {
            da.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            MessageBox.Show("DAL.SQL.GetDataTable:" + ex.Message, "错误");
            return null;
        }
    }

    public static object ExecuteScalar(string Querystr, SQLiteConnection connStr)
    {
        SQLiteCommand cmd = new SQLiteCommand(Querystr, connStr);
        if (!(connStr.State == ConnectionState.Open))
            connStr.Open();
        return cmd.ExecuteScalar();
    }

    public static int ExecuteNonQuery(string Querystr, SQLiteConnection connStr)
    {
        SQLiteCommand cmd = new SQLiteCommand(Querystr, connStr);
        if (!(connStr.State == ConnectionState.Open))
            connStr.Open();
        return cmd.ExecuteNonQuery();
    }

    public static SQLiteDataReader ExecuteReader(string Querystr, SQLiteConnection connStr)
    {
        SQLiteCommand cmd = new SQLiteCommand(Querystr, connStr);
        if (!(connStr.State == ConnectionState.Open))
            connStr.Open();
        return cmd.ExecuteReader();
    }
}
