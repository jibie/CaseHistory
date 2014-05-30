using System;
using System.Data;
using System.Data.SQLite;
namespace DBUtility
{
    public class SQLiteHelper
    {
        public SQLiteHelper(string sqlConn)
        {
            connection = new SQLiteConnection(sqlConn);
        }

        SQLiteConnection connection;

        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public SQLiteDataReader ExecuteReader(string strSQL)
        {
            SQLiteCommand cmd = new SQLiteCommand(strSQL, connection);
            OpenDBConn();
            SQLiteDataReader myReader = cmd.ExecuteReader();
            return myReader;
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        private void OpenDBConn()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, params SQLiteParameter[] cmdParms)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OpenDBConn();
                int rows = cmd.ExecuteNonQuery();
                //connection.Close();
                return rows;
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString, params SQLiteParameter[] cmdParms)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OpenDBConn();
                object obj = cmd.ExecuteScalar();
                //connection.Close();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }

            }

        }
        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(cmdParms);
        
        }

    }
}
