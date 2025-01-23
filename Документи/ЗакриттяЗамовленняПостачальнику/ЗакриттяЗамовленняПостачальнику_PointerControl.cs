

/*     
        ЗакриттяЗамовленняПостачальнику_PointerControl.cs
        PointerControl
*/
using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ЗакриттяЗамовленняПостачальнику_PointerControl : PointerControl
    {
        event EventHandler<ЗакриттяЗамовленняПостачальнику_Pointer> PointerChanged;

        public ЗакриттяЗамовленняПостачальнику_PointerControl()
        {
            pointer = new ЗакриттяЗамовленняПостачальнику_Pointer();
            WidthPresentation = 300;
            Caption = $"{ЗакриттяЗамовленняПостачальнику_Const.FULLNAME}:";
            PointerChanged += async (object? _, ЗакриттяЗамовленняПостачальнику_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ЗакриттяЗамовленняПостачальнику_Pointer pointer;
        public ЗакриттяЗамовленняПостачальнику_Pointer Pointer
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            BeforeClickOpenFunc?.Invoke();
            ЗакриттяЗамовленняПостачальнику page = new ЗакриттяЗамовленняПостачальнику
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ЗакриттяЗамовленняПостачальнику_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ЗакриттяЗамовленняПостачальнику_Const.FULLNAME}", () => page);
            page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗакриттяЗамовленняПостачальнику_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
