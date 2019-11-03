''' <summary>
''' Repeater with support for DataPager
''' </summary>
<ToolboxData("<{0}:DataPagerRepeater runat=server PersistentDataSource=true></{0}:DataPagerRepeater>")>
Public Class DataPagerRepeater
    Inherits Repeater
    Implements System.Web.UI.WebControls.IPageableItemContainer, INamingContainer

    ''' <summary>
    ''' Number of rows to show
    ''' </summary>
    Public ReadOnly Property MaximumRows() As Integer Implements System.Web.UI.WebControls.IPageableItemContainer.MaximumRows
        Get
            Return If(ViewState("MaximumRows") IsNot Nothing, CInt(Math.Truncate(ViewState("MaximumRows"))), -1)
        End Get
    End Property

    ''' <summary>
    ''' First row to show
    ''' </summary>
    Public ReadOnly Property StartRowIndex() As Integer Implements System.Web.UI.WebControls.IPageableItemContainer.StartRowIndex
        Get
            Return If(ViewState("StartRowIndex") IsNot Nothing, CInt(Math.Truncate(ViewState("StartRowIndex"))), -1)
        End Get
    End Property

    ''' <summary>
    ''' Total rows. When PagingInDataSource is set to true you must get the total records from the datasource (without paging) at the FetchingData event
    ''' When PagingInDataSource is set to true you also need to set this when you load the data the first time.
    ''' </summary>
    Public Property TotalRows() As Integer
        Get
            Return If(ViewState("TotalRows") IsNot Nothing, CInt(Math.Truncate(ViewState("TotalRows"))), -1)
        End Get
        Set(ByVal value As Integer)
            ViewState("TotalRows") = value
        End Set
    End Property

    ''' <summary>
    ''' If repeater should store data source in view state. If false you need to get and bind data at post back. When using a connected data source this is handled by the data source.  
    ''' </summary>        
    Public Property PersistentDataSource() As Boolean
        Get
            Return If(ViewState("PersistentDataSource") IsNot Nothing, CBool(ViewState("PersistentDataSource")), True)
        End Get
        Set(ByVal value As Boolean)
            ViewState("PersistentDataSource") = value
        End Set
    End Property

    ''' <summary>
    ''' Set to true if you want to handle paging in the data source. 
    ''' Ex if you are selecting data from the database and only select the current rows 
    ''' you must set this property to true and get and rebind data at the FetchingData event. 
    ''' If this is true you must also set the TotalRecords property at the FetchingData event.     
    ''' </summary>
    ''' <seealso cref="FetchingData"/>
    ''' <seealso cref="TotalRows"/>
    Public Property PagingInDataSource() As Boolean
        Get
            Return If(ViewState("PageingInDataSource") IsNot Nothing, CBool(ViewState("PageingInDataSource")), False)
        End Get
        Set(ByVal value As Boolean)
            ViewState("PageingInDataSource") = value
        End Set
    End Property

    ''' <summary>
    ''' Checks if you need to rebind data source at postback
    ''' </summary>
    Public ReadOnly Property NeedsDataSource() As Boolean
        Get
            If PagingInDataSource Then
                Return True
            End If
            If IsBoundUsingDataSourceID = False AndAlso (Not Page.IsPostBack) Then
                Return True
            End If
            If IsBoundUsingDataSourceID = False AndAlso PersistentDataSource = False AndAlso Page.IsPostBack Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Loading ViewState
    ''' </summary>
    ''' <param name="savedState"></param>
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        If Page.IsPostBack Then
            If NeedsDataSource AndAlso FetchingDataEvent IsNot Nothing Then
                If PagingInDataSource Then
                    SetPageProperties(StartRowIndex, MaximumRows, True)
                End If
                RaiseEvent FetchingData(Me, Nothing)
            End If

            If (Not IsBoundUsingDataSourceID) AndAlso PersistentDataSource AndAlso ViewState("DataSource") IsNot Nothing Then
                Me.DataSource = ViewState("DataSource")
                Me.DataBind()
            End If
            If IsBoundUsingDataSourceID Then
                Me.DataBind()
            End If
        End If

        MyBase.OnLoad(e)
    End Sub

    ''' <summary>
    ''' Method used by pager to set totalrecords
    ''' </summary>
    ''' <param name="startRowIndex">startRowIndex</param>
    ''' <param name="maximumRows">maximumRows</param>
    ''' <param name="databind">databind</param>
    Public Sub SetPageProperties(ByVal startRowIndex As Integer, ByVal maximumRows As Integer, ByVal databind As Boolean) Implements System.Web.UI.WebControls.IPageableItemContainer.SetPageProperties
        ViewState("StartRowIndex") = startRowIndex
        ViewState("MaximumRows") = maximumRows

        If TotalRows > -1 Then
            RaiseEvent TotalRowCountAvailable(Me, New PageEventArgs(CInt(Math.Truncate(ViewState("StartRowIndex"))), CInt(Math.Truncate(ViewState("MaximumRows"))), TotalRows))
        End If
    End Sub

    ''' <summary>
    ''' OnDataPropertyChanged
    ''' </summary>
    Protected Overrides Sub OnDataPropertyChanged()
        If MaximumRows <> -1 OrElse IsBoundUsingDataSourceID Then
            Me.RequiresDataBinding = True
        End If

        MyBase.OnDataPropertyChanged()
    End Sub

    ''' <summary>
    ''' Renders only current items selected by pager
    ''' </summary>
    ''' <param name="writer"></param>
    Protected Overrides Sub RenderChildren(ByVal writer As HtmlTextWriter)
        If (Not PagingInDataSource) AndAlso MaximumRows <> -1 Then
            For Each item As RepeaterItem In Me.Items
                If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                    item.Visible = False
                    If item.ItemIndex >= CInt(Math.Truncate(ViewState("StartRowIndex"))) AndAlso item.ItemIndex < (CInt(Math.Truncate(ViewState("StartRowIndex"))) + CInt(Math.Truncate(ViewState("MaximumRows")))) Then
                        item.Visible = True
                    End If
                Else
                    item.Visible = True
                End If
            Next item
        End If
        MyBase.RenderChildren(writer)
    End Sub

    ''' <summary>
    ''' Get Data
    ''' </summary>
    ''' <returns></returns>
    Protected Overrides Function GetData() As System.Collections.IEnumerable
        Dim dataObjects As System.Collections.IEnumerable = MyBase.GetData()

        If dataObjects Is Nothing AndAlso Me.DataSource IsNot Nothing Then
            If TypeOf Me.DataSource Is System.Collections.IEnumerable Then
                dataObjects = DirectCast(Me.DataSource, System.Collections.IEnumerable)
            Else
                dataObjects = DirectCast(Me.DataSource, System.ComponentModel.IListSource).GetList()
            End If
        End If

        If (Not PagingInDataSource) AndAlso MaximumRows <> -1 AndAlso dataObjects IsNot Nothing Then
            Dim i As Integer = -1

            If dataObjects IsNot Nothing Then
                i = 0
                For Each o As Object In dataObjects
                    i += 1
                Next o
            End If

            ViewState("TotalRows") = i

            If (Not IsBoundUsingDataSourceID) AndAlso PersistentDataSource Then
                ViewState("DataSource") = Me.DataSource
            End If

            SetPageProperties(StartRowIndex, MaximumRows, True)
        End If

        If PagingInDataSource AndAlso (Not Page.IsPostBack) Then
            SetPageProperties(StartRowIndex, MaximumRows, True)
        End If

        Return dataObjects
    End Function

    ''' <summary>
    ''' Event when pager/repeater have counted total rows
    ''' </summary>
    Public Event TotalRowCountAvailable As System.EventHandler(Of PageEventArgs) Implements System.Web.UI.WebControls.IPageableItemContainer.TotalRowCountAvailable

    ''' <summary>
    ''' Event when repeater gets the data on postback
    ''' </summary>
    Public Event FetchingData As System.EventHandler(Of PageEventArgs)
End Class
