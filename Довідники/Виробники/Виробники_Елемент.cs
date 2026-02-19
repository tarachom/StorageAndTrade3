
/*
        Виробники_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Виробники_Елемент : ДовідникЕлемент
    {
        public Виробники_Objest Елемент { get; set; } = new Виробники_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        public Виробники_Елемент()
        {
             Element = Елемент;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
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