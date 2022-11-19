
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class БанківськіРахункиОрганізацій_PointerControl : PointerControl
    {
        public БанківськіРахункиОрганізацій_PointerControl()
        {
            pointer = new БанківськіРахункиОрганізацій_Pointer();
            WidthPresentation = 300;
            Caption = "Банківський рахунок:";
        }

        БанківськіРахункиОрганізацій_Pointer pointer;
        public БанківськіРахункиОрганізацій_Pointer Pointer
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
            БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (БанківськіРахункиОрганізацій_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Банківські рахунки контрагентів", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new БанківськіРахункиОрганізацій_Pointer();
        }
    }
}