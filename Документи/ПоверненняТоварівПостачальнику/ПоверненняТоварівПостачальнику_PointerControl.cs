
/*

        ПоверненняТоварівПостачальнику_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПоверненняТоварівПостачальнику_PointerControl : PointerControl
    {
        event EventHandler<ПоверненняТоварівПостачальнику_Pointer> PointerChanged;

        public ПоверненняТоварівПостачальнику_PointerControl()
        {
            pointer = new ПоверненняТоварівПостачальнику_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПоверненняТоварівПостачальнику_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПоверненняТоварівПостачальнику_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПоверненняТоварівПостачальнику_Pointer pointer;
        public ПоверненняТоварівПостачальнику_Pointer Pointer
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
            ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ПоверненняТоварівПостачальнику_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПоверненняТоварівПостачальнику_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПоверненняТоварівПостачальнику_Pointer();
        }
    }
}