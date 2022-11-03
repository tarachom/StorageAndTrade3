using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    abstract class DirectoryControl : HBox
    {
        Label labelCaption = new Label();
        Entry entryText = new Entry();

        public DirectoryControl() : base()
        {
            PackStart(labelCaption, false, false, 5);
            PackStart(entryText, false, false, 0);

            Button bOpen = new Button(new Image("find.png"));
            bOpen.Clicked += OpenSelect;

            PackStart(bOpen, false, false, 2);
        }

        protected virtual void OpenSelect(object? sender, EventArgs args) { }

        public string Caption
        {
            get
            {
                return labelCaption.Text;
            }
            set
            {
                labelCaption.Text = value;
            }
        }

        public System.Action? Select { get; set; }

        protected string Presentation
        {
            get
            {
                return entryText.Text;
            }
            set
            {
                entryText.Text = value;
            }
        }
    }
}