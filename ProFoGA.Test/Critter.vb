Public Class Critter
  Const maxProtein As Integer = 200

	Private _age As Byte
	Private _flag1 As Boolean
	Private _flag2 As Boolean
	Private genestor As Byte()
	Private _length As Integer

  Public Property Age() As Byte
    Get
      Return _age
    End Get
    Set(ByVal value As Byte)
      _age = value
    End Set
  End Property

  Public Property Flag1() As Boolean
    Get
      Return _flag1
    End Get
    Set(ByVal value As Boolean)
      _flag1 = value
    End Set
  End Property

  Public Property Flag2() As Boolean
    Get
      Return _flag2
    End Get
    Set(ByVal value As Boolean)
      _flag2 = value
    End Set
  End Property

  Public Property Length() As Integer
    Get
      Return _length
    End Get
    Set(ByVal value As Integer)
      _length = value
    End Set
  End Property

  Public Sub New(ByVal initialLength As Integer)
    MyBase.New()

    'Dim num1, num2 As UInteger
    Me.genestor = New Byte((25) - 1) {}
    Age = 0
    Length = initialLength

    genestor = System.Array.CreateInstance(GetType(Byte), initialLength)
    genestor.Initialize()
  End Sub

  Public Property Gene(ByVal index As Integer) As Byte
    Get
      Return (Me.genestor(index >> 3) And (1 << (index And 7))) >> (index And 7)
    End Get
    Set(ByVal Value As Byte)
      Value = (Value And 1)
      Me.genestor(index >> 3) = Me.genestor(index >> 3) And Not (1 << (index And 7))
      Me.genestor(index >> 3) = Me.genestor(index >> 3) Or (Value << (index And 7))
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

  Public Sub Display(ByVal ACanvas As System.Drawing.Graphics, ByVal ARect As System.Drawing.RectangleF)
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
      Dim lNewVariable As Single = ARect.Left + i * 2
      ACanvas.DrawLine(pn, lNewVariable + 24, ARect.Top + 1, lNewVariable + 24, ARect.Bottom - 1)
      ACanvas.DrawLine(pn, lNewVariable + 25, ARect.Top + 1, lNewVariable + 25, ARect.Bottom - 1)
    Next

  End Sub

  Public Sub Sibling(ByVal Father As Critter, ByVal Mother As Critter)
    Const maxCO As Integer = 10 ', maxMut As Integer = 0.1
    Dim COnum As Integer
    Dim CrossOver() As Integer
    Dim fromMother As Boolean
    Dim rand As New System.Random

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
