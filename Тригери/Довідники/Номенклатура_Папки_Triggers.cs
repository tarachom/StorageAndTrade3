
/*
    Номенклатура_Папки_Triggers.cs
    Тригери для довідника Номенклатура_Папки
*/

using StorageAndTrade_1_0.Константи;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Довідники
{
    class Номенклатура_Папки_Triggers
    {
        public static async ValueTask New(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Номенклатура_Папки_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Номенклатура_Папки_Objest ДовідникОбєкт, Номенклатура_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Номенклатура_Папки_Objest ДовідникОбєкт, bool label)
        {
            //Якщо встановлюється мітка на видалення
            if (label)
            {
                //Елементи помічаються на видалення
                {
                    Номенклатура_Select select = new Номенклатура_Select();
                    select.QuerySelect.Where.Add(new Where(Номенклатура_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                    select.QuerySelect.Where.Add(new Where(Номенклатура_Const.DELETION_LABEL, Comparison.NOT, true));
                    await select.Select();

                    while (select.MoveNext())
                        if (select.Current != null)
                        {
                            Номенклатура_Objest? Обєкт = await select.Current.GetDirectoryObject();
                            if (Обєкт != null)
                                await Обєкт.SetDeletionLabel();
                        }
                }

                //Вкладені папки помічаються на видалення
                {
                    Номенклатура_Папки_Select select = new Номенклатура_Папки_Select();
                    select.QuerySelect.Where.Add(new Where(Номенклатура_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                    await select.Select();

                    while (select.MoveNext())
                        if (select.Current != null)
                        {
                            Номенклатура_Папки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                            if (Обєкт != null)
                                await Обєкт.SetDeletionLabel();

                        }
                }
            }
        }

        public static async ValueTask BeforeDelete(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            //Елементи переносяться на верхній рівень
            {
                Номенклатура_Select select = new Номенклатура_Select();
                select.QuerySelect.Where.Add(new Where(Номенклатура_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        Номенклатура_Objest? Обєкт = await select.Current.GetDirectoryObject();
                        if (Обєкт != null)
                        {
                            //Ставиться помітка на видалення для елементу
                            await Обєкт.SetDeletionLabel();

                            Обєкт.Папка = new Номенклатура_Папки_Pointer();
                            await Обєкт.Save();
                        }
                    }
            }

            //Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
            {
                Номенклатура_Папки_Select select = new Номенклатура_Папки_Select();
                select.QuerySelect.Where.Add(new Where(Номенклатура_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        Номенклатура_Папки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                        if (Обєкт != null)
                            await Обєкт.Delete();
                    }
            }
        }
    }
}
