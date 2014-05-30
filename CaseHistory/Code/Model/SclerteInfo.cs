using System;
namespace OfIllness.Model
{
    /// <summary>
    /// 骨片信息
    /// </summary>
    [Serializable]
    public class SclerteInfo
    {

        public long ScleriteId
        {
            get;
            set;
        }

        public long PatientId
        {
            get;
            set;
        }

        public string Title
        { get; set; }

        public string SleritePicUrl
        {
            get;
            set;
        }
        public string SleriteNote
        {
            get;
            set;
        }

    }
}
