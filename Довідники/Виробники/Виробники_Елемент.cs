using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Виробники_Елемент : VBox
    {
        public Виробники? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public Виробники_Objest Виробники_Objest { get; set; } = new Виробники_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry КороткаНазва = new Entry() { WidthRequest = 500 };
        Entry Код_R030 = new Entry() { WidthRequest = 500 };

        public Виробники_Елемент() : base()
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
                Виробники_Objest.Код = (++НумераціяДовідників.Виробники_Const).ToString("D6");

            Код.Text = Виробники_Objest.Код;
            Назва.Text = Виробники_Objest.Назва;
        }

        void GetValue()
        {
            Виробники_Objest.Код = Код.Text;
            Виробники_Objest.Назва = Назва.Text;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
                Виробники_Objest.New();

            GetValue();

            Виробники_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Валюта: {Виробники_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = Виробники_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}