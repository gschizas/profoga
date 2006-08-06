Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft
Imports Microsoft.DirectX
'Imports Microsoft.DirectX.Direct3D

Public Class frmDisplay
	Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call

	End Sub

	'Form overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	Friend WithEvents picRender As TerraSoft.Biotech.ProFoGA.DisplayDXDemo.D3DDisplay
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.picRender = New TerraSoft.Biotech.ProFoGA.DisplayDXDemo.D3DDisplay
		Me.SuspendLayout()
		'
		'picRender
		'
		Me.picRender.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
								Or System.Windows.Forms.AnchorStyles.Left) _
								Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.picRender.Location = New System.Drawing.Point(72, 8)
		Me.picRender.Name = "picRender"
		Me.picRender.Size = New System.Drawing.Size(344, 352)
		Me.picRender.TabIndex = 0
		Me.picRender.TabStop = False
		'
		'frmDisplay
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(424, 366)
		Me.Controls.Add(Me.picRender)
		Me.Name = "frmDisplay"
		Me.Text = "frmDisplay"
		Me.ResumeLayout(False)

	End Sub

#End Region

	Private WithEvents theDevice As Direct3D.Device = Nothing
	Private WithEvents vertexBuffer As Direct3D.VertexBuffer = Nothing

	Public Sub InitializeGraphics()
		Try
			Dim presentParams As New Direct3D.PresentParameters
			presentParams.Windowed = True
			presentParams.SwapEffect = Direct3D.SwapEffect.Copy
			presentParams.PresentationInterval = Direct3D.PresentInterval.Default
			presentParams.DeviceWindow = Me.picRender
			theDevice = New Direct3D.Device(0, Direct3D.DeviceType.Hardware, Me.picRender, Direct3D.CreateFlags.HardwareVertexProcessing, presentParams)
		Catch ex As Direct3D.Direct3DXException
			MessageBox.Show(ex.ToString)
		End Try

		theDevice.Lights(0).Ambient = Color.White

		vertexBuffer = New Direct3D.VertexBuffer(GetType(Direct3D.CustomVertex.TransformedColored), 3, theDevice, 0, Direct3D.CustomVertex.TransformedColored.Format, Direct3D.Pool.Default)

		Me.OnCreateVertexBuffer(vertexBuffer, Nothing)


	End Sub

	Private Sub Render(ByVal e As System.Windows.Forms.PaintEventArgs) Handles picRender.Render
		If theDevice Is Nothing Then Return
		Try
			theDevice.Clear(Direct3D.ClearFlags.Target, System.Drawing.Color.Chocolate, 1.0F, 0)
			theDevice.BeginScene()

			SetupMatrices()

			theDevice.SetStreamSource(0, vertexBuffer, 0)
			theDevice.VertexFormat = Direct3D.CustomVertex.TransformedColored.Format
			theDevice.DrawPrimitives(Direct3D.PrimitiveType.TriangleList, 0, 1)


			'Dim anAtom As Direct3D.Mesh
			'Dim materials() As Direct3D.ExtendedMaterial
			'materials = Array.CreateInstance(GetType(Direct3D.ExtendedMaterial), 1)

			'anAtom = Direct3D.Mesh.FromFile("C:\Documents and Settings\GSchizas\My Documents\Biotech\DISS\ProFoGA.NET\atom.x", Direct3D.MeshFlags.SystemMemory, theDevice, materials)

			'anAtom.DrawSubset(0)

			theDevice.EndScene()
			theDevice.Present()
		Catch ex As Direct3D.Direct3DXException
			MessageBox.Show(ex.ToString)
		End Try
	End Sub

	Private Sub SetupMatrices()
		' For our world matrix, we will just rotate the object about the y-axis.

		' Set up the rotation matrix to generate 1 full rotation (2*PI radians) 
		' every 1000 ms. To avoid the loss of precision inherent in very high 
		' floating point numbers, the system time is modulated by the rotation 
		' period before conversion to a radian angle.
		Dim iTime As Integer = Environment.TickCount Mod 1000
		Dim fAngle As Single = iTime * (2.0F * Math.PI) / 1000.0F
		theDevice.Transform.World = Matrix.RotationY(fAngle)

		' Set up our view matrix. A view matrix can be defined given an eye point,
		' a point to lookat, and a direction for which way is up. Here, we set the
		' eye five units back along the z-axis and up three units, look at the
		' origin, and define "up" to be in the y-direction.
		theDevice.Transform.View = Matrix.LookAtLH(New Vector3(0.0F, 3.0F, -5.0F), New Vector3(0.0F, 0.0F, 0.0F), New Vector3(0.0F, 1.0F, 0.0F))

		' For the projection matrix, we set up a perspective transform (which
		' transforms geometry from 3D view space to 2D viewport space, with
		' a perspective divide making objects smaller in the distance). To build
		' a perpsective transform, we need the field of view (1/4 pi is common),
		' the aspect ratio, and the near and far clipping planes (which define at
		' what distances geometry should be no longer be rendered).
		theDevice.Transform.Projection = Matrix.PerspectiveFovLH(CSng(Math.PI) / 4, 1.0F, 1.0F, 100.0F)
	End Sub	 'SetupMatrices

	Private Sub frmDisplay_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Me.InitializeGraphics()
	End Sub

	Public Sub OnCreateDevice(ByVal sender As Object, ByVal e As EventArgs)
		Dim dev As Direct3D.Device = CType(sender, Direct3D.Device)
		' Now Create the VB
		vertexBuffer = New Direct3D.VertexBuffer(GetType(Direct3D.CustomVertex.PositionColored), 3, dev, 0, Direct3D.CustomVertex.PositionColored.Format, Direct3D.Pool.Default)
		AddHandler vertexBuffer.Created, AddressOf Me.OnCreateVertexBuffer
		Me.OnCreateVertexBuffer(vertexBuffer, Nothing)
	End Sub	 'OnCreateDevice

	Public Sub OnResetDevice(ByVal sender As Object, ByVal e As EventArgs) Handles theDevice.DeviceReset
		Dim dev As Direct3D.Device = CType(sender, Direct3D.Device)
		' Turn off culling, so we see the front and back of the triangle
		dev.RenderState.CullMode = Direct3D.Cull.None
		' Turn off D3D lighting, since we are providing our own vertex colors
		dev.RenderState.Lighting = False
	End Sub	 'OnResetDevice


	Private Sub OnCreateVertexBuffer(ByVal sender As Object, ByVal e As System.EventArgs) Handles vertexBuffer.Created
		Dim vb As Direct3D.VertexBuffer = DirectCast(sender, Direct3D.VertexBuffer)
		Dim verts As Direct3D.CustomVertex.PositionColored() = CType(vb.Lock(0, 0), Direct3D.CustomVertex.PositionColored())
		verts(0).X = -1.0F
		verts(0).Y = -1.0F
		verts(0).Z = 0.0F
		verts(0).Color = System.Drawing.Color.DarkGoldenrod.ToArgb()
		verts(1).X = 1.0F
		verts(1).Y = -1.0F
		verts(1).Z = 0.0F
		verts(1).Color = System.Drawing.Color.MediumOrchid.ToArgb()
		verts(2).X = 0.0F
		verts(2).Y = 1.0F
		verts(2).Z = 0.0F
		verts(2).Color = System.Drawing.Color.Cornsilk.ToArgb()
		vb.Unlock()

	End Sub
End Class

