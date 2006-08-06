Module ModuleMain

	Sub Main()
		Main1()
		Console.WriteLine("Press [Enter]")
		Console.ReadLine()

	End Sub

	<Flags()> _
	Public Enum ShiftState
		ssAlt = 2
		ssCtrl = 4
		ssDouble = 64
		ssLeft = 8
		ssMiddle = 32
		ssRight = 16
		ssShift = 1
	End Enum

	Private Sub Main1()
		Dim s As ShiftState
		s = CType(0, ShiftState)
		Console.WriteLine(s.ToString)
	End Sub

	Private Sub Main2()
		Dim br As System.Drawing.SolidBrush
		br = New System.Drawing.SolidBrush(System.Drawing.Color.Blue)
	End Sub



End Module
