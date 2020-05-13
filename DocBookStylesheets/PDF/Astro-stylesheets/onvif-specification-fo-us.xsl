<?xml version="1.0" encoding="iso-8859-1"?>
<!DOCTYPE xsl:stylesheet
[
  <!ENTITY upper 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'>
  <!ENTITY lower 'abcdefghijklmnopqrstuvwxyz'>
  <!ENTITY logo  SYSTEM './ONVIF_Color_A_1.jpg' NDATA dummy>
  <!NOTATION dummy SYSTEM "">
]>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"
                xmlns:saxon="http://icl.com/saxon"
                xmlns:lxslt="http://xml.apache.org/xslt"
                xmlns:xalanredirect="org.apache.xalan.xslt.extensions.Redirect"
                xmlns:exsl="http://exslt.org/common"
                xmlns:d="http://docbook.org/ns/docbook"
                extension-element-prefixes="saxon xalanredirect lxslt exsl"
                version="1.0">
<!-- $Id: onvif-specification-fo-a4.xsl,v 1.28 2010/07/08 12:26:21 admin Exp $ -->

<!-- This stylesheet is a customization of the DocBook XSL Stylesheets -->
<!-- from http://www.onvif-open.org/spectools/stylesheets/onvif-docbook-fo.xsl
     to include additional formatting for programlisting, and parameter
     settings for international paper size (for an international standards
     organization) and body indentation (new with DocBook stylesheets);
     note also that directory locations have been parameterized; a number of
     changes were made to conform to revised ONVIF presentation reqts -->
<!-- See http://sourceforge.net/projects/docbook/ -->
<xsl:import href="../docbook/xsl/fo/docbook.xsl"/>
<xsl:import href="titlepage-fo.xsl"/>

<!-- ============================================================ -->
<!-- Parameters -->

<xsl:param name="section.autolabel" select="'1'"/>
<xsl:param name="section.label.includes.component.label" select="1"/>
<xsl:param name="title.margin.left" select="'0pt'"/>
  <xsl:param name="paper.type" select="'A4'"/>
<xsl:param name="body.start.indent" select="'0pt'"/>
<xsl:param name="linenumbering.extension" select="'1'"/>
<xsl:param name="body.font.family" select="'Arial,Helvetica,sans-serif'"/>
<xsl:param name="body.fontset" select="'Arial,Helvetica,sans-serif'"/>
<xsl:param name="title.fontset" select="'Arial,Helvetica,sans-serif'"/>
<xsl:param name="toc.section.depth">3</xsl:param>
<xsl:param name="footer.rule">0</xsl:param>
<xsl:param name="ulink.hyphenate">&#x200b;</xsl:param>
<xsl:param name="chapter.autolabel" select="1"></xsl:param>
<xsl:param name="shade.verbatim" select="'1'"/>
 <!-- Fonts-->
  <xsl:param name="body.font.master" select="'10.5'"/>
  <xsl:param name="line-height">1.53</xsl:param> <!--line spacing and spacing after-->
  <xsl:param name="dingbat.font.family">Arial</xsl:param><!-- Font for trademark-->
  <!-- Page fields -->
  <xsl:param name="page.margin.top">1cm</xsl:param>
  <xsl:param name="page.margin.bottom">5cm</xsl:param>
  <xsl:param name="page.margin.inner">0cm</xsl:param>
  <xsl:param name="page.margin.outer">0cm</xsl:param>
  <xsl:param name="body.margin.inner">2.5cm</xsl:param>
  <xsl:param name="body.margin.outer">2.5cm</xsl:param>
  <xsl:param name="body.margin.bottom">4cm</xsl:param>
  <xsl:param name="body.margin.top">2.5cm</xsl:param>
<!-- Page numbering -->
  <xsl:param name="double.sided" select="1" />
  <xsl:template name="initial.page.number">auto</xsl:template>
  <xsl:template name="page.number.format">1</xsl:template>
  <xsl:template name="force.page.count">no-force</xsl:template>
<!--  Lists-->
  <xsl:attribute-set name="itemizedlist.properties" use-attribute-sets="list.block.properties">
    <xsl:attribute name="font-weight">normal</xsl:attribute>
  </xsl:attribute-set>
  <xsl:attribute-set name="list.block.spacing">
    <xsl:attribute name="margin-left">0.63cm</xsl:attribute>
  </xsl:attribute-set>
  <xsl:param name="variablelist.as.blocks" select="0"></xsl:param>
  <xsl:attribute-set name="variablelist.term.properties">
    <xsl:attribute name="font-weight">bold</xsl:attribute>
    <xsl:attribute name="letter-spacing">0.4pt</xsl:attribute>
    <xsl:attribute name="font-size">10pt</xsl:attribute>
    <xsl:attribute name="line-height">1</xsl:attribute>
  </xsl:attribute-set>
  <xsl:attribute-set name="variablelist.item.properties">
    <xsl:attribute name="font-weight">normal</xsl:attribute>
    <xsl:attribute name="letter-spacing">0.4pt</xsl:attribute>
    <xsl:attribute name="font-size">10pt</xsl:attribute>
    <xsl:attribute name="line-height">1</xsl:attribute>
  </xsl:attribute-set>
  <xsl:template name="list.item.spacing"> <!--space between listitem-->
    <xsl:attribute name="space-before.optimum">1em</xsl:attribute>
    <xsl:attribute name="space-before.minimum">0.8em</xsl:attribute>
    <xsl:attribute name="space-before.maximum">1.2em</xsl:attribute>
  </xsl:template>
  <xsl:param name="orderedlist.label.width">1.7em</xsl:param> 
  <!-- footer properties-->
  <xsl:attribute-set name="region.after.properties">
    <xsl:attribute name="extent">4cm</xsl:attribute>
    <xsl:attribute name="precedence">true</xsl:attribute>
    <xsl:attribute name="display-align">center</xsl:attribute>
    <xsl:attribute name="background-image">../Astro-stylesheets/Onvif-footer.png</xsl:attribute>
    <xsl:attribute name="background-repeat">no-repeat</xsl:attribute>
    <xsl:attribute name="background-attachment">fixed</xsl:attribute>
    <xsl:attribute name="background-position-horizontal">center</xsl:attribute>
  </xsl:attribute-set>
  <!-- header properties-->
  <xsl:attribute-set name="region.before.properties">
    <xsl:attribute name="extent">3.35cm</xsl:attribute>
    <xsl:attribute name="precedence">true</xsl:attribute>
    <xsl:attribute name="display-align">center</xsl:attribute>
    <xsl:attribute name="background-image">../Astro-stylesheets/Onvif-header.png</xsl:attribute>
    <xsl:attribute name="background-repeat">no-repeat</xsl:attribute>
    <xsl:attribute name="background-attachment">fixed</xsl:attribute>
    <xsl:attribute name="background-position-horizontal">center</xsl:attribute>
  </xsl:attribute-set>
<!--  revision history-->
  <xsl:attribute-set name="revhistory.title.properties">
    <xsl:attribute name="font-size">10pt</xsl:attribute>
    <xsl:attribute name="font-weight">bold</xsl:attribute>
    <xsl:attribute name="letter-spacing">0.4pt</xsl:attribute>
  </xsl:attribute-set>
  <xsl:attribute-set name="revhistory.table.cell.properties">
    <xsl:attribute name="border">0.5pt solid black</xsl:attribute>
    <xsl:attribute name="font-size">10pt</xsl:attribute>
    <xsl:attribute name="letter-spacing">0.4pt</xsl:attribute>
    <xsl:attribute name="padding">4pt</xsl:attribute>
    <xsl:attribute name="line-height">1</xsl:attribute>
  </xsl:attribute-set>
  <xsl:template name="revhistory.list.item.spacing"> <!--space between listitem-->
    <xsl:attribute name="space-before.optimum">0em</xsl:attribute>
    <xsl:attribute name="space-before.minimum">0em</xsl:attribute>
    <xsl:attribute name="space-before.maximum">0em</xsl:attribute>
  </xsl:template>
