
/*
    Контрагенти_Папки_Triggers.cs
    Тригери для довідника Контрагенти_Папки
*/

using GeneratedCode.Константи;
using AccountingSoftware;

namespace GeneratedCode.Довідники
{
    class Контрагенти_Папки_Triggers
    {
        public static async ValueTask New(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Контрагенти_Папки_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Контрагенти_Папки_Objest ДовідникОбєкт, Контрагенти_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Контрагенти_Папки_Objest ДовідникОбєкт, bool label)
        {
            //Якщо встановлюється мітка на видалення
            if (label)
            {
                //Елементи помічаються на видалення
                {
                    Контрагенти_Select select = new Контрагенти_Select();
                    select.QuerySelect.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                    select.QuerySelect.Where.Add(new Where(Контрагенти_Const.DELETION_LABEL, Comparison.NOT, true));
                    await select.Select();

                    while (select.MoveNext())
                        if (select.Current != null)
                        {
                            Контрагенти_Objest? Обєкт = await select.Current.GetDirectoryObject();
                            if (Обєкт != null)
                                await Обєкт.SetDeletionLabel();
                        }
                }

                //Вкладені папки помічаються на видалення
                {
                    Контрагенти_Папки_Select select = new Контрагенти_Папки_Select();
                    select.QuerySelect.Where.Add(new Where(Контрагенти_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                    await select.Select();

                    while (select.MoveNext())
                        if (select.Current != null)
                        {
                            Контрагенти_Папки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                            if (Обєкт != null)
                                await Обєкт.SetDeletionLabel();

                        }
                }
            }
        }

        public static async ValueTask BeforeDelete(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            //Елементи переносяться на верхній рівень
            {
                Контрагенти_Select select = new Контрагенти_Select();
                select.QuerySelect.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        Контрагенти_Objest? Обєкт = await select.Current.GetDirectoryObject();
                        if (Обєкт != null)
                        {
                            //Ставиться помітка на видалення для елементу
                            await Обєкт.SetDeletionLabel();

                            //Зміна папки
                            Обєкт.Папка = new Контрагенти_Папки_Pointer();
                            await Обєкт.Save();
                        }
                    }
            }

            //Вкладені папки видаляються. Для кожної папки буде викликана функція BeforeDelete
            {
                Контрагенти_Папки_Select select = new Контрагенти_Папки_Select();
                select.QuerySelect.Where.Add(new Where(Контрагенти_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        Контрагенти_Папки_Objest? контрагенти_Папки_Objest = await select.Current.GetDirectoryObject();
                        if (контрагенти_Папки_Objest != null)
                            await контрагенти_Папки_Objest.Delete();
                    }
            }
        }
    }
}
