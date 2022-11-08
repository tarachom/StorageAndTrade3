using Gtk;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_Елемент : VBox
    {
        public ПартіяТоварівКомпозит? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозит_Objest { get; set; } = new ПартіяТоварівКомпозит_Objest();

        Entry Назва = new Entry() { WidthRequest = 500 };

        public ПартіяТоварівКомпозит_Елемент() : base()
        {
            new VBox();
            HBox hBox = new HBox();

            Button bSave = new Button("Зберегти");
            bSave.Clicked += OnSaveClick;

            hBox.PackStart(bSave, false, false, 10);

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBox.PackStart(bClose, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            hPaned.Pack1(vBox, false, false);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();



            hPaned.Pack2(vBox, false, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            Назва.Text = ПартіяТоварівКомпозит_Objest.Назва;
        }

        void GetValue()
        {
            ПартіяТоварівКомпозит_Objest.Назва = Назва.Text;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ПартіяТоварівКомпозит_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПартіяТоварівКомпозит_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Партія: {ПартіяТоварівКомпозит_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = ПартіяТоварівКомпозит_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}