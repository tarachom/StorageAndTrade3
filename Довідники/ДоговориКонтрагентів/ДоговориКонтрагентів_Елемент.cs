using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ДоговориКонтрагентів_Елемент : VBox
    {
        public ДоговориКонтрагентів? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest { get; set; } = new ДоговориКонтрагентів_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        public ДоговориКонтрагентів_Елемент() : base()
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

            //Код
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);

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
            if (IsNew)
                ДоговориКонтрагентів_Objest.Код = (++НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");

            Код.Text = ДоговориКонтрагентів_Objest.Код;
            Назва.Text = ДоговориКонтрагентів_Objest.Назва;
        }

        void GetValue()
        {
            ДоговориКонтрагентів_Objest.Код = Код.Text;
            ДоговориКонтрагентів_Objest.Назва = Назва.Text;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ДоговориКонтрагентів_Objest.New();
                IsNew = false;
            }

            GetValue();

            ДоговориКонтрагентів_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Договір: {ДоговориКонтрагентів_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = ДоговориКонтрагентів_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}