<!--  Tables-->
  <xsl:attribute-set name="table.properties" use-attribute-sets="formal.object.properties">
    <xsl:attribute name="keep-together.within-column">auto</xsl:attribute>
    <xsl:attribute name="font-size">10pt</xsl:attribute>
  </xsl:attribute-set>
  <xsl:attribute-set name="table.cell.padding">
    <xsl:attribute name="padding-start">4pt</xsl:attribute>
    <xsl:attribute name="padding-end">4pt</xsl:attribute>
    <xsl:attribute name="padding-top">2pt</xsl:attribute>
    <xsl:attribute name="padding-bottom">2pt</xsl:attribute>
  </xsl:attribute-set>
  <!-- =========================================================== -->
<xsl:attribute-set name="shade.verbatim.style">
  <xsl:attribute name="background-color">#E7DEEF</xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="xref.properties">
  <xsl:attribute name="color">blue</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="informal.object.properties">
  <xsl:attribute name="border-before-style">solid</xsl:attribute>
  <xsl:attribute name="border-before-width">1pt</xsl:attribute>
  <xsl:attribute name="border-after-style">solid</xsl:attribute>
  <xsl:attribute name="border-after-width">1pt</xsl:attribute>
</xsl:attribute-set>



<xsl:param name="onvif.logo">
  <xsl:for-each select="document('')">
    <xsl:value-of select="unparsed-entity-uri('logo')"/>
  </xsl:for-each>
</xsl:param>

<xsl:param name="method" select="'xml'"/>
<xsl:param name="indent" select="'no'"/>
<xsl:param name="encoding" select="'utf-8'"/>

<xsl:param name="automatic-output-filename" select="'no'"/>
  
  <!--ToC-->
  <xsl:param name="generate.component.toc" select="'1'"/>
  <xsl:param name="generate.toc">
    appendix  nop
    article   toc,title
    book      toc,title
    chapter   toc,title
    part      nop
    preface   nop
    qandadiv  nop
    qandaset  nop
    reference toc,title
    section   toc
    set       toc
  </xsl:param>
 <!-- Symbols after number and befor text in TOC-->
  <xsl:param name="autotoc.label.separator"><xsl:text xml:space="preserve">&#160;&#160;&#160;&#160;&#160;</xsl:text></xsl:param>
  <!-- Properties for body of ToC -->
  <xsl:attribute-set name="toc.line.properties">
    <xsl:attribute name="font-family">Arial</xsl:attribute>
    <xsl:attribute name="font-size">10.5pt</xsl:attribute>
    <xsl:attribute name="font-weight">
      <xsl:choose>
        <xsl:when test="self::d:chapter | self::d:preface | self::d:appendix">bold</xsl:when>
      <xsl:otherwise>normal</xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
    <xsl:attribute name="line-height">2</xsl:attribute>
  </xsl:attribute-set>
  
  <xsl:attribute-set name="index.div.title.properties">
    <xsl:attribute name="margin-{$direction.align.start}">0pt</xsl:attribute>
    <xsl:attribute name="color">blue</xsl:attribute>
    <xsl:attribute name="font-size">30pt</xsl:attribute>
    <xsl:attribute name="font-family"><xsl:value-of select="$title.fontset"></xsl:value-of></xsl:attribute>
    <xsl:attribute name="font-weight">bold</xsl:attribute>
    <xsl:attribute name="keep-with-next.within-column">always</xsl:attribute>
    <xsl:attribute name="space-before.optimum"><xsl:value-of select="concat($body.font.master,'pt')"></xsl:value-of></xsl:attribute>
    <xsl:attribute name="space-before.minimum"><xsl:value-of select="concat($body.font.master,'pt * 0.8')"></xsl:value-of></xsl:attribute>
    <xsl:attribute name="space-before.maximum"><xsl:value-of select="concat($body.font.master,'pt * 1.2')"></xsl:value-of></xsl:attribute>
    <xsl:attribute name="start-indent">0pt</xsl:attribute>
  </xsl:attribute-set>
<!-- ======================================================== -->
  

  
  
<xsl:attribute-set name="section.title.properties">
  <xsl:attribute name="text-align">start</xsl:attribute>
  <xsl:attribute name="font-size">16pt</xsl:attribute>  
  <xsl:attribute name="font-weight">normal</xsl:attribute> 
</xsl:attribute-set>

  <xsl:attribute-set name="section.title.level1.properties">
    <xsl:attribute name="font-size">16pt</xsl:attribute>  
    <xsl:attribute name="font-weight">normal</xsl:attribute>  
  </xsl:attribute-set>
  <xsl:attribute-set name="section.title.level2.properties">
    <xsl:attribute name="font-size">16pt</xsl:attribute>
    <xsl:attribute name="font-weight">normal</xsl:attribute>
  </xsl:attribute-set>

<xsl:attribute-set name="component.title.properties">
  <xsl:attribute name="text-align">start</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="root.properties">
  <xsl:attribute name="id">entire_publication</xsl:attribute>
</xsl:attribute-set>

<xsl:attribute-set name="table.of.contents.titlepage.recto.style">
</xsl:attribute-set>
<xsl:attribute-set name="article.appendix.title.properties">
  <xsl:attribute name="border-before-width">1pt</xsl:attribute>
  <xsl:attribute name="border-before-style">solid</xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="section.level1.properties">
  <xsl:attribute name="space-before.minimum">12pt</xsl:attribute>
  <xsl:attribute name="space-before.optimum">12pt</xsl:attribute>
</xsl:attribute-set>
<xsl:attribute-set name="appendix.title.augmented.properties">
  <xsl:attribute name="space-before.minimum">12pt</xsl:attribute>
  <xsl:attribute name="space-before.maximum">12pt</xsl:attribute>
  <xsl:attribute name="space-before.optimum">12pt</xsl:attribute>
</xsl:attribute-set>

<xsl:template match="article/appendix">
  <fo:block break-after="page"/><!--this appears to get around an XEP bug-->
  <xsl:apply-imports/>
</xsl:template>

<xsl:template match="article/appendix" mode="object.title.markup">
  <fo:block xsl:use-attribute-sets="appendix.title.augmented.properties">
    <xsl:apply-imports/>
  </fo:block>
