using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Services;
using System.Web.Services;

namespace CameraWebService.Support
{
    /// <summary>
    /// Summary description for InteractionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class InteractionService : WebService
    {

        [WebMethod]
        [ScriptMethod]
        public int UploadFile()
        {
            var content = ReadAllContent().Result.ToArray();
            return content.Length;
        }

        private const int DROP_OUT_REQUEST_SIZE = 25000;

        [WebMethod]
        [ScriptMethod]
        public int UploadFileStopped()
        {
            bool isAuthorizedRequest = CheckAuthorization();
            int readBytes = 0;
            using (var input = Context.Request.GetBufferlessInputStream())
            {
                byte[] buffer = new byte[4096];
                int currentBatch = 0;
                do
                {
                    currentBatch = input.Read(buffer, 0, buffer.Length);
                    readBytes += currentBatch;
                    if (!isAuthorizedRequest && readBytes > DROP_OUT_REQUEST_SIZE)
                    {
                        break;
                    }
                } while (currentBatch > 0);
            }
            if (!isAuthorizedRequest && readBytes > DROP_OUT_REQUEST_SIZE)
            {
                RequestAuthorization();
            }
            return readBytes;
        }

        private bool CheckAuthorization()
        {
            //Authorization: Digest algorithm=MD5, realm="TV-IP318PI", username="admin", nonce="4e554930524463794d5545364e6d5977595751795a57553d", uri="/onvif/device_service", qop="auth", nc=00000001, cnonce="386d4630ca53f8405fa3", response="bf939a582406d0b6c581e7701babfbc6"
            //http.authorization
            // Digest algorithm=MD5, realm="TV-IP318PI", username="admin", nonce="4e554930524463794d5545364e6d5977595751795a57553d", uri="/onvif/device_service", qop="auth", nc=00000001, cnonce="386d4630ca53f8405fa3", response="bf939a582406d0b6c581e7701babfbc6"
            return Context.Request.Headers["Authorization"] != null;
        }

        private void RequestAuthorization()
        {
            Context.Response.StatusCode = 401;
            Context.Response.StatusDescription = "Unautorized in the middle of response";
            Context.Response.AddHeader("WWW-Authenticate", "Digest qop=\"auth\", realm=\"TV-IP318PI\", nonce=\"4e554930524463794d5545364e6d5977595751795a57553d\"");
            Context.Response.SuppressContent = true;
            Context.Response.Flush();
            Context.Request.Abort();
        }

        public async Task<IEnumerable<byte>> ReadAllContent()
        {
            List<byte> res = new List<byte>();
            using (var contentStream = Context.Request.GetBufferedInputStream())
            {
                byte[] buffer = new byte[4096];
                int readBytes = 0;
                do
                {
                    readBytes = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    res.AddRange(buffer.Take(readBytes));
                } while (readBytes > 0);
            }
            return res;
        }
    }
}
