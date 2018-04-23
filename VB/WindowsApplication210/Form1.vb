Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Collections
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid

Namespace WindowsApplication210
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()

			For i As Integer = 0 To 4
				dataTable1.Rows.Add(New Object() {i, i*i, "abcde".Substring(i,1) })
			Next i

			Dim grid As GridControl = New CGrid.CGridControl()
			grid.DataSource = dataTable1
			Dim view As New CGrid.CGridView(grid)
			view.RowNameField = "Column3"
			grid.MainView = view
			view.OptionsView.ShowAutoFilterRow = True
			view.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom
			view.IndicatorWidth = 40
			view.OptionsBehavior.Editable = False
			grid.Dock = DockStyle.Fill
			Me.Controls.Add(grid)

		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		End Sub

	End Class
End Namespace