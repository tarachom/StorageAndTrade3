
/*
        ШтрихкодиНоменклатури_Елемент.cs
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриВідомостей;

namespace StorageAndTrade
{
    class ШтрихкодиНоменклатури_Елемент : РегістриВідомостейЕлемент
    {
        public ШтрихкодиНоменклатури_Objest Елемент { get; set; } = new ШтрихкодиНоменклатури_Objest();

        public Номенклатура_Pointer НоменклатураДляНового { get; set; } = new Номенклатура_Pointer();
        public ХарактеристикиНоменклатури_Pointer ХарактеристикаДляНового { get; set; } = new ХарактеристикиНоменклатури_Pointer();

        DateTimeControl ДатаШтрихкоду = new DateTimeControl();
        Entry Штрихкод = new Entry() { WidthRequest = 500 };
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl();
        ХарактеристикиНоменклатури_PointerControl ХарактеристикаНоменклатури = new ХарактеристикиНоменклатури_PointerControl();
        ПакуванняОдиниціВиміру_PointerControl ПакуванняОдиниціВиміру = new ПакуванняОдиниціВиміру_PointerControl();

        public ШтрихкодиНоменклатури_Елемент() 
        {
            Element = Елемент;
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
                Елемент.Номенклатура = НоменклатураДляНового;

                if (!НоменклатураДляНового.IsEmpty())
                {
                    Номенклатура_Objest? Номенклатура_Objest = await НоменклатураДляНового.GetDirectoryObject();
                    if (Номенклатура_Objest != null)
                        Елемент.Пакування = Номенклатура_Objest.ОдиницяВиміру;
                }

                Елемент.ХарактеристикаНоменклатури = ХарактеристикаДляНового;
            }

            ДатаШтрихкоду.Value = Елемент.Period;
            Штрихкод.Text = Елемент.Штрихкод;
            Номенклатура.Pointer = Елемент.Номенклатура;
            ХарактеристикаНоменклатури.Pointer = Елемент.ХарактеристикаНоменклатури;
            ПакуванняОдиниціВиміру.Pointer = Елемент.Пакування;
        }

        protected override void GetValue()
        {
            Елемент.Period = ДатаШтрихкоду.Value;
            Елемент.Штрихкод = Штрихкод.Text;
            Елемент.Номенклатура = Номенклатура.Pointer;
            Елемент.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури.Pointer;
            Елемент.Пакування = ПакуванняОдиниціВиміру.Pointer;
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