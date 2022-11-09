
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Склади_PointerControl : PointerControl
    {
        public Склади_PointerControl()
        {
            pointer = new Склади_Pointer();
            WidthPresentation = 300;
            Caption = "Склад:";
        }

        Склади_Pointer pointer;
        public Склади_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Склади", () =>
            {
                Склади page = new Склади(true);

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (Склади_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadTree();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Склади_Pointer();
        }
    }
}