using Gtk;

using AccountingSoftware;

namespace StorageAndTrade
{
    class DirectoryControl : HBox
    {

        Entry entryText = new Entry();

        public DirectoryControl() : base()
        {
            PackStart(entryText, false, false, 5);

            Button bOpen = new Button("...");
            //bOpen.Clicked += (object? sender, EventArgs args) => { GeneralForm?.CloseCurrentPageNotebook(); };
        }

        private DirectoryPointer? mDirectoryPointerItem;

        /// <summary>
        /// Вказівник на елемент довідника
        /// </summary>
        public DirectoryPointer? DirectoryPointerItem
        {
            get { return mDirectoryPointerItem; }

            set
            {
                mDirectoryPointerItem = value;

                if (mDirectoryPointerItem != null)
                    {
                        //ReadPresentation();
                    }
                else
                    entryText.Text = "";
            }
        }
    }
}