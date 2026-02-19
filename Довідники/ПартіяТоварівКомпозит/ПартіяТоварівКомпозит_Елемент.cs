/*

        ПартіяТоварівКомпозит_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode.Довідники;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_Елемент : ДовідникЕлемент
    {
        public ПартіяТоварівКомпозит_Objest Елемент { get; set; } = new ПартіяТоварівКомпозит_Objest();

        Entry Назва = new Entry() { WidthRequest = 500 };
        ComboBoxText ТипДокументу = new ComboBoxText();
        ПоступленняТоварівТаПослуг_PointerControl ПоступленняТоварівТаПослуг = new ПоступленняТоварівТаПослуг_PointerControl();
        ВведенняЗалишків_PointerControl ВведенняЗалишків = new ВведенняЗалишків_PointerControl();

        public ПартіяТоварівКомпозит_Елемент()
        {
            Element = Елемент;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            foreach (var field in ПсевдонімиПерелічення.ТипДокументуПартіяТоварівКомпозит_List())
                ТипДокументу.Append(field.Value.ToString(), field.Name);

            ТипДокументу.ActiveId = ТипДокументуПартіяТоварівКомпозит.ПоступленняТоварівТаПослуг.ToString();
        }

        protected override void CreatePack1(Box vBox)
        {
            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Тип
            CreateField(vBox, "Тип документу:", ТипДокументу);

            //ПоступленняТоварівТаПослуг
            CreateField(vBox, null, ПоступленняТоварівТаПослуг);

            //ВведенняЗалишків
            CreateField(vBox, null, ВведенняЗалишків);
        }

        protected override void CreatePack2(Box vBox)
        {
            Box hBoxInfo = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
            vBox.PackStart(hBoxInfo, false, false, 5);

            hBoxInfo.PackStart(new Label("Редагувати дозволено тільки назву"), false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Назва.Text = Елемент.Назва;
            ТипДокументу.ActiveId = Елемент.ТипДокументу.ToString();
            ПоступленняТоварівТаПослуг.Pointer = Елемент.ПоступленняТоварівТаПослуг;
            ВведенняЗалишків.Pointer = Елемент.ВведенняЗалишків;
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;

            /*
            Редагування заборонено, тільки назва

            Елемент.ТипДокументу = Enum.Parse<Перелічення.ТипДокументуПартіяТоварівКомпозит>(ТипДокументу.ActiveId);
            Елемент.ПоступленняТоварівТаПослуг = ПоступленняТоварівТаПослуг.Pointer;
            Елемент.ВведенняЗалишків = ВведенняЗалишків.Pointer;
            
            */
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