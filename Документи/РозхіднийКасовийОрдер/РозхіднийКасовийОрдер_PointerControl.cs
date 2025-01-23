
/*

        РозхіднийКасовийОрдер_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class РозхіднийКасовийОрдер_PointerControl : PointerControl
    {
        event EventHandler<РозхіднийКасовийОрдер_Pointer> PointerChanged;

        public РозхіднийКасовийОрдер_PointerControl()
        {
            pointer = new РозхіднийКасовийОрдер_Pointer();
            WidthPresentation = 300;
            Caption = $"{РозхіднийКасовийОрдер_Const.FULLNAME}:";
            PointerChanged += async (object? _, РозхіднийКасовийОрдер_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        РозхіднийКасовийОрдер_Pointer pointer;
        public РозхіднийКасовийОрдер_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                PointerChanged?.Invoke(null, pointer);
            }
        }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new РозхіднийКасовийОрдер_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {РозхіднийКасовийОрдер_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РозхіднийКасовийОрдер_Pointer();
        }
    }
}