</xsl:template>
<!-- ============================================================ -->
<!-- Page Masters -->
  <!-- title pages -->
  <xsl:template name="user.pagemasters">
    <fo:simple-page-master master-name="titlepage-first-onvif" 
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.inner"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.outer"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">titlepage-first-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="9.45cm"
        column-gap="{$column.gap.titlepage}"
        column-count="{$column.count.titlepage}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-first"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-first"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">first</xsl:with-param>
        <xsl:with-param name="pageclass">titlepage</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">first</xsl:with-param>
        <xsl:with-param name="pageclass">titlepage</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <fo:simple-page-master master-name="titlepage-odd-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.inner"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.outer"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">titlepage-odd-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.titlepage}"
        column-count="{$column.count.titlepage}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-odd"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-odd"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">odd</xsl:with-param>
        <xsl:with-param name="pageclass">titlepage</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">odd</xsl:with-param>
        <xsl:with-param name="pageclass">titlepage</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <fo:simple-page-master master-name="titlepage-even-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.outer"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.inner"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">titlepage-even-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.titlepage}"
        column-count="{$column.count.titlepage}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-even"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-even"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">even</xsl:with-param>
        <xsl:with-param name="pageclass">titlepage</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">even</xsl:with-param>
        <xsl:with-param name="pageclass">titlepage</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <!-- list-of-title pages -->
    <fo:simple-page-master master-name="lot-first-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.inner"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.outer"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">lot-first-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.lot}"
        column-count="{$column.count.lot}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-first"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-first"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">first</xsl:with-param>
        <xsl:with-param name="pageclass">lot</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">first</xsl:with-param>
        <xsl:with-param name="pageclass">lot</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <fo:simple-page-master master-name="lot-odd-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.inner"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.outer"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">lot-odd-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.lot}"
        column-count="{$column.count.lot}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-odd"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-odd"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">odd</xsl:with-param>
        <xsl:with-param name="pageclass">lot</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">odd</xsl:with-param>
        <xsl:with-param name="pageclass">lot</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <fo:simple-page-master master-name="lot-even-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.outer"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.inner"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">lot-even-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.lot}"
        column-count="{$column.count.lot}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-even"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-even"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">even</xsl:with-param>
        <xsl:with-param name="pageclass">lot</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">even</xsl:with-param>
        <xsl:with-param name="pageclass">lot</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    
    <!-- body pages -->
    <fo:simple-page-master master-name="body-first-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">

      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.inner"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.outer"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">body-first-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.body}"
        column-count="{$column.count.body}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-first"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-first"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">first</xsl:with-param>
        <xsl:with-param name="pageclass">body</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">first</xsl:with-param>
        <xsl:with-param name="pageclass">body</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <fo:simple-page-master master-name="body-odd-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.inner"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.outer"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">body-odd-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.body}"
        column-count="{$column.count.body}"
        margin-left="2.5cm"
        margin-right="2.5cm">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-odd"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-odd"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="pageclass">body</xsl:with-param>
        <xsl:with-param name="sequence">odd</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="pageclass">body</xsl:with-param>
        <xsl:with-param name="sequence">odd</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <fo:simple-page-master master-name="body-even-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.outer"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.inner"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">body-even-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.body}"
        column-count="{$column.count.body}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-even"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-even"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="pageclass">body</xsl:with-param>
        <xsl:with-param name="sequence">even</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="pageclass">body</xsl:with-param>
        <xsl:with-param name="sequence">even</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <!-- backmatter pages -->
    <fo:simple-page-master master-name="back-first-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.inner"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.outer"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">back-first-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.back}"
        column-count="{$column.count.back}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-first"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-first"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">first</xsl:with-param>
        <xsl:with-param name="pageclass">back</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">first</xsl:with-param>
        <xsl:with-param name="pageclass">back</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <fo:simple-page-master master-name="back-odd-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.inner"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.outer"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">back-odd-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.back}"
        column-count="{$column.count.back}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-odd"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-odd"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">odd</xsl:with-param>
        <xsl:with-param name="pageclass">back</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">odd</xsl:with-param>
        <xsl:with-param name="pageclass">back</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    
    <fo:simple-page-master master-name="back-even-onvif"
      page-width="{$page.width}"
      page-height="{$page.height}">
      <xsl:attribute name="margin-{$direction.align.start}">
        <xsl:value-of select="$page.margin.outer"/>
        <xsl:if test="$fop.extensions != 0">
          <xsl:value-of select="concat(' - (',$title.margin.left,')')"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:attribute name="margin-{$direction.align.end}">
        <xsl:value-of select="$page.margin.inner"/>
      </xsl:attribute>
      <xsl:if test="$axf.extensions != 0">
        <xsl:call-template name="axf-page-master-properties">
          <xsl:with-param name="page.master">back-even-onvif</xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <fo:region-body margin-bottom="{$body.margin.bottom}"
        margin-top="{$body.margin.top}"
        column-gap="{$column.gap.back}"
        column-count="{$column.count.back}">
        <xsl:attribute name="margin-{$direction.align.start}">
          <xsl:value-of select="$body.margin.outer"/>
        </xsl:attribute>
        <xsl:attribute name="margin-{$direction.align.end}">
          <xsl:value-of select="$body.margin.inner"/>
        </xsl:attribute>
      </fo:region-body>
      <fo:region-before region-name="xsl-region-before-even"
        xsl:use-attribute-sets="region.before.properties"/>
      <fo:region-after region-name="xsl-region-after-even"
        xsl:use-attribute-sets="region.after.properties"/>
      <xsl:call-template name="region.outer">
        <xsl:with-param name="sequence">even</xsl:with-param>
        <xsl:with-param name="pageclass">back</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="region.inner">
        <xsl:with-param name="sequence">even</xsl:with-param>
        <xsl:with-param name="pageclass">back</xsl:with-param>
      </xsl:call-template>
    </fo:simple-page-master>
    <!-- setup for title page(s) -->
    <fo:page-sequence-master master-name="titlepage-onvif">
      <fo:repeatable-page-master-alternatives>
        <fo:conditional-page-master-reference master-reference="blank"
          blank-or-not-blank="blank"/>
        <xsl:if test="$force.blank.pages != 0">
          <fo:conditional-page-master-reference master-reference="titlepage-first-onvif"
            page-position="first"/>
        </xsl:if>
        <fo:conditional-page-master-reference master-reference="titlepage-odd-onvif"
          odd-or-even="odd"/>
        <fo:conditional-page-master-reference 
          odd-or-even="even">
          <xsl:attribute name="master-reference">
            <xsl:choose>
              <xsl:when test="$double.sided != 0">titlepage-even-onvif</xsl:when>
              <xsl:otherwise>titlepage-odd-onvif</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </fo:conditional-page-master-reference>
      </fo:repeatable-page-master-alternatives>
    </fo:page-sequence-master>
    <!-- setup for lots -->
    <fo:page-sequence-master master-name="lot-custom">
      <fo:repeatable-page-master-alternatives>
        <fo:conditional-page-master-reference master-reference="blank"
          blank-or-not-blank="blank"/>
        <xsl:if test="$force.blank.pages != 0">
          <fo:conditional-page-master-reference master-reference="lot-first-onvif"
            page-position="first"/>
        </xsl:if>
        <fo:conditional-page-master-reference master-reference="lot-odd-onvif"
          odd-or-even="odd"/>
        <fo:conditional-page-master-reference 
          odd-or-even="even">
          <xsl:attribute name="master-reference">
            <xsl:choose>
              <xsl:when test="$double.sided != 0">lot-even-onvif</xsl:when>
              <xsl:otherwise>lot-odd-onvif</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </fo:conditional-page-master-reference>
      </fo:repeatable-page-master-alternatives>
    </fo:page-sequence-master>
    <!-- body -->
    <fo:page-sequence-master master-name="body-custom">
      <fo:repeatable-page-master-alternatives>
        <fo:conditional-page-master-reference master-reference="blank"
          blank-or-not-blank="blank"/>
        <fo:conditional-page-master-reference master-reference="body-first-onvif"
          page-position="first"
          odd-or-even="odd"/>
        <fo:conditional-page-master-reference master-reference="body-even-onvif"
          page-position="first"
          odd-or-even="even"/>
        <fo:conditional-page-master-reference master-reference="body-odd-onvif"
          odd-or-even="odd"/>
        <fo:conditional-page-master-reference odd-or-even="even">
          <xsl:attribute name="master-reference">
            <xsl:choose>
              <xsl:when test="$double.sided != 0">body-even-onvif</xsl:when>
              <xsl:otherwise>body-odd-onvif</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </fo:conditional-page-master-reference>
      </fo:repeatable-page-master-alternatives>
    </fo:page-sequence-master>
    <!-- setup back matter -->
    <fo:page-sequence-master master-name="back-custom">
      <fo:repeatable-page-master-alternatives>
        <fo:conditional-page-master-reference master-reference="blank"
          blank-or-not-blank="blank"/>
        <xsl:if test="$force.blank.pages != 0">
          <fo:conditional-page-master-reference master-reference="back-first-onvif"
            page-position="first"/>
        </xsl:if>
        <fo:conditional-page-master-reference master-reference="back-odd-onvif"
          odd-or-even="odd"/>
        <fo:conditional-page-master-reference 
          odd-or-even="even">
          <xsl:attribute name="master-reference">
            <xsl:choose>
              <xsl:when test="$double.sided != 0">back-even-onvif</xsl:when>
              <xsl:otherwise>back-odd-onvif</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </fo:conditional-page-master-reference>
      </fo:repeatable-page-master-alternatives>
    </fo:page-sequence-master>
    
  </xsl:template>

  
  <xsl:template name="select.user.pagemaster">
    <xsl:param name="element"/>
    <xsl:param name="pageclass"/>
    <xsl:param name="default-pagemaster"/>   
    <xsl:choose>
      <xsl:when test="$default-pagemaster = 'titlepage'"> 
        <xsl:value-of select="'titlepage-onvif'" />
      </xsl:when>
      <xsl:when test="$default-pagemaster = 'body'">
        <xsl:value-of select="'body-custom'" />
      </xsl:when>
      <xsl:when test="$default-pagemaster = 'lot'">
        <xsl:value-of select="'lot-custom'" />
      </xsl:when>
      <xsl:when test="$default-pagemaster = 'back'">
        <xsl:value-of select="'back-custom'" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$default-pagemaster"/>
      </xsl:otherwise> 
    </xsl:choose>
  </xsl:template>
 
