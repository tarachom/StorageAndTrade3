using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class СеріїНоменклатури_Елемент : VBox
    {
        public СеріїНоменклатури? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public СеріїНоменклатури_Objest СеріїНоменклатури_Objest { get; set; } = new СеріїНоменклатури_Objest();

        Entry Номер = new Entry() { WidthRequest = 500 };

        public СеріїНоменклатури_Елемент() : base()
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

            //Номер
            HBox hBoxNomer = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNomer, false, false, 5);

            hBoxNomer.PackStart(new Label("Назва:"), false, false, 5);
            hBoxNomer.PackStart(Номер, false, false, 5);

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
            Номер.Text = СеріїНоменклатури_Objest.Номер;
        }

        void GetValue()
        {
            СеріїНоменклатури_Objest.Номер = Номер.Text;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
                СеріїНоменклатури_Objest.New();

            GetValue();

            СеріїНоменклатури_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Серійний номер: {СеріїНоменклатури_Objest.Номер}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = СеріїНоменклатури_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}