using Gtk;

// !!! delete

namespace StorageAndTrade
{
    public class NotebookPage
    {
        public NotebookPage()
        {
            NamePage = "";
        }

        public int NumPage { get; set; }
        public string NamePage { get; set; }
        public ScrolledWindow? ScrolledWindow { get; set; }
        public VBox? VBox { get; set; }
        public bool IsConstruct { get; set; }

        public void AddVBox(VBox vBox)
        {
            VBox = vBox;
            ScrolledWindow?.Add(vBox);
            IsConstruct = true;
        }
    }
}