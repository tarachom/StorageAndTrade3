

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
        public ЦіниНоменклатури_Objest Елемент { get; set; } = new ЦіниНоменклатури_Objest();
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
            Element = Елемент;
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
            Період.Value = Елемент.Period;

            Номенклатура.Pointer = Елемент.Номенклатура;
            ХарактеристикаНоменклатури.Pointer = Елемент.ХарактеристикаНоменклатури;
            ВидЦіни.Pointer = Елемент.ВидЦіни;
            Ціна.Value = Елемент.Ціна;
            Пакування.Pointer = Елемент.Пакування;
            Валюта.Pointer = Елемент.Валюта;

        }

        protected override void GetValue()
        {
            Елемент.Period = Період.Value;

            Елемент.Номенклатура = Номенклатура.Pointer;
            Елемент.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури.Pointer;
            Елемент.ВидЦіни = ВидЦіни.Pointer;
            Елемент.Ціна = Ціна.Value;
            Елемент.Пакування = Пакування.Pointer;
            Елемент.Валюта = Валюта.Pointer;

        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                await Елемент.Save();
                return true;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(new UuidAndText(Елемент.UnigueID.UGuid), Caption, ex);
                return false;
            }
        }
    }
}
