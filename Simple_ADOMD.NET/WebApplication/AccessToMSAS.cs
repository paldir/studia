using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AnalysisServices.AdomdClient;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;


namespace WebApplication
{
    public class AccessToMSAS
    {
        private AdomdConnection connection;

        public AccessToMSAS()
        {
            connection = EstablishConnection();
        }

        private AdomdConnection EstablishConnection()
        {
            AdomdConnection connection = new AdomdConnection("Data Source=localhost;");
            connection.Open();
            return connection;
        }

        private AdomdCommand CreateCommand(string commandText)
        {
            AdomdCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        private CellSet ExecuteQuery(string query)
        {
            AdomdCommand command = CreateCommand(query);
            CellSet cellset = command.ExecuteCellSet();
            return cellset;
        }

        public string ResultOfQueryAsString(string query)
        {
            StringBuilder result = new StringBuilder();
            CellSet cellSet = ExecuteQuery(query);

            result.Append("\t");
            TupleCollection tuplesOnColumns = cellSet.Axes[0].Set.Tuples;
            foreach (Microsoft.AnalysisServices.AdomdClient.Tuple column in tuplesOnColumns)
            {
                result.Append(column.Members[0].Caption + "\t");
            }
            result.AppendLine();

            TupleCollection tuplesOnRows = cellSet.Axes[1].Set.Tuples;
            for (int row = 0; row < tuplesOnRows.Count; row++)
            {
                result.Append(tuplesOnRows[row].Members[0].Caption + "\t");
                for (int col = 0; col < tuplesOnColumns.Count; col++)
                {
                    result.Append(cellSet.Cells[col, row].FormattedValue + "\t");
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        public Table ResultOfQueryAsTable(string query)
        {
            Table result = new Table();
            CellSet cellSet = ExecuteQuery(query);

            TupleCollection tuplesOnColumns = cellSet.Axes[0].Set.Tuples;
            TableRow rows = new TableRow();
            TableCell cell=new TableCell();
            cell.Controls.Add(new LiteralControl(""));
            rows.Cells.Add(cell);
            foreach (Microsoft.AnalysisServices.AdomdClient.Tuple column in tuplesOnColumns)
            {
                cell = new TableCell();
                cell.Font.Bold = true;
                cell.Controls.Add(new LiteralControl(column.Members[0].Caption));
                rows.Cells.Add(cell);
            }
            result.Rows.Add(rows);

            TupleCollection tuplesOnRows = cellSet.Axes[1].Set.Tuples;
            for (int row = 0; row < tuplesOnRows.Count; row++)
            {
                rows = new TableRow();
                cell = new TableCell();
                cell.Font.Bold = true;
                cell.Controls.Add(new LiteralControl(tuplesOnRows[row].Members[0].Caption));
                rows.Cells.Add(cell);
                for (int col = 0; col < tuplesOnColumns.Count; col++)
                {
                    cell = new TableCell();
                    cell.Controls.Add(new LiteralControl(cellSet.Cells[col, row].FormattedValue));
                    rows.Cells.Add(cell);
                }
                result.Rows.Add(rows);
            }

            result.BorderWidth = 1;
            result.GridLines = (GridLines)3;
            result.ID = "resultTable";

            return result;
        }

        public string GetStructure()
        {
            string result = "<b>Cubes</b>";
            result += "<ul>";
            foreach (CubeDef cube in connection.Cubes)
            {
                if (cube.Name[0] != '$')
                {
                    result += "<li>" + cube.Name + "<br />";
                    result += "<b>Dimensions</b>";
                    result += "<ul>";
                    foreach (Dimension dimension in cube.Dimensions)
                    {
                        if (dimension.Name[0] != '$')
                            result += "<li>" + dimension.Name + "</li>";
                    }
                    result += "</ul>";
                    result += "<b>Measures</b>";
                    result += "<ul>";
                    foreach (Measure measure in cube.Measures)
                    {
                        if (measure.Name[0] != '$')
                            result += "<li>" + measure.Name + "</li>";
                    }
                    result += "</ul>";
                    result+="<b>KPIs</b>";
                    result += "<ul>";
                    foreach (Kpi kpi in cube.Kpis)
                    {
                        if (kpi.Name[0] != '$')
                            result += "<li>" + kpi.Name + "</li>";
                    }
                    result += "</ul>";
                    result += "<b>Named sets</b>";
                    result += "<ul>";
                    foreach (NamedSet namedSet in cube.NamedSets)
                    {
                        if (namedSet.Name[0] != '$')
                            result += "<li>" + namedSet + "</li>";
                    }
                    result += "</ul>";
                    result += "</li>";
                }
            }
            result += "</ul>";
            return result;
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}