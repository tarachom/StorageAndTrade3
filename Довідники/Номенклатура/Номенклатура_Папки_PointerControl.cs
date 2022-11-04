
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Номенклатура_Папки_PointerControl : PointerControl
    {
        public Номенклатура_Папки_PointerControl()
        {
            pointer = new Номенклатура_Папки_Pointer();
            WidthPresentation = 300;
            Caption = "Родич:";
        }

        public string UidOpenFolder { get; set; } = "";

        Номенклатура_Папки_Pointer pointer;
        public Номенклатура_Папки_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Номенклатура Папки", () =>
            {
                Номенклатура_Папки_Дерево page = new Номенклатура_Папки_Дерево(true);

                page.DirectoryPointerItem = Pointer;
                page.UidOpenFolder = UidOpenFolder;
                page.CallBack_OnSelectPointer = (Номенклатура_Папки_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadTree();

                return page;
            });
        }
    }
}