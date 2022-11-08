using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ЗамовленняКлієнта_Елемент : VBox
    {
        public ЗамовленняКлієнта? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest { get; set; } = new ЗамовленняКлієнта_Objest();

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ЗамовленняКлієнта_ТабличнаЧастина_Товари Товари = new ЗамовленняКлієнта_ТабличнаЧастина_Товари();

        public ЗамовленняКлієнта_Елемент() : base()
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
                ЗамовленняКлієнта_Objest.НомерДок = (++НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");
                ЗамовленняКлієнта_Objest.ДатаДок = DateTime.Now;
            }

            НомерДок.Text = ЗамовленняКлієнта_Objest.НомерДок;
            Назва.Text = ЗамовленняКлієнта_Objest.Назва;

            Товари.ЗамовленняКлієнта_Objest = ЗамовленняКлієнта_Objest;
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ЗамовленняКлієнта_Objest.НомерДок = НомерДок.Text;
            ЗамовленняКлієнта_Objest.Назва = $"Замовлення клієнта №{ЗамовленняКлієнта_Objest.НомерДок} від {ЗамовленняКлієнта_Objest.ДатаДок.ToShortDateString()}";
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ЗамовленняКлієнта_Objest.New();
                IsNew = false;
            }

            GetValue();

            ЗамовленняКлієнта_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ЗамовленняКлієнта_Objest.Назва}");

            if (PageList != null)
            {
                Товари.LoadRecords();

                PageList.SelectPointerItem = ЗамовленняКлієнта_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }
    }
}