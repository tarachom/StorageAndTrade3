using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВнутрішнєСпоживанняТоварів_Елемент : VBox
    {
        public ВнутрішнєСпоживанняТоварів? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварів_Objest { get; set; } = new ВнутрішнєСпоживанняТоварів_Objest();

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари Товари = new ВнутрішнєСпоживанняТоварів_ТабличнаЧастина_Товари();

        public ВнутрішнєСпоживанняТоварів_Елемент() : base()
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
                ВнутрішнєСпоживанняТоварів_Objest.НомерДок = (++НумераціяДокументів.ВнутрішнєСпоживанняТоварів_Const).ToString("D8");
                ВнутрішнєСпоживанняТоварів_Objest.ДатаДок = DateTime.Now;
            }

            НомерДок.Text = ВнутрішнєСпоживанняТоварів_Objest.НомерДок;
            Назва.Text = ВнутрішнєСпоживанняТоварів_Objest.Назва;

            Товари.ВнутрішнєСпоживанняТоварів_Objest = ВнутрішнєСпоживанняТоварів_Objest;
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ВнутрішнєСпоживанняТоварів_Objest.НомерДок = НомерДок.Text;
            ВнутрішнєСпоживанняТоварів_Objest.Назва = $"Внутрішнє споживання товарів №{ВнутрішнєСпоживанняТоварів_Objest.НомерДок} від {ВнутрішнєСпоживанняТоварів_Objest.ДатаДок.ToShortDateString()}";
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ВнутрішнєСпоживанняТоварів_Objest.New();
                IsNew = false;
            }

            GetValue();

            ВнутрішнєСпоживанняТоварів_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ВнутрішнєСпоживанняТоварів_Objest.Назва}");

            if (PageList != null)
            {
                Товари.LoadRecords();

                PageList.SelectPointerItem = ВнутрішнєСпоживанняТоварів_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }
    }
}