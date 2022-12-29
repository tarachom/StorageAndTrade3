using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ПакуванняОдиниціВиміру_Елемент : VBox
    {
        public ПакуванняОдиниціВиміру? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПакуванняОдиниціВиміру_Objest ПакуванняОдиниціВиміру_Objest { get; set; } = new ПакуванняОдиниціВиміру_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry НазваПовна = new Entry() { WidthRequest = 500 };
        IntegerControl КількістьУпаковок = new IntegerControl();

        public ПакуванняОдиниціВиміру_Елемент() : base()
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

            //НазваПовна
            HBox hBoxNameFull = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNameFull, false, false, 5);

            hBoxNameFull.PackStart(new Label("Назва повна:"), false, false, 5);
            hBoxNameFull.PackStart(НазваПовна, false, false, 5);

            //КількістьУпаковок
            HBox hBoxKvoPack = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKvoPack, false, false, 5);

            hBoxKvoPack.PackStart(new Label("Кількість упаковок:"), false, false, 5);
            hBoxKvoPack.PackStart(КількістьУпаковок, false, false, 5);

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
                ПакуванняОдиниціВиміру_Objest.Код = (++НумераціяДовідників.ПакуванняОдиниціВиміру_Const).ToString("D6");

            Код.Text = ПакуванняОдиниціВиміру_Objest.Код;
            Назва.Text = ПакуванняОдиниціВиміру_Objest.Назва;
            НазваПовна.Text = ПакуванняОдиниціВиміру_Objest.НазваПовна;
            КількістьУпаковок.Value = ПакуванняОдиниціВиміру_Objest.КількістьУпаковок;
        }

        void GetValue()
        {
            ПакуванняОдиниціВиміру_Objest.Код = Код.Text;
            ПакуванняОдиниціВиміру_Objest.Назва = Назва.Text;
            ПакуванняОдиниціВиміру_Objest.НазваПовна = НазваПовна.Text;
            ПакуванняОдиниціВиміру_Objest.КількістьУпаковок = КількістьУпаковок.Value;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ПакуванняОдиниціВиміру_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПакуванняОдиниціВиміру_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Валюта: {ПакуванняОдиниціВиміру_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = ПакуванняОдиниціВиміру_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}