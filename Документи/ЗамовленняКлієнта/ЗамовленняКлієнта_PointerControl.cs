
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ЗамовленняКлієнта_PointerControl : PointerControl
    {
        public ЗамовленняКлієнта_PointerControl()
        {
            pointer = new ЗамовленняКлієнта_Pointer();
            WidthPresentation = 300;
            Caption = "Замовлення клієнта:";
        }

        ЗамовленняКлієнта_Pointer pointer;
        public ЗамовленняКлієнта_Pointer Pointer
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
        public bool UseWherePeriod { get; set; } = false;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ЗамовленняКлієнта page = new ЗамовленняКлієнта(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ЗамовленняКлієнта_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Замовлення клієнта", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗамовленняКлієнта_Pointer();
        }
    }
}