
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class БанківськіРахункиКонтрагентів_PointerControl : PointerControl
    {
        public БанківськіРахункиКонтрагентів_PointerControl()
        {
            pointer = new БанківськіРахункиКонтрагентів_Pointer();
            WidthPresentation = 300;
            Caption = "Валюта:";
        }

        БанківськіРахункиКонтрагентів_Pointer pointer;
        public БанківськіРахункиКонтрагентів_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Банківські рахунки контрагентів", () =>
            {
                БанківськіРахункиКонтрагентів page = new БанківськіРахункиКонтрагентів(true);

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (БанківськіРахункиКонтрагентів_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new БанківськіРахункиКонтрагентів_Pointer();
        }
    }
}