<!-- ============================================================ -->
<!-- The document -->
<xsl:template match="/">
  <xsl:variable name="content">
    <xsl:apply-imports/>
  </xsl:variable>

  <xsl:variable name="filename">
    <xsl:value-of select="/article/info/productname[1]"/>
    <xsl:if test="/article/info/productnumber">
      <xsl:text>-</xsl:text>
      <xsl:value-of select="/article/info/productnumber[1]"/>
    </xsl:if>
  </xsl:variable>

  <xsl:choose>
    <xsl:when test="$automatic-output-filename!='yes' or
                    not(normalize-space($filename))">
      <xsl:copy-of select="$content"/>      
    </xsl:when>
    <xsl:when test="element-available('exsl:document')">
      <xsl:message>Writing <xsl:value-of select="$filename"/>.fo</xsl:message>
      <exsl:document href="{$filename}.fo"
                     method="{$method}"
                     encoding="{$encoding}"
                     indent="{$indent}">
        <xsl:copy-of select="$content"/>
      </exsl:document>
    </xsl:when>
    <xsl:when test="element-available('saxon:output')">
      <xsl:message>Writing <xsl:value-of select="$filename"/>.fo</xsl:message>
      <saxon:output href="{$filename}.fo"
                    method="{$method}"
                    encoding="{$encoding}"
                    indent="{$indent}">
        <xsl:copy-of select="$content"/>
      </saxon:output>
    </xsl:when>
    <xsl:when test="element-available('xalanredirect:write')">
      <!-- Xalan uses xalanredirect -->
      <xsl:message>Writing <xsl:value-of select="$filename"/>.fo</xsl:message>
      <xalanredirect:write file="{$filename}.fo">
        <xsl:copy-of select="$content"/>
      </xalanredirect:write>
    </xsl:when>
    <xsl:otherwise>
      <xsl:copy-of select="$content"/>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>

<!-- ============================================================ -->
<!-- Titlepage -->

<xsl:attribute-set name="onvif-metadata-title"
                   use-attribute-sets="component.title.properties">
  <xsl:attribute name="space-before">0pt</xsl:attribute>
  <xsl:attribute name="font-family">
    <xsl:value-of select="$title.font.family"/>
  </xsl:attribute>
  <xsl:attribute name="font-weight">bold</xsl:attribute>
  <xsl:attribute name="font-size">9pt</xsl:attribute>
</xsl:attribute-set>

<xsl:template match="pubdate" mode="titlepage.mode">
  <fo:block keep-with-next="always"
            font-size="18pt"
            space-before="10pt"
            space-after="8pt"
            font-weight="bold"
            font-family="{$title.font.family}"
            xsl:use-attribute-sets="component.title.properties">
    <xsl:choose>
      <xsl:when test="/*/@status">
        <xsl:value-of select="/*/@status"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>???Unknown Status???</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </fo:block>
  <fo:block keep-with-next="always"
            font-size="18pt"
            space-before="10pt"
            space-after="8pt"
            font-weight="bold"
            font-family="{$title.font.family}"
            xsl:use-attribute-sets="component.title.properties">
    <xsl:apply-templates mode="titlepage.mode"/>
  </fo:block>
</xsl:template>

<xsl:template match="releaseinfo[@role='committee']" mode="titlepage.mode" priority="2">
    <fo:block>
      <fo:block xsl:use-attribute-sets="onvif-metadata-title">
        Technical Committee:
      </fo:block>
      <fo:block margin-left="2em">
        <xsl:apply-templates/>
      </fo:block>
    </fo:block>
</xsl:template>

<!--intercept these links because the URL must not be exposed-->
<xsl:template match="info//ulink">
    <fo:basic-link external-destination='url("{@url}")'
                   color="blue">
        <xsl:apply-templates/>
    </fo:basic-link>
</xsl:template>

<xsl:template match="releaseinfo[@role='onvif-id']" mode="titlepage.mode" priority="2">
    <fo:block>
      <fo:block xsl:use-attribute-sets="onvif-metadata-title">
        ONVIF Identifier:
      </fo:block>
      <fo:block margin-left="2em">
        <xsl:apply-templates/>
      </fo:block>
    </fo:block>
</xsl:template>

<xsl:template match="releaseinfo[@role='product']" mode="titlepage.mode" priority="2">
  <!-- suppress -->
</xsl:template>

<xsl:template match="releaseinfo[starts-with(@role,'ONVIF-specification-')]"
              mode="titlepage.mode" priority="2">
  <xsl:if test="not(preceding-sibling::releaseinfo
                                 [starts-with(@role,'ONVIF-specification-')])">
    <xsl:variable name="locations" 
                  select="../releaseinfo[starts-with(@role,
                                         'ONVIF-specification-')]"/>
    <fo:block>
      <fo:block xsl:use-attribute-sets="onvif-metadata-title">
        Specification URIs:
      </fo:block>
      <xsl:call-template name="spec-uri-group">
        <xsl:with-param name="header">This Version:</xsl:with-param>
        <xsl:with-param name="uris" 
           select="$locations[starts-with(@role,'ONVIF-specification-this')]"/>
      </xsl:call-template>
      <xsl:call-template name="spec-uri-group">
        <xsl:with-param name="header">Previous Version:</xsl:with-param>
        <xsl:with-param name="uris" 
       select="$locations[starts-with(@role,'ONVIF-specification-previous')]"/>
      </xsl:call-template>
      <xsl:call-template name="spec-uri-group">
        <xsl:with-param name="header">Latest Version:</xsl:with-param>
        <xsl:with-param name="uris" 
         select="$locations[starts-with(@role,'ONVIF-specification-latest')]"/>
      </xsl:call-template>
    </fo:block>
  </xsl:if>
