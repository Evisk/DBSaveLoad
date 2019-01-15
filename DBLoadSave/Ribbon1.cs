using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;

namespace DBLoadSave
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }
        //Load Button functionality
        private void btnLoad_Click(object sender, RibbonControlEventArgs e)
        {
            //Connect to Webservice
            DBService.WebService1SoapClient client = new DBService.WebService1SoapClient("WebService1Soap");
            //Get active worksheet and build database names
            //More for looks then anything else
            Excel.Window window = e.Control.Context;
            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)window.Application.ActiveSheet);
            Excel.Range firstRow = activeWorksheet.get_Range("A1");
            Excel.Range secondRow = activeWorksheet.get_Range("B1");
            Excel.Range thirdRow = activeWorksheet.get_Range("C1");
            Excel.Range fourthRow = activeWorksheet.get_Range("D1");
            firstRow.Value2 = "ID";
            secondRow.Value2 = "First Name";
            thirdRow.Value2 = "Last Name";
            fourthRow.Value2 = "Age";

            //Load database into worksheet
            int a = 1;
            foreach (var i in client.LoadDB())
             {
                a++;

                firstRow = activeWorksheet.get_Range("A"+a);
                secondRow = activeWorksheet.get_Range("B"+a);
                thirdRow = activeWorksheet.get_Range("C"+a);
                fourthRow = activeWorksheet.get_Range("D"+a);
                firstRow.Value2 = a-1;
                secondRow.Value2 = i.FirstName;
                thirdRow.Value2 = i.LastName;
                fourthRow.Value2 = i.Age;
            }
            //Close client connection
            client.Close();
        }
        //Save Button functionality 
        private void btnSave_Click(object sender, RibbonControlEventArgs e)
        {
            //Get active worksheet
            Excel.Window window = e.Control.Context;
            Excel.Worksheet activeWorksheet = ((Excel.Worksheet)window.Application.ActiveSheet);
            //Get currently used cells
            Excel.Range usedRange = activeWorksheet.UsedRange;
            //Create bad way to return data
            DBService.ArrayOfString fName = new DBService.ArrayOfString();
            DBService.ArrayOfString lName = new DBService.ArrayOfString();
            DBService.ArrayOfInt age = new DBService.ArrayOfInt();

            //Fill bad way with data
            var col = activeWorksheet.UsedRange.Columns;

            foreach (Excel.Range row in usedRange.Rows)
            {
                if (row.Cells[1, 2].Value2.ToString() == "First Name")
                    continue;
                fName.Add(row.Cells[1, 2].Value2.ToString());
                lName.Add(row.Cells[1, 3].Value2.ToString());
                age.Add(Convert.ToInt32(row.Cells[1, 4].Value2.ToString()));
            }
            //Connect to service
            DBService.WebService1SoapClient client = new DBService.WebService1SoapClient("WebService1Soap");
            //Call save function and pass data
            client.SaveDB(fName, lName, age);
            //Close connection
            client.Close();
            //Informat user data is saved
            MessageBox.Show("Save Complete");
           

        }
    }
}
