
/*

        КурсиВалют_Елемент.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриВідомостей;

namespace StorageAndTrade
{
    class КурсиВалют_Елемент : РегістриВідомостейЕлемент
    {
        public КурсиВалют_Objest КурсиВалют_Objest { get; set; } = new КурсиВалют_Objest();

        public Валюти_Pointer ВалютаДляНового { get; set; } = new Валюти_Pointer();

        DateTimeControl ДатаКурсу = new DateTimeControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        NumericControl Курс = new NumericControl();
        IntegerControl Кратність = new IntegerControl();

        public КурсиВалют_Елемент() 
        {
            КурсиВалют_Objest.UnigueIDChanged += UnigueIDChanged;
            КурсиВалют_Objest.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //ДатаКурсу
            CreateField(vBox, "Дата:", ДатаКурсу);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Курс
            CreateField(vBox, "Курс:", Курс);

            //Кратність
            CreateField(vBox, "Кратність:", Кратність);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                КурсиВалют_Objest.Валюта = ВалютаДляНового;
                КурсиВалют_Objest.Кратність = 1;
            }

            ДатаКурсу.Value = КурсиВалют_Objest.Period;
            Валюта.Pointer = КурсиВалют_Objest.Валюта;
            Курс.Value = КурсиВалют_Objest.Курс;
            Кратність.Value = КурсиВалют_Objest.Кратність;
        }

        protected override void GetValue()
        {
            КурсиВалют_Objest.Period = ДатаКурсу.Value;
            КурсиВалют_Objest.Валюта = Валюта.Pointer;
            КурсиВалют_Objest.Курс = Курс.Value;
            КурсиВалют_Objest.Кратність = Кратність.Value;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                await КурсиВалют_Objest.Save();
                return true;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(new UuidAndText(КурсиВалют_Objest.UnigueID.UGuid), Caption, ex);
                return false;
            }
        }
    }
}