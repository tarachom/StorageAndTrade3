
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПоступленняТоварівТаПослуг_PointerControl : PointerControl
    {
        public ПоступленняТоварівТаПослуг_PointerControl()
        {
            pointer = new ПоступленняТоварівТаПослуг_Pointer();
            WidthPresentation = 300;
            Caption = "Поступлення товарів та послуг:";
        }

        ПоступленняТоварівТаПослуг_Pointer pointer;
        public ПоступленняТоварівТаПослуг_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;

                if (pointer != null)
                    Presentation = pointer.GetPresentation();
                else
                    Presentation = "";
            }
        }

        //Відбір по періоду в журналі
        public bool UseWherePeriod { get; set; } = true;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ПоступленняТоварівТаПослуг_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Поступлення товарів та послуг", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПоступленняТоварівТаПослуг_Pointer();
        }
    }
}