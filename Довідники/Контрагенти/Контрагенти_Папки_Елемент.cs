using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Контрагенти_Папки_Елемент : VBox
    {
        public Контрагенти_Папки_Дерево? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public Контрагенти_Папки_Pointer РодичДляНового { get; set; } = new Контрагенти_Папки_Pointer();

        public Контрагенти_Папки_Objest Контрагенти_Папки_Objest { get; set; } = new Контрагенти_Папки_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Контрагенти_Папки_PointerControl Родич = new Контрагенти_Папки_PointerControl();

        public Контрагенти_Папки_Елемент() : base()
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

            //Родич
            HBox hBoxParent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxParent, false, false, 5);

            hBoxParent.PackStart(Родич, false, false, 5);

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
            {
                Контрагенти_Папки_Objest.Код = (++НумераціяДовідників.Контрагенти_Папки_Const).ToString("D6");
                Контрагенти_Папки_Objest.Родич = РодичДляНового;
            }
            else
                Родич.UidOpenFolder = Контрагенти_Папки_Objest.UnigueID.ToString();

            Код.Text = Контрагенти_Папки_Objest.Код;
            Назва.Text = Контрагенти_Папки_Objest.Назва;
            Родич.Pointer = Контрагенти_Папки_Objest.Родич;
        }

        void GetValue()
        {
            Контрагенти_Папки_Objest.Код = Код.Text;
            Контрагенти_Папки_Objest.Назва = Назва.Text;
            Контрагенти_Папки_Objest.Родич = Родич.Pointer;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
                Контрагенти_Папки_Objest.New();

            GetValue();

            Контрагенти_Папки_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Контрагент Папки: {Контрагенти_Папки_Objest.Назва}");

            if (PageList != null)
            {
                PageList.Parent_Pointer = Контрагенти_Папки_Objest.GetDirectoryPointer();
                PageList.LoadTree();
            }
        }
    }
}