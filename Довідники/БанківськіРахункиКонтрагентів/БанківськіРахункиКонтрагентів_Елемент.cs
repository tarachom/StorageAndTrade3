
/*
        БанківськіРахункиКонтрагентів_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class БанківськіРахункиКонтрагентів_Елемент : ДовідникЕлемент
    {
        public БанківськіРахункиКонтрагентів_Objest Елемент { get; set; } = new БанківськіРахункиКонтрагентів_Objest();

        #region Fields
        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Банки_PointerControl Банк = new Банки_PointerControl() { Caption = "Банк", WidthPresentation = 500 };
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl() { Caption = "Контрагент", WidthPresentation = 500 };
        Entry НомерРахунку = new Entry() { WidthRequest = 500 };
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта", WidthPresentation = 300 };

        #endregion

        #region TabularParts

        #endregion

        public БанківськіРахункиКонтрагентів_Елемент() : base()
        {
            Element = Елемент;


        }

        protected override void CreatePack1(Box vBox)
        {

            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Банк
            CreateField(vBox, null, Банк);

            // Контрагент
            CreateField(vBox, null, Контрагент);

            // НомерРахунку
            CreateField(vBox, "Номер рахунку:", НомерРахунку);

            // Валюта
            CreateField(vBox, null, Валюта);

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
            Контрагент.Pointer = Елемент.Контрагент;
            НомерРахунку.Text = Елемент.НомерРахунку;
            Валюта.Pointer = Елемент.Валюта;

        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Банк = Банк.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.НомерРахунку = НомерРахунку.Text;
            Елемент.Валюта = Валюта.Pointer;

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
