
/*
        КасиККМ_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class КасиККМ_Елемент : ДовідникЕлемент
    {
        public КасиККМ_Objest Елемент { get; init; } = new КасиККМ_Objest();

        #region Fields
        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_PointerControl Склад = new Склади_PointerControl() { Caption = "Склад", WidthPresentation = 300 };
        ComboBoxText Тип = new ComboBoxText();
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта", WidthPresentation = 300 };

        #endregion

        #region TabularParts

        #endregion

        public КасиККМ_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;


            foreach (var field in ПсевдонімиПерелічення.ТипККМ_List())
                Тип.Append(field.Value.ToString(), field.Name);
        }

        protected override void CreatePack1(Box vBox)
        {

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Склад
            CreateField(vBox, null, Склад);

            // Тип
            CreateField(vBox, "Тип:", Тип);

            // Валюта
            CreateField(vBox, null, Валюта);
        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                Елемент.Валюта = ЗначенняТипові.ОсновнаВалюта_Const;
                Елемент.Склад = ЗначенняТипові.ОсновнийСклад_Const;
                Елемент.Тип = ТипККМ.Фіскальний;
            }

            Назва.Text = Елемент.Назва;
            Склад.Pointer = Елемент.Склад;
            Тип.ActiveId = Елемент.Тип.ToString();
            Валюта.Pointer = Елемент.Валюта;

        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Склад = Склад.Pointer;
            Елемент.Тип = ПсевдонімиПерелічення.ТипККМ_FindByName(Тип.ActiveId);
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
