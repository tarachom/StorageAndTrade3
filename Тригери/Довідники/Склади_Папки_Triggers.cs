
/*
    Склади_Папки_Triggers.cs
    Тригери для довідника Склади_Папки
*/

using GeneratedCode.Константи;
using AccountingSoftware;

namespace GeneratedCode.Довідники
{
    class Склади_Папки_Triggers
    {
        public static async ValueTask New(Склади_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Склади_Папки_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Склади_Папки_Objest ДовідникОбєкт, Склади_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Склади_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Склади_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Склади_Папки_Objest ДовідникОбєкт, bool label)
        {
            //Якщо встановлюється мітка на видалення
            if (label)
            {
                //Елементи помічаються на видалення
                {
                    Склади_Select select = new Склади_Select();
                    select.QuerySelect.Where.Add(new Where(Склади_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                    select.QuerySelect.Where.Add(new Where(Склади_Const.DELETION_LABEL, Comparison.NOT, true));
                    await select.Select();

                    while (select.MoveNext())
                        if (select.Current != null)
                        {
                            Склади_Objest? Обєкт = await select.Current.GetDirectoryObject();
                            if (Обєкт != null)
                                await Обєкт.SetDeletionLabel();
                        }
                }

                //Вкладені папки помічаються на видалення
                {
                    Склади_Папки_Select select = new Склади_Папки_Select();
                    select.QuerySelect.Where.Add(new Where(Склади_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                    await select.Select();

                    while (select.MoveNext())
                        if (select.Current != null)
                        {
                            Склади_Папки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                            if (Обєкт != null)
                                await Обєкт.SetDeletionLabel();
                        }
                }
            }
        }

        public static async ValueTask BeforeDelete(Склади_Папки_Objest ДовідникОбєкт)
        {
            //Елементи переносяться на верхній рівень
            {
                Склади_Select select = new Склади_Select();
                select.QuerySelect.Where.Add(new Where(Склади_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        Склади_Objest? Обєкт = await select.Current.GetDirectoryObject();

                        if (Обєкт != null)
                        {
                            //Ставиться помітка на видалення для елементу
                            await Обєкт.SetDeletionLabel();

                            Обєкт.Папка = new Склади_Папки_Pointer();
                            await Обєкт.Save();
                        }
                    }
            }

            //Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
            {
                Склади_Папки_Select select = new Склади_Папки_Select();
                select.QuerySelect.Where.Add(new Where(Склади_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        Склади_Папки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                        if (Обєкт != null)
                            await Обєкт.Delete();
                    }
            }
        }
    }
}
