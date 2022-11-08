using Gtk;

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class БанківськіРахункиОрганізацій_Елемент : VBox
    {
        public БанківськіРахункиОрганізацій? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest { get; set; } = new БанківськіРахункиОрганізацій_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        Організації_PointerControl Організація = new Організації_PointerControl();

        public БанківськіРахункиОрганізацій_Елемент() : base()
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

            //Валюта
            HBox hBoxValuta = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxValuta, false, false, 5);

            hBoxValuta.PackStart(Валюта, false, false, 5);

            //Організація
            HBox hBoxOrganisation = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOrganisation, false, false, 5);

            hBoxOrganisation.PackStart(Організація, false, false, 5);

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
                БанківськіРахункиОрганізацій_Objest.Код = (++НумераціяДовідників.БанківськіРахункиОрганізацій_Const).ToString("D6");

            Код.Text = БанківськіРахункиОрганізацій_Objest.Код;
            Назва.Text = БанківськіРахункиОрганізацій_Objest.Назва;
            Валюта.Pointer = БанківськіРахункиОрганізацій_Objest.Валюта;
            Організація.Pointer = БанківськіРахункиОрганізацій_Objest.Організація;
        }

        void GetValue()
        {
            БанківськіРахункиОрганізацій_Objest.Код = Код.Text;
            БанківськіРахункиОрганізацій_Objest.Назва = Назва.Text;
            БанківськіРахункиОрганізацій_Objest.Валюта = Валюта.Pointer;
            БанківськіРахункиОрганізацій_Objest.Організація = Організація.Pointer;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                БанківськіРахункиОрганізацій_Objest.New();
                IsNew = false;
            }

            GetValue();

            БанківськіРахункиОрганізацій_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Банківський рахунок організації: {БанківськіРахункиОрганізацій_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = БанківськіРахункиОрганізацій_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}