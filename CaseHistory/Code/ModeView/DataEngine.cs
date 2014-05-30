using DBUtility;
using OfIllness.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows;
namespace OfIllness.ModeView
{
    public class DataEngine : IDataEngine
    {
        SQLiteHelper DbHelperSQLite = new SQLiteHelper("Data Source=Data.db");
        public List<long> QueryOperationPatient(DateTime? startDT, DateTime? endDT)
        {
            List<long> lsPartientId = new List<long>();
            StringBuilder sb = new StringBuilder("SELECT distinct PatientId FROM OperationInfo where 1=1");
            if (startDT.HasValue)
            {
                sb.AppendFormat(" and OperationDate >='{0:yyyy-MM-dd}'", startDT);
            }
            if (endDT.HasValue)
            {
                sb.AppendFormat(" and OperationDate <='{0:yyyy-MM-dd 23:59:59.999}'", endDT);
            }
            using (DbDataReader dr = DbHelperSQLite.ExecuteReader(sb.ToString()))
            {
                while (dr.Read())
                {
                    lsPartientId.Add(Int32.Parse(dr["PatientId"].ToString()));
                }
            }
            return lsPartientId;
        }
        public List<PatientInfo> QueryPatientBaseInfo(string where)
        {
            List<PatientInfo> list = new List<PatientInfo>();
            StringBuilder stringBuilder = new StringBuilder("select * from PatientBaseInfo where 1=1 ");
            if (!string.IsNullOrWhiteSpace(where))
            {
                stringBuilder.Append(where);
            }
            stringBuilder.Append(" order by BeHospitalizedDate");
            //stringBuilder.Append(" LIMIT 500");
            uint seq = 0;
            using (DbDataReader dbDataReader = DbHelperSQLite.ExecuteReader(stringBuilder.ToString()))
            {
                while (dbDataReader.Read())
                {
                    PatientInfo patientInfo = new PatientInfo { Seq = ++seq };
                    object obj = dbDataReader["PatientId"];
                    if (obj != DBNull.Value)
                    {
                        patientInfo.Id = Convert.ToInt64(obj);
                    }
                    patientInfo.Name = dbDataReader["PatientName"].ToString();
                    obj = dbDataReader["PatientAge"];
                    if (obj != DBNull.Value)
                    {
                        patientInfo.Age = Convert.ToUInt16(obj);
                    }
                    patientInfo.Sex = dbDataReader["PatientSex"].ToString();
                    obj = dbDataReader["BeHospitalizedDate"];
                    if (obj != null && obj != DBNull.Value)
                    {
                        patientInfo.BeHospitalizedDate = new DateTime?((DateTime)obj);
                    }
                    patientInfo.Bed = dbDataReader["Bed"].ToString();
                    patientInfo.CourtyardState = dbDataReader["CourtyardState"].ToString();
                    obj = dbDataReader["LeaveHospitalDate"];
                    if (obj != null && obj != DBNull.Value)
                    {
                        patientInfo.LeaveHospitalDate = new DateTime?((DateTime)obj);
                    }
                    patientInfo.LeaveHospitalAgent = dbDataReader["LeaveHospitalAgent"].ToString();
                    patientInfo.Diacrisis = dbDataReader["Diacrisis"].ToString();

                    patientInfo.Teach = dbDataReader["Teach"].ToString();
                    list.Add(patientInfo);
                }

            }
            return list;
        }
        public List<OperationInfo> QueryOperationInfo(long patientId)
        {
            List<OperationInfo> list = new List<OperationInfo>();
            string strSQL = string.Format("SELECT * FROM OperationInfo where PatientId={0}", patientId);
            using (DbDataReader dbDataReader = DbHelperSQLite.ExecuteReader(strSQL))
            {
                while (dbDataReader.Read())
                {
                    OperationInfo operationInfo = new OperationInfo();
                    object obj = dbDataReader["OperationId"];
                    if (obj != null && obj != DBNull.Value)
                    {
                        operationInfo.OperationId = Convert.ToInt64(obj);
                    }
                    obj = dbDataReader["PatientId"];
                    if (obj != null && obj != DBNull.Value)
                    {
                        operationInfo.PatientId = Convert.ToInt64(obj);
                    }
                    obj = dbDataReader["OperationDate"];
                    if (obj != null && obj != DBNull.Value)
                    {
                        operationInfo.OperationDate = (DateTime)obj;
                    }
                    operationInfo.Surgeon = dbDataReader["Surgeon"].ToString();
                    operationInfo.Note = dbDataReader["Note"].ToString();
                    operationInfo.OperationName = dbDataReader["OperationName"].ToString();
                    list.Add(operationInfo);
                }
            }
            return list;
        }
        public List<SclerteInfo> QuerySclerteInfo(long partientId)
        {
            List<SclerteInfo> list = new List<SclerteInfo>();
            string strSQL = string.Format("select * from SclerteInfo where PatientId={0}", partientId);
            using (DbDataReader dbDataReader = DbHelperSQLite.ExecuteReader(strSQL))
            {
                while (dbDataReader.Read())
                {
                    SclerteInfo sclerteInfo = new SclerteInfo();
                    object obj = dbDataReader["ScleriteId"];
                    if (obj != null && obj != DBNull.Value)
                    {
                        sclerteInfo.ScleriteId = Convert.ToInt64(obj);
                    }
                    obj = dbDataReader["PatientId"];
                    if (obj != null && obj != DBNull.Value)
                    {
                        sclerteInfo.PatientId = Convert.ToInt64(obj);
                    }
                    sclerteInfo.SleritePicUrl = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, dbDataReader["SleritePicUrl"]);
                    sclerteInfo.SleriteNote = dbDataReader["SleriteNote"].ToString();
                    sclerteInfo.Title = dbDataReader["Title"].ToString();
                    list.Add(sclerteInfo);
                }
            }
            return list;
        }


