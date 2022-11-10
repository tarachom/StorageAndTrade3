using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СтаттяРухуКоштів_Елемент : VBox
    {
        public СтаттяРухуКоштів? PageList { get; set; }

        public bool IsNew { get; set; } = true;

        public СтаттяРухуКоштів_Objest СтаттяРухуКоштів_Objest { get; set; } = new СтаттяРухуКоштів_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry КореспондуючийРахунок = new Entry() { WidthRequest = 200 };
        ComboBoxText ВидРухуКоштів = new ComboBoxText();
        TextView Опис = new TextView();

        #endregion

        public СтаттяРухуКоштів_Елемент() : base()
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

            //КореспондуючийРахунок
            HBox hBoxKorRahunok = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKorRahunok, false, false, 5);

            hBoxKorRahunok.PackStart(new Label("Кореспондентський рахунок:"), false, false, 5);
            hBoxKorRahunok.PackStart(КореспондуючийРахунок, false, false, 5);

            //ВидРухуКоштів
            HBox hBoxVidRuhu = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVidRuhu, false, false, 5);

            foreach (ConfigurationEnumField field in Config.Kernel!.Conf.Enums["ВидиРухуКоштів"].Fields.Values)
                ВидРухуКоштів.Append(field.Name, field.Desc);

            hBoxVidRuhu.PackStart(new Label("Вид руху коштів:"), false, false, 5);
            hBoxVidRuhu.PackStart(ВидРухуКоштів, false, false, 5);

            hPaned.Pack1(vBox, false, false);

            //Опис
            HBox hBoxOpys = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOpys, false, false, 5);

            hBoxOpys.PackStart(new Label("Опис:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextViewOpys = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 200 };
            scrollTextViewOpys.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextViewOpys.Add(Опис);

            hBoxOpys.PackStart(scrollTextViewOpys, false, false, 5);
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
                СтаттяРухуКоштів_Objest.Код = (++НумераціяДовідників.СтаттяРухуКоштів_Const).ToString("D6");
                СтаттяРухуКоштів_Objest.ВидРухуКоштів = Перелічення.ВидиРухуКоштів.ОплатаОборотнихАктивів;
            }

            Код.Text = СтаттяРухуКоштів_Objest.Код;
            Назва.Text = СтаттяРухуКоштів_Objest.Назва;
            КореспондуючийРахунок.Text = СтаттяРухуКоштів_Objest.КореспондуючийРахунок;
            ВидРухуКоштів.ActiveId = СтаттяРухуКоштів_Objest.ВидРухуКоштів.ToString();
            Опис.Buffer.Text = СтаттяРухуКоштів_Objest.Опис;
        }

        void GetValue()
        {
            СтаттяРухуКоштів_Objest.Код = Код.Text;
            СтаттяРухуКоштів_Objest.Назва = Назва.Text;
            СтаттяРухуКоштів_Objest.КореспондуючийРахунок = КореспондуючийРахунок.Text;
            СтаттяРухуКоштів_Objest.ВидРухуКоштів = Enum.Parse<Перелічення.ВидиРухуКоштів>(ВидРухуКоштів.ActiveId);
            СтаттяРухуКоштів_Objest.Опис = Опис.Buffer.Text;
        }

        #endregion

        void OnSaveClick(object? sender, EventArgs args)
        {
            if (IsNew)
            {
                СтаттяРухуКоштів_Objest.New();
                IsNew = false;
            }

            GetValue();

            СтаттяРухуКоштів_Objest.Save();

            Program.GeneralForm?.RenameCurrentPageNotebook($"Валюта: {СтаттяРухуКоштів_Objest.Назва}");

            if (PageList != null)
            {
                PageList.SelectPointerItem = СтаттяРухуКоштів_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}