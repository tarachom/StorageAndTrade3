

/*     
        ЗакриттяЗамовленняКлієнта_PointerControl.cs
        PointerControl
*/
using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ЗакриттяЗамовленняКлієнта_PointerControl : PointerControl
    {
        event EventHandler<ЗакриттяЗамовленняКлієнта_Pointer> PointerChanged;

        public ЗакриттяЗамовленняКлієнта_PointerControl()
        {
            pointer = new ЗакриттяЗамовленняКлієнта_Pointer();
            WidthPresentation = 300;
            Caption = $"{ЗакриттяЗамовленняКлієнта_Const.FULLNAME}:";
            PointerChanged += async (object? _, ЗакриттяЗамовленняКлієнта_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ЗакриттяЗамовленняКлієнта_Pointer pointer;
        public ЗакриттяЗамовленняКлієнта_Pointer Pointer
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
            ЗакриттяЗамовленняКлієнта page = new ЗакриттяЗамовленняКлієнта
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ЗакриттяЗамовленняКлієнта_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ЗакриттяЗамовленняКлієнта_Const.FULLNAME}", () => page);
            page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗакриттяЗамовленняКлієнта_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
