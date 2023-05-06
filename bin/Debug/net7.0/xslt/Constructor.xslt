<?xml version="1.0" encoding="utf-8"?>
<!--
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
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="text" indent="yes" />

    <!-- Розділ -->
    <xsl:param name="Section" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$Section = 'Directory'">
                <xsl:call-template name="Directory" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

    <xsl:template name="Directory">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="Fields" select="Directory/Fields/Field"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>

/*     файл:     <xsl:value-of select="$DirectoryName"/>_Елемент.cs     */

using Gtk;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DirectoryName"/>_Елемент : ДовідникЕлемент
    {
        public <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest { get; set; } = new <xsl:value-of select="$DirectoryName"/>_Objest();
        
        #region Fields
        <xsl:for-each select="$Fields">
            <xsl:choose>
                <xsl:when test="Type = 'string'">
                    Entry <xsl:value-of select="Name"/> = new Entry() { WidthRequest = 500 };
                </xsl:when>
                <xsl:when test="Type = 'pointer'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="$namePointer"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="Name"/>", WidthPresentation = 300 };
                </xsl:when>
                <xsl:when test="Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    ComboBoxText <xsl:value-of select="$namePointer"/> = new ComboBoxText();
                </xsl:when>
            </xsl:choose>
        </xsl:for-each>
        #endregion

        #region TabularParts
        <xsl:for-each select="$TabularParts">
            <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/><xsl:text> </xsl:text>
                <xsl:value-of select="Name"/> = new <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/>();
        </xsl:for-each>
        #endregion

        public <xsl:value-of select="$DirectoryName"/>_Елемент() : base() 
        { 
            <xsl:for-each select="$Fields">
                <xsl:if test="Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="$namePointer"/>_List())
                        <xsl:value-of select="$namePointer"/>.Append(field.Value.ToString(), field.Name);
                </xsl:if>
            </xsl:for-each>
        }

        protected override void CreatePack1(VBox vBox)
        {
            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        CreateField(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        CreateField(vBox, null, <xsl:value-of select="$namePointer"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        CreateField(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="$namePointer"/>);
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        protected override void CreatePack2(VBox vBox)
        {
            <xsl:for-each select="$TabularParts">
                CreateTablePart(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
            </xsl:for-each>
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                <xsl:value-of select="$DirectoryName"/>_Objest.New();

            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:value-of select="Name"/>.Text = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$namePointer"/>.Pointer = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$namePointer"/>.ActiveId = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>.ToString();
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>

            <xsl:for-each select="$TabularParts">
                /* Таблична частина: <xsl:value-of select="Name"/> */
                <xsl:value-of select="Name"/>.<xsl:value-of select="$DirectoryName"/>_Objest = <xsl:value-of select="$DirectoryName"/>_Objest;
                <xsl:value-of select="Name"/>.LoadRecords();
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            UnigueID = <xsl:value-of select="$DirectoryName"/>_Objest.UnigueID;
            Caption = Назва.Text;

            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Text;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="$namePointer"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = Enum.Parse&lt;<xsl:value-of select="$namePointer"/>&gt;(<xsl:value-of select="$namePointer"/>.ActiveId);
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        #endregion

        protected override void Save()
        {
            try
            {
                <xsl:value-of select="$DirectoryName"/>_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            <xsl:for-each select="$TabularParts">
                <xsl:value-of select="Name"/>.SaveRecords();
            </xsl:for-each>
        }
    }
}
    </xsl:template>

</xsl:stylesheet>