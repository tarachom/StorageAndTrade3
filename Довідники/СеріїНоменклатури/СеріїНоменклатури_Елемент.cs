/*

        СеріїНоменклатури_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class СеріїНоменклатури_Елемент : ДовідникЕлемент
    {
        public СеріїНоменклатури_Objest Елемент { get; set; } = new СеріїНоменклатури_Objest();

        Entry Номер = new Entry() { WidthRequest = 500 };
        Entry Коментар = new Entry() { WidthRequest = 500 };
        DateTimeControl ДатаСтворення = new DateTimeControl();

        public СеріїНоменклатури_Елемент()  
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Номер
            CreateField(vBox, "Номер:", Номер);

            //Коментар
            CreateField(vBox, "Коментар:", Коментар);

            //ДатаСтворення
            CreateField(vBox, "Cтворений:", ДатаСтворення);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Номер.Text = Елемент.Номер;
            Коментар.Text = Елемент.Коментар;
            ДатаСтворення.Value = Елемент.ДатаСтворення;
        }

        protected override void GetValue()
        {
            Елемент.Номер = Номер.Text;
            Елемент.Коментар = Коментар.Text;
            Елемент.ДатаСтворення = ДатаСтворення.Value;
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