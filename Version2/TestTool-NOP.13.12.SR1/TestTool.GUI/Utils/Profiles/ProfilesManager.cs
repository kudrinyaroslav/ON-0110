///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

using System.Reflection;
using TestTool.GUI.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace TestTool.GUI.Utils
{

    /// <summary>
    /// Provides "save settings" functionality.
    /// </summary>
    class ProfilesManager
    {
        public static string FileName = "profiles.xml";

        public static ProfilesSet Load()
        {
            ProfilesSet result = new ProfilesSet();

            try
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.Machine | IsolatedStorageScope.Application,
                                                                            new System.Security.Policy.Url("www.onvif.org/OnvifTestTool"));
                IsolatedStorageFileStream isoFile = new IsolatedStorageFileStream(FileName, FileMode.Open, isoStore);
                StreamReader reader = new StreamReader(isoFile);

                XmlSerializer s = new XmlSerializer(typeof (ProfilesSet));
                result = (ProfilesSet)s.Deserialize(reader);
                reader.Close();
            }
            catch
            {

            }
            return result;
        }

        public static void Save(ProfilesSet profiles)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.Machine | IsolatedStorageScope.Application, 
                                                                        new System.Security.Policy.Url("www.onvif.org/OnvifTestTool"));
            IsolatedStorageFileStream isoFile = new IsolatedStorageFileStream(FileName, FileMode.Create, isoStore);
            StreamWriter writer = new StreamWriter(isoFile);
            XmlSerializer s = new XmlSerializer(typeof(ProfilesSet));
            s.Serialize(writer, profiles);
            writer.Close();
        }
    }
}
