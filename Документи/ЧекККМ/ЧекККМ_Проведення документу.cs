

/*
        ЧекККМ_SpendTheDocument.cs
        Модуль проведення документу
*/

using AccountingSoftware;
using GeneratedCode;

using GeneratedCode.Довідники;
using GeneratedCode.РегістриНакопичення;
using GeneratedCode.РегістриВідомостей;

namespace GeneratedCode.Документи
{
    static class ЧекККМ_SpendTheDocument
    {
        public static async ValueTask<bool> Spend(ЧекККМ_Objest ДокументОбєкт)
        {
            try
            {
                // Проведення документу
                // ...

                return true;
            }
            catch (Exception ex)
            {
                await СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                return false;
            }
        }

        public static async ValueTask Clear(ЧекККМ_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
    