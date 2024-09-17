
/*
        БанківськіРахункиОрганізацій_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class БанківськіРахункиОрганізацій_Елемент : ДовідникЕлемент
    {
        public БанківськіРахункиОрганізацій_Objest Елемент { get; set; } = new БанківськіРахункиОрганізацій_Objest();

        #region Fields
        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Банки_PointerControl Банк = new Банки_PointerControl() { Caption = "Банк:", WidthPresentation = 500 };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація:", WidthPresentation = 500 };
        Entry НомерРахунку = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта:", WidthPresentation = 300 };
        СтруктураПідприємства_PointerControl Підрозділ = new СтруктураПідприємства_PointerControl() { Caption = "Підрозділ:", WidthPresentation = 500 };

        #endregion

        #region TabularParts

        #endregion

        public БанківськіРахункиОрганізацій_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Банк
            CreateField(vBox, null, Банк);

            // Організація
            CreateField(vBox, null, Організація);

            // НомерРахунку
            CreateField(vBox, "Номер рахунку:", НомерРахунку);

            // Валюта
            CreateField(vBox, null, Валюта);

            // Підрозділ
            CreateField(vBox, null, Підрозділ);
        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Банк.Pointer = Елемент.Банк;
            Організація.Pointer = Елемент.Організація;
            НомерРахунку.Text = Елемент.НомерРахунку;
            Валюта.Pointer = Елемент.Валюта;
            Підрозділ.Pointer = Елемент.Підрозділ;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Банк = Банк.Pointer;
            Елемент.Організація = Організація.Pointer;
            Елемент.НомерРахунку = НомерРахунку.Text;
            Елемент.Валюта = Валюта.Pointer;
            Елемент.Підрозділ = Підрозділ.Pointer;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSaved = false;
            try
            {
                if (await Елемент.Save())
                {
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }
            return isSaved;
        }
    }
}
