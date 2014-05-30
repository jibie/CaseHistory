using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace OfIllness.Model
{
    /// <summary>
    /// 患者信息
    /// </summary>
    [Serializable]
    public class PatientInfo : ICloneable
    {
        public UInt32 Seq
        {
            get;
            set;
        }

        public long Id
        { get; set; }
        public string Name
        { get; set; }
        public ushort Age
        { get; set; }
        public string Sex
        { get; set; }
        public DateTime? BeHospitalizedDate
        {
            get;
            set;
        }
        public string Bed
        { get; set; }
        public string CourtyardState
        { get; set; }
        /// <summary>
        /// 诊断
        /// </summary>
        public string Diacrisis
        { get; set; }
        public DateTime? LeaveHospitalDate
        { get; set; }
        public string LeaveHospitalAgent
        { get; set; }
        public string Teach
        { get; set; }


        private ObservableCollection<OperationInfo> ocOperation;
        public ObservableCollection<OperationInfo> OcOperation
        {
            get
            {
                if (ocOperation == null)
                {
                    ocOperation = new ObservableCollection<OperationInfo>(AppGlobal.DataQuery.QueryOperationInfo(Id));
                }
                return ocOperation;
            }
        }
        private ObservableCollection<SclerteInfo> ocSclerteInfo;
        public ObservableCollection<SclerteInfo> OcSclerteInfo
        {
            get
            {
                if (ocSclerteInfo == null)
                {
                    ocSclerteInfo = new ObservableCollection<SclerteInfo>(AppGlobal.DataQuery.QuerySclerteInfo(Id));
                }
                return ocSclerteInfo;
            }
        }

        public PatientInfo Clone()
        {
            MemoryStream ms = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            PatientInfo pi = formatter.Deserialize(ms) as PatientInfo;
            ms.Close();
            return pi;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

    }
}
