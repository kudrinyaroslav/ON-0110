///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTool.HttpTransport
{
    /// <summary>
    /// Controller which is able to modify outcoming messages (for negative tests).
    /// </summary>
    public interface ISoapMessageMutator : IEncodingController
    {
        byte[] ProcessMessage(byte[] original);
    }
}
