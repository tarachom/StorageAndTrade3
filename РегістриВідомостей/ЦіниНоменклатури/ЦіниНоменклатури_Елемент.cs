

/*
        ЦіниНоменклатури_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.РегістриВідомостей;

namespace StorageAndTrade
{
    class ЦіниНоменклатури_Елемент : РегістриВідомостейЕлемент
    {
        public ЦіниНоменклатури_Objest ЦіниНоменклатури_Objest { get; set; } = new ЦіниНоменклатури_Objest();
        DateTimeControl Період = new DateTimeControl();

        #region Fields

        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl() { Caption = "Номенклатура", WidthPresentation = 300 };
        ХарактеристикиНоменклатури_PointerControl ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_PointerControl() { Caption = "ХарактеристикаНоменклатури", WidthPresentation = 300 };
        ВидиЦін_PointerControl ВидЦіни = new ВидиЦін_PointerControl() { Caption = "ВидЦіни", WidthPresentation = 300 };

        NumericControl Ціна = new NumericControl();
        ПакуванняОдиниціВиміру_PointerControl Пакування = new ПакуванняОдиниціВиміру_PointerControl() { Caption = "Пакування", WidthPresentation = 300 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта", WidthPresentation = 300 };


        #endregion

        public ЦіниНоменклатури_Елемент() 
        {
            ЦіниНоменклатури_Objest.UnigueIDChanged += UnigueIDChanged;
            ЦіниНоменклатури_Objest.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Період
            CreateField(vBox, "Період:", Період);

            //Номенклатура

            CreateField(vBox, null, Номенклатура);

            //ХарактеристикаНоменклатури

            CreateField(vBox, null, ХарактеристикаНоменклатури);

            //ВидЦіни

            CreateField(vBox, null, ВидЦіни);

            //Ціна
            CreateField(vBox, "Ціна:", Ціна);

            //Пакування

            CreateField(vBox, null, Пакування);

            //Валюта

            CreateField(vBox, null, Валюта);

        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Період.Value = ЦіниНоменклатури_Objest.Period;

            Номенклатура.Pointer = ЦіниНоменклатури_Objest.Номенклатура;
            ХарактеристикаНоменклатури.Pointer = ЦіниНоменклатури_Objest.ХарактеристикаНоменклатури;
            ВидЦіни.Pointer = ЦіниНоменклатури_Objest.ВидЦіни;
            Ціна.Value = ЦіниНоменклатури_Objest.Ціна;
            Пакування.Pointer = ЦіниНоменклатури_Objest.Пакування;
            Валюта.Pointer = ЦіниНоменклатури_Objest.Валюта;

        }

        protected override void GetValue()
        {
            ЦіниНоменклатури_Objest.Period = Період.Value;

            ЦіниНоменклатури_Objest.Номенклатура = Номенклатура.Pointer;
            ЦіниНоменклатури_Objest.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури.Pointer;
            ЦіниНоменклатури_Objest.ВидЦіни = ВидЦіни.Pointer;
            ЦіниНоменклатури_Objest.Ціна = Ціна.Value;
            ЦіниНоменклатури_Objest.Пакування = Пакування.Pointer;
            ЦіниНоменклатури_Objest.Валюта = Валюта.Pointer;

        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                await ЦіниНоменклатури_Objest.Save();
                return true;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(new UuidAndText(ЦіниНоменклатури_Objest.UnigueID.UGuid), Caption, ex);
                return false;
            }
        }
    }
}
