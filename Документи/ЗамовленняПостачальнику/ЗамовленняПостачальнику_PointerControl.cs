
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ЗамовленняПостачальнику_PointerControl : PointerControl
    {
        public ЗамовленняПостачальнику_PointerControl()
        {
            pointer = new ЗамовленняПостачальнику_Pointer();
            WidthPresentation = 300;
            Caption = "Замовлення постачальнику:";
        }

        ЗамовленняПостачальнику_Pointer pointer;
        public ЗамовленняПостачальнику_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Документи: Замовлення постачальнику", () =>
            {
                ЗамовленняПостачальнику page = new ЗамовленняПостачальнику(true);

                page.DocumentPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (ЗамовленняПостачальнику_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗамовленняПостачальнику_Pointer();
        }
    }
}