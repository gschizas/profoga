Public Class Critter
	Const maxProtein As Integer = 200

	Public Age As Byte
	Public flag1 As Boolean
	Public flag2 As Boolean
	Private genestor As Byte()
	Public Length As Integer

	Public Sub New(ByVal l As Integer)
		MyBase.New()

		Dim num1 As UInt16
		Dim num2 As UInt16
		Me.genestor = New Byte((25) - 1) {}
		Me.Age = 0
		Me.Length = l

		genestor = System.Array.CreateInstance(GetType(Byte), l)
		genestor.Initialize()
	End Sub

	Public Property Gene(ByVal idx As Integer) As Byte
		Get
			Return (Me.genestor(idx >> 3) And (1 << (idx And 7))) >> (idx And 7)
		End Get
		Set(ByVal Value As Byte)
			Value = (Value And 1)
			Me.genestor(idx >> 3) = Me.genestor(idx >> 3) And Not (1 << (idx And 7))
			Me.genestor(idx >> 3) = Me.genestor(idx >> 3) Or (Value << (idx And 7))
		End Set
	End Property

	Public Sub RandomInit()
		Dim rand As New System.Random
		rand.NextBytes(genestor)
	End Sub

	Public ReadOnly Property Fitness() As Double
		Get
			Dim result As Double
			result = 0.5
			For i As Integer = 1 To Length
				' Fake fitness -> to lead into a gene 010101010101...
				If ((i And 1) Xor Gene(i)) = 1 Then
					result += (0.5 / Length)
				Else
					result -= (0.5 / Length)
				End If
			Next
			Return Math.Round(result * 1000000.0) / 1000000.0
		End Get
	End Property

	Public Sub Display(ByVal ACanvas As System.drawing.Graphics, ByVal ARect As System.Drawing.RectangleF)
		Dim i As Integer
		Dim sb As System.Text.StringBuilder
		Dim pn As System.Drawing.Pen
		Dim br As System.Drawing.SolidBrush
		Dim fit As Integer
		Dim fitRect As System.Drawing.Rectangle
		sb = New System.Text.StringBuilder

		fit = Math.Floor(Fitness * 20)

		fitRect = System.Drawing.Rectangle.Round(ARect)
		fitRect.Inflate(-1, -1)

		pn = System.Drawing.Pens.Green
		br = System.Drawing.Brushes.White
		ACanvas.FillRectangle(br, fitRect)
		ACanvas.DrawRectangle(pn, fitRect)
		br = System.Drawing.Brushes.Green
		ACanvas.FillRectangle(br, fitRect)

		For i = 1 To Length
			pn = IIf(Gene(i) = 1, System.Drawing.Pens.Black, System.Drawing.Pens.Yellow)
			ACanvas.DrawLine(pn, ARect.Left + i * 2 + 24, ARect.Top + 1, ARect.Left + i * 2 + 24, ARect.Bottom - 1)
			ACanvas.DrawLine(pn, ARect.Left + i * 2 + 25, ARect.Top + 1, ARect.Left + i * 2 + 25, ARect.Bottom - 1)
		Next

	End Sub

	Public Sub Sibling(ByVal Father As Critter, ByVal Mother As Critter)
		Const maxCO = 10, maxMut = 0.1
		Dim COnum As Integer
		Dim CrossOver() As Integer
		Dim fromMother As Boolean
		Dim rand As System.Random

		CrossOver = System.Array.CreateInstance(GetType(Integer), maxCO)

		COnum = 1 + Int(rand.Next(0, maxCO))
		fromMother = (rand.Next(1, 2) = 2)
		For j As Integer = 1 To COnum
			CrossOver(j) = rand.Next(Length + 2)
		Next
		For i As Integer = 1 To Length
			Gene(i) = IIf(fromMother, Mother.Gene(i), Father.Gene(i))
			For j As Integer = 1 To COnum
				If i = CrossOver(j) Then
					fromMother = Not fromMother
				End If
			Next
		Next

	End Sub

End Class
