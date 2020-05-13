using System;
using System.IO;
using System.Xml.Serialization;
using DUT.PACS.Simulator.Configuration;

namespace DUT.PACS.Simulator.BackDoorServices
{
    public partial class Configuration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConfigure_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                Stream stream = fileSelector.FileContent;
                
                System.Xml.Serialization.XmlSerializer ser = new XmlSerializer(typeof(SerializableConfiguration));
                SerializableConfiguration config = (SerializableConfiguration)ser.Deserialize(stream);

                ConfStorage storage = ConfStorage.Load(config);
                Application["m_ConfStorage"] = storage;


            }
            catch (Exception exc)
            {
                lblError.Text = "Error: " + exc.Message;
                
            }
        }
    }
}
