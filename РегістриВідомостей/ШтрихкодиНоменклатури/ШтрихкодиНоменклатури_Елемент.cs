
/*
        ШтрихкодиНоменклатури_Елемент.cs
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ШтрихкодиНоменклатури_Елемент : РегістриВідомостейЕлемент
    {
        public ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest { get; set; } = new ШтрихкодиНоменклатури_Objest();

        public Номенклатура_Pointer НоменклатураДляНового { get; set; } = new Номенклатура_Pointer();
        public ХарактеристикиНоменклатури_Pointer ХарактеристикаДляНового { get; set; } = new ХарактеристикиНоменклатури_Pointer();

        DateTimeControl ДатаШтрихкоду = new DateTimeControl();
        Entry Штрихкод = new Entry() { WidthRequest = 500 };
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        ХарактеристикиНоменклатури_PointerControl ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        ПакуванняОдиниціВиміру_PointerControl ПакуванняОдиниціВиміру = new ПакуванняОдиниціВиміру_PointerControl();

        public ШтрихкодиНоменклатури_Елемент() 
        {
            ШтрихкодиНоменклатури_Objest.UnigueIDChanged += UnigueIDChanged;
            ШтрихкодиНоменклатури_Objest.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //ДатаШтрихкоду
            CreateField(vBox, "Дата:", ДатаШтрихкоду);

            //Штрихкод
            CreateField(vBox, "Штрихкод:", Штрихкод);

            //Номенклатура
            CreateField(vBox, null, Номенклатура);

            //ХарактеристикаНоменклатури
            CreateField(vBox, null, ХарактеристикаНоменклатури);

            //ПакуванняОдиниціВиміру
            CreateField(vBox, null, ПакуванняОдиниціВиміру);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                ШтрихкодиНоменклатури_Objest.Номенклатура = НоменклатураДляНового;

                if (!НоменклатураДляНового.IsEmpty())
                {
                    Номенклатура_Objest? Номенклатура_Objest = await НоменклатураДляНового.GetDirectoryObject();
                    if (Номенклатура_Objest != null)
                        ШтрихкодиНоменклатури_Objest.Пакування = Номенклатура_Objest.ОдиницяВиміру;
                }

                ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = ХарактеристикаДляНового;
            }

            ДатаШтрихкоду.Value = ШтрихкодиНоменклатури_Objest.Period;
            Штрихкод.Text = ШтрихкодиНоменклатури_Objest.Штрихкод;
            Номенклатура.Pointer = ШтрихкодиНоменклатури_Objest.Номенклатура;
            ХарактеристикаНоменклатури.Pointer = ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури;
            ПакуванняОдиниціВиміру.Pointer = ШтрихкодиНоменклатури_Objest.Пакування;
        }

        protected override void GetValue()
        {
            ШтрихкодиНоменклатури_Objest.Period = ДатаШтрихкоду.Value;
            ШтрихкодиНоменклатури_Objest.Штрихкод = Штрихкод.Text;
            ШтрихкодиНоменклатури_Objest.Номенклатура = Номенклатура.Pointer;
            ШтрихкодиНоменклатури_Objest.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури.Pointer;
            ШтрихкодиНоменклатури_Objest.Пакування = ПакуванняОдиниціВиміру.Pointer;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                await ШтрихкодиНоменклатури_Objest.Save();
                return true;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(new UuidAndText(ШтрихкодиНоменклатури_Objest.UnigueID.UGuid), Caption, ex);
                return false;
            }
        }
    }
}