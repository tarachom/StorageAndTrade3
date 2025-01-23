/*

        ДоговориКонтрагентів_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk;

using GeneratedCode.Довідники;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ДоговориКонтрагентів_Елемент : ДовідникЕлемент
    {
        public ДоговориКонтрагентів_Objest Елемент { get; set; } = new ДоговориКонтрагентів_Objest();
        public Контрагенти_Pointer КонтрагентиДляНового { get; set; } = new Контрагенти_Pointer();
        
        #region Fields

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        DateTimeControl Дата = new DateTimeControl() { OnlyDate = true };
        Entry Номер = new Entry() { WidthRequest = 100 };
        БанківськіРахункиОрганізацій_PointerControl БанківськийРахунок = new БанківськіРахункиОрганізацій_PointerControl() { Caption = "Банківський рахунок:" };
        БанківськіРахункиКонтрагентів_PointerControl БанківськийРахунокКонтрагента = new БанківськіРахункиКонтрагентів_PointerControl() { Caption = "Банківський рахунок:" };
        Валюти_PointerControl ВалютаВзаєморозрахунків = new Валюти_PointerControl() { Caption = "Валюта:" };
        DateTimeControl ДатаПочаткуДії = new DateTimeControl() { OnlyDate = true };
        DateTimeControl ДатаЗакінченняДії = new DateTimeControl() { OnlyDate = true };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація:" };
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl() { Caption = "Контрагент:" };
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:" };
        CheckButton Узгоджений = new CheckButton("Узгоджений");
        ComboBoxText Статус = new ComboBoxText();
        ComboBoxText ГосподарськаОперація = new ComboBoxText();
        ComboBoxText ТипДоговору = new ComboBoxText();
        NumericControl ДопустимаСумаЗаборгованості = new NumericControl();
        NumericControl Сума = new NumericControl();
        Entry Коментар = new Entry() { WidthRequest = 500 };

        #endregion

        public ДоговориКонтрагентів_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            //1
            foreach (var field in ПсевдонімиПерелічення.ГосподарськіОперації_List())
                ГосподарськаОперація.Append(field.Value.ToString(), field.Name);

            ГосподарськаОперація.ActiveId = ГосподарськіОперації.РеалізаціяКлієнту.ToString();

            //2
            foreach (var field in ПсевдонімиПерелічення.СтатусиДоговорівКонтрагентів_List())
                Статус.Append(field.Value.ToString(), field.Name);

            Статус.ActiveId = СтатусиДоговорівКонтрагентів.Діє.ToString();

            //3
            foreach (var field in ПсевдонімиПерелічення.ТипДоговорів_List())
                ТипДоговору.Append(field.Value.ToString(), field.Name);

            ТипДоговору.ActiveId = ТипДоговорів.ЗПокупцями.ToString();
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Дата
            CreateField(vBox, "Дата:", Дата);

            //Номер
            CreateField(vBox, "Номер:", Номер);

            //БанківськийРахунок
            CreateField(vBox, null, БанківськийРахунок);

            //БанківськийРахунокКонтрагента
            CreateField(vBox, null, БанківськийРахунокКонтрагента);

            //ВалютаВзаєморозрахунків
            CreateField(vBox, null, ВалютаВзаєморозрахунків);

            //ДатаПочаткуДії
            CreateField(vBox, "Дата початку дії:", ДатаПочаткуДії);

            //ДатаЗакінченняДії
            CreateField(vBox, "Дата закінчення дії:", ДатаЗакінченняДії);

            //Організація
            CreateField(vBox, null, Організація);

            //Контрагент
            CreateField(vBox, null, Контрагент);

            //Підрозділ
            CreateField(vBox, null, Підрозділ);

            //Узгоджений
            CreateField(vBox, null, Узгоджений);

            //Статус
            CreateField(vBox, "Статус:", Статус);

            //ГосподарськаОперація
            CreateField(vBox, "Господарська операція:", ГосподарськаОперація);

            //ТипДоговору
            CreateField(vBox, "Тип договору:", ТипДоговору);

            //ДопустимаСумаЗаборгованості
            CreateField(vBox, "Допустима сума заборгованості:", ДопустимаСумаЗаборгованості);

            //Сума
            CreateField(vBox, "Сума:", Сума);

            //Коментар
            CreateField(vBox, "Коментар:", Коментар);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Елемент.Контрагент = КонтрагентиДляНового;

            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Дата.Value = Елемент.Дата;
            Номер.Text = Елемент.Номер;
            БанківськийРахунок.Pointer = Елемент.БанківськийРахунок;
            БанківськийРахунокКонтрагента.Pointer = Елемент.БанківськийРахунокКонтрагента;
            ВалютаВзаєморозрахунків.Pointer = Елемент.ВалютаВзаєморозрахунків;
            ДатаПочаткуДії.Value = Елемент.ДатаПочаткуДії;
            ДатаЗакінченняДії.Value = Елемент.ДатаЗакінченняДії;
            Організація.Pointer = Елемент.Організація;
            Контрагент.Pointer = Елемент.Контрагент;
            Підрозділ.Pointer = Елемент.Підрозділ;
            Узгоджений.Active = Елемент.Узгоджений;
            Статус.ActiveId = Елемент.Статус.ToString();
            ГосподарськаОперація.ActiveId = Елемент.ГосподарськаОперація.ToString();
            ТипДоговору.ActiveId = Елемент.ТипДоговору.ToString();
            ДопустимаСумаЗаборгованості.Value = Елемент.ДопустимаСумаЗаборгованості;
            Сума.Value = Елемент.Сума;
            Коментар.Text = Елемент.Коментар;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Дата = Дата.Value;
            Елемент.Номер = Номер.Text;
            Елемент.БанківськийРахунок = БанківськийРахунок.Pointer;
            Елемент.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента.Pointer;
            Елемент.ВалютаВзаєморозрахунків = ВалютаВзаєморозрахунків.Pointer;
            Елемент.ДатаПочаткуДії = ДатаПочаткуДії.Value;
            Елемент.ДатаЗакінченняДії = ДатаЗакінченняДії.Value;
            Елемент.Організація = Організація.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Підрозділ = Підрозділ.Pointer;
            Елемент.Узгоджений = Узгоджений.Active;
            Елемент.Статус = Enum.Parse<СтатусиДоговорівКонтрагентів>(Статус.ActiveId);
            Елемент.ГосподарськаОперація = Enum.Parse<ГосподарськіОперації>(ГосподарськаОперація.ActiveId);
            Елемент.ТипДоговору = Enum.Parse<ТипДоговорів>(ТипДоговору.ActiveId);
            Елемент.ДопустимаСумаЗаборгованості = ДопустимаСумаЗаборгованості.Value;
            Елемент.Сума = Сума.Value;
            Елемент.Коментар = Коментар.Text;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                return await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
                return false;
            }
        }
    }
}