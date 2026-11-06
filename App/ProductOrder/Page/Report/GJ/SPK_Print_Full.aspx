<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SPK_Print_Full.aspx.cs" Inherits="Page_Register_SPK_Print" %>


<div runat="server" id="div_Print">
    <style id="SPK PDD - HTML Template_4699_Styles">
        <!-- table {
            mso-displayed-decimal-separator: "\.";
            mso-displayed-thousand-separator: "\,";
        }

        .xl654699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: none;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl664699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl674699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: none;
            border-bottom: none;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl684699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: 1.0px solid windowtext;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl694699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl704699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl714699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl724699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl734699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl744699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 13.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl754699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: general;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl764699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: general;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl774699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl784699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl794699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl804699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: general;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl814699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: left;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl824699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: left;
            vertical-align: middle;
            border-top: none;
            border-right: 1.0px solid windowtext;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl834699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: general;
            vertical-align: middle;
            border-top: none;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl844699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: none;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl854699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl864699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl874699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl884699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl894699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl904699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: none;
            border-bottom: none;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl914699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl924699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #BFBFBF;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: 1.0px solid windowtext;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl934699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl944699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: left;
            vertical-align: top;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl954699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: left;
            vertical-align: top;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl964699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl974699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #A6A6A6;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl984699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #A6A6A6;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl994699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #A6A6A6;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1004699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: #A6A6A6;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: general;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1014699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: none;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1024699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 41.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: none;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1034699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 41.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1044699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 41.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1054699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 41.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1064699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 41.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1074699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 41.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: none;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1084699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 41.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1094699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: left;
            vertical-align: top;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: none;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1104699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: .5px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1114699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: .5px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1124699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: .5px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1134699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 18.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1144699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 18.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1154699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 18.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: 1.0px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: none;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1164699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            border-top: 1.0px solid windowtext;
            border-right: .5px solid windowtext;
            border-bottom: 1.0px solid windowtext;
            border-left: .5px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1174699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Arial, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: left;
            vertical-align: top;
            border-top: 1.0px solid windowtext;
            border-right: none;
            border-bottom: 1.0px solid windowtext;
            border-left: 1.0px solid windowtext;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: normal;
        }

        .xl1184699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 13.0px;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Calibri, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: general;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1194699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Calibri, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: general;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }

        .xl1204699 {
            padding-top: 1px;
            padding-right: 1px;
            padding-left: 1px;
            mso-ignore: padding;
            color: black;
            font-size: 12.0px;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            font-family: Calibri, sans-serif;
            mso-font-charset: 0;
            mso-number-format: General;
            text-align: center;
            vertical-align: middle;
            mso-background-source: auto;
            mso-pattern: auto;
            white-space: nowrap;
        }
        -->
    </style>


    <!--[if !excel]>&nbsp;&nbsp;<![endif]-->
    <!--The following information was generated by Microsoft Excel's Publish as Web
Page wizard.-->
    <!--If the same item is republished from Excel, all information between the DIV
