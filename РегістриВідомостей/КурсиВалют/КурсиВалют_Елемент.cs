
/*

        КурсиВалют_Елемент.cs

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриВідомостей;

namespace StorageAndTrade
{
    class КурсиВалют_Елемент : РегістриВідомостейЕлемент
    {
        public КурсиВалют_Objest Елемент { get; set; } = new КурсиВалют_Objest();

        public Валюти_Pointer ВалютаДляНового { get; set; } = new Валюти_Pointer();

        DateTimeControl ДатаКурсу = new DateTimeControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        NumericControl Курс = new NumericControl();
        IntegerControl Кратність = new IntegerControl();

        public КурсиВалют_Елемент() 
        {
            Element = Елемент;
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
                Елемент.Валюта = ВалютаДляНового;
                Елемент.Кратність = 1;
            }

            ДатаКурсу.Value = Елемент.Period;
            Валюта.Pointer = Елемент.Валюта;
            Курс.Value = Елемент.Курс;
            Кратність.Value = Елемент.Кратність;
        }

        protected override void GetValue()
        {
            Елемент.Period = ДатаКурсу.Value;
            Елемент.Валюта = Валюта.Pointer;
            Елемент.Курс = Курс.Value;
            Елемент.Кратність = Кратність.Value;
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