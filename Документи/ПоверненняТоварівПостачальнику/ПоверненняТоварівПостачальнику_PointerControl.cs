
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПоверненняТоварівПостачальнику_PointerControl : PointerControl
    {
        public ПоверненняТоварівПостачальнику_PointerControl()
        {
            pointer = new ПоверненняТоварівПостачальнику_Pointer();
            WidthPresentation = 300;
            Caption = "Повернення товарів постачальнику:";
        }

        ПоверненняТоварівПостачальнику_Pointer pointer;
        public ПоверненняТоварівПостачальнику_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Документи: Повернення товарів постачальнику", () =>
            {
                ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику(true);

                page.DocumentPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (ПоверненняТоварівПостачальнику_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПоверненняТоварівПостачальнику_Pointer();
        }
    }
}