        public long AddPatientBaseInfo(PatientInfo model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("insert into PatientBaseInfo(");
            stringBuilder.Append("PatientAge,PatientName,PatientSex,BeHospitalizedDate,Bed,CourtyardState,LeaveHospitalDate,LeaveHospitalAgent,Diacrisis)");
            stringBuilder.Append(" values (");
            stringBuilder.Append("@PatientAge,@PatientName,@PatientSex,@BeHospitalizedDate,@Bed,@CourtyardState,@LeaveHospitalDate,@LeaveHospitalAgent,@Diacrisis)");
            stringBuilder.Append(";select LAST_INSERT_ROWID()");
            SQLiteParameter[] array = new SQLiteParameter[]
			{
				new SQLiteParameter("@PatientAge", DbType.Int16),
				new SQLiteParameter("@PatientName", DbType.String),
				new SQLiteParameter("@PatientSex", DbType.String),
				new SQLiteParameter("@BeHospitalizedDate", DbType.Date),
				new SQLiteParameter("@Bed", DbType.String),
				new SQLiteParameter("@CourtyardState", DbType.String),
				new SQLiteParameter("@LeaveHospitalDate", DbType.Date),
				new SQLiteParameter("@LeaveHospitalAgent", DbType.String),
				new SQLiteParameter("@Diacrisis", DbType.String)
			};
            array[0].Value = model.Age;
            array[1].Value = model.Name;
            array[2].Value = model.Sex;
            array[3].Value = model.BeHospitalizedDate;
            array[4].Value = model.Bed;
            array[5].Value = model.CourtyardState;
            array[6].Value = model.LeaveHospitalDate;
            array[7].Value = model.LeaveHospitalAgent;
            array[8].Value = model.Diacrisis;
            object single = DbHelperSQLite.GetSingle(stringBuilder.ToString(), array);
            return Convert.ToInt64(single);
        }
        public long AddOperationInfo(OperationInfo model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("insert into OperationInfo(");
            stringBuilder.Append("PatientId,OperationDate,Surgeon,Note,OperationName)");
            stringBuilder.Append(" values (");
            stringBuilder.Append("@PatientId,@OperationDate,@Surgeon,@Note,@OperationName)");
            stringBuilder.Append(";select LAST_INSERT_ROWID()");
            SQLiteParameter[] array = new SQLiteParameter[]
			{
				new SQLiteParameter("@PatientId", DbType.Int64),
				new SQLiteParameter("@OperationDate", DbType.Date),
				new SQLiteParameter("@Surgeon", DbType.String),
				new SQLiteParameter("@Note", DbType.String),
				new SQLiteParameter("@OperationName", DbType.String)
			};
            array[0].Value = model.PatientId;
            array[1].Value = model.OperationDate;
            array[2].Value = model.Surgeon;
            array[3].Value = model.Note;
            array[4].Value = model.OperationName;
            object single = DbHelperSQLite.GetSingle(stringBuilder.ToString(), array);
            return Convert.ToInt64(single);
        }
        public long AddSclerteInfo(SclerteInfo model)
        {
            DateTime now = DateTime.Now;
            string text = string.Format("Pic\\{0}\\{1}\\{2}\\{3}", new object[]
			{
				now.Year,
				now.Month,
				model.PatientId,
				Path.GetFileName(model.SleritePicUrl)
			});
            string text2 = AppDomain.CurrentDomain.BaseDirectory + text;
            if (File.Exists(text2))
            {
                UMessageBox.Show("图片上传出错!", "已有相同文件(名)存在!", MessageBoxButton.OK);
            }
            else
            {
                string directoryName = Path.GetDirectoryName(text2);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                File.Copy(model.SleritePicUrl, text2);
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("insert into SclerteInfo(");
            stringBuilder.Append("PatientId,SleritePicUrl,SleriteNote,Title)");
            stringBuilder.Append(" values (");
            stringBuilder.Append("@PatientId,@SleritePicUrl,@SleriteNote,@Title)");
            stringBuilder.Append(";select LAST_INSERT_ROWID()");
            SQLiteParameter[] array = new SQLiteParameter[]
			{
				new SQLiteParameter("@PatientId", DbType.Int64),
				new SQLiteParameter("@SleritePicUrl", DbType.String),
				new SQLiteParameter("@SleriteNote", DbType.String),
				new SQLiteParameter("@Title", DbType.String)
			};
            array[0].Value = model.PatientId;
            array[1].Value = text;
            array[2].Value = model.SleriteNote;
            array[3].Value = model.Title;
            object single = DbHelperSQLite.GetSingle(stringBuilder.ToString(), array);
            return Convert.ToInt64(single);
        }

        public long AddPatientInfo(PatientInfo model)
        {
            model.Id = this.AddPatientBaseInfo(model);

            if (model.OcOperation != null)
            {
                foreach (OperationInfo current in model.OcOperation)
                {

                    current.PatientId = model.Id;
                    current.OperationId = this.AddOperationInfo(current);

                }
            }
            if (model.OcSclerteInfo != null)
            {
                foreach (SclerteInfo current2 in model.OcSclerteInfo)
                {
                    current2.PatientId = model.Id;
                    current2.ScleriteId = this.AddSclerteInfo(current2);
                }
            }
            return model.Id;
        }

        public bool EditPatientBaseInfo(PatientInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PatientBaseInfo set ");
            strSql.Append("PatientAge=@PatientAge,");
            strSql.Append("PatientName=@PatientName,");
            strSql.Append("PatientSex=@PatientSex,");
            strSql.Append("BeHospitalizedDate=@BeHospitalizedDate,");
            strSql.Append("Bed=@Bed,");
            strSql.Append("CourtyardState=@CourtyardState,");
            strSql.Append("LeaveHospitalDate=@LeaveHospitalDate,");
            strSql.Append("LeaveHospitalAgent=@LeaveHospitalAgent,");
            strSql.Append("Diacrisis=@Diacrisis,");
            strSql.Append("Teach=@Teach");
            strSql.Append(" where PatientId=@PatientId");
            SQLiteParameter[] parameters = {
					new SQLiteParameter("@PatientAge", DbType.Int32),
					new SQLiteParameter("@PatientName", DbType.String),
					new SQLiteParameter("@PatientSex", DbType.String),
					new SQLiteParameter("@BeHospitalizedDate", DbType.Date),
					new SQLiteParameter("@Bed", DbType.String),
					new SQLiteParameter("@CourtyardState", DbType.String),
					new SQLiteParameter("@LeaveHospitalDate", DbType.Date),
					new SQLiteParameter("@LeaveHospitalAgent", DbType.String),
					new SQLiteParameter("@Diacrisis", DbType.String),
					new SQLiteParameter("@Teach", DbType.String),
					new SQLiteParameter("@PatientId", DbType.Int64)};
            parameters[0].Value = model.Age;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Sex;
            parameters[3].Value = model.BeHospitalizedDate;
            parameters[4].Value = model.Bed;
            parameters[5].Value = model.CourtyardState;
            parameters[6].Value = model.LeaveHospitalDate;
            parameters[7].Value = model.LeaveHospitalAgent;
            parameters[8].Value = model.Diacrisis;
            parameters[9].Value = model.Teach;
            parameters[10].Value = model.Id;

            int rows = DbHelperSQLite.ExecuteSql(strSql.ToString(), parameters);
            return rows > 0;
        }
        public bool EditSclerteInfo(SclerteInfo model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("update SclerteInfo set ");
            stringBuilder.Append("PatientId=@PatientId,");
            stringBuilder.Append("SleritePicUrl=@SleritePicUrl,");
            stringBuilder.Append("SleriteNote=@SleriteNote,");
            stringBuilder.Append("Title=@Title");
            stringBuilder.Append(" where ScleriteId=@ScleriteId");
            SQLiteParameter[] array = new SQLiteParameter[]
			{
				new SQLiteParameter("@PatientId", DbType.Int64),
				new SQLiteParameter("@SleritePicUrl", DbType.String),
				new SQLiteParameter("@SleriteNote", DbType.String),
				new SQLiteParameter("@Title", DbType.String),
				new SQLiteParameter("@ScleriteId", DbType.Int64)
			};
            array[0].Value = model.PatientId;
            array[1].Value = model.SleritePicUrl;
            array[2].Value = model.SleriteNote;
            array[3].Value = model.Title;
            array[4].Value = model.ScleriteId;
            int num = DbHelperSQLite.ExecuteSql(stringBuilder.ToString(), array);
            return num > 0;
        }
        public bool EditOperationInfo(OperationInfo model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("update OperationInfo set ");
            stringBuilder.Append("PatientId=@PatientId,");
            stringBuilder.Append("OperationDate=@OperationDate,");
            stringBuilder.Append("Surgeon=@Surgeon,");
            stringBuilder.Append("Note=@Note,");
            stringBuilder.Append("OperationName=@OperationName");
            stringBuilder.Append(" where OperationId=@OperationId");
            SQLiteParameter[] array = new SQLiteParameter[]
			{
				new SQLiteParameter("@PatientId", DbType.Int64),
				new SQLiteParameter("@OperationDate", DbType.Date),
				new SQLiteParameter("@Surgeon", DbType.String),
				new SQLiteParameter("@Note", DbType.String),
				new SQLiteParameter("@OperationName", DbType.String),
				new SQLiteParameter("@OperationId", DbType.Int64)
			};
            array[0].Value = model.PatientId;
            array[1].Value = model.OperationDate;
            array[2].Value = model.Surgeon;
            array[3].Value = model.Note;
            array[4].Value = model.OperationName;
            array[5].Value = model.OperationId;
            int num = DbHelperSQLite.ExecuteSql(stringBuilder.ToString(), array);
            return num > 0;
        }

        public bool DeleteSclerteInfo(SclerteInfo model)
        {
            try
            {
                File.Delete(model.SleritePicUrl);
            }
            catch { }
            model.SleritePicUrl = null;
            string sQLString = string.Format("DELETE FROM [SclerteInfo] where ScleriteId={0}", model.ScleriteId);

            return DbHelperSQLite.ExecuteSql(sQLString) > 0;
        }
        public bool DeleteOperationInfo(long operationId)
        {
            string sQLString = string.Format("delete from OperationInfo where OperationId={0}", operationId);
            return DbHelperSQLite.ExecuteSql(sQLString) > 0;
        }
        public bool DeletePatientInfo(PatientInfo model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("delete from PatientBaseInfo where PatientId={0};", model.Id);
            stringBuilder.AppendFormat("delete from OperationInfo   where PatientId={0};", model.Id);
            stringBuilder.AppendFormat("delete from SclerteInfo  where PatientId={0};", model.Id);
            foreach (SclerteInfo sclerte in model.OcSclerteInfo)
            {
                File.Delete(sclerte.SleritePicUrl);
            }
            return DbHelperSQLite.ExecuteSql(stringBuilder.ToString()) > 0;
        }

    }
}
