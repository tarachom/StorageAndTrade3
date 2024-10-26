
/*
        РозміщенняНоменклатуриПоКоміркам_PointerControl.cs
        PointerControl
*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РозміщенняНоменклатуриПоКоміркам_PointerControl : PointerControl
    {
        event EventHandler<РозміщенняНоменклатуриПоКоміркам_Pointer> PointerChanged;

        public РозміщенняНоменклатуриПоКоміркам_PointerControl()
        {
            pointer = new РозміщенняНоменклатуриПоКоміркам_Pointer();
            WidthPresentation = 300;
            Caption = $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}";
            PointerChanged += async (object? _, РозміщенняНоменклатуриПоКоміркам_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        РозміщенняНоменклатуриПоКоміркам_Pointer pointer;
        public РозміщенняНоменклатуриПоКоміркам_Pointer Pointer
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
            РозміщенняНоменклатуриПоКоміркам page = new РозміщенняНоменклатуриПоКоміркам
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new РозміщенняНоменклатуриПоКоміркам_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РозміщенняНоменклатуриПоКоміркам_Pointer();
        }
    }
}