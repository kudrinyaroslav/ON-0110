using System.ServiceModel;

namespace TestTool.Transport
{
    public class CommonUtils
    {
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        public static void ReturnFault(params string[] codes)
        {
            FaultCode subCode = null;
            for (int i = codes.Length - 1; i > 0; i--)
            {
                FaultCode currentSubCode = new FaultCode(codes[i], "http://www.onvif.org/ver10/error", subCode);
                subCode = currentSubCode;
            }
            FaultCode code = new FaultCode(codes[0], "http://www.w3.org/2003/05/soap-envelope", subCode);
            throw new FaultException(code.Name, code);
        }
    }
}