</xsl:template>

<xsl:template name="spec-uri-group">
  <xsl:param name="header"/>
  <xsl:param name="uris"/>
  <fo:block>
    <fo:block xsl:use-attribute-sets="onvif-metadata-title">
      <xsl:copy-of select="$header"/>
    </fo:block>
  </fo:block>
  <fo:block margin-left="2em">
    <xsl:choose>
      <xsl:when test="not($uris)">
        N/A
      </xsl:when>
      <xsl:otherwise>
        <xsl:for-each select="$uris">
          <xsl:choose>
            <xsl:when test="contains(@role,'-draft')">
              <xsl:apply-templates/>
            </xsl:when>
            <xsl:otherwise>
              <fo:basic-link external-destination='url("{.}")'
                             color="blue">
                <xsl:value-of select="."/>
              </fo:basic-link>
              <xsl:if test="contains(@role,'-authoritative')">
                (Authoritative)
              </xsl:if>
            </xsl:otherwise>
          </xsl:choose>
          <fo:block/>
        </xsl:for-each>
      </xsl:otherwise>
    </xsl:choose>
  </fo:block>
</xsl:template>

<xsl:template match="authorgroup" mode="titlepage.mode">
  <xsl:variable name="editors" select="editor"/>
  <xsl:variable name="authors" select="author"/>
  <xsl:variable name="chair" select="othercredit"/>

  <xsl:if test="$chair">
    <fo:block>
      <fo:block xsl:use-attribute-sets="onvif-metadata-title">
        <xsl:text>Chair</xsl:text>
        <xsl:if test="count($chair) &gt; 1">s</xsl:if>
        <xsl:text>:</xsl:text>
      </fo:block>
      <fo:block margin-left="2em">
        <xsl:apply-templates select="$chair" mode="titlepage.mode"/>
      </fo:block>
    </fo:block>
  </xsl:if>

  <xsl:if test="$editors">
    <fo:block>
      <fo:block xsl:use-attribute-sets="onvif-metadata-title">
        <xsl:text>Editor</xsl:text>
        <xsl:if test="count($editors) &gt; 1">s</xsl:if>
        <xsl:text>:</xsl:text>
      </fo:block>
      <fo:block margin-left="2em">
        <xsl:apply-templates select="$editors" mode="titlepage.mode"/>
      </fo:block>
    </fo:block>
  </xsl:if>

  <xsl:if test="$authors">
    <fo:block>
      <fo:block xsl:use-attribute-sets="onvif-metadata-title">
        <xsl:text>Author</xsl:text>
        <xsl:if test="count($authors) &gt; 1">s</xsl:if>
        <xsl:text>:</xsl:text>
      </fo:block>
      <fo:block margin-left="2em">
        <xsl:apply-templates select="$authors" mode="titlepage.mode"/>
      </fo:block>
    </fo:block>
  </xsl:if>

</xsl:template>

<xsl:template match="ulink" mode="revision-links">
  <xsl:if test="position() = 1"> (</xsl:if>
  <xsl:if test="position() &gt; 1">, </xsl:if>
  <xsl:value-of select="@role"/>
  <xsl:if test="position() = last()">)</xsl:if>
</xsl:template>

<xsl:template match="editor|author|othercredit" mode="titlepage.mode">
  <xsl:call-template name="person.name"/>
  <xsl:if test="contrib">
    <xsl:text> (</xsl:text>
    <xsl:apply-templates select="contrib" mode="titlepage.mode"/>
    <xsl:text>)</xsl:text>
  </xsl:if>
  <xsl:if test="org/orgname">
    <xsl:text>, </xsl:text>
    <xsl:apply-templates select="org/orgname" mode="titlepage.mode"/>
  </xsl:if>
  <xsl:apply-templates select="org/address/email"
                       mode="titlepage.mode"/>
  <xsl:if test="position()&lt;last()"><fo:block/></xsl:if>
</xsl:template>

<xsl:template match="email" mode="titlepage.mode">
  <xsl:text>&#160;&lt;</xsl:text>
  <fo:basic-link external-destination='url("mailto:{.}")'
                 color="blue">
    <xsl:apply-templates/>
  </fo:basic-link>
  <xsl:text>></xsl:text>
</xsl:template>

<xsl:template match="abstract" mode="titlepage.mode">
  <fo:block>
    <fo:block xsl:use-attribute-sets="onvif-metadata-title">
      <xsl:apply-templates select="." mode="object.title.markup"/>
      <xsl:text>:</xsl:text>
    </fo:block>
    <fo:block margin-left="2em">
      <xsl:apply-templates mode="titlepage.mode"/>
    </fo:block>
  </fo:block>
</xsl:template>

<xsl:template match="info/abstract/para[1]">
  <fo:block>
    <xsl:apply-templates/>
  </fo:block>
</xsl:template>
  
  <xsl:template match="info/legalnotice/para[1]">
    <fo:block text-align="justify">
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

<xsl:template match="copyright" mode="titlepage.mode">
  <fo:block text-align="center">
    <xsl:apply-imports/>
  </fo:block>
</xsl:template>

<xsl:template match="releaseinfo" mode="titlepage.mode">
  <xsl:comment>
    <xsl:text> </xsl:text>
    <xsl:apply-templates/>
    <xsl:text> </xsl:text>
  </xsl:comment>
</xsl:template>

<xsl:template match="jobtitle|shortaffil|org|orgname|contrib"
              mode="titlepage.mode">
  <xsl:apply-templates/>
</xsl:template>
  
  <xsl:template match="jobtitle|shortaffil|org|orgname|contrib">
    <xsl:apply-templates/>
  </xsl:template>

<xsl:template match="phrase[@role='keyword']//text()">
  <xsl:value-of select="translate(.,'&lower;','&upper;')"/>
</xsl:template>

<!-- ============================================================ -->
<!-- Component TOC -->

<xsl:template name="component.toc">
  <xsl:param name="toc-context" select="."/>

  <xsl:variable name="id">
    <xsl:call-template name="object.id"/>
  </xsl:variable>

  <xsl:variable name="cid">
    <xsl:call-template name="object.id">
      <xsl:with-param name="object" select="$toc-context"/>
    </xsl:call-template>
  </xsl:variable>

  <xsl:variable name="nodes" select="section|sect1"/>
  <xsl:variable name="apps" select="bibliography|glossary|appendix"/>

  <xsl:if test="$nodes">
    <fo:block id="toc...{$id}"
              xsl:use-attribute-sets="toc.margin.properties">
      <xsl:call-template name="table.of.contents.titlepage"/>
      <xsl:apply-templates select="$nodes" mode="toc">
        <xsl:with-param name="toc-context" select="$toc-context"/>
      </xsl:apply-templates>
    </fo:block>
  </xsl:if>

  <xsl:if test="$apps">
    <fo:block id="toc...a{$id}"
              xsl:use-attribute-sets="toc.margin.properties">

      <fo:block space-after="1em"
                margin-left="{$title.margin.left}"
                font-size="12pt"
                font-weight="bold"
                font-family="{$title.font.family}">
        <fo:inline>Appendix</fo:inline>
        <xsl:if test="count($apps) &gt; 1">es</xsl:if>
      </fo:block>

      <xsl:apply-templates select="$apps" mode="toc">
        <xsl:with-param name="toc-context" select="$toc-context"/>
      </xsl:apply-templates>
    </fo:block>
  </xsl:if>
<!--  <fo:block break-after='page'/>-->
</xsl:template>

