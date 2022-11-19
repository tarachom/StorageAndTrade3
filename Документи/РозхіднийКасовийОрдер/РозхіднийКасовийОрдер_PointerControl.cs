
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РозхіднийКасовийОрдер_PointerControl : PointerControl
    {
        public РозхіднийКасовийОрдер_PointerControl()
        {
            pointer = new РозхіднийКасовийОрдер_Pointer();
            WidthPresentation = 300;
            Caption = "Розхідний касовий ордер:";
        }

        РозхіднийКасовийОрдер_Pointer pointer;
        public РозхіднийКасовийОрдер_Pointer Pointer
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (РозхіднийКасовийОрдер_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Розхідний касовий ордер", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РозхіднийКасовийОрдер_Pointer();
        }
    }
}