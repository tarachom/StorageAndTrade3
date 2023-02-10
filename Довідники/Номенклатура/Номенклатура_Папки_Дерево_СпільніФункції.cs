/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

using Gtk;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Номенклатура_Папки_Дерево_СпільніФункції
    {
        public static void AddColumns(TreeView TreeViewGrid)
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 0) { Visible = false });
            TreeViewGrid.AppendColumn(new TreeViewColumn("Папки", new CellRendererText(), "text", 1));
        }

        public static void FillTree(TreeView TreeViewGrid, TreeStore TreeStore, string UidOpenFolder, Номенклатура_Папки_Pointer Parent_Pointer)
        {
            TreeStore.Clear();

            TreeIter rootIter = TreeStore.AppendValues(Guid.Empty.ToString(), " Номенклатура ");

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Номенклатура_Папки_Const.Назва}, 
        {Номенклатура_Папки_Const.Родич}, 
        1 AS level 
    FROM {Номенклатура_Папки_Const.TABLE}
    WHERE {Номенклатура_Папки_Const.Родич} = '{Guid.Empty}'";

            if (!String.IsNullOrEmpty(UidOpenFolder))
            {
                query += $@"
AND uid != '{UidOpenFolder}'
";
            }

            query += $@"
    UNION ALL
    SELECT 
        {Номенклатура_Папки_Const.TABLE}.uid, 
        {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Назва}, 
        {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич}, 
        r.level + 1 AS level
    FROM {Номенклатура_Папки_Const.TABLE}
        JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid";

            if (!String.IsNullOrEmpty(UidOpenFolder))
            {
                query += $@"
WHERE {Номенклатура_Папки_Const.TABLE}.uid != '{UidOpenFolder}'
";
            }

            query += $@"
)
SELECT 
    uid, 
    {Номенклатура_Папки_Const.Назва}, 
    {Номенклатура_Папки_Const.Родич}, 
    level FROM r
ORDER BY level ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> NodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] o in listRow)
                {
                    string uid = o[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = o[1]?.ToString() ?? "";
                    string fieldParent = o[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)o[3];

                    if (level == 1)
                    {
                        TreeIter Iter = TreeStore.AppendValues(rootIter, uid, fieldName);
                        NodeDictionary.Add(uid, Iter);
                    }
                    else
                    {
                        TreeIter parentIter = NodeDictionary[fieldParent];

                        TreeIter Iter = TreeStore.AppendValues(parentIter, uid, fieldName);
                        NodeDictionary.Add(uid, Iter);
                    }
                }

            TreePath rootPath = TreeViewGrid.Model.GetPath(rootIter);
            TreeViewGrid.ExpandToPath(rootPath);

            if (Parent_Pointer.IsEmpty())
            {
                TreeViewGrid.SetCursor(rootPath, TreeViewGrid.Columns[0], false);
            }
            else
            {
                if (NodeDictionary.ContainsKey(Parent_Pointer.UnigueID.ToString()))
                {
                    TreeIter parentIter = NodeDictionary[Parent_Pointer.UnigueID.ToString()];
                    TreePath parentPath = TreeViewGrid.Model.GetPath(parentIter);
                    TreeViewGrid.ExpandToPath(parentPath);
                    TreeViewGrid.SetCursor(parentPath, TreeViewGrid.Columns[0], false);
                }
                else
                {
                    Parent_Pointer = new Номенклатура_Папки_Pointer();
                    TreeViewGrid.SetCursor(rootPath, TreeViewGrid.Columns[0], false);
                }
            }
        }
    }
}