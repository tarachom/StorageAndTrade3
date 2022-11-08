using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ЗамовленняПостачальнику_Елемент : VBox
    {
        public ЗамовленняПостачальнику? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest { get; set; } = new ЗамовленняПостачальнику_Objest();

        Entry НомерДок = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ЗамовленняПостачальнику_ТабличнаЧастина_Товари Товари = new ЗамовленняПостачальнику_ТабличнаЧастина_Товари();

        public ЗамовленняПостачальнику_Елемент() : base()
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
                ЗамовленняПостачальнику_Objest.НомерДок = (++НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
                ЗамовленняПостачальнику_Objest.ДатаДок = DateTime.Now;
            }

            НомерДок.Text = ЗамовленняПостачальнику_Objest.НомерДок;
            Назва.Text = ЗамовленняПостачальнику_Objest.Назва;

            Товари.ЗамовленняПостачальнику_Objest = ЗамовленняПостачальнику_Objest;
            Товари.LoadRecords();
        }

        void GetValue()
        {
            ЗамовленняПостачальнику_Objest.НомерДок = НомерДок.Text;
            ЗамовленняПостачальнику_Objest.Назва = $"Замовлення постачальнику №{ЗамовленняПостачальнику_Objest.НомерДок} від {ЗамовленняПостачальнику_Objest.ДатаДок.ToShortDateString()}";
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                ЗамовленняПостачальнику_Objest.New();
                IsNew = false;
            }

            GetValue();

            ЗамовленняПостачальнику_Objest.Save();
            Товари.SaveRecords();

            Program.GeneralForm?.RenameCurrentPageNotebook($"{ЗамовленняПостачальнику_Objest.Назва}");

            if (PageList != null)
            {
                Товари.LoadRecords();

                PageList.SelectPointerItem = ЗамовленняПостачальнику_Objest.GetDocumentPointer();
                PageList.LoadRecords();
            }
        }
    }
}