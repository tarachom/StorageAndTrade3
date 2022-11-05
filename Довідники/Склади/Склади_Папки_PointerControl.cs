
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Склади_Папки_PointerControl : PointerControl
    {
        public Склади_Папки_PointerControl()
        {
            pointer = new Склади_Папки_Pointer();
            WidthPresentation = 300;
            Caption = "Родич:";
        }

        public string UidOpenFolder { get; set; } = "";

        Склади_Папки_Pointer pointer;
        public Склади_Папки_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Склади Папки", () =>
            {
                Склади_Папки_Дерево page = new Склади_Папки_Дерево(true);

                page.DirectoryPointerItem = Pointer;
                page.UidOpenFolder = UidOpenFolder;
                page.CallBack_OnSelectPointer = (Склади_Папки_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadTree();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Склади_Папки_Pointer();
        }
    }
}