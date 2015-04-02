using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Controls;

namespace LostAndFound.ViewModels
{

    class ReportFoundViewModel
    {
        public ReportFoundViewModel()
        {
            //var dtExcel = new DataTable();

            //dtExcel.TableName = "MyExcelData";

            //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='C:\git\LostAndFound\LostAndFound\LostAndFound\Resources\LostAndFound.xlsx';Extended Properties= 'Excel 12.0 Xml;HDR=Yes;IMEX=1'";

            //OleDbConnection con = new OleDbConnection(SourceConstr);
            //string query = "Select * from [Sheet1$]";

            //OleDbDataAdapter data = new OleDbDataAdapter(query, con);

            //data.Fill(dtExcel);


            //for (int i = 0; i < dtExcel.Rows.Count; i++)
            //{
            //    DataRow drow = dtExcel.Rows[i];

            //    //Todo: Creat a model for the obejcts here and add them to the list so we can iterate over that list.
            //}   

        }

        public String Text { get; set; }

    }
}
