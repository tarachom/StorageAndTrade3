using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВведенняЗалишків_Елемент : VBox
    {
        public ВведенняЗалишків? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ВведенняЗалишків_Objest ВведенняЗалишків_Objest { get; set; } = new ВведенняЗалишків_Objest();

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ВведенняЗалишків_ТабличнаЧастина_Товари Товари = new ВведенняЗалишків_ТабличнаЧастина_Товари();

        public ВведенняЗалишків_Елемент() : base()
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
            hPaned.Pack2(Товари, true, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                ВведенняЗалишків_Objest.НомерДок = (++НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");
                ВведенняЗалишків_Objest.ДатаДок = DateTime.Now;
            }

            НомерДок.Text = ВведенняЗалишків_Objest.НомерДок;
            Назва.Text = ВведенняЗалишків_Objest.Назва;

            Товари.ВведенняЗалишків_Objest = ВведенняЗалишків_Objest;
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ВведенняЗалишків_Objest.НомерДок = НомерДок.Text;
            ВведенняЗалишків_Objest.Назва = $"Введення залишків №{ВведенняЗалишків_Objest.НомерДок} від {ВведенняЗалишків_Objest.ДатаДок.ToShortDateString()}";
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
                ВведенняЗалишків_Objest.New();

            GetValue();

            ВведенняЗалишків_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Введення залишків: {ВведенняЗалишків_Objest.Назва}");

            if (PageList != null)
            {
                Товари.LoadRecords();

                PageList.SelectPointerItem = ВведенняЗалишків_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }
    }
}