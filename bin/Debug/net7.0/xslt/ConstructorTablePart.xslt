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

    <!-- Файл -->
    <xsl:param name="File" />

    <xsl:template match="root">

        <xsl:choose>

            <xsl:when test="$File = 'TablePart'">
                <xsl:call-template name="DirectoryElement" />
            </xsl:when>
            
        </xsl:choose>

    </xsl:template>

    <!-- Таблична Частина -->
    <xsl:template name="TablePart">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>
        <xsl:variable name="Fields" select="TablePart/Fields/Field"/>

/*
        <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/>.cs
        Таблична Частина
*/

    </xsl:template>

</xsl:stylesheet>