<xsl:template match="appendix" mode="object.title.template">
  <xsl:text>Appendix </xsl:text>
  <xsl:apply-imports/>
</xsl:template>

<!-- ================================================================= -->

<!-- support role='non-normative' -->
<xsl:template match="preface|chapter|appendix" mode="title.markup">
  <xsl:param name="allow-anchors" select="'0'"/>
  <xsl:variable name="title" select="(docinfo/title
                                      |prefaceinfo/title
                                      |chapterinfo/title
                                      |appendixinfo/title
                                      |title)[1]"/>
  <xsl:apply-templates select="$title" mode="title.markup">
    <xsl:with-param name="allow-anchors" select="$allow-anchors"/>
  </xsl:apply-templates>
  <xsl:if test="@role='non-normative'">
    <xsl:text> (Non-Normative)</xsl:text>
  </xsl:if>
</xsl:template>

<!-- support role='non-normative' -->
<xsl:template match="section
                     |sect1|sect2|sect3|sect4|sect5
                     |refsect1|refsect2|refsect3
                     |simplesect|task"
              mode="title.markup">
  <xsl:param name="allow-anchors" select="'1'"/>
  <xsl:variable name="title" select="(info/title
                                      |sectioninfo/title
                                      |sect1info/title
                                      |sect2info/title
                                      |sect3info/title
                                      |sect4info/title
                                      |sect5info/title
                                      |refsect1info/title
                                      |refsect2info/title
                                      |refsect3info/title
                                      |title)[1]"/>

  <xsl:apply-templates select="$title" mode="title.markup">
    <xsl:with-param name="allow-anchors" select="$allow-anchors"/>
  </xsl:apply-templates>
  <xsl:if test="@role='non-normative'">
    <xsl:text> (Non-Normative)</xsl:text>
  </xsl:if>
</xsl:template>

<!-- ============================================================ -->
<!-- Formatting changes for ONVIF look&amp;feel -->

<xsl:template match="quote">
  <xsl:variable name="depth">
    <xsl:call-template name="dot.count">
      <xsl:with-param name="string">
        <xsl:number level="multiple"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:variable>
  <xsl:choose>
    <xsl:when test="$depth mod 2 = 0">
      <xsl:text>"</xsl:text>
      <xsl:call-template name="inline.charseq"/>
      <xsl:text>"</xsl:text>
    </xsl:when>
    <xsl:otherwise>
      <xsl:text>'</xsl:text>
      <xsl:call-template name="inline.charseq"/>
      <xsl:text>'</xsl:text>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>

<xsl:template match="filename">
  <fo:inline font-weight="bold">
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>

<xsl:template match="programlisting">
  <fo:wrapper white-space-treatment="preserve">
    <xsl:apply-imports/>
  </fo:wrapper>
</xsl:template>

<xsl:template match="appendixx">
  <fo:block break-before="page"
            border-before-style="solid" border-before-width="1pt"/>
  <xsl:apply-imports/>
</xsl:template>

<!--remove these interim features
<xsl:param name="section-automatic-page-break" select="'no'"/>
<xsl:param name="appendix-automatic-page-break" select="'no'"/>
<xsl:template match="section//section | appendix//section">
  <xsl:apply-imports/>
</xsl:template>
<xsl:template match="section">
  <xsl:if test="$section-automatic-page-break='yes'">
    <fo:block break-before="page"/>
  </xsl:if>
  <xsl:apply-imports/>
</xsl:template>
<xsl:template match="appendix">
  <xsl:if test="$appendix-automatic-page-break='yes'">
    <fo:block break-before="page"/>
  </xsl:if>
  <xsl:apply-imports/>
</xsl:template>
-->

<xsl:template match="section[bibliography]/para">
  <!--suppress the paragraphs in the references, per ONVIF layout-->
  <xsl:if test="normalize-space(.)">
    <xsl:message>
      <xsl:text>Warning: non-empty bibliographic paragraphs are </xsl:text>
      <xsl:text>ignored in order to meet ONVIF layout requirements.</xsl:text>
    </xsl:message>
  </xsl:if>
</xsl:template>

<xsl:template match="bibliography">
  <xsl:apply-templates select="bibliomixed"/>
  <xsl:if test="*[not(self::bibliomixed)][normalize-space()]">
    <xsl:message>
    <xsl:text>Warning: non-empty non-bibliomixed children of </xsl:text>
    <xsl:text>bibliography elements are </xsl:text>
    <xsl:text>ignored in order to meet ONVIF layout requirements.</xsl:text>
    </xsl:message>
  </xsl:if>
</xsl:template>

<xsl:template match="table">
  <!--this needs to relax the "always" constraint used by DocBook-->
  <fo:block keep-together.within-column="1">
    <xsl:apply-imports/>
  </fo:block>
</xsl:template>

<xsl:template match="table[starts-with(@role,'font-size-')]//td/node() |
                     table[starts-with(@role,'font-size-')]//entry/node()">
  <fo:wrapper font-size="{substring-after(ancestor::table[@role][1]/@role,
                                          'font-size-')}">
    <xsl:apply-imports/>
  </fo:wrapper>
</xsl:template>

<!-- ============================================================ -->

<xsl:param name="header.rule" select="'0'"/>
<xsl:param name="region.before.extent" select="'0pt'"/>
<!--<xsl:param name="header.column.widths">20 0 1</xsl:param>-->
<xsl:template name="header.content">
  <xsl:param name="location" select="'header'"/>
  <xsl:param name="sequence" select="''"/>
  <xsl:param name="position" select="''"/>
  <xsl:param name="pageclass" select="''"/>
  <xsl:variable name="header.logo.width">
    <xsl:text>5.8cm</xsl:text>
  </xsl:variable>
  
  <!-- Prints the referenced Picture on the right of the header content -->
<!--  <xsl:choose>
    <xsl:when test="$position = 'right'">
      <fo:external-graphic width="auto"  height="1cm" 
        content-width="{$header.logo.width}" src="../Astro-stylesheets/Onvif-header.png"/>
    </xsl:when>
  </xsl:choose>-->
  <xsl:choose>
  <xsl:when test="$position = 'left'"> 
    <fo:block white-space="nowrap" line-height="1.5cm">
    <xsl:value-of select="ancestor-or-self::d:book/d:info/d:orgname"></xsl:value-of>
      <xsl:text> </xsl:text>
    <xsl:value-of select="ancestor-or-self::d:book/d:info/d:title"></xsl:value-of>
      <xsl:text> </xsl:text>
      <xsl:value-of select="ancestor-or-self::d:book/d:info/d:subtitle"></xsl:value-of>
      <xsl:text> </xsl:text>
      <xsl:call-template name="draft.text"/>
    </fo:block>
  </xsl:when>
  </xsl:choose>
</xsl:template>
  
  <xsl:attribute-set name="header.content.properties">
    <xsl:attribute name="font-family">Arial</xsl:attribute>
    <xsl:attribute name="font-size">9pt</xsl:attribute>
<!--    <xsl:attribute name="background-color">#00AEEF</xsl:attribute>-->
    <xsl:attribute name="margin-right">-0.8cm</xsl:attribute>
<!--    <xsl:attribute name="margin-top">-1.2cm</xsl:attribute>-->
    <xsl:attribute name="padding-left">1.25cm</xsl:attribute>
    <xsl:attribute name="padding-right">11.02cm</xsl:attribute>
<!--    <xsl:attribute name="padding-top">1.15cm</xsl:attribute>-->
  </xsl:attribute-set>
