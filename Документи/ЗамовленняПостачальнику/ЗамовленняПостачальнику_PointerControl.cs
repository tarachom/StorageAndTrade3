
/*

        ЗамовленняПостачальнику_PointerControl.cs
        PointerControl

*/

using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ЗамовленняПостачальнику_PointerControl : PointerControl
    {
        event EventHandler<ЗамовленняПостачальнику_Pointer> PointerChanged;

        public ЗамовленняПостачальнику_PointerControl()
        {
            pointer = new ЗамовленняПостачальнику_Pointer();
            WidthPresentation = 300;
            Caption = $"{ЗамовленняПостачальнику_Const.FULLNAME}:";
            PointerChanged += async (object? _, ЗамовленняПостачальнику_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ЗамовленняПостачальнику_Pointer pointer;
        public ЗамовленняПостачальнику_Pointer Pointer
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
            ЗамовленняПостачальнику page = new ЗамовленняПостачальнику
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ЗамовленняПостачальнику_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ЗамовленняПостачальнику_Const.FULLNAME}", () => page);
            await page.SetValue();

        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗамовленняПостачальнику_Pointer();
        }
    }
}