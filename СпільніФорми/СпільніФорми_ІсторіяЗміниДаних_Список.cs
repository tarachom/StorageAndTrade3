/*
 
Історія зміни даних - Список

*/

using AccountingSoftware;
using InterfaceGtk;
using GeneratedCode;

namespace StorageAndTrade
{
    class СпільніФорми_ІсторіяЗміниДаних_Список : InterfaceGtk.СпільніФорми_ІсторіяЗміниДаних_Список
    {
        public static async ValueTask Сформувати(UuidAndText obj)
        {
            СпільніФорми_ІсторіяЗміниДаних_Список page = new() { Obj = obj };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Історія зміни даних - Список", () => page);
            await page.Load();
        }

        public СпільніФорми_ІсторіяЗміниДаних_Список() : base(Config.Kernel) { }

        protected override async ValueTask OpenElement(Guid versionID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Елемент.Сформувати(versionID, Obj);
        }
    }
}