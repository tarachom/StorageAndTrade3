
/*
    КорегуванняБоргу_Triggers.cs
    Тригери для документу КорегуванняБоргу
*/

using GeneratedCode.Константи;
using StorageAndTrade;

namespace GeneratedCode.Документи
{
    class КорегуванняБоргу_Triggers
    {
        public static async ValueTask New(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.КорегуванняБоргу_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(КорегуванняБоргу_Objest ДокументОбєкт, КорегуванняБоргу_Objest Основа)
        {
            ДокументОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{КорегуванняБоргу_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(КорегуванняБоргу_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
