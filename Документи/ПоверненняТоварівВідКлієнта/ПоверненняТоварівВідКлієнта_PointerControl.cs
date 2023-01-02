
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПоверненняТоварівВідКлієнта_PointerControl : PointerControl
    {
        public ПоверненняТоварівВідКлієнта_PointerControl()
        {
            pointer = new ПоверненняТоварівВідКлієнта_Pointer();
            WidthPresentation = 300;
            Caption = "Повернення товарів від клієнта:";
        }

        ПоверненняТоварівВідКлієнта_Pointer pointer;
        public ПоверненняТоварівВідКлієнта_Pointer Pointer
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
            ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ПоверненняТоварівВідКлієнта_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Повернення товарів від клієнта", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПоверненняТоварівВідКлієнта_Pointer();
        }
    }
}