/*
        Контрагенти_Папки_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Контрагенти_Папки_Елемент : ДовідникЕлемент
    {
        public Контрагенти_Папки_Objest Елемент { get; set; } = new Контрагенти_Папки_Objest();
        public Контрагенти_Папки_Pointer РодичДляНового { get; set; } = new Контрагенти_Папки_Pointer();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Контрагенти_Папки_PointerControl Родич = new Контрагенти_Папки_PointerControl() { Caption = "Папка:" };

        public Контрагенти_Папки_Елемент()  
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

            //Родич
            CreateField(vBox, null, Родич);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Елемент.Родич = РодичДляНового;
            else
                Родич.OpenFolder = Елемент.UnigueID;

            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Родич.Pointer = Елемент.Родич;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Родич = Родич.Pointer;
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