<xsl:template name="header.footer.width">
  <xsl:param name="location"/>
  <xsl:param name="position"/>
  <xsl:choose>
    <xsl:when test="$location='header'">
      <xsl:choose>
        <xsl:when test="$position=2">1</xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:when>
    <xsl:otherwise>
      <xsl:choose>
        <xsl:when test="$position=2">0</xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>
  
  
  <xsl:template name="footer.content">
    <xsl:param name="pageclass" select="''"/>
    <xsl:param name="sequence" select="''"/>
    <xsl:param name="position" select="''"/>
    <xsl:param name="gentext-key" select="''"/>
    <fo:block>
      <!-- pageclass can be front, body, back -->
      <!-- sequence can be odd, even, first, blank -->
      <!-- position can be left, center, right -->
      <xsl:choose>
        <!--titlepage-->
        <xsl:when test="$pageclass = 'titlepage-onvif' and $sequence = 'first' and ($position='left' or $position='right')">
        </xsl:when>
        
        <xsl:when test="$pageclass = 'titlepage-onvif' and $sequence = 'first' and $position='center'">
          <fo:block color="#00B0F0" text-align-last="center" text-align="center">
            <xsl:text>www.onvif.org</xsl:text>
          </fo:block>
        </xsl:when>
       <!-- lot-->
        <xsl:when test="$pageclass = 'lot-custom' and $sequence = 'first' and $position='left'">
          <fo:block  left="1cm" margin-left="2.5cm" >
            <fo:page-number color="#00B0F0" />
          </fo:block>
        </xsl:when>
        <xsl:when test="$pageclass = 'lot-custom' and $sequence = 'first' and $position='right'">
        </xsl:when>
        
        <xsl:when test="$position='center'">
          <fo:block color="#00B0F0" text-align-last="center" text-align="center">
            <xsl:text>www.onvif.org</xsl:text>
          </fo:block>
        </xsl:when>
        
        <xsl:when test="$double.sided != 0 and $sequence = 'even'
          and $position='left'">
          <fo:block  left="1cm" margin-left="2.5cm" >
          <fo:page-number color="#00B0F0" />
          </fo:block>
        </xsl:when>
        
        <xsl:when test="$double.sided != 0 and ($sequence = 'odd' or $sequence = 'first')
          and $position='right' and $pageclass != 'titlepage'">
          <fo:block  left="1cm" margin-right="2.5cm" >
            <fo:page-number color="#00B0F0" />
          </fo:block>
        </xsl:when>
        
        <xsl:when test="$double.sided = 0 and $position='center'">
          <fo:page-number color="#00B0F0"/>
        </xsl:when>
        
        <xsl:when test="$sequence='blank'">
          <xsl:choose>
            <xsl:when test="$double.sided != 0 and $position = 'left'">
              <fo:block  left="1cm" margin-left="2.5cm" >
                <fo:page-number color="#00B0F0" />
              </fo:block>
            </xsl:when>
            <xsl:when test="$double.sided = 0 and $position = 'center'">
              <fo:page-number color="#00B0F0"/>
            </xsl:when>
            <xsl:otherwise>
              <!-- nop -->
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <!-- nop -->
        </xsl:otherwise>
      </xsl:choose>
    </fo:block>
  </xsl:template>
<xsl:attribute-set name="footer.content.properties">
  <xsl:attribute name="font-size">80%</xsl:attribute>
<!--  <xsl:attribute name="margin-left">1cm</xsl:attribute>
  <xsl:attribute name="margin-right">1cm</xsl:attribute>-->
  <xsl:attribute name="margin-bottom">0.5cm</xsl:attribute>
</xsl:attribute-set>

<!-- ============================================================ -->

<xsl:template match="para/revhistory">
  <fo:table table-layout="fixed" border="solid 1pt">
    <fo:table-column column-number="1" column-width="33%"/>
    <fo:table-column column-number="2" column-width="33%"/>
    <fo:table-column column-number="3" column-width="33%"/>
    <fo:table-body>
      <xsl:apply-templates mode="titlepage.mode"/>
    </fo:table-body>
  </fo:table>
</xsl:template>

<!-- ============================================================ -->

<xsl:template match="processing-instruction('lb')">
  <fo:block/>
</xsl:template>

<!-- ============================================================ -->

<xsl:template match="processing-instruction('pb')">
  <fo:block break-after="page"/>
</xsl:template>

<!-- ============================================================ -->
<!--  Copyright-->
  <xsl:template match="d:copyright" mode="titlepage.mode">
    <xsl:call-template name="dingbat">
      <xsl:with-param name="dingbat">copyright</xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="gentext.space"/>
    <xsl:call-template name="copyright.years">
      <xsl:with-param name="years" select="d:year"/>
      <xsl:with-param name="print.ranges" select="$make.year.ranges"/>
      <xsl:with-param name="single.year.ranges"
        select="$make.single.year.ranges"/>
    </xsl:call-template>
    <xsl:call-template name="gentext.space"/>
    <xsl:apply-templates select="d:holder" mode="titlepage.mode"/>
  </xsl:template>
  <!-- ============================================================ -->
<!--Gentext-->
<xsl:param name="local.l10n.xml" select="document('')"/> 
<l:i18n xmlns:l="http://docbook.sourceforge.net/xmlns/l10n/1.0"> 
  <l:l10n language="en"> 

    <l:context name="title-numbered"> 
      <!--  Label punctuation in doc, not in ToC-->
      <l:template name="section" text="%n  %t"/> 
      <l:template name="chapter" text="%n  %t"/>
      <l:template name="appendix" text="Annex %n %t"/>
    </l:context>    
    <l:context name="title"> 
      <l:template name="chapter" text="%n  %t"/>
      <!--Appendixes/Annexes-->
      <l:template name="appendix" text="Annex %n %t"/>
    </l:context>
    <l:gentext key="Appendix" text="Annex"/>
    <l:gentext key="appendix" text="Annex"/>
  </l:l10n>
</l:i18n>
  <!-- ============================================================ -->
