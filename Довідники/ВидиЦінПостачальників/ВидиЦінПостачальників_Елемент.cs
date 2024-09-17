
/*
        ВидиЦінПостачальників_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ВидиЦінПостачальників_Елемент : ДовідникЕлемент
    {
        public ВидиЦінПостачальників_Objest Елемент { get; set; } = new ВидиЦінПостачальників_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта:" };

        public ВидиЦінПостачальників_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Валюта
            CreateField(vBox, null, Валюта);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Валюта.Pointer = Елемент.Валюта;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Валюта = Валюта.Pointer;
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