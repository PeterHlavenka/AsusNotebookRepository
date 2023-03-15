using System;
using System.Text;
using System.IO ;
using System.Data.SqlClient;
namespace CustomerInfo
{
    public class CustomerLoader : System.MarshalByRefObject
    {
        // private SqlConnection myConnection = null ;
        // private SqlDataReader myReader ;
        internal CustomerLoader()
        {
            Console.WriteLine("New Customer Instance Created");
        }
        
        public event EventHandler<string> NecoJsemNapsal;

        public virtual void OnNecoJsemNapsal(string e)
        {
            NecoJsemNapsal?.Invoke(this, e);
        }
        
        
        
        
        
        
        // public void Init(string userid , string password)
        // {
        //     try
        //     {
        //         string myConnectString = "user id="+userid+";password="+password+
        //                                  ";Database=Northwind;Server=SKER;Connect Timeout=30";
        //         myConnection = new SqlConnection(myConnectString);
        //         myConnection.Open();
        //         if ( myConnection == null )
        //         {
        //             Console.WriteLine("OPEN NULL VALUE =====================");
        //             return ;
        //         }
        //     }
        //     catch(Exception es)
        //     {
        //         Console.WriteLine("[Error WITH DB CONNECT...] " + es.Message);
        //     }
        // }
        // public void ExecuteSelectCommand(string selCommand)
        // {
        //     Console.WriteLine("EXECUTING .. " + selCommand);
        //     SqlCommand myCommand = new SqlCommand(selCommand);
        //     if ( myConnection == null )
        //     {
        //         Console.WriteLine("NULL VALUE =====================");
        //         return ;
        //     }
        //     myCommand.Connection = myConnection;
        //     myCommand.ExecuteNonQuery();
        //     myReader = myCommand.ExecuteReader();
        // }
        // public string GetRow()
        // {
        //     if ( ! myReader.Read() )
        //     {
        //         myReader.Close();
        //         return "" ;
        //     }
        //     int nCol = myReader.FieldCount ;
        //     string outstr ="";
        //     for ( int i=0; i < nCol ; i ++)
        //     {
        //         if ( myReader.IsDBNull(i) ) continue ;
        //         outstr+= myReader.GetString(i);
        //     }
        //     return outstr;
        // }


    }
}