<!--variablelist-->
  <xsl:template match="d:varlistentry" mode="vl.as.list">
    <xsl:variable name="id">
      <xsl:call-template name="object.id"/>
    </xsl:variable>
    
    <xsl:variable name="keep.together">
      <xsl:call-template name="pi.dbfo_keep-together"/>
    </xsl:variable>
    
    <xsl:variable name="item.contents">
      <fo:list-item-label end-indent="label-end()" text-align="start">
        <fo:block xsl:use-attribute-sets="variablelist.term.properties">
          <xsl:apply-templates select="d:term"/>
        </fo:block>
      </fo:list-item-label>
      <fo:list-item-body start-indent="body-start()">
        <fo:block xsl:use-attribute-sets="variablelist.item.properties">
          <xsl:apply-templates select="d:listitem"/>
        </fo:block>
      </fo:list-item-body>
    </xsl:variable>
    
    <xsl:choose>
      <xsl:when test="parent::*/@spacing = 'compact'">
        <fo:list-item id="{$id}"
          xsl:use-attribute-sets="compact.list.item.spacing">
          <xsl:if test="$keep.together != ''">
            <xsl:attribute name="keep-together.within-column"><xsl:value-of
              select="$keep.together"/></xsl:attribute>
          </xsl:if>
          <xsl:copy-of select="$item.contents"/>
        </fo:list-item>
      </xsl:when>
      <xsl:otherwise>
        <fo:list-item id="{$id}" xsl:use-attribute-sets="list.item.spacing">
          <xsl:if test="$keep.together != ''">
            <xsl:attribute name="keep-together.within-column"><xsl:value-of
              select="$keep.together"/></xsl:attribute>
          </xsl:if>
          <xsl:copy-of select="$item.contents"/>
        </fo:list-item>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  

  <!-- ============================================================ -->
 <!-- Revision history-->
  <xsl:template name="revhistory.page">
    <fo:block break-after="page">
      <fo:leader/>
    </fo:block>
    <fo:block font-size="16pt" text-transform ="uppercase"  margin-top="6pt"  space-after="10pt">
          <xsl:call-template name="gentext">
            <xsl:with-param name="key" select="'RevHistory'"/>
          </xsl:call-template>
    </fo:block>
    <fo:block>
      <xsl:apply-templates mode="book.titlepage.verso.auto.mode" select="d:info/d:revhistory"/>
    </fo:block>
  </xsl:template>
  
  <xsl:template match="d:revhistory" mode="book.titlepage.verso.auto.mode">
    <fo:block xsl:use-attribute-sets="book.titlepage.verso.style">
      <xsl:apply-templates select="." mode="book.titlepage.verso.mode"/>
    </fo:block>
  </xsl:template>
  
  <xsl:template match="d:revhistory" mode="titlepage.mode">
    
    <xsl:variable name="explicit.table.width">
      <xsl:call-template name="pi.dbfo_table-width"/>
    </xsl:variable>
    
    <xsl:variable name="table.width">
      <xsl:choose>
        <xsl:when test="$explicit.table.width != ''">
          <xsl:value-of select="$explicit.table.width"/>
        </xsl:when>
        <xsl:when test="$default.table.width = ''">
          <xsl:text>100%</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$default.table.width"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    
    <fo:table table-layout="fixed" width="{$table.width}" xsl:use-attribute-sets="revhistory.table.properties">
      <fo:table-column column-number="1" column-width="11%"/>
      <fo:table-column column-number="2" column-width="16%"/>
      <fo:table-column column-number="3" column-width="73%"/>
      <fo:table-body start-indent="0pt" end-indent="0pt">
        <fo:table-row>
          <fo:table-cell xsl:use-attribute-sets="revhistory.table.cell.properties">
            <fo:block xsl:use-attribute-sets="revhistory.title.properties">
            <xsl:text>Vers.</xsl:text>
            </fo:block>
          </fo:table-cell>
          <fo:table-cell xsl:use-attribute-sets="revhistory.table.cell.properties">
            <fo:block xsl:use-attribute-sets="revhistory.title.properties">
              <xsl:text>Date</xsl:text>
            </fo:block>
          </fo:table-cell>
          <fo:table-cell xsl:use-attribute-sets="revhistory.table.cell.properties">
            <fo:block xsl:use-attribute-sets="revhistory.title.properties">
              <xsl:text>Description</xsl:text>
            </fo:block>
          </fo:table-cell>
        </fo:table-row>
        <xsl:apply-templates select="*[not(self::d:title)]" mode="titlepage.mode"/>
      </fo:table-body>
    </fo:table>
    
  </xsl:template>
  
  <xsl:template match="d:revhistory/d:revision" mode="titlepage.mode">
    <xsl:variable name="revnumber" select="d:revnumber"/>
    <xsl:variable name="revdate"   select="d:date"/>
    <xsl:variable name="revdescription" select="d:revdescription"/>
    <fo:table-row>
      <fo:table-cell xsl:use-attribute-sets="revhistory.table.cell.properties">
        <fo:block>
          <xsl:if test="$revnumber">
            <xsl:apply-templates select="$revnumber[1]" mode="titlepage.mode"/>
          </xsl:if>
        </fo:block>
      </fo:table-cell>
      <fo:table-cell xsl:use-attribute-sets="revhistory.table.cell.properties">
        <fo:block>
          <xsl:apply-templates select="$revdate[1]" mode="titlepage.mode"/>
        </fo:block>
      </fo:table-cell>
<!--      without padding-end text will be overlap border-->
      <fo:table-cell xsl:use-attribute-sets="revhistory.table.cell.properties"  padding-end="24pt">
        <fo:block >
          <xsl:apply-templates select="$revdescription[1]" mode="titlepage.mode"/>
        </fo:block>
      </fo:table-cell>
    </fo:table-row>
  </xsl:template>
  
  <!-- ============================================================ -->
<!--  itemzedlist-->
  <xsl:template match="d:itemizedlist/d:listitem">
    <xsl:variable name="id"><xsl:call-template name="object.id"/></xsl:variable>
    
    <xsl:variable name="keep.together">
      <xsl:call-template name="pi.dbfo_keep-together"/>
    </xsl:variable>
    
    <xsl:variable name="item.contents">
      <fo:list-item-label end-indent="label-end()" xsl:use-attribute-sets="itemizedlist.label.properties">
        <fo:block>
          <xsl:call-template name="itemizedlist.label.markup">
            <xsl:with-param name="itemsymbol">
              <xsl:call-template name="list.itemsymbol">
                <xsl:with-param name="node" select="parent::d:itemizedlist"/>
              </xsl:call-template>
            </xsl:with-param>
          </xsl:call-template>
        </fo:block>
      </fo:list-item-label>
      <fo:list-item-body start-indent="body-start()">
        <fo:block>
          <xsl:apply-templates/>
        </fo:block>
      </fo:list-item-body>
    </xsl:variable>
    
    <xsl:choose>
      <xsl:when test="parent::*/@spacing = 'compact'">
        <fo:list-item id="{$id}" xsl:use-attribute-sets="compact.list.item.spacing">
          <xsl:if test="$keep.together != ''">
            <xsl:attribute name="keep-together.within-column"><xsl:value-of
              select="$keep.together"/></xsl:attribute>
          </xsl:if>
          <xsl:copy-of select="$item.contents"/>
        </fo:list-item>
      </xsl:when>
      <xsl:otherwise>
        <fo:list-item id="{$id}" >
          <xsl:choose>
            <!--   compact lists for revhistory-->
            <xsl:when test="ancestor::d:revhistory">
              <xsl:call-template name="revhistory.list.item.spacing"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:call-template name="list.item.spacing"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="$keep.together != ''">
            <xsl:attribute name="keep-together.within-column"><xsl:value-of
              select="$keep.together"/></xsl:attribute>
          </xsl:if>
          <xsl:copy-of select="$item.contents"/>
        </fo:list-item>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- ============================================================ -->
  <!--Tables-->
  <xsl:template name="table.row.properties">
    <xsl:if test="ancestor::d:thead">
      <xsl:attribute name="font-weight">bold</xsl:attribute>
      <xsl:attribute name="background-color">#BFBFBF</xsl:attribute>
      <xsl:attribute name="letter-spacing">0.4pt</xsl:attribute>
    </xsl:if>
  </xsl:template>
  <!-- ============================================================ -->
  <!--trademark-->
  <xsl:template match="d:trademark">
    <xsl:call-template name="inline.charseq"/>
    <fo:inline>
<!--      <xsl:if test="ancestor::orgname">-->
        <xsl:attribute name="font-size">27pt</xsl:attribute>
 <!--       <xsl:attribute name="baseline-shift">super</xsl:attribute>-->
      <!--</xsl:if>-->
    <xsl:choose>
      <xsl:when test="@class = 'copyright'
        or @class = 'registered'">
        <xsl:call-template name="dingbat">
          <xsl:with-param name="dingbat" select="@class"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="@class = 'service'">
        <xsl:call-template name="inline.superscriptseq">
          <xsl:with-param name="content" select="'SM'"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="dingbat">
          <xsl:with-param name="dingbat" select="'trademark'"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
    </fo:inline>
  </xsl:template>
  <!-- ============================================================ -->
  
  
</xsl:stylesheet>



