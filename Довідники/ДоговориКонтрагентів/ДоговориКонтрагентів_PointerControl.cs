
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ДоговориКонтрагентів_PointerControl : PointerControl
    {
        public ДоговориКонтрагентів_PointerControl()
        {
            pointer = new ДоговориКонтрагентів_Pointer();
            WidthPresentation = 300;
            Caption = "Договір:";
        }

        ДоговориКонтрагентів_Pointer pointer;
        public ДоговориКонтрагентів_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Договори", () =>
            {
                ДоговориКонтрагентів page = new ДоговориКонтрагентів();

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (ДоговориКонтрагентів_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ДоговориКонтрагентів_Pointer();
        }
    }
}