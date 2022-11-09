
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Контрагенти_PointerControl : PointerControl
    {
        public Контрагенти_PointerControl()
        {
            pointer = new Контрагенти_Pointer();
            WidthPresentation = 300;
            Caption = "Контрагент:";
        }

        Контрагенти_Pointer pointer;
        public Контрагенти_Pointer Pointer
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
            if (BeforeClickOpenFunc != null)
                BeforeClickOpenFunc.Invoke();

            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Контрагенти", () =>
            {
                Контрагенти page = new Контрагенти(true);

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (Контрагенти_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;

                    if (AfterSelectFunc != null)
                        AfterSelectFunc.Invoke();
                };

                page.LoadTree();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Контрагенти_Pointer();
        }
    }
}