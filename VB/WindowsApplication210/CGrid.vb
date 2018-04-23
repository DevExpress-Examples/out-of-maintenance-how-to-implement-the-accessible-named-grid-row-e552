Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Registrator
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Data
Imports DevExpress.XtraGrid.Views.Grid.Customization
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraGrid.Views.Grid.Drawing
Imports DevExpress.XtraGrid.Accessibility
Imports DevExpress.Accessibility

Namespace CGrid
	Public Class CGridControl
		Inherits GridControl
		Protected Overrides Sub RegisterAvailableViewsCore(ByVal collection As InfoCollection)
			MyBase.RegisterAvailableViewsCore(collection)
			collection.Add(New CGridViewInfoRegistrator())
		End Sub
		Protected Overrides Function CreateDefaultView() As BaseView
			Return CreateView("CGridView")
		End Function
	End Class

	Public Class CGridView
		Inherits GridView
		Implements IAccessibleGrid
		Public Sub New()
			Me.New(Nothing)
		End Sub

		Public Sub New(ByVal gc As GridControl)
			MyBase.New(gc)
		End Sub

		Protected Overrides ReadOnly Property ViewName() As String
			Get
				Return "CGridView"
			End Get
		End Property

		Private rowNameField_Renamed As String
		Public Property RowNameField() As String
			Get
				Return rowNameField_Renamed
			End Get
			Set(ByVal value As String)
				rowNameField_Renamed = value
				OnPropertiesChanged()
			End Set
		End Property
		Public Function GetRowName(ByVal rowHandle As Integer) As String
			If (Not Me.IsDataRow(rowHandle)) Then
				Return Nothing
			End If
			Return TryCast(Me.GetRowCellValue(rowHandle, RowNameField), String)
		End Function

		Protected Overrides Sub RaiseCustomDrawRowIndicator(ByVal e As RowIndicatorCustomDrawEventArgs)
			If e.Info IsNot Nothing Then
				e.Info.DisplayText = GetRowName(e.RowHandle)
			End If
			MyBase.RaiseCustomDrawRowIndicator(e)
		End Sub

		Public Shadows Function RowHandle2AccessibleIndex(ByVal rowHandle As Integer) As Integer
			Return MyBase.RowHandle2AccessibleIndex(rowHandle)
		End Function

		#Region "IAccessibleGrid Members"

		Private Function GetRow(ByVal index As Integer) As IAccessibleGridRow Implements IAccessibleGrid.GetRow
			Dim rowHandle As Integer = Me.AccessibleIndex2RowHandle(index)
			If (Not Me.IsValidRowHandle(rowHandle)) Then
				Return Nothing
			End If
			If Me.IsGroupRow(rowHandle) Then
				Return New GridAccessibleGroupRow(Me, rowHandle)
			End If
			Return New GridAccessibleDataRowEx(Me, rowHandle)
		End Function

		#End Region
	End Class

	Public Class CGridViewInfoRegistrator
		Inherits GridInfoRegistrator
		Public Overrides ReadOnly Property ViewName() As String
			Get
				Return "CGridView"
			End Get
		End Property
		Public Overrides Function CreateView(ByVal grid As GridControl) As BaseView
			Return New CGridView(TryCast(grid, GridControl))
		End Function
	End Class

	Friend Class GridAccessibleDataRowEx
		Inherits GridAccessibleDataRow
		Public Sub New(ByVal view As GridView, ByVal rowHandle As Integer)
			MyBase.New(view, rowHandle)
		End Sub

		Protected Overrides Function GetAccessibleRowName() As String
			If TypeOf View Is CGridView AndAlso Me.View.IsDataRow(MyBase.RowHandle) Then
				Dim name As String = (CType(View, CGridView)).GetRowName(RowHandle)
				If (Not String.IsNullOrEmpty(name)) Then
					Return String.Format(Me.GetString(AccStringId.GridRow), name)
				End If
			End If
			Return MyBase.GetAccessibleRowName()
		End Function
		Public Overrides Function GetCell(ByVal index As Integer) As IAccessibleGridRowCell
			If index >= MyBase.View.VisibleColumns.Count Then
				Return Nothing
			End If
			Return New GridAccessibleRowCellEx(MyBase.View, MyBase.RowHandle, MyBase.View.VisibleColumns(index))
		End Function
	End Class
	Friend Class GridAccessibleRowCellEx
		Inherits GridAccessibleRowCell
		Implements IAccessibleGridRowCell
		Public Sub New(ByVal view As GridView, ByVal rowHandle As Integer, ByVal column As GridColumn)
			MyBase.New(view, rowHandle, column)
		End Sub

		#Region "IAccessibleGridRowCell Members"

		Private Function GetName() As String Implements IAccessibleGridRowCell.GetName
			Dim str As String
			If (Me.Column.GetTextCaption() = "") Then
				str = Me.Column.ToolTip
			Else
				str = Me.Column.GetTextCaption()
			End If
			Dim rowName As String = (CType(Me.View, IAccessibleGrid)).GetRow((CType(Me.View, CGridView)).RowHandle2AccessibleIndex(Me.RowHandle)).GetName()
			Return (str & " " & rowName)
		End Function

		#End Region
	End Class
End Namespace