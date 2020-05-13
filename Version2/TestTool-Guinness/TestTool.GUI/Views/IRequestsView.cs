///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////

namespace TestTool.GUI.Views
{
    interface IRequestsView : IView
    {
        Enums.DutService Service { get; set; }
        string ServiceAddress { get; set; }

        string Request { get; set; }
        void DisplayResponse(string response);
        
        void DisplayFolders(Data.RequestFolder folder);
        void AddFolder(Data.RequestFolder folder, Data.RequestFolder parentFolder);
        void AddFile(Data.RequestFile file, Data.RequestFolder parentFolder);
        void DeleteFile(Data.RequestFile file, Data.RequestFolder parentFolder);
    }
}
