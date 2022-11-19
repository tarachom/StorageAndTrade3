
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class БанківськіРахункиКонтрагентів_PointerControl : PointerControl
    {
        public БанківськіРахункиКонтрагентів_PointerControl()
        {
            pointer = new БанківськіРахункиКонтрагентів_Pointer();
            WidthPresentation = 300;
            Caption = "Банківський рахунок контрагента:";
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
            БанківськіРахункиКонтрагентів page = new БанківськіРахункиКонтрагентів(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (БанківськіРахункиКонтрагентів_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Банківські рахунки контрагентів", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new БанківськіРахункиКонтрагентів_Pointer();
        }
    }
}