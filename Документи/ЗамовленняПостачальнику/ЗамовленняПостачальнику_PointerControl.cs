
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

        //Відбір по періоду в журналі
        public bool UseWherePeriod { get; set; } = false;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ЗамовленняПостачальнику page = new ЗамовленняПостачальнику(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ЗамовленняПостачальнику_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Замовлення постачальнику", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗамовленняПостачальнику_Pointer();
        }
    }
}