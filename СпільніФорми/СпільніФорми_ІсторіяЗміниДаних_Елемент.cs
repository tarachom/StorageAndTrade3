/*
 
Історія зміни даних - Елемент

*/

using AccountingSoftware;
using InterfaceGtk3;
using GeneratedCode;

namespace StorageAndTrade
{
    class СпільніФорми_ІсторіяЗміниДаних_Елемент : InterfaceGtk3.СпільніФорми_ІсторіяЗміниДаних_Елемент
    {
        public static async ValueTask Сформувати(Guid versionID, UuidAndText obj)
        {
            СпільніФорми_ІсторіяЗміниДаних_Елемент page = new();
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Історія зміни даних - Елемент", () => page);
            await page.Load(versionID, obj);
        }

        public СпільніФорми_ІсторіяЗміниДаних_Елемент() : base(Config.Kernel) { }

        protected override CompositePointerControl CreateCompositControl() =>
            new CompositePointerControl() { Caption = "", ClearSensetive = false, TypeSelectSensetive = false };

        protected override async ValueTask<CompositePointerPresentation_Record> CompositePointerPresentation(UuidAndText uuidAndText) =>
            await Functions.CompositePointerPresentation(uuidAndText);
    }
}