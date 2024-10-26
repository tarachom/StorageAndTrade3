<?xml version="1.0" encoding="utf-8"?>
<!--
/*

Перша ступінь перетворення ХМЛ файлу вигружених даних в ХМЛ файл згрупованих SQL запитів

Результат:

  <sql tab="tab_a77">
    <row name="uid" type="Guid">1a13e463-e587-4184-b8e3-be2c28bd82bd</row>
    <row name="col_a2" type="DateTime">25.07.2022 17:16:53</row>
    <row name="col_a9" type="String">1fcdb976-2ca4-40ff-96ef-8d57eb424f0e</row>
    <row name="col_a1" type="String">АктВиконанихРобіт</row>
    <row name="col_a3" type="DateTime">25.07.2022 0:00:00</row>
    <row name="col_a8" type="String">Add</row>
    <row name="col_a5" type="Boolean">False</row>
    <row name="col_a6" type="Boolean">True</row>
    <row name="col_a7" type="String"></row>
  </sql>
  
*/
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/">

		<root>

		<xsl:for-each select="root/Constants">
			<sql tab="tab_constants">
				<row name="uid" type="Guid">00000000-0000-0000-0000-000000000000</row>
				<xsl:for-each select="Constant">
					<xsl:for-each select="row">
						<xsl:apply-templates select="node()" />
					</xsl:for-each>
				</xsl:for-each>
			</sql>
		</xsl:for-each>
		
		<xsl:for-each select="root/Constants/Constant">
		    <xsl:for-each select="TablePart">			    
			   <xsl:apply-templates select="row" />
		    </xsl:for-each>
		</xsl:for-each>

		<xsl:for-each select="root/Directories/Directory">
			<xsl:apply-templates select="row" />

		    <xsl:for-each select="TablePart">			    
			   <xsl:apply-templates select="row" />
		    </xsl:for-each>
		</xsl:for-each>
		
		<xsl:for-each select="root/Documents/Document">
			<xsl:apply-templates select="row" />

		    <xsl:for-each select="TablePart">			    
			   <xsl:apply-templates select="row" />
		    </xsl:for-each>
		</xsl:for-each>
			
		<xsl:for-each select="root/RegistersInformation/Register">
			<xsl:apply-templates select="row" />
		</xsl:for-each>
		
		<xsl:for-each select="root/RegistersAccumulation/Register">
			<xsl:apply-templates select="row" />

			<xsl:for-each select="TablePart">			    
			   <xsl:apply-templates select="row" />
		    </xsl:for-each>
		</xsl:for-each>
		
		</root>
		
    </xsl:template>

	<xsl:template match="row">
		<sql tab="{../@tab}">
			<xsl:apply-templates select="node()" />
		</sql>
	</xsl:template>
	
	<xsl:template match="node()">
		<xsl:if test="self::*">
			<row name="{name(.)}" type="{@type}">
				<xsl:choose>
					<xsl:when test="@type = 'String[]' or @type = 'Int32[]' or @type = 'Decimal[]' or @type = 'UuidAndText'">
						<xsl:copy-of select="node()"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="."/>
					</xsl:otherwise>
				</xsl:choose>
			</row>
		</xsl:if>
	</xsl:template>
	
</xsl:stylesheet>
