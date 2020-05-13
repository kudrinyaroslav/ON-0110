using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace AutomatedTesting.GUI.Controllers
{
    internal class StateController
    {
        const string FILENAME = "AppState.xml";

        public void SaveState()
        {
            Context.TreeState state = Context.AppContext.Instance.TreeState;

            XmlSerializer serializer = new XmlSerializer(typeof (Context.TreeState));
            if (File.Exists(FILENAME))
            {
                File.Delete(FILENAME);
            }
            Stream stream = File.OpenWrite(FILENAME);
            serializer.Serialize(stream, state);
            stream.Close();
        }

        public void LoadState()
        {
            if (File.Exists(FILENAME))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Context.TreeState));
                Stream stream = File.OpenRead(FILENAME);
                Context.TreeState state = (Context.TreeState)(serializer.Deserialize(stream));
                stream.Close();
                Context.AppContext.Instance.AttachTreeState(state);
            }
        }

    }
}
