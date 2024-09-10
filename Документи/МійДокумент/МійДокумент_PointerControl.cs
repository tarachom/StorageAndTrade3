

/*     
        МійДокумент_PointerControl.cs
        PointerControl
*/
using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class МійДокумент_PointerControl : PointerControl
    {
        event EventHandler<МійДокумент_Pointer> PointerChanged;

        public МійДокумент_PointerControl()
        {
            pointer = new МійДокумент_Pointer();
            WidthPresentation = 300;
            Caption = $"{МійДокумент_Const.FULLNAME}:";
            PointerChanged += async (object? _, МійДокумент_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        МійДокумент_Pointer pointer;
        public МійДокумент_Pointer Pointer
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
            МійДокумент page = new МійДокумент
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new МійДокумент_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {МійДокумент_Const.FULLNAME}", () => page);
            page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new МійДокумент_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
