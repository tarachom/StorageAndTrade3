
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВведенняЗалишків_PointerControl : PointerControl
    {
        public ВведенняЗалишків_PointerControl()
        {
            pointer = new ВведенняЗалишків_Pointer();
            WidthPresentation = 300;
            Caption = "Введення залишків:";
        }

        ВведенняЗалишків_Pointer pointer;
        public ВведенняЗалишків_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Документи: Введення залишків", () =>
            {
                ВведенняЗалишків page = new ВведенняЗалишків(true);

                page.DocumentPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (ВведенняЗалишків_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВведенняЗалишків_Pointer();
        }
    }
}