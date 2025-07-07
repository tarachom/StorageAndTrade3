

/*     
        ЗакриттяРахункуФактури_PointerControl.cs
        PointerControl
*/
using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ЗакриттяРахункуФактури_PointerControl : PointerControl
    {
        event EventHandler<ЗакриттяРахункуФактури_Pointer> PointerChanged;

        public ЗакриттяРахункуФактури_PointerControl()
        {
            pointer = new ЗакриттяРахункуФактури_Pointer();
            WidthPresentation = 300;
            Caption = $"{ЗакриттяРахункуФактури_Const.FULLNAME}:";
            PointerChanged += async (object? _, ЗакриттяРахункуФактури_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ЗакриттяРахункуФактури_Pointer pointer;
        public ЗакриттяРахункуФактури_Pointer Pointer
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
            ЗакриттяРахункуФактури page = new ЗакриттяРахункуФактури
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ЗакриттяРахункуФактури_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ЗакриттяРахункуФактури_Const.FULLNAME}", () => page);
            page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗакриттяРахункуФактури_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
