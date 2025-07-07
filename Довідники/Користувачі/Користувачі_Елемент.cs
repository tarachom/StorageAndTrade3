/*

        Користувачі_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Користувачі_Елемент : ДовідникЕлемент
    {
        public Користувачі_Objest Елемент { get; set; } = new Користувачі_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ФізичніОсоби_PointerControl ФізичнаОсоба = new ФізичніОсоби_PointerControl() { Caption = "Фізична особа:" };
        TextView Коментар = new TextView();

        public Користувачі_Елемент() 
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

            //ФізичнаОсоба
            CreateField(vBox, null, ФізичнаОсоба);

            //Коментар
            CreateFieldView(vBox, "Коментар:", Коментар, 500, 200);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            ФізичнаОсоба.Pointer = Елемент.ФізичнаОсоба;
            Коментар.Buffer.Text = Елемент.Коментар;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.ФізичнаОсоба = ФізичнаОсоба.Pointer;
            Елемент.Коментар = Коментар.Buffer.Text;
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