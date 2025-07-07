

/*     
        ЧекККМ_PointerControl.cs
        PointerControl
*/
using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    public class ЧекККМ_PointerControl : PointerControl
    {
        event EventHandler<ЧекККМ_Pointer> PointerChanged;

        public ЧекККМ_PointerControl()
        {
            pointer = new ЧекККМ_Pointer();
            WidthPresentation = 300;
            Caption = $"{ЧекККМ_Const.FULLNAME}:";
            PointerChanged += async (_, pointer) => Presentation = pointer != null ? await pointer.GetPresentation() : "";
        }

        ЧекККМ_Pointer pointer;
        public ЧекККМ_Pointer Pointer
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
            ЧекККМ page = new ЧекККМ
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = selectPointer =>
                {
                    Pointer = new ЧекККМ_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ЧекККМ_Const.FULLNAME}", () => page);
            page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЧекККМ_Pointer();
            AfterSelectFunc?.Invoke();
            AfterClearFunc?.Invoke();
        }
    }
}
    