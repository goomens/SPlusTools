using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.Utilities;

namespace UvA.SPlusTools.Data
{
    /// <summary>
    /// Tools for executing OLEDB queries against Syllabus+
    /// </summary>
    public class QueryTool
    {
        protected string ProgID { get; set; }

        protected string GetSingleField(string field, string table, Dictionary<string, object> pars)
        {
            return GetField(field, table, pars).SingleOrDefault();
        }

        protected IEnumerable<string> GetField(string field, string table, Dictionary<string, object> pars)
        {
            return DoQuery(field, table, pars).Select(r => r[0].ToString());
        }

        protected IEnumerable<DataRow> DoQuery(string fields, string table, Dictionary<string, object> pars = null)
        {
            string condition = pars == null ? "" : "WHERE " + pars.ToSeparatedString(p =>
            {
                if (p.Value is IEnumerable<string>)
                    return string.Format("{0} IN ({1})", p.Key, ((IEnumerable<string>)p.Value).ToSeparatedString(z => "'" + z + "'"));
                return string.Format("{0} = {2}{1}{2}", p.Key, p.Value, p.Value is string ? "'" : "");
            });
            return DoQuery(string.Format("SELECT {0} FROM {1} {2}", fields, table, condition));
        }

        protected IEnumerable<DataRow> DoQuery(string q)
        {
            var dataset = new System.Data.DataSet();
            OleDbConnection conn = new OleDbConnection(string.Format("Provider={0};Data Source={1};{2}",
                    ProgID + ".OLEDB.1", ProgID, "User ID=dummy;Password=dummy;"));
            conn.Open();
            OleDbDataAdapter adapter;

            var now = DateTime.Now;

            adapter = new OleDbDataAdapter(q, conn);
            adapter.Fill(dataset, "?");
            conn.Close();
            if (q.Contains("WHERE"))
                q = q.Substring(0, q.IndexOf("WHERE") + 6) + "...";
            Console.WriteLine(q);
            Console.WriteLine("{0} seconds", DateTime.Now.Subtract(now).TotalSeconds);
            return dataset.Tables[0].Rows.Cast<DataRow>();
        }


        protected Dictionary<string, object> Parameter(params Tuple<string, object>[] pars)
        {
            var dict = new Dictionary<string, object>();
            pars.ForEach(z => dict.Add(z.Item1, z.Item2));
            return dict;
        }

        protected Dictionary<string, object> Parameter(string s, object obj)
        {
            return Parameter(Tuple.Create(s, obj));
        }

    }
}
