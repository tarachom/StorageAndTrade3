using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПрихіднийКасовийОрдер_Елемент : VBox
    {
        public ПрихіднийКасовийОрдер? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдер_Objest { get; set; } = new ПрихіднийКасовийОрдер_Objest();

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        public ПрихіднийКасовийОрдер_Елемент() : base()
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

            HPaned hPaned = new HPaned() { Orientation = Orientation.Vertical, BorderWidth = 5 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, true, true, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();

            //НомерДок
            HBox hBoxNumberDoc = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNumberDoc, false, false, 5);

            hBoxNumberDoc.PackStart(new Label("Номер:"), false, false, 5);
            hBoxNumberDoc.PackStart(НомерДок, false, false, 5);

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


            hPaned.Pack2(vBox, true, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                ПрихіднийКасовийОрдер_Objest.НомерДок = (++НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
                ПрихіднийКасовийОрдер_Objest.ДатаДок = DateTime.Now;
            }

            НомерДок.Text = ПрихіднийКасовийОрдер_Objest.НомерДок;
            Назва.Text = ПрихіднийКасовийОрдер_Objest.Назва;
        }

        void GetValue()
        {
            ПрихіднийКасовийОрдер_Objest.НомерДок = НомерДок.Text;
            ПрихіднийКасовийОрдер_Objest.Назва = $"Прихідний касовий ордер №{ПрихіднийКасовийОрдер_Objest.НомерДок} від {ПрихіднийКасовийОрдер_Objest.ДатаДок.ToShortDateString()}";
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ПрихіднийКасовийОрдер_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПрихіднийКасовийОрдер_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ПрихіднийКасовийОрдер_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = ПрихіднийКасовийОрдер_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }
    }
}