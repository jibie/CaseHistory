using System;

namespace OfIllness.Model
{
    /// <summary>
    /// 手术信息
    /// </summary>
     [Serializable]
    public class OperationInfo
    {

        public long OperationId
        {
            get;
            set;
        }
        public long PatientId
        {
            get;
            set;
        }
        public string OperationName
        {
            get;
            set;
        }
        public DateTime OperationDate
        {
            get;
            set;
        }

        /// <summary>
        /// 手术者
        /// </summary>
        public string Surgeon
        {
            get;
            set;
        }
        public string Note
        {
            get;
            set;
        }
    }
}
