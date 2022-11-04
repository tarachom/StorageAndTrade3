
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Номенклатура_PointerControl : PointerControl
    {
        public Номенклатура_PointerControl()
        {
            pointer = new Номенклатура_Pointer();
            WidthPresentation = 300;
            Caption = "Контрагент:";
        }

        Номенклатура_Pointer pointer;
        public Номенклатура_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Номенклатура", () =>
            {
                Номенклатура page = new Номенклатура();

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (Номенклатура_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadTree();

                return page;
            });
        }
    }
}