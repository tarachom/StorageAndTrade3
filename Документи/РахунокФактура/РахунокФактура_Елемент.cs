using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РахунокФактура_Елемент : VBox
    {
        public РахунокФактура? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public РахунокФактура_Objest РахунокФактура_Objest { get; set; } = new РахунокФактура_Objest();

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        РахунокФактура_ТабличнаЧастина_Товари Товари = new РахунокФактура_ТабличнаЧастина_Товари();

        public РахунокФактура_Елемент() : base()
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
            Notebook notebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false };
            notebook.TabPos = PositionType.Top;
            notebook.AppendPage(Товари, new Label("Товари"));

            hPaned.Pack2(notebook, true, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                РахунокФактура_Objest.НомерДок = (++НумераціяДокументів.РахунокФактура_Const).ToString("D8");
                РахунокФактура_Objest.ДатаДок = DateTime.Now;
            }

            НомерДок.Text = РахунокФактура_Objest.НомерДок;
            Назва.Text = РахунокФактура_Objest.Назва;

            Товари.РахунокФактура_Objest = РахунокФактура_Objest;
            Товари.LoadRecords();
        }

        void GetValue()
        {
            РахунокФактура_Objest.НомерДок = НомерДок.Text;
            РахунокФактура_Objest.Назва = $"Рахунок фактура №{РахунокФактура_Objest.НомерДок} від {РахунокФактура_Objest.ДатаДок.ToShortDateString()}";
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                РахунокФактура_Objest.New();
                IsNew = false;
            }

            GetValue();

            РахунокФактура_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{РахунокФактура_Objest.Назва}");

            if (PageList != null)
            {
                Товари.LoadRecords();

                PageList.SelectPointerItem = РахунокФактура_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }
    }
}