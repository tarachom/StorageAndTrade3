using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    abstract class DirectoryControl : HBox
    {
        Label labelCaption = new Label();
        Entry entryText = new Entry();

        public FormStorageAndTrade? GeneralForm { get; set; }

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
        DirectoryPointer? directoryPointerItem;

        protected DirectoryPointer? DirectoryPointerItem
        {
            get
            {
                return directoryPointerItem;
            }
            set
            {
                directoryPointerItem = value;
            }
        }
    }

    // class DirectoryControl2 : DirectoryControl
    // {
    //     public DirectoryControl2()
    //     {
    //         DirectoryPointer = new Організації_Pointer();
    //     }

    //     Організації_Pointer? directoryPointer;
    //     public Організації_Pointer? DirectoryPointer
    //     {
    //         get
    //         {
    //             return directoryPointer;
    //         }
    //         set
    //         {
    //             directoryPointer = value;

    //             if (directoryPointer != null)
    //                 Presentation = directoryPointer.GetPresentation();
    //             else
    //                 Presentation = "";
    //         }
    //     }

    //     protected override void OpenSelect(object? sender, EventArgs args)
    //     {
    //         GeneralForm?.CreateNotebookPage("Вибір - Довідник: Організації", () =>
    //         {
    //             Організації page = new Організації
    //             {
    //                 GeneralForm = GeneralForm
    //             };

    //             page.DirectoryPointerItem = DirectoryPointer;
                
    //             page.CallBack_OnSelectPointer = (Організації_Pointer selectPointer) =>
    //             {
    //                 DirectoryPointer = selectPointer;
    //             };

    //             page.LoadRecords();

    //             return page;
    //         });
    //     }
    // }
}