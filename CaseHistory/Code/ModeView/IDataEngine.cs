using System.Collections.Generic;
using OfIllness.Model;
using System;

namespace OfIllness.ModeView
{
    public interface IDataEngine
    {
        List<long> QueryOperationPatient(DateTime? startDT, DateTime? endDT);
        List<PatientInfo> QueryPatientBaseInfo(string where);
        List<OperationInfo> QueryOperationInfo(long patientId);
        List<SclerteInfo> QuerySclerteInfo(long partientId);
        long AddPatientInfo(PatientInfo pi);
        long AddPatientBaseInfo(PatientInfo model);
        long AddOperationInfo(OperationInfo model);
        long AddSclerteInfo(SclerteInfo model);
        bool DeletePatientInfo(PatientInfo model);
        bool DeleteOperationInfo(long operationId);
        bool DeleteSclerteInfo(SclerteInfo model);
        bool EditSclerteInfo(SclerteInfo model);
        bool EditOperationInfo(OperationInfo pi);
        bool EditPatientBaseInfo(PatientInfo pi);

    }
}
