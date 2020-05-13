using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestTool.HttpTransport.Interfaces
{
    public interface IValidatingController : IEncodingController
    {
        void Validate(Stream stream);
    }
}
