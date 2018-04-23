Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraEditors.Repository
Imports System.Drawing
Imports System.Reflection
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors
Imports System.ComponentModel
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing.Helpers

Namespace WindowsApplication3
	'The attribute that points to the registration method


	<UserRepositoryItem("RegisterCustomEdit")> _
	Public Class RepositoryItemCustomEdit
		Inherits RepositoryItemDateEdit

		'The static constructor which calls the registration method
		Shared Sub New()
			RegisterCustomEdit()
		End Sub


		'The unique name for the custom editor
		Public Const CustomEditName As String = "CustomEdit"

		'Return the unique name
		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return CustomEditName
			End Get
		End Property

		'Register the editor
		Public Shared Sub RegisterCustomEdit()
			'Icon representing the editor within a container editor's Designer
			Dim img As Image = Nothing
			Try
				img = CType(Bitmap.FromStream(System.Reflection.Assembly.GetExecutingAssembly(). GetManifestResourceStream("DevExpress.CustomEditors.CustomEdit.bmp")), Bitmap)
			Catch
			End Try
			EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(CustomEditName, GetType(CustomEdit), GetType(RepositoryItemCustomEdit), GetType(DateEditViewInfo), New ButtonEditPainter(), True, img))
		End Sub

		'Override the Assign method
		Public Overrides Sub Assign(ByVal item As RepositoryItem)
			BeginUpdate()
			Try
				MyBase.Assign(item)
				Dim source As RepositoryItemCustomEdit = TryCast(item, RepositoryItemCustomEdit)
				If source Is Nothing Then
					Return
				End If
			Finally
				EndUpdate()
			End Try
		End Sub
	End Class


	Public Class CustomEdit
		Inherits DateEdit

		'The static constructor which calls the registration method
		Shared Sub New()
			RepositoryItemCustomEdit.RegisterCustomEdit()
		End Sub

		'Initialize the new instance
		Public Sub New()
			'...
		End Sub

		'Return the unique name
		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return RepositoryItemCustomEdit.CustomEditName
			End Get
		End Property

		'Override the Properties property
		'Simply type-cast the object to the custom repository item type
		<DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public Shadows ReadOnly Property Properties() As RepositoryItemCustomEdit
			Get
				Return TryCast(MyBase.Properties, RepositoryItemCustomEdit)
			End Get
		End Property

		Protected Friend Overridable Function IsVistaDisplayMode() As Boolean
			If Properties.VistaDisplayMode = DefaultBoolean.False Then
				Return False
			End If
			If Properties.VistaDisplayMode = DefaultBoolean.True Then
				Return True
			End If
			Return NativeVista.IsVista
		End Function


		Protected Overrides Function CreatePopupForm() As DevExpress.XtraEditors.Popup.PopupBaseForm
			If IsVistaDisplayMode() Then
				Return New CustomVistaPopupDateEditForm(Me)
			End If
			Return New CustomPopupDateEditForm(Me)
		End Function
	End Class

	Public Class CustomVistaPopupDateEditForm
		Inherits VistaPopupDateEditForm
		Public Sub New(ByVal de As DateEdit)
			MyBase.New(de)
		End Sub
		Protected Overrides Function CreateCalendar() As DateEditCalendar
			Return New CustomVistaDateEditCalendar(OwnerEdit.Properties, OwnerEdit.EditValue)
		End Function
	End Class

	Public Class CustomVistaDateEditCalendar
		Inherits VistaDateEditCalendar
		Public Sub New(ByVal de As RepositoryItemDateEdit, ByVal dt As Object)
			MyBase.New(de, dt)
		End Sub
		Private btn As SimpleButton
		Private indent As Integer = 5
		Public ReadOnly Property Button() As SimpleButton
			Get
				Return btn
			End Get
		End Property
		Protected Overrides Sub CreateButtons()
			MyBase.CreateButtons()
			btn = New SimpleButton()
			AddHandler btn.Click, AddressOf OnNewButtonClick
			btn.Text = "New Button"
			Controls.Add(btn)
		End Sub

		Private Sub OnNewButtonClick(ByVal sender As Object, ByVal e As EventArgs)
			XtraMessageBox.Show("New Button Click")
		End Sub

		Protected Overrides Sub UpdateButtons()
			MyBase.UpdateButtons()
			If Button IsNot Nothing Then
				Button.Visible = True
				Button.Size = New Size(100, 30)
				Button.Location = New Point(Bounds.X + Bounds.Width \ 2 - Button.Size.Width \ 2, Bounds.Bottom - Button.Size.Height - indent)
			End If
		End Sub
		Public Overrides Function CalcBestSize(ByVal g As Graphics) As Size
			Dim bestSize As Size = MyBase.CalcBestSize(g)
			bestSize.Height += Button.Height + indent
			Return bestSize
		End Function
	End Class

	Public Class CustomPopupDateEditForm
		Inherits PopupDateEditForm
		Public Sub New(ByVal de As DateEdit)
			MyBase.New(de)
		End Sub
		Protected Overrides Function CreateCalendar() As DateEditCalendar
			Return New CustomDateEditCalendar(OwnerEdit.Properties, OwnerEdit.EditValue)
		End Function
	End Class

	Public Class CustomDateEditCalendar
		Inherits DateEditCalendar
		Public Sub New(ByVal de As RepositoryItemDateEdit, ByVal dt As Object)
			MyBase.New(de, dt)
		End Sub
		Private btn As SimpleButton
		Private indent As Integer = 5
		Public ReadOnly Property Button() As SimpleButton
			Get
				Return btn
			End Get
		End Property
		Protected Overrides Sub CreateButtons()
			MyBase.CreateButtons()
			btn = New SimpleButton()
			AddHandler btn.Click, AddressOf OnNewButtonClick
			btn.Text = "New Button"
			Controls.Add(btn)
		End Sub

		Private Sub OnNewButtonClick(ByVal sender As Object, ByVal e As EventArgs)
			XtraMessageBox.Show("New Button Click")
		End Sub

		Protected Overrides Sub UpdateButtons()
			MyBase.UpdateButtons()
			If Button IsNot Nothing Then
				Button.Visible = True
				Button.Size = New Size(100, 30)
				Button.Location = New Point(Bounds.X + Bounds.Width \ 2 - Button.Size.Width \ 2, Bounds.Bottom - Button.Size.Height - indent)
			End If
		End Sub
		Public Overrides Function CalcBestSize(ByVal g As Graphics) As Size
			Dim bestSize As Size = MyBase.CalcBestSize(g)
			bestSize.Height += Button.Height + indent
			Return bestSize
		End Function
	End Class

End Namespace
