Public Class Form1
    Dim time As Integer = 234
    Private WithEvents Timer1 As New Timer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 20
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If time > 0 Then
            Label1.Text = "I'm " & time & " km away from u :D"
            time -= 2
        Else
            Timer1.Stop() ' Stop the timer when time reaches 0
            Label1.Visible = False
            MessageBox.Show("Hello, " & Environment.UserName & " ;)")
            MessageBox.Show("I planted 2.7 kilograms of C4 around your house :P")
            MessageBox.Show("BUT!!!1 If you solve my puzzel...")
            MessageBox.Show("You have an CHANCE OF ESCAPING!111!1!1 :DDDDDDD")
            Form2.Show()
        End If
    End Sub
End Class