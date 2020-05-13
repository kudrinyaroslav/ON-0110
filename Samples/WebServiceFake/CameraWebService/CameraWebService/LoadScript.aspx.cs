using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CameraWebService
{
    public partial class LoadScript : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                XmlReader rdr = XmlReader.Create(fileUpload.FileContent);
                XmlDocument doc = new XmlDocument();
                doc.Load(rdr);

                HttpApplicationState state = HttpContext.Current.Application;
                Script script;
                if (state["script"] == null)
                {
                    script = new Script();
                    state["script"] = script;
                }
                else
                {
                    script = (Script)state["script"];
                }

                script.LoadScript(doc);

            }
            catch (Exception exc)
            {
                Response.Write("Error: " + exc.Message);
            }

        }

        protected void ButtonReset_Click(object sender, EventArgs e)
        {
            HttpApplicationState state = HttpContext.Current.Application;
            state["step"] = 0;
        }
    }
}
