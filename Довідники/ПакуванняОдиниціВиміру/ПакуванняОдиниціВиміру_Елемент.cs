/*

        ПакуванняОдиниціВиміру_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk;

using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class ПакуванняОдиниціВиміру_Елемент : ДовідникЕлемент
    {
        public ПакуванняОдиниціВиміру_Objest Елемент { get; set; } = new ПакуванняОдиниціВиміру_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry НазваПовна = new Entry() { WidthRequest = 500 };
        IntegerControl КількістьУпаковок = new IntegerControl();

        public ПакуванняОдиниціВиміру_Елемент() 
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

            //НазваПовна
            CreateField(vBox, "Назва повна:", НазваПовна);

            //КількістьУпаковок
            CreateField(vBox, "Коєфіціент:", КількістьУпаковок);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Елемент.КількістьУпаковок = 1;

            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            НазваПовна.Text = Елемент.НазваПовна;
            КількістьУпаковок.Value = Елемент.КількістьУпаковок;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.НазваПовна = НазваПовна.Text;
            Елемент.КількістьУпаковок = КількістьУпаковок.Value;
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