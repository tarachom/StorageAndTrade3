using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Склади_Елемент : VBox
    {
        public Склади? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public Склади_Папки_Pointer РодичДляНового { get; set; } = new Склади_Папки_Pointer();

        public Склади_Objest Склади_Objest { get; set; } = new Склади_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_Папки_PointerControl Родич = new Склади_Папки_PointerControl() { Caption = "Папка:" };

        public Склади_Елемент() : base()
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

            //Родич
            HBox hBoxParent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxParent, false, false, 5);

            hBoxParent.PackStart(Родич, false, false, 5);

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
            {
                Склади_Objest.Код = (++НумераціяДовідників.Склади_Const).ToString("D6");
                Склади_Objest.Папка = РодичДляНового;
            }

            Код.Text = Склади_Objest.Код;
            Назва.Text = Склади_Objest.Назва;
            Родич.Pointer = Склади_Objest.Папка;
        }

        void GetValue()
        {
            Склади_Objest.Код = Код.Text;
            Склади_Objest.Назва = Назва.Text;
            Склади_Objest.Папка = Родич.Pointer;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
                Склади_Objest.New();

            GetValue();

            Склади_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Контрагент: {Склади_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = Склади_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}