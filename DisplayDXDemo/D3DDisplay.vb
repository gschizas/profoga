Public Class D3DDisplay
	Inherits System.Windows.Forms.Control
#Region " Component Designer generated code "

	Public Sub New()
		MyBase.New()

		' This call is required by the Component Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call

	End Sub

	'Control overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Control Designer
	Private components As System.ComponentModel.IContainer

	' NOTE: The following procedure is required by the Component Designer
	' It can be modified using the Component Designer.  Do not modify it
	' using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		components = New System.ComponentModel.Container
	End Sub

#End Region

	Protected Overrides Sub OnPaint(ByVal pe As System.Windows.Forms.PaintEventArgs)
		MyBase.OnPaint(pe)
		'Add your custom paint code here
		RaiseEvent Render(pe)
	End Sub



	Public Event Render(ByVal pe As System.Windows.Forms.PaintEventArgs)

End Class
