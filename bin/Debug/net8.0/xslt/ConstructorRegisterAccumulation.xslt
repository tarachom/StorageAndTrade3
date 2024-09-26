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
                <xsl:call-template name="RegisterAccumulationList" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Список ============================
//
-->

    <!-- Список -->
    <xsl:template name="RegisterAccumulationList">
        <xsl:variable name="RegisterAccumulationName" select="RegisterAccumulation/Name"/>
        <xsl:variable name="TabularList" select="RegisterAccumulation/TabularList"/>

        <!-- Додатова інформація -->
        <xsl:variable name="RegisterAccumulationType" select="RegisterAccumulation/Type"/>

/*     
        <xsl:value-of select="$RegisterAccumulationName"/>.cs
        Список

        Табличний список - <xsl:value-of select="$TabularList"/>
*/

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGenerationCode"/>.РегістриНакопичення.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$RegisterAccumulationName"/> : РегістриНакопиченняЖурнал
    {
        public <xsl:value-of select="$RegisterAccumulationName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid<xsl:if test="$RegisterAccumulationType = 'Turnover'">, ["income"]</xsl:if>);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }
        
        const string КлючНалаштуванняКористувача = "РегістриНакопичення.<xsl:value-of select="$RegisterAccumulationName"/>";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();           
        }

        #endregion
    }
}
    </xsl:template>

</xsl:stylesheet>