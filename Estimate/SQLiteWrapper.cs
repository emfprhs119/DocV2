using System;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
/*
// doc
string sql = "create table doc (docID integer primary key AUTOINCREMENT,columnWidths TEXT,";*/

namespace Estimate
{
    class SQLiteWrapper
    {
        static readonly string ConnectionString = @"Data Source=save.db;Version=3;";
        static readonly string dbFileName = "save.db";
        static SQLiteConnection conn;
        static SQLiteCommand command;
        public static void PrepareDB(int oppLength,int itemLength)
        {
            /* for debug code */
            //File.Delete(dbFileName);
            conn = new SQLiteConnection(ConnectionString);
            if (!File.Exists(dbFileName))
            {
                
                SQLiteConnection.CreateFile(dbFileName);
                conn.Open();
                // doc
                string sql = "create table doc (docID TEXT,num integer,";
                for (int i = 0; i < oppLength; i++) {
                    sql += "info_"+i+ " TEXT";
                    if (i < oppLength - 1)
                    {
                        sql += ",";
                    }
                    else
                    {
                        sql += ");";
                    }
                }
                // item
                sql += "create table item (docID TEXT, indexID integer,";
                for (int i = 0; i < itemLength; i++)
                {
                    sql += "column_" + i + " TEXT";
                    if (i < itemLength - 1)
                    {
                        sql += ",";
                    }
                    else
                    {
                        sql += ");";
                    }
                }
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                int result = command.ExecuteNonQuery();
            }
        }

        internal static string LoadDoc(string key)
        {
            conn.Open();
            string query = "select * from doc where docID = '" + key+"';";
            var cmd = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            reader.Read();

            string data = "";
            for (int i = 2; i < reader.FieldCount; i++)
                data += reader.GetString(i) + ",";
            data += "SplitText";

            query = "select * from item where docID = '" + key + "'order by indexID ;";
            cmd = new SQLiteCommand(query, conn);

            reader = cmd.ExecuteReader();
            while (reader.Read())

            {
                for (int i = 2; i < reader.FieldCount; i++)
                {
                    data += reader.GetString(i);
                    if (i < reader.FieldCount - 1)
                        data += "\t";
                    else
                        data += "\n";
                }

            }
            conn.Close();
            return data;
        }

        public static DataTable GetDataTable(string query)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        conn.Close();
                        return dataTable;
                    }
                }
            }
        }

        public static string AddDoc(string docID,string[] info,string[][] items)
        {
            if (docID != null)
                DeleteDoc(docID);
            docID = InsertDoc(docID, info);
            InsertItems(docID, items);
            return docID;
        }

        public static void DeleteDoc(string docID)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                new SQLiteCommand("delete from doc where docID = '" + docID + "'", conn).ExecuteNonQuery();
                new SQLiteCommand("delete from item where docID = '" + docID + "'", conn).ExecuteNonQuery();
                conn.Close();
            }
        }

        private static string InsertDoc(string docID, string[] info)
        {
            Int64 id = 0;
            string sql;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                if (docID == null)
                {
                    sql = "select num from doc where info_1 = '" + info[1] + "' and info_2 = '" + info[2] + "'order by num DESC ;";
                    command = new SQLiteCommand(sql, conn);
                    object scalar = command.ExecuteScalar();
                    if (scalar != null)
                        id = ((Int64)scalar) + 1;
                    docID = info[1] + "_" + info[2] + "_" + id;
                }
                else
                {
                    string[] stn = docID.Split('_');
                    id = Int64.Parse(stn[stn.Length - 1]);
                }
                sql = "insert into doc values('" + docID + "','" + id + "'," + ArrToQueryString(info) + ")";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                conn.Close();
            }
            return docID;
        }

        private static void InsertItems(string docID, string[][] items)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                for (int i = 0; i < items.Length; i++)
                {
                    string sql = "insert into item values('"+ docID + "'," + i + "," + ArrToQueryString(items[i]) + ");";
                    new SQLiteCommand(sql, conn).ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        private static string ArrToQueryString(string[] item)
        {
            string tmp = "";
            for (int i = 0; i < item.Length; i++)
            {
                tmp += "'" +item[i]+ "'";
                if (i < item.Length - 1)
                {
                    tmp += ",";
                }
            }
            return tmp;
        }

    }
}
