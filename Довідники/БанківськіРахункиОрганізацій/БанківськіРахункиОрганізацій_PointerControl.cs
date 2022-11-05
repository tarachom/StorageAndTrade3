
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class БанківськіРахункиОрганізацій_PointerControl : PointerControl
    {
        public БанківськіРахункиОрганізацій_PointerControl()
        {
            pointer = new БанківськіРахункиОрганізацій_Pointer();
            WidthPresentation = 300;
            Caption = "Валюта:";
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Банківські рахунки контрагентів", () =>
            {
                БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій();

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (БанківськіРахункиОрганізацій_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new БанківськіРахункиОрганізацій_Pointer();
        }
    }
}