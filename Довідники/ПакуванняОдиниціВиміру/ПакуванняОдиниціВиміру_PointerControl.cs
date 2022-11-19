
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ПакуванняОдиниціВиміру_PointerControl : PointerControl
    {
        public ПакуванняОдиниціВиміру_PointerControl()
        {
            pointer = new ПакуванняОдиниціВиміру_Pointer();
            WidthPresentation = 300;
            Caption = "Пакування:";
        }

        ПакуванняОдиниціВиміру_Pointer pointer;
        public ПакуванняОдиниціВиміру_Pointer Pointer
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
            ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ПакуванняОдиниціВиміру_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Пакування одиниці виміру", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПакуванняОдиниціВиміру_Pointer();
        }
    }
}