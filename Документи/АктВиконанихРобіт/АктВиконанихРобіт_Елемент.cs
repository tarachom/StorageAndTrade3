using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class АктВиконанихРобіт_Елемент : VBox
    {
        public АктВиконанихРобіт? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest { get; set; } = new АктВиконанихРобіт_Objest();

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        АктВиконанихРобіт_ТабличнаЧастина_Послуги Послуги = new АктВиконанихРобіт_ТабличнаЧастина_Послуги();

        public АктВиконанихРобіт_Елемент() : base()
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
            hPaned.Pack2(Послуги, true, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                АктВиконанихРобіт_Objest.НомерДок = (++НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");
                АктВиконанихРобіт_Objest.ДатаДок = DateTime.Now;
            }

            НомерДок.Text = АктВиконанихРобіт_Objest.НомерДок;
            Назва.Text = АктВиконанихРобіт_Objest.Назва;

            Послуги.АктВиконанихРобіт_Objest = АктВиконанихРобіт_Objest;
            Послуги.LoadRecords();
        }

        void GetValue()
        {
            АктВиконанихРобіт_Objest.НомерДок = НомерДок.Text;
            АктВиконанихРобіт_Objest.Назва = $"Акт виконаних робіт №{АктВиконанихРобіт_Objest.НомерДок} від {АктВиконанихРобіт_Objest.ДатаДок.ToShortDateString()}";
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
                АктВиконанихРобіт_Objest.New();

            GetValue();

            АктВиконанихРобіт_Objest.Save();
            Послуги.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"АктВиконанихРобіт: {АктВиконанихРобіт_Objest.Назва}");

            if (PageList != null)
            {
                Послуги.LoadRecords();

                PageList.SelectPointerItem = АктВиконанихРобіт_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }
    }
}