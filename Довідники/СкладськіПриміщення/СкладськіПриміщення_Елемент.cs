/*

        СкладськіПриміщення_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode.Довідники;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class СкладськіПриміщення_Елемент : ДовідникЕлемент
    {
        public СкладськіПриміщення_Objest Елемент { get; set; } = new СкладськіПриміщення_Objest();
        public Склади_Pointer СкладДляНового { get; set; } = new Склади_Pointer();

        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_PointerControl Склад = new Склади_PointerControl() { Caption = "Склад:" };
        ComboBoxText Налаштування = new ComboBoxText();

        public СкладськіПриміщення_Елемент() 
        {
            Element = Елемент;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Склад
            CreateField(vBox, null, Склад);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //НалаштуванняАдресногоЗберігання
            foreach (var field in ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_List())
                Налаштування.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Адресне зберігання:", Налаштування);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Елемент.Склад = СкладДляНового;

            Назва.Text = Елемент.Назва;
            Склад.Pointer = Елемент.Склад;
            Налаштування.ActiveId = Елемент.НалаштуванняАдресногоЗберігання.ToString();

            if (Налаштування.Active == -1)
                Налаштування.ActiveId = НалаштуванняАдресногоЗберігання.НеВикористовувати.ToString();
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Склад = Склад.Pointer;
            Елемент.НалаштуванняАдресногоЗберігання = Enum.Parse<НалаштуванняАдресногоЗберігання>(Налаштування.ActiveId);
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