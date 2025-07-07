<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGeneratedCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="RegisterAccumulationList" />
            </xsl:when>
            <xsl:when test="$File = 'Report'">
                <xsl:call-template name="RegisterAccumulationReport" />
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

using InterfaceGtk3;
using AccountingSoftware;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриНакопичення.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>.РегістриНакопичення
{
    public class <xsl:value-of select="$RegisterAccumulationName"/> : РегістриНакопиченняЖурнал
    {
        public <xsl:value-of select="$RegisterAccumulationName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid<xsl:if test="$RegisterAccumulationType = 'Turnover'">, ["income"]</xsl:if>);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);
            await ТабличніСписки.<xsl:value-of select="$RegisterAccumulationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
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
            await BeforeLoadRecords();
        }

        #endregion
    }
}
    </xsl:template>

<!--- 
//
// ============================ Звіт ============================
//
-->

    <!-- Список -->
    <xsl:template name="RegisterAccumulationReport">
        <xsl:variable name="RegisterAccumulationName" select="RegisterAccumulation/Name"/>
        <xsl:variable name="FieldsTL" select="RegisterAccumulation/ElementFields/Field"/>

/*
        <xsl:value-of select="$RegisterAccumulationName"/>_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриНакопичення;

namespace <xsl:value-of select="$NameSpace"/>
{
    public static class <xsl:value-of select="$RegisterAccumulationName"/>_Звіт
    {
        public static async ValueTask Сформувати()
        {
            <xsl:variable name="CountFieldsTL" select="count($FieldsTL)"/>
            string query = $@"
SELECT
    <xsl:value-of select="$RegisterAccumulationName"/>.period,
    <xsl:value-of select="$RegisterAccumulationName"/>.income,
    <xsl:value-of select="$RegisterAccumulationName"/>.owner,
    <xsl:for-each select="$FieldsTL">
    <xsl:choose>
        <xsl:when test="Type = 'pointer'">
            <xsl:variable name="name" select="Name" />
            <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
            <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
            <xsl:variable name="index">
                <xsl:for-each select="$FieldsTL[Type = 'pointer' and $namePointer = substring-after(Pointer, '.')]">
                    <xsl:if test="$name = Name and position() &gt; 1"><xsl:value-of select="position()"/></xsl:if>
                </xsl:for-each>
            </xsl:variable>
            <xsl:value-of select="$RegisterAccumulationName"/>.{<xsl:value-of select="$RegisterAccumulationName"/>_Const.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>,
            <xsl:choose>
                <xsl:when test="PresetntationFields/@Count = 1">
                    <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/>.{<xsl:value-of select="$namePointer"/>_Const.<xsl:value-of select="PresetntationFields/Field"/><xsl:text>}</xsl:text>
                </xsl:when>
                <xsl:when test="PresetntationFields/@Count &gt; 1">
                    <xsl:text>concat_ws (', '</xsl:text>
                    <xsl:for-each select="PresetntationFields/Field">
                        <xsl:value-of select="concat(', ', $nameGroup, '_', $namePointer, $index, '.{', $namePointer, '_Const.', text(), '}')"/>
                    </xsl:for-each>
                    <xsl:text>)</xsl:text>
                </xsl:when>
                <xsl:otherwise>'#'</xsl:otherwise>
            </xsl:choose> AS <xsl:value-of select="concat(Name, '_Назва')"/>
        </xsl:when>
        <xsl:otherwise>
            <xsl:value-of select="$RegisterAccumulationName"/>.{<xsl:value-of select="$RegisterAccumulationName"/>_Const.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>
        </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="position() != $CountFieldsTL">,
    </xsl:if>
    </xsl:for-each>
FROM
    {<xsl:value-of select="$RegisterAccumulationName"/>_Const.TABLE} AS <xsl:value-of select="$RegisterAccumulationName"/>
    <xsl:for-each select="$FieldsTL[Type = 'pointer']">
        <xsl:variable name="name" select="Name" />
        <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
        <xsl:variable name="index">
            <xsl:for-each select="$FieldsTL[Type = 'pointer' and $namePointer = substring-after(Pointer, '.')]">
                <xsl:if test="$name = Name and position() &gt; 1"><xsl:value-of select="position()"/></xsl:if>
            </xsl:for-each>
        </xsl:variable>
    LEFT JOIN {<xsl:value-of select="$namePointer"/>_Const.TABLE} AS <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/> ON <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/>.uid = 
        <xsl:value-of select="$RegisterAccumulationName"/>.{<xsl:value-of select="$RegisterAccumulationName"/>_Const.<xsl:value-of select="Name"/>}
    </xsl:for-each>
";
            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "<xsl:value-of select="$RegisterAccumulationName"/>_Звіт",
                Caption = "<xsl:value-of select="$RegisterAccumulationName"/>",
                Query = query,
                GetInfo = () =&gt; ValueTask.FromResult("")
            };

            Звіт.ColumnSettings.Add("period", new("Період"));
            Звіт.ColumnSettings.Add("income", new("income"));
            Звіт.ColumnSettings.Add("owner", new("owner"));
            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>_Назва", new("<xsl:value-of select="Caption"/>", "<xsl:value-of select="Name"/>", <xsl:value-of select="$namePointer"/>_Const.POINTER));
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric'">
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>", new("<xsl:value-of select="Caption"/>", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>", new("<xsl:value-of select="Caption"/>"));
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:for-each>
            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
    </xsl:template>

</xsl:stylesheet>