tags will be replaced.-->
    <!----------------------------->
    <!--START OF OUTPUT FROM EXCEL PUBLISH AS WEB PAGE WIZARD -->
    <!----------------------------->

    <div id="SPK PDD - HTML Template_4699" align="center" x:publishsource="Excel">
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="1040" class="xl1184699"
            style='border-collapse: collapse; table-layout: fixed; width: 784px'>
            <col class="xl1184699" width="50" style='mso-width-source: userset; mso-width-alt: 1828; width: 38px'>
            <col class="xl1184699" width="48" style='mso-width-source: userset; mso-width-alt: 1755; width: 36px'>
            <col class="xl1184699" width="58" style='mso-width-source: userset; mso-width-alt: 2121; width: 44px'>
            <col class="xl1184699" width="45" span="11" style='mso-width-source: userset; mso-width-alt: 1645; width: 34px'>
            <col class="xl1184699" width="51" span="3" style='mso-width-source: userset; mso-width-alt: 1865; width: 38px'>
            <col class="xl1184699" width="56" style='mso-width-source: userset; mso-width-alt: 2048; width: 42px'>
            <col class="xl1184699" width="45" span="4" style='mso-width-source: userset; mso-width-alt: 1645; width: 34px'>
            <tr height="28" style='mso-height-source: userset; height: 21.0px'>

                <td colspan="11" class="xl1134699" width="508" style='border-right: 1.0px solid black; border-left: 1.0px solid black; width: 384px'>LEMBAR PENGESAHAN<span
                    style='mso-spacerun: yes'> </span></td>
                <td colspan="2" rowspan="2" height="86" class="xl654699" width="98" style='border-right: 1.0px solid black; border-bottom: 1.0px solid black; height: 64.5px; width: 74px'>
                    <asp:image runat="server" id="img_Logo" style="border: none; max-width: 66px; max-height: 66px; padding: 0" />
                </td>
                <td class="xl694699" width="45" style='width: 34px'></td>
                <td colspan="8" class="xl694699" width="389" style='width: 292px'></td>
            </tr>
            <tr height="29" style='mso-height-source: userset; height: 21.75px'>
                <td colspan="11" height="58" class="xl1024699" style='border-right: 1.0px solid black; border-bottom: 1.0px solid black; height: 43.5px'>SPK</td>
                <td class="xl694699"></td>
                <td colspan="8" class="xl1084699"></td>
            </tr>
            <tr height="29" style='mso-height-source: userset; height: 21.75px'>
                <td height="29" colspan="14" class="" style='height: 21.75px'>&nbsp;</td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>No . LDKP</td>
                <td colspan="5" class="xl724699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_NomorLDKP" />
                </td>
            </tr>
            <tr class="xl1194699" height="29" style='mso-height-source: userset; height: 21.75px'>
                <td colspan="13" height="29" class="xl804699" style='height: 21.75px; border-top: none !important;'>&nbsp;</td>
                <td class="xl694699"></td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>Status
  Order</td>
                <td colspan="5" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_StatusOrder" />
                </td>
            </tr>
            <tr class="xl1194699" height="29" style='mso-height-source: userset; height: 21.75px'>
                <td colspan="4" height="29" class="xl814699" style='border-right: 1.0px solid black; height: 21.75px'>No. &amp; Nama Customer</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_NomorCustomer" />
                </td>
                <td colspan="6" class="xl784699" style='border-right: 1.0px solid black'>
                    <asp:literal runat="server" id="lit_NamaCustomer" />
                </td>
                <td class="xl694699"></td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>Beban
  Susut</td>
                <td colspan="5" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_BebanSusut" />
                </td>
            </tr>
            <tr class="xl1194699" height="30" style='mso-height-source: userset; height: 22.5px'>
                <td colspan="4" height="30" class="xl814699" style='border-right: 1.0px solid black; height: 22.5px'>No. Invoice DP**</td>
                <td colspan="9" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_NomorInvoiceDP" />
                </td>
                <td class="xl694699"></td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>Komponen</td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Komponen" />
                </td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Poin</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_PoinKomponen" />
                </td>
            </tr>
            <tr class="xl1194699" height="30" style='mso-height-source: userset; height: 22.5px'>
                <td colspan="4" height="30" class="xl814699" style='border-right: 1.0px solid black; height: 22.5px'>No. PO*</td>
                <td colspan="9" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_NomorPO" />
                </td>
                <td class="xl694699"></td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>Nama
  Toko*</td>
                <td colspan="5" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_NamaToko" />
                </td>
            </tr>
            <tr class="xl1194699" height="30" style='mso-height-source: userset; height: 22.5px'>
                <td colspan="4" height="30" class="xl814699" style='border-right: 1.0px solid black; height: 22.5px'>No. SPK - Tgl. SPK</td>
                <td colspan="4" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_NomorSPK" />
                </td>
                <td colspan="5" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_TanggalSPK" />
                </td>
                <td class="xl694699"></td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>Tingkat
  Kesulitan<span style='mso-spacerun: yes'>  </span></td>
                <td colspan="5" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_TingkatKesulitan" />
                </td>
            </tr>
            <tr class="xl1194699" height="30" style='mso-height-source: userset; height: 22.5px'>
                <td colspan="4" height="30" class="xl814699" style='border-right: 1.0px solid black; height: 22.5px'>Tgl. Persetujuan SPK</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_TanggalPersetujuanSPK" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Jatuh Tempo</td>
                <td colspan="3" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_TanggalJatuhTempo" />
                </td>
                <td class="xl694699"></td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>Brand
  Batu</td>
                <td colspan="5" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_BrandBatu" />
                </td>
            </tr>
            <tr class="xl1194699" height="30" style='mso-height-source: userset; height: 22.5px'>
                <td colspan="4" height="30" class="xl814699" style='border-right: 1.0px solid black; height: 22.5px'>Kelas Harga</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_KelasHarga" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Status Barang</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_StatusBarang" />
                </td>
                <td class="xl694699"></td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>Nama
  koleksi</td>
                <td colspan="5" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_NamaKoleksi" />
                </td>
            </tr>
            <tr class="xl1194699" height="30" style='mso-height-source: userset; height: 22.5px'>
                <td colspan="14" height="30" class="xl814699" style='height: 22.5px'></td>
                <td colspan="3" class="xl814699" style='border-right: 1.0px solid black'>Product Category</td>
                <td colspan="5" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="productcategory" />
                </td>
            </tr>
            <tr class="xl1194699" height="15" style='mso-height-source: userset; height: 11.25px'>
                <td colspan="22" height="15" class="xl834699" style='height: 11.25px'>&nbsp;</td>
            </tr>
            <tr class="xl1194699" height="23" style='mso-height-source: userset; height: 17.25px'>
                <td colspan="3" height="23" class="xl774699" style='border-right: 1.0px solid black; height: 17.25px'>No. Kode Karet</td>
                <td colspan="4" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Jenis</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Jumlah</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Panjang</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Ring Size</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Lebar</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Dia (cm)</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Berat</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Warna Emas</td>
            </tr>
            <tr class="xl1194699" height="60" style='mso-height-source: userset; height: 45.0px'>
                <td colspan="3" height="60" class="xl844699" style='border-right: 1.0px solid black; height: 45.0px'>
                    <asp:literal runat="server" id="lit_KodeKaret" />
                    <asp:literal runat="server" id="kodekaretlama" />
                </td>
                <td colspan="4" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Jenis" />
                </td>
                <td colspan="2" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Jumlah" />
                </td>
                <td colspan="2" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Panjang" />
                </td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_RingSize" />
                </td>
                <td colspan="2" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Lebar" />
                </td>
                <td colspan="2" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Diameter" />
                </td>
                <td colspan="2" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Berat" />
                </td>
                <td colspan="3" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_WarnaEmas" />
                </td>
            </tr>
            <tr class="xl1194699" height="24" style='mso-height-source: userset; height: 18.0px'>
                <td colspan="14" height="24" class="xl774699" style='border-right: 1.0px solid black; height: 18.0px'>Gambar &amp; Keterangan Lengkap</td>
                <td colspan="8" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Stone</td>
            </tr>
            <tr class="xl1194699" height="80" style='mso-height-source: userset; height: 60.0px'>
                <td colspan="13" height="80" class="xl1094699" style='height: 60.0px'>
                    <br />
                    <asp:image runat="server" id="imgpicture"  />
                    <br />
                    <br />
                    Keterangan Gambar :<br />
                    <span style="font-style: normal; font-family: Calibri, sans-serif; font-weight: 400; white-space: normal;">
                        <asp:literal runat="server" id="lit_KeteranganGambar" />
                    </span>
                </td>
                <td class="xl854699" style='border-top: none'>&nbsp;</td>
                <td colspan="8" valign="top" style="height: 450px;">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; border: solid 1px black; border-top: none; border-collapse: collapse; text-align: center; font-weight: bold; font-family: Arial, sans-serif; font-size: 12px;">
                        <tr>
                            <td colspan="5">
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid black;">Bentuk<br />
                                (Parcel)</td>
                            <td style="border: 1px solid black;">Jml<br />
                                butir</td>
                            <td style="border: 1px solid black;">Carat/<br />
                                Butir</td>
                            <td style="border: 1px solid black;">Total<br />
                                Carat</td>
                            <td style="border: 1px solid black;">Dimensi<br />
                                (mm)</td>
                        </tr>
                        <asp:literal runat="server" id="lit_Parcel" />
                        <%--<tr>
			<td style="border: 1px solid black;">B004S</td>
			<td style="border: 1px solid black;"></td>
			<td style="border: 1px solid black;"></td>
			<td style="border: 1px solid black;"></td>
			<td style="border: 1px solid black;"></td>
		</tr>--%>
                        <tr>
                            <td style="border: 1px solid black;">TOTAL</td>
                            <td style="border: 1px solid black;">
                                <asp:literal runat="server" id="lit_JumlahButir" />
                            </td>
                            <td style="border: 1px solid black;">
                                <asp:literal runat="server" id="lit_CaratPerButir" />
                            </td>
                            <td style="border: 1px solid black;">
                                <asp:literal runat="server" id="lit_TotalCarat" />
                            </td>
                            <td style="border: 1px solid black; background-color: black;">&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="xl1194699" height="15" style='mso-height-source: userset; height: 11.25px'>
                <td colspan="22" height="15" class="xl834699" style='height: 11.25px'>&nbsp;</td>
            </tr>
            <%--<tr class="xl1194699" height="39" style='mso-height-source: userset; height: 29.25px'>
                <td colspan="3" height="39" class="xl774699" style='border-right: 1.0px solid black; height: 29.25px'>Kode Komponen</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_KodeKomponen_1" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_KodeKomponen_2" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_KodeKomponen_3" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_KodeKomponen_4" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_KodeKomponen_5" />
                </td>
                <td colspan="4" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_KodeKomponen_6" />
                </td>
            </tr>
            <tr class="xl1194699" height="39" style='mso-height-source: userset; height: 29.25px'>
                <td colspan="3" height="39" class="xl774699" style='border-right: 1.0px solid black; height: 29.25px'>Jumlah</td>
                <td colspan="3" class="xl1104699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_JumlahKomponen_1" />
                </td>
                <td colspan="3" class="xl1104699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_JumlahKomponen_2" />
                </td>
                <td colspan="3" class="xl1104699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_JumlahKomponen_3" />
                </td>
                <td colspan="3" class="xl1104699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_JumlahKomponen_4" />
                </td>
                <td colspan="3" class="xl1104699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_JumlahKomponen_5" />
                </td>
                <td colspan="4" class="xl1104699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_JumlahKomponen_6" />
                </td>
            </tr>--%>
            <%--<tr class="xl1194699" height="15" style='mso-height-source: userset; height: 11.25px'>
                <td colspan="22" height="15" class="xl754699" style='height: 11.25px'>&nbsp;</td>
            </tr>--%>
        </table>

        <table border="0" cellpadding="0" cellspacing="0" width="1040" class="xl1184699"
            style='border-collapse: collapse; table-layout: fixed; width: 784px'>
            <tr class="xl1204699" height="28" style='mso-height-source: userset; height: 21.0px' />
                <td height="28" class="xl1014699" style='height: 21.0px; border-top: none'>Size</td>
                <td class="xl774699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Size_2" />
                </td>
                <td class="xl934699" style='border-top: none'>Grafir</td>
                <%--<td class="xl934699" style='border-top: none' colspan="19"><asp:Literal runat="server" id="lit_Grafir" /></td>--%>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_1" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_2" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_3" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_4" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_5" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_6" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_7" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_8" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_9" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_10" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_11" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_12" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_13" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_14" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_15" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_16" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_17" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_18" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir_19" />
                </td>
            </tr>
            <tr class="xl1204699" height="28" style='mso-height-source: userset; height: 21.0px' />
                <td height="28" class="xl1014699" style='height: 21.0px; border-top: none'</td>
                <td class="xl774699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Size_2_2" />
                </td>
                <td class="xl934699" style='border-top: none'>Grafir</td>
                <%--<td class="xl934699" style='border-top: none' colspan="19"><asp:Literal runat="server" id="Literal2" /></td>--%>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_1" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_2" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_3" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_4" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_5" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_6" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_7" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_8" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_9" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_10" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_11" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_12" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_13" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_14" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_15" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_16" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_17" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_18" />
                </td>
                <td class="xl934699" style='border-top: none; border-left: none'>
                    <asp:literal runat="server" id="lit_Grafir2_19" />
                </td>
            </tr>
            <tr class="xl1194699" height="23" style='mso-height-source: userset; height: 17.25px'>
                <td colspan="2" rowspan="2" height="45" class="xl844699" style='border-right: 1.0px solid black; border-bottom: 1.0px solid black; height: 33.75px'>TITIPAN</td>
                <td colspan="3" class="xl874699" style='border-right: 1.0px solid black; border-left: none'>EMAS</td>
                <td colspan="4" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>STONE</td>
                <td colspan="4" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>COLOUR STONE***</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>CABANG*</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>PUSAT*</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>FACTORY*</td>
            </tr>
            <tr class="xl1194699" height="22" style='mso-height-source: userset; height: 16.5px'>
                <td colspan="3" height="22" class="xl774699" style='border-right: 1.0px solid black; height: 16.5px; border-left: none'>Berat</td>
                <td colspan="2" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>Carat</td>
                <td colspan="2" class="xl844699" style='border-right: 1.0px solid black; border-left: none'>Butir</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Carat</td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Butir</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'><span style='mso-spacerun: yes'></span>/<span
                    style='mso-spacerun: yes'>      </span>/<span
                        style='mso-spacerun: yes'>  </span></td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>/<span style='mso-spacerun: yes'>      </span>/</td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>/<span style='mso-spacerun: yes'>      </span>/</td>
            </tr>
            <tr class="xl1194699" height="30" style='mso-height-source: userset; height: 22.5px'>
                <td colspan="2" height="30" class="xl874699" style='border-right: 1.0px solid black; height: 22.5px'>
                    <asp:literal runat="server" id="lit_Titipan" />
                </td>
                <td colspan="3" class="xl774699" style='border-left: none'>
                    <asp:literal runat="server" id="lit_EmasBerat" />
                </td>
                <td colspan="2" class="xl1104699" style='border-right: 1.0px solid black'>
                    <asp:literal runat="server" id="lit_StoneCarat" />
                </td>
                <td colspan="2" class="xl1104699" style='border-left: none'>
                    <asp:literal runat="server" id="lit_StoneButir" />
                </td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black'>
                    <asp:literal runat="server" id="lit_ColourStoneCarat" />
                </td>
                <td colspan="2" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_ColourStoneButir" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Cabang" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Pusat" />
                </td>
                <td colspan="3" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>
                    <asp:literal runat="server" id="lit_Factory" />
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="1040" class="xl1184699"
            style='border-collapse: collapse; table-layout: fixed; width: 784px'>
            <tr class="xl1194699" height="55" style='mso-height-source: userset; height: 41.25px'>
                <td colspan="22" height="55" class="xl1174699" width="1040" style='border-right: 1.0px solid black; height: 41.25px; width: 784px'>Keterangan/Perubahan Materi SPK :<br>
                    <span style="font-style: normal; font-family: Calibri, sans-serif; font-weight: 400; white-space: normal;">
                        <asp:literal runat="server" id="lit_KeteranganPerubahanSPK" />
                    </span>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="1040" class="xl1184699"
            style='border-collapse: collapse; table-layout: fixed; width: 784px'>
            <tr class="xl1194699" height="25" style='mso-height-source: userset; height: 18.75px'>
                <td colspan="4" height="25" class="xl774699" style='border-right: 1.0px solid black; height: 18.75px'>QC Master</td>
                <td colspan="4" height="25" class="xl774699" style='border-right: 1.0px solid black; height: 18.75px'>QC BRJ</td>
                <td colspan="4" class="xl784699">Foto BRJ</td>
                <td colspan="4" class="xl774699" style='border-right: 1.0px solid black'>Pemesan,</td>
                <td colspan="6" class="xl784699" style='border-right: 1.0px solid black'>Menyetujui,</td>
                <td colspan="4" class="xl774699" style='border-right: 1.0px solid black; border-left: none'>Diketahui,</td>
            </tr>
            <tr class="xl1194699" height="100" style='mso-height-source: userset; height: 75.0px'>
                <td colspan="4" height="100" class="xl904699" style='border-right: 1.0px solid black; height: 75.0px'>&nbsp;</td>
                <td colspan="4" height="100" class="xl904699" style='border-right: 1.0px solid black; height: 75.0px'>&nbsp;</td>
                <td colspan="4" class="xl914699"></td>
                <td colspan="4" class="xl904699" style='border-right: 1.0px solid black'>&nbsp;</td>
                <td colspan="3" class="xl914699"></td>
                <td colspan="3" class="xl904699" style='border-right: 1.0px solid black'>&nbsp;</td>
                <td colspan="4" class="xl904699" style='border-right: 1.0px solid black; border-left: none'>&nbsp;</td>
            </tr>
            <tr class="xl1194699" height="40" style='mso-height-source: userset; height: 30.0px'>
                <td colspan="4" height="40" class="xl974699" style='border-right: 1.0px solid black; height: 30.0px'></td>
                <td colspan="4" height="40" class="xl974699" style='border-right: 1.0px solid black; height: 30.0px'>PM</td>
                <td colspan="4" class="xl984699">Staff Foto</td>
                <td colspan="4" class="xl974699" style='border-right: 1.0px solid black'>JP Toko</td>
                <td colspan="3" class="xl984699">HPM</td>
                <td colspan="3" class="xl974699" style='border-right: 1.0px solid black'>PM/BM</td>
                <td colspan="4" class="xl974699" style='border-right: 1.0px solid black; border-left: none'>Manager Produksi</td>
            </tr>
            <tr class="xl1194699" height="14" style='mso-height-source: userset; height: 10.5px'>
                <td colspan="22" height="14" class="xl1004699" style='height: 10.5px'>&nbsp;</td>
            </tr>
            <%--tr>
                <td colspan="12">
                    <div align="left">
                        <b>Kode Karet Lama</b> :
                        &nbsp;
                        <asp:literal id="kodekaretlama" runat="server" />
                    </div>

                </td>

            </tr--%>
        </table>
        <br />
        <br />
        <asp:literal runat="server" id="lit_PrintBigImageNextPage" />
    </div>

    <style type="text/css">
        table {
            page-break-inside: avoid;
            page-break-after: auto;
        }

        @media print {
            table {
                page-break-inside: avoid;
                page-break-after: auto;
            }
        }
    </style>
    <!----------------------------->
    <!--END OF OUTPUT FROM EXCEL PUBLISH AS WEB PAGE WIZARD-->
    <!----------------------------->
</div>

