using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПереміщенняТоварів_Елемент : VBox
    {
        public ПереміщенняТоварів? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ПереміщенняТоварів_Objest ПереміщенняТоварів_Objest { get; set; } = new ПереміщенняТоварів_Objest();

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ПереміщенняТоварів_ТабличнаЧастина_Товари Товари = new ПереміщенняТоварів_ТабличнаЧастина_Товари();

        public ПереміщенняТоварів_Елемент() : base()
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
                ПереміщенняТоварів_Objest.НомерДок = (++НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");
                ПереміщенняТоварів_Objest.ДатаДок = DateTime.Now;
            }

            НомерДок.Text = ПереміщенняТоварів_Objest.НомерДок;
            Назва.Text = ПереміщенняТоварів_Objest.Назва;

            Товари.ПереміщенняТоварів_Objest = ПереміщенняТоварів_Objest;
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ПереміщенняТоварів_Objest.НомерДок = НомерДок.Text;
            ПереміщенняТоварів_Objest.Назва = $"Переміщення товарів №{ПереміщенняТоварів_Objest.НомерДок} від {ПереміщенняТоварів_Objest.ДатаДок.ToShortDateString()}";
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ПереміщенняТоварів_Objest.New();
                IsNew = false;
            }

            GetValue();

            ПереміщенняТоварів_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ПереміщенняТоварів_Objest.Назва}");

            if (PageList != null)
            {
                Товари.LoadRecords();

                PageList.SelectPointerItem = ПереміщенняТоварів_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }
    }
}