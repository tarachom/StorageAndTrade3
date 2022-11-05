
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ФізичніОсоби_PointerControl : PointerControl
    {
        public ФізичніОсоби_PointerControl()
        {
            pointer = new ФізичніОсоби_Pointer();
            WidthPresentation = 300;
            Caption = "Фізична особа:";
        }

        ФізичніОсоби_Pointer pointer;
        public ФізичніОсоби_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Фізичні особи", () =>
            {
                ФізичніОсоби page = new ФізичніОсоби();

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (ФізичніОсоби_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ФізичніОсоби_Pointer();
        }
    }
}