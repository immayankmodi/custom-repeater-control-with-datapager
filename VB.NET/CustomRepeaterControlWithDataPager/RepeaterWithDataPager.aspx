<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RepeaterWithDataPager.aspx.vb"
    Inherits="CustomRepeaterControlWithDataPager.RepeaterWithDataPager" %>

<%--<%@ Register TagPrefix="aspRepeater" Namespace="CustomRepeaterControlWithDataPager"
    Assembly="CustomRepeaterControlWithDataPager" %>--%>

<!DOCTYPE html "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AspnetO.com | Custom Repeater Control With DataPager Example in Asp.net </title>
    <style type="text/css">
        /*Style your Repeater Control*/
        #rptCustom tr td {
            text-align: center;
            vertical-align: middle;
            padding: 3px;
        }

        #rptCustom tr {
            border: 1px solid #e2e2e2;
        }

        .rptHeader > th {
            background: #ff6600;
            font-weight: bold;
            color: White;
            width: 120px;
            height: 28px;
        }

        .rptDataRows:nth-child(odd) {
            background: #f5f5f5;
        }

        /*Style your DataPager Control*/
        .pager {
            float: left;
            height: 40px;
            margin-left: -5px;
            margin-top: 15px;
        }

            .pager ul li a, .pagerbuttons, .selected {
                float: left;
                font-size: 12px;
                font-weight: bold;
                height: 18px;
                margin: 0 5px;
                min-width: 20px;
                padding: 5px;
                text-align: center;
                vertical-align: middle;
                color: White;
            }

            .pager ul li a, .pagerbuttons {
                background: #ff6600;
            }

                .pager ul li a.active, .pager ul li a:hover, .selected, .pager a:hover {
                    background: #000;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 10px;">
            <h4>Custom Repeater Control With DataPager Example in Asp.net</h4>
            <aspRepeater:DataPagerRepeater ID="rptCustomRepeater" PersistentDataSource="true"
                runat="server">
                <HeaderTemplate>
                    <table id="rptCustom">
                        <tr class="rptHeader">
                            <th>SubjectId
                            </th>
                            <th>SubjectName
                            </th>
                            <th>Marks
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="rptDataRows">
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "SubjectId")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "SubjectName")%>
                        </td>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "Marks")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </aspRepeater:DataPagerRepeater>
            <asp:DataPager ID="pgrCustomRepeater" runat="server" PagedControlID="rptCustomRepeater"
                class="pager" PageSize="4">
                <Fields>
                    <asp:NextPreviousPagerField ButtonCssClass="pagerbuttons" RenderDisabledButtonsAsLabels="true"
                        ShowFirstPageButton="false" ShowNextPageButton="false" ShowLastPageButton="false"
                        ShowPreviousPageButton="true" PreviousPageText="Prev" />
                    <asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="selected" NextPreviousButtonCssClass="pagerbuttons"
                        RenderNonBreakingSpacesBetweenControls="true" NumericButtonCssClass="pagerbuttons" />
                    <asp:NextPreviousPagerField ButtonCssClass="pagerbuttons" RenderDisabledButtonsAsLabels="true"
                        ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowNextPageButton="true"
                        ShowLastPageButton="false" NextPageText="Next" />
                </Fields>
            </asp:DataPager>
        </div>
    </form>
</body>
</html>
