<?xml version="1.0" encoding="utf-8"?>
<!--
/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGenerationCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="RegistersAccumulationList" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Список ============================
//
-->

    <!-- Список -->
    <xsl:template name="RegistersAccumulationList">
        <xsl:variable name="RegistersAccumulationName" select="RegistersAccumulation/Name"/>
        <xsl:variable name="TabularList" select="RegistersAccumulation/TabularList"/>

/*     
        <xsl:value-of select="$RegistersAccumulationName"/>.cs
        Список

        Табличний список - <xsl:value-of select="$TabularList"/>
*/

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGenerationCode"/>.РегістриНакопичення.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$RegistersAccumulationName"/> : РегістриНакопиченняЖурнал
    {
        public <xsl:value-of select="$RegistersAccumulationName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length &lt; 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.<xsl:value-of select="$RegistersAccumulationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }
        
        const string КлючНалаштуванняКористувача = "РегістриНакопичення.<xsl:value-of select="$RegistersAccumulationName"/>";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();           
        }

        #endregion
    }
}
    </xsl:template>

</xsl:stylesheet>